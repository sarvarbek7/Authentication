// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            string randomPassword = CreateRandomPassword();
            string password = randomPassword;
            User someUser = CreateRandomUser(dates: dateTime);
            SqlException sqlException = GetSqlException();
            var failedStorageException = new FailedUserStorageException(sqlException);

            var expectedUserDependencyException =
                new UserDependencyException(failedStorageException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(someUser, password)).ThrowsAsync(sqlException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(someUser, password);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(someUser, password), Times.Once);

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
            string randomPassword = CreateRandomPassword();
            string password = randomPassword;
            User inputUser = randomUser;
            var duplicateKeyException = new DuplicateKeyException(message: GetRandomMessage());

            var alreadyExistsUserException =
                new AlreadyExistsUserException(duplicateKeyException);

            var expectedUserDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password)).ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<User> registerUserTask = this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, password), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyValidationException))),
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowDependenyExceptionOnRegisterIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDate = CreateRandomDateTime();
            User randomUser = CreateRandomUser(dates: randomDate);
            string randomPassword = CreateRandomPassword();
            string password = randomPassword;
            User inputUser = randomUser;
            DbUpdateException dbUpdateException = new DbUpdateException();
            var failedStorageException = new FailedUserStorageException(dbUpdateException);
            var expectedUserDependencyException = new UserDependencyException(failedStorageException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password)).ThrowsAsync(dbUpdateException);

            // when
            ValueTask<User> registerUserTask = this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, password), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyException))),
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowServiceExceptionOnRegisterIfErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDate = CreateRandomDateTime();
            User randomUser = CreateRandomUser(dates: randomDate);
            string randomPassword = CreateRandomPassword();
            string password = randomPassword;
            User inputUser = randomUser;
            var exception = new Exception();
            var failedUserServiceException = new FailedUserServiceException(exception);
            
            var expectedUserServiceException = 
                new UserServiceException(failedUserServiceException);

            userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password)).ThrowsAsync(exception);

            // when
            ValueTask<User> registerUserTask = 
                this.userService.RegisterUserAsync(inputUser, password);

            // then
            await Assert.ThrowsAsync<UserServiceException>(() =>
                registerUserTask.AsTask());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputUser, password), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
