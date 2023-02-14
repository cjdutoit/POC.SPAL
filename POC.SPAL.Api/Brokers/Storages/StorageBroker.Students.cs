// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Standard.Providers.Storage.EntityFramework.Models.Students;

namespace POC.SPAL.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
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
}
