// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Authentication.Web.Api.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Web.Api.Brokers.UserManagament
{
    public class UserManagementBroker : IUserManagementBroker
    {
        private readonly UserManager<User> userManager;

        public UserManagementBroker(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async ValueTask<User> InsertUserAsync(User user, string password)
        {
            var broker = new UserManagementBroker(this.userManager);
            await broker.userManager.CreateAsync(user, password);

            return user;
        }
    }
}
