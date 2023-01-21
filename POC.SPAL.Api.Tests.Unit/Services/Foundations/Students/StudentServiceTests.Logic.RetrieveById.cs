// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using POC.SPAL.Api.Models.Students;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveStudentByIdAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student storageStudent = randomStudent;
            Student expectedStudent = storageStudent.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudent.Id))
                    .ReturnsAsync(storageStudent);

            // when
            Student actualStudent =
                await this.studentService.RetrieveStudentByIdAsync(inputStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudent.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}