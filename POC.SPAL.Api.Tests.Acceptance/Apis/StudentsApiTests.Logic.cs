// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using POC.SPAL.Api.Tests.Acceptance.Models.Students;
using RESTFulSense.Exceptions;
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

        [Fact]
        public async Task ShouldGetAllStudentsAsync()
        {
            // given
            List<Student> randomStudents = await PostRandomStudentsAsync();
            List<Student> expectedStudents = randomStudents;

            // when
            List<Student> actualStudents = await this.apiBroker.GetAllStudentsAsync();

            // then
            foreach (Student expectedStudent in expectedStudents)
            {
                Student actualStudent = actualStudents.Single(approval => approval.Id == expectedStudent.Id);
                actualStudent.Should().BeEquivalentTo(expectedStudent);
                await this.apiBroker.DeleteStudentByIdAsync(actualStudent.Id);
            }
        }

        [Fact]
        public async Task ShouldGetStudentAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            Student expectedStudent = randomStudent;

            // when
            Student actualStudent = await this.apiBroker.GetStudentByIdAsync(randomStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);
            await this.apiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }

        [Fact]
        public async Task ShouldPutStudentAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            Student modifiedStudent = UpdateStudentWithRandomValues(randomStudent);

            // when
            await this.apiBroker.PutStudentAsync(modifiedStudent);
            Student actualStudent = await this.apiBroker.GetStudentByIdAsync(randomStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(modifiedStudent);
            await this.apiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }

        [Fact]
        public async Task ShouldDeleteStudentAsync()
        {
            // given
            Student randomStudent = await PostRandomStudentAsync();
            Student inputStudent = randomStudent;
            Student expectedStudent = inputStudent;

            // when
            Student deletedStudent =
                await this.apiBroker.DeleteStudentByIdAsync(inputStudent.Id);

            ValueTask<Student> getStudentbyIdTask =
                this.apiBroker.GetStudentByIdAsync(inputStudent.Id);

            // then
            deletedStudent.Should().BeEquivalentTo(expectedStudent);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getStudentbyIdTask.AsTask());
        }
    }
}