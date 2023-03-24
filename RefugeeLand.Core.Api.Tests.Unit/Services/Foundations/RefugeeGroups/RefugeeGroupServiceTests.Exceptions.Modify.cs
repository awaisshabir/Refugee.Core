﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using Xunit;

namespace RefugeeLand.Core.Api.Tests.Unit.Services.Foundations.RefugeeGroups
{
    public partial class RefugeeGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            RefugeeGroup randomRefugeeGroup = CreateRandomRefugeeGroup();
            SqlException sqlException = GetSqlException();
    
            var failedRefugeeGroupStorageException =
                new FailedRefugeeGroupStorageException(sqlException);
    
            var expectedRefugeeGroupDependencyException =
                new RefugeeGroupDependencyException(failedRefugeeGroupStorageException);
    
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);
    
            // when
            ValueTask<RefugeeGroup> modifyRefugeeGroupTask =
                this.refugeeGroupService.ModifyRefugeeGroupAsync(randomRefugeeGroup);
    
            RefugeeGroupDependencyException actualRefugeeGroupDependencyException =
                await Assert.ThrowsAsync<RefugeeGroupDependencyException>(
                    modifyRefugeeGroupTask.AsTask);
    
            // then
            actualRefugeeGroupDependencyException.Should()
                .BeEquivalentTo(expectedRefugeeGroupDependencyException);
    
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);
    
            this.storageBrokerMock.Verify(broker =>
                broker.SelectRefugeeGroupByIdAsync(randomRefugeeGroup.Id),
                    Times.Never);
    
            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedRefugeeGroupDependencyException))),
                        Times.Once);
    
            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRefugeeGroupAsync(randomRefugeeGroup),
                    Times.Never);
    
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}