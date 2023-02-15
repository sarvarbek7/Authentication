// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace Authentication.Web.Api.Models.Users.Exceptions
{
    public class UserDependencyValidationException : Xeption
    {
        public UserDependencyValidationException(Xeption innerException)
            : base(
                  message: "User dependency validation error occured, please try again",
                  innerException)
        { }
    }
}
