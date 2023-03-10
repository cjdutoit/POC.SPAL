# POC - Standardized Provider Abstraction Libraries (SPAL)

Playing with SPAL as documented [here](https://github.com/hassanhabib/The-Standard/blob/master/1.%20Brokers/1.8%20Broker%20Provider%20Abstraction.md#18-standardized-provider-abstraction-libraries-spal)

Startup.cs
```cs
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddControllers();
            services.AddDbContext<EntityFrameworkStorageProvider>().As<IStorageProvider>();
            AddBrokers(services);
            AddServices(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "POC.SPAL.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "POC.SPAL.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IStorageAbstractProvider, StorageAbstractProvider>();
            services.AddTransient<IStorageProvider, EntityFrameworkStorageProvider>();
            services.AddTransient<IStudentService, StudentService>();
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<IStorageAbstractProvider, StorageAbstractProvider>();
            services.AddTransient<IStorageProvider, EntityFrameworkStorageProvider>();
        }
    }
```

StorageBroker.cs
```cs
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IStorageAbstractProvider storageAbstractProvider;

        public StorageBroker(IStorageAbstractProvider storageAbstractProvider)
        {
            // The broker are now only aware of the Storage Abstract Provider.  DBContext is abstracted away.
            this.storageAbstractProvider = storageAbstractProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddStudentConfigurations(modelBuilder);
        }

        public override void Dispose() { }
    }

```

StorageBroker.Students.cs
```cs
    public partial class StorageBroker
    {
        // The broker now runs the methods of the storage abstraction provider
    
        public async ValueTask<Student> InsertStudentAsync(Student student) =>
            await this.storageAbstractProvider.InsertAsync(student);

        public IQueryable<Student> SelectAllStudents() =>
            this.storageAbstractProvider.SelectAll<Student>();

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId) =>
            await this.storageAbstractProvider.SelectAsync<Student>(studentId);

        public async ValueTask<Student> UpdateStudentAsync(Student student) =>
            await this.storageAbstractProvider.UpdateAsync(student);

        public async ValueTask<Student> DeleteStudentAsync(Student student) =>
            await this.storageAbstractProvider.DeleteAsync(student);
    }

```

StorageAbstractProvider.cs 
```cs
    public class StorageAbstractProvider : IStorageAbstractProvider
    {
        private readonly IStorageProvider provider;

        public StorageAbstractProvider(IStorageProvider provider)
        {
            this.provider = provider;
        }

        public void Dispose() =>
            this.provider.Dispose();
            
        // The abstract provider now calls the storage provider that was setup through DI.

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            var x = await this.provider.InsertAsync(@object);
            return x;
        }

        public IQueryable<T> SelectAll<T>() where T : class =>
            this.provider.SelectAll<T>();

        public async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class =>
            await this.provider.SelectAsync<T>(objectIds);

        public async ValueTask<T> UpdateAsync<T>(T @object) =>
            await this.provider.UpdateAsync(@object);

        public async ValueTask<T> DeleteAsync<T>(T @object) =>
           await this.provider.DeleteAsync(@object);
    }
```

EntityFrameworkStorageProvider.cs
```cs
    public partial class EntityFrameworkStorageProvider : EFxceptionsContext, IStorageProvider
    {
        // The EntityFrameworkStorageProvider looks the same as what the broker used to look like

private readonly IConfiguration configuration;

        public EntityFrameworkStorageProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddStudentConfigurations(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString(name: "DefaultConnection");
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseSqlServer(connectionString);
        }

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(this.configuration);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        public IQueryable<T> SelectAll<T>() where T : class
        {
            using var broker = new EntityFrameworkStorageProvider(this.configuration);

            return broker.Set<T>();
        }

        public async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class =>
            await FindAsync<T>(objectIds);

        public async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(this.configuration);
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(this.configuration);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }

        public override void Dispose() { }
    }
```
