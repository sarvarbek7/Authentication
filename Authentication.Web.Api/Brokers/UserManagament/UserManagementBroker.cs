// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

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
    }
}
