using System;
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