using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;
using TaskManager.Api.Services;
using TaskManager.Models.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly NpgDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UsersService _usersService;
        private readonly AccountService _accountService;
        private readonly TasksService _tasksService;
        private readonly DesksService _desksService;

        public TasksController(NpgDbContext dbContext, IConfiguration configuration, UsersService userService, AccountService accountService, TasksService tasksService, DesksService desksService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _usersService = userService;
            _accountService = accountService;
            _tasksService = tasksService;
            _desksService = desksService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _accountService.GetUser(login);
                if (currentUser != null)
                {
                    taskDto.CreatorId = currentUser.Id;
                    taskDto.ExecutorId = taskDto.ExecutorId == default ? currentUser.Id : taskDto.ExecutorId;
                    var result = await _tasksService.CreateAsync(taskDto);

                    return Ok(result);
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpGet("desk/{deskId}")]
        public async Task<IActionResult> GetTasksCollectionByDeskId(Guid deskId)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _usersService.GetAsync(login);
                if (currentUser != null && currentUser.DesksIds.Any(id => id == deskId))
                {
                    var desk = await _desksService.GetAsync(deskId);
                    return Ok(await _tasksService.GetAsync(desk));
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasksCollection()
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _usersService.GetAsync(login);
                if (currentUser != null)
                {
                    return Ok(await _tasksService.GetAsync(currentUser));
                }
                return NotFound();
            }

            return Unauthorized();
        }
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _usersService.GetAsync(login);
                if (currentUser != null && currentUser.TasksIds.Any(id => id == taskId))
                {
                    var currentTask = await _tasksService.GetAsync(taskId);
                    bool isCurrentUserDeskAdmin = await _desksService.IsUserDeskAdmin(currentUser.Id, currentTask.DeskId);
                    bool isCurrentUserTaskCreator = currentTask.CreatorId == currentUser.Id;
                    bool isCurrentUserTaskExecutor = currentTask.ExecutorId == currentUser.Id;

                    if (isCurrentUserDeskAdmin || isCurrentUserTaskCreator || isCurrentUserTaskExecutor)
                    {
                        var result = await _tasksService.DeleteAsync(taskId);
                        return Ok(result);
                    }
                    return Unauthorized();
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpPatch("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] TaskDto taskDto)
        {
            var login = HttpContext.User.Identity.Name;
            if (login != null)
            {
                var currentUser = await _usersService.GetAsync(login);
                if (currentUser != null && currentUser.TasksIds.Any(t => t == taskId))
                {
                    taskDto.Id = taskId;

                    var result = await _tasksService.UpdateAsync(taskDto);
                    return Ok(result);
                }
                return BadRequest();
            }
            return Unauthorized();
        }
    }
}
