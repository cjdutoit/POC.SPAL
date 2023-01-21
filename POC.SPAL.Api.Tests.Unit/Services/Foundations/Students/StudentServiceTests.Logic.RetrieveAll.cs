// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using POC.SPAL.Api.Models.Students;
using Xunit;

namespace POC.SPAL.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public void ShouldReturnStudents()
        {
            // given
            IQueryable<Student> randomStudents = CreateRandomStudents();
            IQueryable<Student> storageStudents = randomStudents;
            IQueryable<Student> expectedStudents = storageStudents;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Returns(storageStudents);

            // when
            IQueryable<Student> actualStudents =
                this.studentService.RetrieveAllStudents();

            // then
            actualStudents.Should().BeEquivalentTo(expectedStudents);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}