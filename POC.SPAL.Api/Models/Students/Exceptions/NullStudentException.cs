using Xeptions;

namespace POC.SPAL.Api.Models.Students.Exceptions
{
    public class NullStudentException : Xeption
    {
        public NullStudentException()
            : base(message: "Student is null.")
        { }
    }
}