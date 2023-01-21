// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRemoveStudentByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputStudentId = randomId;
            Student randomStudent = CreateRandomStudent();
            Student storageStudent = randomStudent;
            Student expectedInputStudent = storageStudent;
            Student deletedStudent = expectedInputStudent;
            Student expectedStudent = deletedStudent.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ReturnsAsync(storageStudent);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentAsync(expectedInputStudent))
                    .ReturnsAsync(deletedStudent);

            // when
            Student actualStudent = await this.studentService
                .RemoveStudentByIdAsync(inputStudentId);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(expectedInputStudent),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}