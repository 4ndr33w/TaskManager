using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;
using TaskManager.Api.Services;
using TaskManager.Models;
using TaskManager.Models.Enums;
using TaskManager.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgDbContext _dbContext;
        private readonly UsersService _usersService;
        private readonly AccountService _accountService;
        private readonly SecurityService _securityService;

        public AccountController(IConfiguration configuration, NpgDbContext dbContext, AccountService accountService, SecurityService securityService, UsersService usersService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _accountService = accountService;
            _securityService = securityService;
            _usersService = usersService;
        }

        [HttpPost("test")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            UserDto user = new UserDto
            {
                Name = "Andr33w",
                Description = "Test",

                LastName = "McFly",
                Email = "123",
                Password = "123",
                Phone = "123",
                UserStatus = UserStatus.Admin
            };

            var result = await _usersService.CreateAsync(user);
            return Ok(result);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            string login = HttpContext.User.Identity.Name;

            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == login);

            currentUser.Password = "********************";

            return Ok(currentUser.ToDto());
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken()
        {
            var userData = _accountService.GetLoginAndPassFromBasicAuth(Request);

            var login = userData.Item1;
            var password = userData.Item2;

            var identity = await _accountService.GetIdentity(login, password);
            if (identity != null)
            {
                var encodedJwt = _securityService.GetEncodedJwt(identity);

                var response = new
                {
                    accessToken = encodedJwt,
                    userName = identity.Name
                };
                return Ok(response);
            }
            return NotFound();
        }
    }
}
