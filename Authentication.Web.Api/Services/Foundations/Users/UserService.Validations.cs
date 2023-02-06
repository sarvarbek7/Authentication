// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;

namespace Authentication.Web.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private void ValidateUserOnRegister(User user)
        {
            ValidateUserNotNull(user);
        }
        private void ValidateUserNotNull(User user)
        {
            if (user == null)
            {
                throw new NullUserException();
            }
        }
    }
}
