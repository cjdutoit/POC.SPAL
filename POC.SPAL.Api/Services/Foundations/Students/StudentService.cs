using System;
using System.Linq;
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
            TryCatch(async () =>
            {
                ValidateStudentOnAdd(student);

                return await this.storageBroker.InsertStudentAsync(student);
            });

        public IQueryable<Student> RetrieveAllStudents() =>
            TryCatch(() => this.storageBroker.SelectAllStudents());

        public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
            TryCatch(async () =>
            {
                ValidateStudentId(studentId);

                Student maybeStudent = await this.storageBroker
                    .SelectStudentByIdAsync(studentId);

                ValidateStorageStudent(maybeStudent, studentId);

                return maybeStudent;
            });

        public ValueTask<Student> ModifyStudentAsync(Student student) =>
            TryCatch(async () =>
            {
                ValidateStudentOnModify(student);

                Student maybeStudent =
                    await this.storageBroker.SelectStudentByIdAsync(student.Id);

                ValidateStorageStudent(maybeStudent, student.Id);
                ValidateAgainstStorageStudentOnModify(inputStudent: student, storageStudent: maybeStudent);

                return await this.storageBroker.UpdateStudentAsync(student);
            });

        public async ValueTask<Student> RemoveStudentByIdAsync(Guid studentId)
        {
            Student maybeStudent = await this.storageBroker
                    .SelectStudentByIdAsync(studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        }
    }
}