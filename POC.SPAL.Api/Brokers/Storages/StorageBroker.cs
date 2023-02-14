// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Standard.Providers.Storage.Abstraction;

namespace POC.SPAL.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IStorageAbstractProvider storageAbstractProvider;

        public StorageBroker(IStorageAbstractProvider storageAbstractProvider)
        {
            this.storageAbstractProvider = storageAbstractProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AddStudentConfigurations(modelBuilder);
        }

        public override void Dispose() { }
    }
}
