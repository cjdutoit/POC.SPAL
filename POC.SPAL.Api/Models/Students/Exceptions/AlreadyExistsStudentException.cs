using System;
using Xeptions;

namespace POC.SPAL.Api.Models.Students.Exceptions
{
    public class AlreadyExistsStudentException : Xeption
    {
        public AlreadyExistsStudentException(Exception innerException)
            : base(message: "Student with the same Id already exists.", innerException)
        { }
    }
}