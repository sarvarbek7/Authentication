﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Brokers.DateTimeBroker;
using Authentication.Web.Api.Brokers.LoggingBroker;
using Authentication.Web.Api.Brokers.UserManagament;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Services.Foundations.Users;
using Moq;
using Tynamix.ObjectFiller;

namespace Authentication.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IUserManagementBroker> userManagementBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.userManagementBrokerMock = new Mock<IUserManagementBroker>();
            this.loggingBrokerMock= new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock= new Mock<IDateTimeBroker>();

            this.userService = new UserService(
                userManagementBroker: this.userManagementBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static User CreateRandomUser(DateTimeOffset dates)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                PhoneNumber = CreateRandomPhoneNumber(),
                FirstName = new Filler<RealNames>().Create().ToString(),
                LastName = new Filler<RealNames>().Create().ToString(),
                CreatedDate = dates,
                UpdatedDate = dates
            };

            return user;
        }

        private static DateTimeOffset CreateRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string CreateRandomPhoneNumber()
        {
            var random = new Random();
            string code = "+9989";
            int number = random.Next(0, 100_000_000);
            number.ToString("D9");
            string phonenumber = code + number;

            return phonenumber;
        }
    }
}