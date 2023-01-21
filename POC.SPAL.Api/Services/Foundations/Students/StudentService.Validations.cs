using POC.SPAL.Api.Models.Students;
using POC.SPAL.Api.Models.Students.Exceptions;

namespace POC.SPAL.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        private void ValidateStudentOnAdd(Student student)
        {
            ValidateStudentIsNotNull(student);
        }

        private static void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }
    }
}