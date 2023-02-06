// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Authentication.Web.Api.Models.Users;

namespace Authentication.Web.Api.Brokers.UserManagament
{
    public interface IUserManagementBroker
    {
        ValueTask<User> InsertUserAsync(User user);
        // IQueryable<User> SelectAllUsers();
        // ValueTask<User> SelectUserByIdAsync(Guid userId);
        // ValueTask<User> UpdateUserAsync(User user);
        // ValueTask<User> DeleteUserAsync(User user);
    }
}