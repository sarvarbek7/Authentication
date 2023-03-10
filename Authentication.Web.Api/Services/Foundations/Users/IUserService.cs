// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Authentication.Web.Api.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Web.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> RegisterUserAsync(User user, string password);
        ValueTask<User> RetrieveByNameAsync(string username);
        IQueryable<User> RetrieveAllUsers();
        //IQueryable<User> RetrieveAllUsers();
        ValueTask<User> RetrieveUserByIdAsync(Guid userId);
        //ValueTask<User> ModifyUserAsync(User user);
        //ValueTask<User> RemoveUserByIdAsync(Guid userId);
    }
}
