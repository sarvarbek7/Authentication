// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Web.Api.Models.Users;
using Authentication.Web.Api.Models.Users.Exceptions;
using Authentication.Web.Api.Services.Foundations.Users;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Authentication.Web.Api.Controllers
{
    [Route("/api")]
    public class UsersController : RESTFulController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService) =>
            this.userService = userService;

        [HttpPost("register")]
        public async ValueTask<ActionResult> RegisterUser([FromBody] User user, string password = "123456")
        {
            try
            {
                User postedUser =
                    await this.userService.RegisterUserAsync(user, password);

                return Created(postedUser);
            }
            catch (UserValidationException userValidationException)
                when (userValidationException.InnerException is AlreadyExistsUserException)
            {
                return Conflict(userValidationException.InnerException.Message);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException.InnerException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpGet("getUser{username}")]
        public async ValueTask<ActionResult<User>> GetUserByNameAsync(string username)
        {
            try
            {
                User storageUser = await this.userService.RetrieveByNameAsync(username);

                return Ok(storageUser);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpGet("users")]
        public ActionResult<IQueryable<User>> GetAllUsers()
        {
            try
            {
                IQueryable storageUsers =
                    this.userService.RetrieveAllUsers();

                return Ok(storageUsers);
            }
            catch (UserDependencyException userDependencyException)
            {
                return Problem(userDependencyException.Message);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }

        [HttpGet("{userId}")]
        public async ValueTask<ActionResult<User>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                User storageUser =
                    await this.userService.RetrieveUserByIdAsync(userId);

                return Ok(storageUser);
            }
            catch (UserServiceException userServiceException)
            {
                return Problem(userServiceException.Message);
            }
        }
    }
}
