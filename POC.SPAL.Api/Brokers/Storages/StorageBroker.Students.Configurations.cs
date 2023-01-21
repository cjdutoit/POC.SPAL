using Microsoft.EntityFrameworkCore;
using POC.SPAL.Api.Models.Students;

namespace POC.SPAL.Api.Brokers.Storages
{
    public partial class StorageBroker
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
