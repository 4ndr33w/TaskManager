
using Microsoft.EntityFrameworkCore;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.Interfaces;
using TaskManager.Models;
using TaskManager.Models.Dtos;

namespace TaskManager.Api.Services
{
    public class ProjectsService : IBaseService<ProjectDto>
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly AccountService _accountService;
        private readonly UsersService _usersService;

        public ProjectsService(NpgDbContext npgDbContext, IConfiguration configuration, AccountService accountService, UsersService usersService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _accountService = accountService;
            _usersService = usersService;
        }

        public async Task<bool> CreateAsync(ProjectDto projectDto)
        {
            try
            {
                Project project = new Project(projectDto);
                _npgDbContext.Add(project);
                await _npgDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid projectId)
        {
            try
            {
                var project = await _npgDbContext.Projects
                    //.Include(P => P.ProjectAdmin)
                    .Include(p => p.Desks)
                    .Include(p => p.Users)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                _npgDbContext.Projects.Remove(project);
                var result = await _npgDbContext.SaveChangesAsync();
              
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProjectDto> GetAsync(Guid projectId)
        {
            try
            {
                Project project = await _npgDbContext.Projects
                    //.Include(P => P.ProjectAdmin)
                    .Include(p => p.Desks)
                    .Include(p => p.Users)
                    .FirstOrDefaultAsync(p => p.Id == projectId);
                if (project != null)
                {
                    return project.ToDto();
                }
                else return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(ProjectDto projectDto)
        {
            try
            {
                Project existingProject = await _npgDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectDto.Id);

                bool isProjectAdminNull = (projectDto.AdminId == null || projectDto.AdminId == default);
                bool isProjectAdminIsChanged = projectDto.AdminId != existingProject.AdminId;

                existingProject.Name = projectDto.Name == null ? existingProject.Name : projectDto.Name;
                existingProject.Description = projectDto.Description == null ? existingProject.Description : projectDto.Description;
                existingProject.Image = projectDto.Image == null ? existingProject.Image : projectDto.Image;
                existingProject.ProjectStatus = projectDto.ProjectStatus == null ? existingProject.ProjectStatus : projectDto.ProjectStatus;
                existingProject.Updated = DateTime.UtcNow;

                _npgDbContext.Projects.Update(existingProject);
                await _npgDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangeProjectAdmin(Guid projectId, Guid newProjectAdminId)
        {
            try
            {
                var project = await _npgDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                project.AdminId = newProjectAdminId;

                _npgDbContext.Projects.Update(project);
                await _npgDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsUserInProjectUsersOfProject(Guid userId, Guid projectId)
        {
            ProjectDto projectDto = await GetAsync(projectId);
            if (projectDto.UsersIds.Any(u => u == userId))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddUsersToProject(ProjectDto projectDto, IEnumerable<Guid> userIdsCollection)
        {
            try
            {
                List<Guid> usersIdsList = new List<Guid>();
                Project project =  await _npgDbContext.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectDto.Id);

                foreach (var userId in userIdsCollection.ToList())
                {
                    var user = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    bool isUserProjectAdmin = project.AdminId == userId;
                    //bool isUserIdsColectionInProjectContainsCurrentUserId = project.UsersIds.Contains(user.Id);

                    if (user != null &&( !isUserProjectAdmin/* && !isUserIdsColectionInProjectContainsCurrentUserId */))
                    {
                        project.Users.Add(user);
                        usersIdsList.Add(userId);
                    }
                }
                //project.UsersIds.AddRange(usersIdsList);
                _npgDbContext.Update(project);
                await _npgDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> RemoveUsersFromProject(Guid projectId, IEnumerable<Guid> userIdsCollection)
        {
            try
            {
                foreach (var userId in userIdsCollection)
                {
                    var user = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    Project project = await _npgDbContext.Projects.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == projectId);

                    bool isUserColectionInProjectContainsCurrentUser = project.Users.Contains(user);

                    if (isUserColectionInProjectContainsCurrentUser)
                    {
                        project.Users.Remove(user);
                    }
                    else return false;
                }
                await _npgDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
