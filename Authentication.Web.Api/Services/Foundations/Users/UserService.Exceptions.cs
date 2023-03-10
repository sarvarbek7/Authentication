// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                    new FailedUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException = 
                    new AlreadyExistsUserException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStorageException = 
                    new FailedUserStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException = 
                    new FailedUserServiceException(exception);

                throw CreateAndLogServiceException(failedUserServiceException);
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

        private UserDependencyException CreateAndLogDependencyException(Xeption innerException)
        {
            var userDependencyException = new UserDependencyException(innerException);
            this.loggingBroker.LogError(userDependencyException);

            return userDependencyException;
        }

        private UserDependencyValidationException CreateAndLogDependencyValidationException(Xeption innerException)
        {
            var userDependencyValidationException = new UserDependencyValidationException(innerException);
            this.loggingBroker.LogError(userDependencyValidationException);

            return userDependencyValidationException;
        }

        private UserServiceException CreateAndLogServiceException(Xeption innerException)
        {
            var userServiceException = new UserServiceException(innerException);
            this.loggingBroker.LogError(userServiceException);

            return userServiceException;
        }
    }
}
