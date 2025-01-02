using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly AccountService _accountService;
        private readonly ProjectsService _projectService;
        private readonly UsersService _userService;

        public ProjectsController(NpgDbContext npgDbContext, IConfiguration configuration, AccountService accountService, ProjectsService projectService, UsersService userService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _accountService = accountService;
            _projectService = projectService;
            _userService = userService;
        }
    }
}
