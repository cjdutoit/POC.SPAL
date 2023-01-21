using System.Threading.Tasks;
using POC.SPAL.Api.Brokers.DateTimes;
using POC.SPAL.Api.Brokers.Loggings;
using POC.SPAL.Api.Brokers.Storages;
using POC.SPAL.Api.Models.Students;

namespace POC.SPAL.Api.Services.Foundations.Students
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Student> AddStudentAsync(Student student) =>
            throw new System.NotImplementedException();
    }
}