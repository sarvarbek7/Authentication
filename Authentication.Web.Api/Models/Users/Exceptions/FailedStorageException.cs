// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace Authentication.Web.Api.Models.Users.Exceptions
{
    public class FailedStorageException : Xeption
    {
        public FailedStorageException(Exception innerException)
            : base(message: "Failed Storage error occured, please contact support",
                  innerException)
        { }
    }
}
