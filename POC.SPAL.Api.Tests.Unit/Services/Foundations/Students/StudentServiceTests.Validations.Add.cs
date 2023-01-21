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
        public async Task ShouldThrowValidationExceptionOnAddIfStudentIsNullAndLogItAsync()
        {
            // given
            Student nullStudent = null;

            var nullStudentException =
                new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(nullStudent);

            StudentValidationException actualStudentValidationException =
                await Assert.ThrowsAsync<StudentValidationException>(
                    addStudentTask.AsTask);

            // then
            actualStudentValidationException.Should().BeEquivalentTo(expectedStudentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}