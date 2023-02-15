// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Web.Api.Brokers.DateTimeBroker;
using Authentication.Web.Api.Brokers.LoggingBroker;
using Authentication.Web.Api.Brokers.UserManagament;
using Authentication.Web.Api.Models.Users;

namespace Authentication.Web.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public UserService(
            IUserManagementBroker userManagementBroker, 
            ILoggingBroker loggingBroker, 
            IDateTimeBroker dateTimeBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<User> RegisterUserAsync(User user, string password) =>
            TryCatch(async () =>
            {
                ValidateUserOnRegister(user);
                
                return await this.userManagementBroker.InsertUserAsync(user, password);
            });

        public async ValueTask<User> RetrieveByNameAsync(string username)
        {
            User maybeUser = await this.userManagementBroker.SelectUserByNameAsync(username);

            return maybeUser;
        }

        public IQueryable<User> RetrieveAllUsers()
        {
            return this.userManagementBroker.SelectAllUsers();
        }

        public async ValueTask<User> RetrieveUserByIdAsync(Guid userId)
        {
            User maybeUser = await this.userManagementBroker.SelectUserByIdAsync(userId);

            return maybeUser;
        }
    }
}
