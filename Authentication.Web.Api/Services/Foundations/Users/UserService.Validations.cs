// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;

namespace Authentication.Web.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private void ValidateUserOnRegister(User user)
        {
            ValidateUserNotNull(user);
            ValidateUserId(user.Id);
            ValidateUserFields(
                (Rule: IsInvalid(user.PhoneNumber), Parameter: nameof(User.PhoneNumber)),
                (Rule: IsInvalid(user.FirstName), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalid(user.LastName), Parameter: nameof(User.LastName)),
                (Rule: IsInvalid(user.Mosque), Parameter: nameof(User.Mosque)),
                (Rule: IsInvalid(user.Profession), Parameter: nameof(User.Profession)),
                (Rule: IsInvalid(user.CreatedDate), Parameter: nameof(User.CreatedDate)),
                (Rule: IsInvalid(user.UpdatedDate), Parameter: nameof(User.UpdatedDate))
                );
        }

        private void ValidateUserNotNull(User user)
        {
            if (user == null)
            {
                throw new NullUserException();
            }
        }

        private void ValidateUserFields(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserException = new InvalidUserException();
            foreach((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserException.AddData(
                        key: parameter,
                        values: rule.Message);
                }
            }

            invalidUserException.ThrowIfContainsErrors();
        }

        private void ValidateUserId(Guid userId)
        {
            if (userId == default)
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.Id),
                    parameterValue: userId);
            }
        }

        static private dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        static private dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };
    }
}
