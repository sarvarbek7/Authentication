using Xeptions;

namespace Authentication.Web.Api.Models.Users.Exceptions
{
    public class InvalidUserException : Xeption
    {
        public InvalidUserException()
            : base(message: "Invalid user. Please fix errors and try again")
        {}

        public InvalidUserException(string parameterName, object parameterValue)
            : base(message: $"Invalid user, " +
                  $"parameterName: {parameterName}, " +
                  $"parameterValue: {parameterValue}.")
        {}
    }
}
