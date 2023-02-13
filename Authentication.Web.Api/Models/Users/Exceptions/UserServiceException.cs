// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace Authentication.Web.Api.Models.Users.Exceptions
{
    public class UserServiceException : Xeption
    {
        public UserServiceException(Xeption innerException) 
            : base(message: "User service error occured, please contact support",
                  innerException) { }
    }
}
