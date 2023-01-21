using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.SPAL.Api.Tests.Acceptance.Models.Students;

namespace POC.SPAL.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string StudentsRelativeUrl = "api/students";

        public async ValueTask<Student> PostStudentAsync(Student student) =>
            await this.apiFactoryClient.PostContentAsync(StudentsRelativeUrl, student);

        public async ValueTask<Student> GetStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.GetContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

        public async ValueTask<List<Student>> GetAllStudentsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Student>>($"{StudentsRelativeUrl}/");

        public async ValueTask<Student> PutStudentAsync(Student student) =>
            await this.apiFactoryClient.PutContentAsync(StudentsRelativeUrl, student);

        public async ValueTask<Student> DeleteStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");
    }
}
