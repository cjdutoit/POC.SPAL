using System.Threading.Tasks;
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
        }

        private StudentValidationException CreateAndLogValidationException(Xeption exception)
        {
            var studentValidationException =
                new StudentValidationException(exception);

            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
    }
}