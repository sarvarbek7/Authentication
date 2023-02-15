// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models.Users;
using FluentAssertions;
using Moq;
using Xunit;

namespace Authentication.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async void ShouldRegisterUserAsync()
        {
            // given
            DateTimeOffset randomDateTime = CreateRandomDateTime();
            DateTimeOffset dateTime= randomDateTime;
            string randomPassword = CreateRandomPassword();
            string password = randomPassword;
            User randomUser = CreateRandomUser(dates: dateTime);
            User inputUser = randomUser;
            User storageUser = randomUser;
            User expectedUser= storageUser;

            this.userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputUser, password))
                    .ReturnsAsync(storageUser);

            // when
            User actualUser = 
                await this.userService.RegisterUserAsync(inputUser, password);

            // then
            actualUser.Should().BeEquivalentTo(expectedUser);

            this.userManagementBrokerMock.Verify(broker => 
                broker.InsertUserAsync(inputUser, password), Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
