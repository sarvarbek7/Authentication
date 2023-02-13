// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace Authentication.Web.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunction();
        private delegate IQueryable<User> ReturningQueryableUserFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageException = 
                    new FailedStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
        }

        private UserValidationException CreateAndLogValidationException(Xeption innerException)
        {
            var userValidationException = new UserValidationException(innerException);
            this.loggingBroker.LogError(userValidationException);

            return userValidationException;
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption innerException)
        {
            var userDependencyException = new UserDependencyException(innerException);
            this.loggingBroker.LogCritical(userDependencyException);

            return userDependencyException;
        }
    }
}
