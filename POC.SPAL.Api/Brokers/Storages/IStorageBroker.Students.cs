using System;
using System.Linq;
using System.Threading.Tasks;
using POC.SPAL.Api.Models.Students;

namespace POC.SPAL.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Student> InsertStudentAsync(Student student);
    }
}
