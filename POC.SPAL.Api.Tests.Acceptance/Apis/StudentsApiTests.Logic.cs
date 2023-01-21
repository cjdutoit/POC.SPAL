using System.Threading.Tasks;
using FluentAssertions;
using POC.SPAL.Api.Tests.Acceptance.Models.Students;
using Xunit;

namespace POC.SPAL.Api.Tests.Acceptance.Apis.Students
{
    public partial class StudentsApiTests
    {
        [Fact]
        public async Task ShouldPostStudentAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student expectedStudent = inputStudent;

            // when 
            await this.apiBroker.PostStudentAsync(inputStudent);

            Student actualStudent =
                await this.apiBroker.GetStudentByIdAsync(inputStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);
            await this.apiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }
    }
}