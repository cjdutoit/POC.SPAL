// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Standard.Providers.Storage.Abstraction;

namespace Standard.Providers.Storage
{
    public class StorageAbstractProvider : IStorageAbstractProvider
    {
        private readonly IStorageProvider provider;

        public StorageAbstractProvider(IStorageProvider provider)
        {
            this.provider = provider;
        }

        public void Dispose() =>
            this.provider.Dispose();

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
}
