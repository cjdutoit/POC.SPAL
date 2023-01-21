// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Standard.Providers.Storage.Abstraction;
using Standard.Providers.Storage.EntityFramework.Models.Students;

namespace Standard.Providers.Storage.EntityFramework
{
    public partial class EntityFrameworkStorageProvider : EFxceptionsContext, IStorageProvider
    {

        private readonly DbContextOptions<EntityFrameworkStorageProvider> dbContextOptions;

        public EntityFrameworkStorageProvider(DbContextOptions<EntityFrameworkStorageProvider> options) : base(options)
        {
            this.dbContextOptions = options;
            this.Database.Migrate();
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddStudentConfigurations(modelBuilder);
        }

        public async ValueTask<T> InsertAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(dbContextOptions);
            broker.Entry(@object).State = EntityState.Added;
            await broker.SaveChangesAsync();

            return @object;
        }

        public IQueryable<T> SelectAll<T>() where T : class
        {
            using var broker = new EntityFrameworkStorageProvider(dbContextOptions);

            return broker.Set<T>();
        }

        public async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class =>
            await FindAsync<T>(objectIds);

        public async ValueTask<T> UpdateAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(dbContextOptions);
            broker.Entry(@object).State = EntityState.Modified;
            await broker.SaveChangesAsync();

            return @object;
        }

        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new EntityFrameworkStorageProvider(dbContextOptions);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }

        public override void Dispose() { }
    }
}
