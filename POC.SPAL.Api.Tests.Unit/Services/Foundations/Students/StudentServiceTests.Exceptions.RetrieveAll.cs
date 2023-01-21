using System;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using POC.SPAL.Api.Models.Students.Exceptions;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedStorageException =
                new FailedStudentStorageException(sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Throws(sqlException);

            // when
            Action retrieveAllStudentsAction = () =>
                this.studentService.RetrieveAllStudents();

            StudentDependencyException actualStudentDependencyException =
                Assert.Throws<StudentDependencyException>(retrieveAllStudentsAction);

            // then
            actualStudentDependencyException.Should()
                .BeEquivalentTo(expectedStudentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}