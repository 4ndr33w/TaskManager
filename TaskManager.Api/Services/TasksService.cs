
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.Interfaces;
using TaskManager.Models;
using TaskManager.Models.Dtos;

namespace TaskManager.Api.Services
{
    public class TasksService : IBaseService<TaskDto>
    {
        private readonly AccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly UsersService _usersService;
        private readonly NpgDbContext _npgDbContext;

        public TasksService(AccountService accountService, IConfiguration configuration, UsersService usersService, NpgDbContext npgDbContext)
        {
            _accountService = accountService;
            _configuration = configuration;
            _usersService = usersService;
            _npgDbContext = npgDbContext;
        }

        public async Task<bool> CreateAsync(TaskDto taskDto)
        {
            try
            {
                var newTask = new TaskModel(taskDto);

                _npgDbContext.Tasks.Add(newTask);
                var result = await _npgDbContext.SaveChangesAsync();

                if (result == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid taskId)
        {
            try
            {
                var task = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
                _npgDbContext.Tasks.Remove(task);
                var result = await _npgDbContext.SaveChangesAsync();

                if(result == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public async Task<TaskDto> GetAsync(Guid taskId)
        {
            try
            {
                var task = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
                return task.ToDto();
            }
            catch (Exception)
            {
            }
            return null;
        }
        public async Task<IEnumerable<TaskDto>> GetAsync(UserDto user)
        {
            var tasksCollection = new List<TaskDto>();

            foreach (var taskId in user.TasksIds)
            {
                var task = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
                tasksCollection.Add(task.ToDto());
            }

            return tasksCollection;
        }
        public async Task<IEnumerable<TaskDto>> GetAsync(DeskDto deskDto)
        {
            var tasksCollection = new List<TaskDto>();

            foreach (var taskId in deskDto.TaskIds)
            {
                var task = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
                tasksCollection.Add(task.ToDto());
            }
            return tasksCollection;
        }
        public async Task<IEnumerable<TaskDto>> GetAsync(ProjectDto projectDto)
        {
            var deskCollection = new List<DeskDto>();
            var tasksCollection = new List<TaskDto>();

            var tasksIdsCollection = new List<Guid>();

            foreach (var deskId in projectDto.DesksIds)
            {
                var desk = await _npgDbContext.Desks.FirstOrDefaultAsync(d => d.Id == deskId);

                tasksIdsCollection.AddRange(desk.ToDto().TaskIds);
            }
            foreach (var taskId in tasksIdsCollection)
            {
                var task = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
                tasksCollection.Add(task.ToDto());
            }
            return tasksCollection;
        }

        public async Task<bool> UpdateAsync(TaskDto taskDto)
        {
            try
            {
                var existingTask = await _npgDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskDto.Id);
                bool isTaskExecutorDefaultOrNotChanged = (taskDto.ExecutorId == default || taskDto.ExecutorId == existingTask.ExecutorId);

                existingTask.Name = taskDto.Name == null? existingTask.Name : taskDto.Name;
                existingTask.Description = taskDto.Description == null? existingTask.Description : taskDto.Description;
                existingTask.Updated = DateTime.UtcNow;
                existingTask.Image = taskDto.Image == null? existingTask.Image : taskDto.Image;

                existingTask.StartDate = (taskDto.StartDate != default && taskDto.StartDate != existingTask.StartDate) ? taskDto.StartDate : existingTask.StartDate;
                existingTask.EndDate = (taskDto.EndDate != default && taskDto.EndDate != existingTask.EndDate) ? taskDto.EndDate : existingTask.EndDate;

                existingTask.File = taskDto.File == null? existingTask.File : taskDto.File;

                if (taskDto.File != null)
                {
                    if (taskDto.FileName == null || taskDto.FileName == string.Empty)
                    {
                        existingTask.FileName = (existingTask.FileName == null || existingTask.FileName == string.Empty) ? "defaultFileName.txt" : existingTask.FileName;
                    }
                    if (taskDto.FileName != null || taskDto.FileName != string.Empty)
                    {
                        existingTask.FileName = taskDto.FileName;
                    }
                }

                if(taskDto.DeskId != default && taskDto.DeskId != existingTask.DeskId)
                {
                    existingTask.DeskId = taskDto.DeskId;
                }
                
                existingTask.Column = taskDto.Column == null? existingTask.Column : taskDto.Column;

                if(taskDto.ExecutorId != default && taskDto.ExecutorId != existingTask.ExecutorId)
                {
                    existingTask.ExecutorId = taskDto.ExecutorId;
                }

                existingTask.Color = (taskDto.Color == null || taskDto.Color == string.Empty) ? existingTask.Color : taskDto.Color;
                existingTask.Priority = taskDto.Priority != existingTask.Priority? taskDto.Priority : existingTask.Priority;

                _npgDbContext.Tasks.Update(existingTask);
                var result = await _npgDbContext.SaveChangesAsync();

                if (result == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
