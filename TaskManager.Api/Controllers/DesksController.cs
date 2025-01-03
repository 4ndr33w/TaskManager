using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services;
using TaskManager.Models.Dtos;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesksController : ControllerBase
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly AccountService _accountService;
        private readonly ProjectsService _projectService;
        private readonly UsersService _userService;
        private readonly DesksService _desksService;

        public DesksController(NpgDbContext npgDbContext, IConfiguration configuration, AccountService accountService, ProjectsService projectService, UsersService userService, DesksService desksService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _accountService = accountService;
            _projectService = projectService;
            _userService = userService;
            _desksService = desksService;
        }

        [HttpPost]
        public async Task<IActionResult> GreateAsync([FromBody] DeskDto deskDto)
        {
            var login = HttpContext.User.Identity.Name;

            if (login != null)
            {
                var currentUser = await _userService.GetAsync(login);
                var currentProject = await _projectService.GetAsync(deskDto.ProjectId);

                bool isUserInProject = currentUser.ProjectsIds.Contains(deskDto.ProjectId) || currentProject.UsersIds.Contains(currentUser.Id);
                bool isUserAdminOrEditor = currentUser.UserStatus == Models.Enums.UserStatus.Admin || currentUser.UserStatus == Models.Enums.UserStatus.Editor;
                bool isUserProjectAdmin = currentUser.Id == currentProject.AdminId;

                if (currentUser != null && currentProject != null)
                {
                    if (isUserAdminOrEditor || isUserInProject || isUserProjectAdmin)
                    {
                        deskDto.ProjectId = currentProject.Id;
                        deskDto.AdminId = deskDto.AdminId == default? currentUser.Id : deskDto.AdminId;
                        var result = await _desksService.CreateAsync(deskDto);
                        return Ok(result);
                    }
                    return Forbid();
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpGet("{deskId}")]
        public async Task<IActionResult> GetDeskById(Guid deskId)
        {
            return Ok();
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectDesks(Guid projectId)
        {
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetUserDesks()
        {
            return Ok();
        }
        [HttpPatch("{deskId}")]
        public async Task<IActionResult> UpdateDeskById(Guid deskId)
        {
            return Ok();
        }
        [HttpDelete("{deskId}")]
        public async Task<IActionResult> DeleteDeskById(Guid deskId)
        {
            return Ok();
        }
    }
}
