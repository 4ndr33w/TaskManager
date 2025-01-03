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
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _userService.GetAsync(login);
                if (currentUser != null)
                {
                    var currentDesk = await _desksService.GetAsync(deskId);
                    if (currentUser.DesksIds.Contains(currentDesk.Id))
                    {
                        return Ok(currentDesk);
                    }
                    return Forbid();
                }
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectDesks(Guid projectId)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _userService.GetAsync(login);
                if (currentUser != null)
                {
                    bool isUserInProject = currentUser.ProjectsIds.Contains(projectId);
                    if (isUserInProject)
                    {
                        var result = await _desksService.GetByProjectIdAsync(projectId, currentUser.Id);
                        return Ok(result);
                    }
                    return Forbid();
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetUserDesks()
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _userService.GetAsync(login);
                if (currentUser != null)
                {
                    return Ok(await _desksService.GetAllUserDesksAsync(currentUser.DesksIds));
                }
                return NotFound();
            }
            return BadRequest();
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
