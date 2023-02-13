// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace Authentication.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = CreateRandomDateTime();
            User someUser = CreateRandomUser(dates: dateTime);
            SqlException sqlException = GetSqlException();
            var failedStorageException = new FailedStorageException(sqlException);
            
            var expectedUserDependencyException = 
                new UserDependencyException(failedStorageException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(someUser)).ThrowsAsync(sqlException);

            // when
            ValueTask<User> registerUserTask = 
                this.userService.RegisterUserAsync(someUser);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(someUser), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedUserDependencyException))), 
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowDependencyValidationExceptionOnRegisterWhenStudentAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset randomDate = CreateRandomDateTime();
            User randomUser = CreateRandomUser(dates: randomDate);
            User inputUser = randomUser;
            var duplicateKeyException = new DuplicateKeyException(message: GetRandomMessage());
            
            var alreadyExistsUserException = 
                new AlreadyExistsUserException(duplicateKeyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser)).ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<User> registerUserTask = this.userService.RegisterUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyValidationException))),
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
