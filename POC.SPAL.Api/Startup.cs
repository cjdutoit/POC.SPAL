// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using POC.SPAL.Api.Brokers.DateTimes;
using POC.SPAL.Api.Brokers.Loggings;
using POC.SPAL.Api.Brokers.Storages;
using POC.SPAL.Api.Services.Foundations.Students;
using Standard.Providers.Storage;
using Standard.Providers.Storage.Abstraction;
using Standard.Providers.Storage.EntityFramework;

namespace POC.SPAL.Api
{
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

            services.AddDbContext<EntityFrameworkStorageProvider>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(name: "DefaultConnection")));

            //services.AddDbContext<EntityFrameworkStorageProvider>(options =>
            //    options.UseInMemoryDatabase(databaseName: "SPAL"));

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
        }
    }
}
