// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Standard.Providers.Storage.Abstraction;
using Standard.Providers.Storage.EntityFramework.Models.Students;

namespace Standard.Providers.Storage.EntityFramework
{
    public partial class EntityFrameworkStorageProvider : EFxceptionsContext, IStorageProvider
    {

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
}
