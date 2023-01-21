using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using POC.SPAL.Api.Models.Students;
using POC.SPAL.Api.Models.Students.Exceptions;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            SqlException sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(someStudent);

            StudentDependencyException actualStudentDependencyException =
                await Assert.ThrowsAsync<StudentDependencyException>(
                    addStudentTask.AsTask);

            // then
            actualStudentDependencyException.Should()
                .BeEquivalentTo(expectedStudentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}