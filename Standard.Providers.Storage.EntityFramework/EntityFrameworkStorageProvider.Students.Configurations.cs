// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Standard.Providers.Storage.EntityFramework.Models.Students;

namespace Standard.Providers.Storage.EntityFramework
{
    public partial class EntityFrameworkStorageProvider
    {
        private static void AddStudentConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(student => student.IdentityNumber)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(student => student.FirstName)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .Property(student => student.LastName)
                .IsRequired();
        }
    }
}
