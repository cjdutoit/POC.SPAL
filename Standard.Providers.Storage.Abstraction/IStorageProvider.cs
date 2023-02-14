// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

namespace Standard.Providers.Storage.Abstraction
{
    public interface IStorageProvider : IDisposable
    {
        ValueTask<T> InsertAsync<T>(T @object);
        IQueryable<T> SelectAll<T>() where T : class;
        ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class;
        ValueTask<T> UpdateAsync<T>(T @object);
        ValueTask<T> DeleteAsync<T>(T @object);
    }
}
