using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POC.SPAL.Api.Tests.Acceptance.Brokers;
using POC.SPAL.Api.Tests.Acceptance.Models.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace POC.SPAL.Api.Tests.Acceptance.Apis.Students
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentsApiTests
    {
        private readonly ApiBroker apiBroker;

        public StudentsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private async ValueTask<Student> PostRandomStudentAsync()
        {
            Student randomStudent = CreateRandomStudent();
            await this.apiBroker.PostStudentAsync(randomStudent);

            return randomStudent;
        }

        private async ValueTask<List<Student>> PostRandomStudentsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomStudents = new List<Student>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomStudents.Add(await PostRandomStudentAsync());
            }

            return randomStudents;
        }

        private static Student CreateRandomStudent() =>
            CreateRandomStudentFiller().Create();

        private static Filler<Student> CreateRandomStudentFiller()
        {
            Guid userId = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Student>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.CreatedByUserId).Use(userId)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnProperty(student => student.UpdatedByUserId).Use(userId);

            return filler;
        }
    }
}