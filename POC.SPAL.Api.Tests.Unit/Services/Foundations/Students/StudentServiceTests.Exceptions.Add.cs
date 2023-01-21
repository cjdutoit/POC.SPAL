using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfStudentAlreadyExsitsAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student alreadyExistsStudent = randomStudent;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsStudentException =
                new AlreadyExistsStudentException(duplicateKeyException);

            var expectedStudentDependencyValidationException =
                new StudentDependencyValidationException(alreadyExistsStudentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(alreadyExistsStudent);

            // then
            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    addStudentTask.AsTask);

            actualStudentDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidStudentReferenceException =
                new InvalidStudentReferenceException(foreignKeyConstraintConflictException);

            var expectedStudentValidationException =
                new StudentDependencyValidationException(invalidStudentReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(someStudent);

            // then
            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    addStudentTask.AsTask);

            actualStudentDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(someStudent),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();

            var databaseUpdateException =
                new DbUpdateException();

            var failedStudentStorageException =
                new FailedStudentStorageException(databaseUpdateException);

            var expectedStudentDependencyException =
                new StudentDependencyException(failedStudentStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            var serviceException = new Exception();

            var failedStudentServiceException =
                new FailedStudentServiceException(serviceException);

            var expectedStudentServiceException =
                new StudentServiceException(failedStudentServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(someStudent);

            StudentServiceException actualStudentServiceException =
                await Assert.ThrowsAsync<StudentServiceException>(
                    addStudentTask.AsTask);

            // then
            actualStudentServiceException.Should()
                .BeEquivalentTo(expectedStudentServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}