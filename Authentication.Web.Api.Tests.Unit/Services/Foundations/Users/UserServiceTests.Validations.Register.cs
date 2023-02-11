// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using Moq;
using Xunit;

namespace Authentication.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRegisterAndLogItAsync()
        {
            // given
            User invalidUser = null;

            var nullUserException = new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(nullUserException);

            // when
            ValueTask<User> createUserTask =
                this.userService.RegisterUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                createUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = CreateRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            User randomUser = CreateRandomUser(dates: dateTime);
            User inputUser = randomUser;
            inputUser.Id = default;

            var invalidUserInputException = new InvalidUserException(
                parameterName: nameof(User.Id),
                parameterValue: inputUser.Id);

            var expectedUserValidationException =
                new UserValidationException(invalidUserInputException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(inputUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async void ShouldThrowValidationExceptionOnRegisterIfUserIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidUser = new User
            {
                FirstName = invalidText,
                LastName = invalidText,
            };

            var invalidUserException = 
                new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "ID is required");

            invalidUserException.AddData(
                key: nameof(User.PhoneNumber),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.FirstName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.LastName),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.Mosque),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.Profession),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.Mosque),
                values: "Text is required");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: "Date is required");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Date is required");

            var expectedUserValidationException =
                new UserValidationException(invalidUserException);

            // when
            ValueTask<User> registerUserTask =
                this.userService.RegisterUserAsync(invalidUser);

            // then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                registerUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
                Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
