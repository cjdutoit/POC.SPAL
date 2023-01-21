using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using POC.SPAL.Api.Models.Students;
using POC.SPAL.Api.Models.Students.Exceptions;
using Xeptions;

namespace POC.SPAL.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private delegate ValueTask<Student> ReturningStudentFunction();

        private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (NullStudentException nullStudentException)
            {
                throw CreateAndLogValidationException(nullStudentException);
            }
            catch (InvalidStudentException invalidStudentException)
            {
                throw CreateAndLogValidationException(invalidStudentException);
            }
            catch (SqlException sqlException)
            {
                var failedStudentStorageException =
                    new FailedStudentStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStudentStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentException =
                    new AlreadyExistsStudentException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsStudentException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidStudentReferenceException =
                    new InvalidStudentReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidStudentReferenceException);
            }
        }

        private StudentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException =
                new StudentValidationException(exception);

            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }

        private StudentDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var studentDependencyException = new StudentDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private StudentDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var studentDependencyValidationException =
                new StudentDependencyValidationException(exception);

            this.loggingBroker.LogError(studentDependencyValidationException);

            return studentDependencyValidationException;
        }
    }
}