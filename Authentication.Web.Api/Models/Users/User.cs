// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Web.Api.Models.Users
{
    public class User : IdentityUser<Guid>
    {
        public override Guid Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public override string PhoneNumber 
        { 
            get => base.PhoneNumber; 
            set => base.PhoneNumber = value; 
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public string Mosque { get; set; }
        public string Profession { get; set; }
        public string ProfilePictureUrl { get; set; }
        public UserStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
