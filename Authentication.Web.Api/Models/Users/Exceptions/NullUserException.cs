// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace Authentication.Web.Api.Models.Users.Exceptions
{
    public class NullUserException : Xeption
    {
        public NullUserException() : base(message: "The user is null.") { }
    }
}
