using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using POC.SPAL.Api.Models.Students;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldModifyStudentAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Student randomStudent = CreateRandomModifyStudent(randomDateTimeOffset);
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent.DeepClone();
            storageStudent.UpdatedDate = randomStudent.CreatedDate;
            Student updatedStudent = inputStudent;
            Student expectedStudent = updatedStudent.DeepClone();
            Guid studentId = inputStudent.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentAsync(inputStudent))
                    .ReturnsAsync(updatedStudent);

            // when
            Student actualStudent =
                await this.studentService.ModifyStudentAsync(inputStudent);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(inputStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}