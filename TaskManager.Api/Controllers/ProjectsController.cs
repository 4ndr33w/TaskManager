using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

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

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto project)
        {
            if (project != null)
            {
                bool result = false;
                string login = HttpContext.User.Identity.Name;
                var currentUser = await _accountService.GetUser(login);
                var userToBeProjectAdmin = new UserDto();

                if (project.AdminId == default || project.AdminId == null)
                {
                    project.AdminId = currentUser.Id;
                }

                if (currentUser != null)
                {
                    if (currentUser.UserStatus == UserStatus.Admin || currentUser.UserStatus == UserStatus.Editor)
                    {
                        result = await _projectService.CreateAsync(project);
                    }
                    return result ? Ok(result) : Forbid();
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _accountService.GetUser(login);
                ProjectDto projectDto = await _projectService.GetAsync(projectId);

                bool isUserAdminOrEditor = currentUser.UserStatus == UserStatus.Admin || currentUser.UserStatus == UserStatus.Editor;
                bool isUserProjectAdmin = currentUser.Id == projectDto.AdminId;
                bool isUserInProjectUsersCollection = await _projectService.IsUserInProjectUsersOfProject(currentUser.Id, projectId);

                if (isUserAdminOrEditor || isUserProjectAdmin || isUserInProjectUsersCollection)
                {
                    return Ok(projectDto);
                }
                return Forbid();
            }
            return Unauthorized();
        }

        [HttpPatch("update/{projectId}/admin/{adminId}")]
        public async Task<IActionResult> ChangeProjectAdmin(Guid projectId, Guid adminId)
        {
            bool result = false;
            var login = HttpContext.User.Identity.Name;

            if (projectId != default && adminId != default && login != null)
            {
                var currentUser = await _userService.GetAsync(login);
                var currentProject = await _projectService.GetAsync(projectId);

                if (currentUser != null && currentProject != null)
                {
                    bool isUserAdminOrEditor = currentUser.UserStatus == UserStatus.Admin || currentUser.UserStatus == UserStatus.Editor;
                    bool isUserProjectAdmin = currentUser.Id == currentProject.AdminId;

                    if (isUserAdminOrEditor || isUserProjectAdmin)
                    {
                        result = await _projectService.ChangeProjectAdmin(projectId, adminId);
                        return Ok(result);
                    }
                    return Forbid();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            bool result = false;

            var login = HttpContext.User.Identity.Name;

            var currentUser = await _accountService.GetUser(login);
            ProjectDto projectDto = await _projectService.GetAsync(projectId);

            if (projectDto != null)
            {
                bool isUserAdminOrEditor = currentUser.UserStatus == UserStatus.Admin || currentUser.UserStatus == UserStatus.Editor;
                bool isUserProjectAdmin = currentUser.Id == projectDto.AdminId;

                if (isUserAdminOrEditor || isUserProjectAdmin)
                {
                    result = await _projectService.DeleteAsync(projectId);
                    return Ok(result);
                }
                else return Forbid();
            }
            else return NotFound();
        }

        [HttpPatch("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectDto projectDto)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _accountService.GetUser(login);
                var existingProject = await _projectService.GetAsync(projectId);

                if (existingProject != null && currentUser != null)
                {
                    projectDto.Id = projectId;

                    bool isUserAdminOrEditor = currentUser.UserStatus == UserStatus.Editor || currentUser.UserStatus == UserStatus.Admin;
                    bool isUserProjectAdmin = existingProject.AdminId == currentUser.Id;

                    if (isUserAdminOrEditor || isUserProjectAdmin)
                    {
                        var result = await _projectService.UpdateAsync(projectDto);
                        return Ok(result);
                    }
                    return Forbid();
                }
                return NotFound();

            }
            return BadRequest();
        }

        [HttpPatch("{projectId}/users/add")]
        public async Task<IActionResult> AddUsersToProject(Guid projectId, [FromBody] List<Guid> usersIds)
        {
            bool result = false;
            if (usersIds != null)
            {
                string login = HttpContext.User.Identity.Name;

                var existingProject = await _projectService?.GetAsync(projectId);
                if (existingProject != null)
                {
                    var currentUser = await _accountService.GetUser(login);

                    bool isCurrentUserAdminOrEditor = currentUser.UserStatus == UserStatus.Admin || currentUser.UserStatus == UserStatus.Editor;
                    bool isCurrentUserProjectAdmin = existingProject.AdminId == currentUser.Id;

                    if (isCurrentUserAdminOrEditor || isCurrentUserProjectAdmin)
                    {
                        result = await _projectService.AddUsersToProject(projectId, usersIds);
                        return result ? Ok(result) : BadRequest();
                    }
                    return Forbid();
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
