using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;
using TaskManager.Api.Services;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Models.Dtos;
using TaskManager.Models;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly NpgDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UsersService _usersService;
        private readonly AccountService _accountService;
        private readonly SecurityService _securityService;

        public UsersController(NpgDbContext dbContext, IConfiguration configuration, UsersService userService, AccountService accountService, SecurityService securityService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _usersService = userService;
            _accountService = accountService;
            _securityService = securityService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            bool isNewUserDataIsEmpty = user == null ? true : false;

            if (!isNewUserDataIsEmpty)
            {
                var isThaiEmailBusy = await _accountService.IsCurrentEmailAlreadyUsedByOtherUser(user.Email);

                if (!isThaiEmailBusy)
                {
                    var result = await _usersService.CreateAsync(user);
                    return Ok(result);
                }
                else
                {
                    return Conflict("Change E-mail. Current e-mail already in use");
                }
            }
            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            bool isDataForUpdateEmpty = user == null ? true : false;
            var login = HttpContext.User.Identity.Name;
            user.Email = login;
            var updateResult = await _usersService.UpdateAsync(user);
            return Ok(updateResult);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            string login = HttpContext.User.Identity.Name;

            var result = await _usersService.DeleteAsync(login);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var login = HttpContext.User.Identity.Name;

            if (login != null)
            {
                var userDto = await _usersService.GetAsync(login);
                if (userDto != null)
                {
                    return Ok(userDto);
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPatch("update/mail")]
        public async Task<IActionResult> ChangeUserEmail([FromBody]object newEmail)
        {
            if (newEmail != null || newEmail.ToString() != string.Empty)
            {
                string newMail = newEmail
                    .ToString()
                    .Split(":")[1]
                    .ReplaceLineEndings()
                    .Replace("\"", "")
                    .Replace("}", "")
                    .Replace("\n", "")
                    .Replace("\r", "");

                if (newMail[0] == ' ')
                {
                    newMail = newMail.Substring(1);
                }

                var login = HttpContext.User.Identity.Name;
                var currentUser = await _usersService.GetAsync(login);

                var isThaiEmailBusy = await _accountService.IsCurrentEmailAlreadyUsedByOtherUser(newMail);

                if (currentUser != null)
                {
                    if (!isThaiEmailBusy && currentUser != null)
                    {
                        var result = await _usersService.ChangeEmailAsync(login, newMail);
                        return Ok(result);
                    }
                    return Conflict("Change E-mail. Current e-mail already in use");
                }
                return NotFound();
            }
           return NoContent();
        }
        [HttpPatch("update/pass")]
        public async Task<IActionResult> ChangeUserPassword([FromBody]object newPass)
        {
            if (newPass != null || newPass.ToString() != string.Empty)
            {
                string newPassword = newPass.ToString()
                    .Split(":")[1]
                    .ReplaceLineEndings()
                    .Replace("\"", "")
                    .Replace("}", "")
                    .Replace("\n", "")
                    .Replace("\r", "");

                if (newPassword[0] == ' ')
                {
                    newPassword = newPassword.Substring(1);
                }
                var login = HttpContext.User.Identity.Name;

                var result = await _usersService.ChangePasswordAsync(login, newPassword);

                var currentUser = await _usersService.GetAsync(login);
                if (currentUser != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
