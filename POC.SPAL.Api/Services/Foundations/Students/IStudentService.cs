using System.Threading.Tasks;
using POC.SPAL.Api.Models.Students;

namespace POC.SPAL.Api.Services.Foundations.Students
{
    public interface IStudentService
    {
        ValueTask<Student> AddStudentAsync(Student student);
    }
}