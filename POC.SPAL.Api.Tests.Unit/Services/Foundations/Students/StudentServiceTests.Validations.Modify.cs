using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using POC.SPAL.Api.Models.Students;
using POC.SPAL.Api.Models.Students.Exceptions;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentIsNullAndLogItAsync()
        {
            // given
            Student nullStudent = null;
            var nullStudentException = new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            // when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(nullStudent);

            StudentValidationException actualStudentValidationException =
                await Assert.ThrowsAsync<StudentValidationException>(
                    modifyStudentTask.AsTask);

            // then
            actualStudentValidationException.Should()
                .BeEquivalentTo(expectedStudentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}