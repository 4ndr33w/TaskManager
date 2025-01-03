using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;
using TaskManager.Models.Dtos;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManager.Api.Services.Interfaces;

namespace TaskManager.Api.Services
{
    public class UsersService : IBaseService<UserDto>
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly SecurityService _securityService;
        private readonly AccountService _accountService;

        public UsersService(NpgDbContext npgDbContext, IConfiguration configuration, SecurityService securityService, AccountService accountService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _securityService = securityService;
            _accountService = accountService;
        }

        public async Task<bool> CreateAsync(UserDto user)
        {
            try
            {
                string paswordHash = _securityService.CreatePasswordHash(user.Password);

                User newUser = new User
                {
                    Name = user.Name,
                    Description = user.Description,
                    Image = user.Image,

                    LastName = user.LastName,
                    Email = user.Email,
                    Password = paswordHash,
                    Phone = user.Phone,
                    UserStatus = user.UserStatus
                };
                _npgDbContext.Users.Add(newUser);
                await _npgDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                User user = await _npgDbContext.Users
                     .Include(u => u.Projects)
                     .Include(u => u.Desks)
                     .Include(u => u.Tasks)
                     .FirstOrDefaultAsync(x => x.Id == id);
                if (user != null || user != default)
                {
                    var deletingResult = _npgDbContext.Users.Remove(user);
                    await _npgDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string login)
        {
            try
            {
                User user = await _npgDbContext.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Desks)
                    .Include(u => u.Tasks)
                    .FirstOrDefaultAsync(u => u.Email == login);//_accountService.GetUser(login);

                if (user != null || user != default)
                {
                    var deletingResult = _npgDbContext.Users.Remove(user);
                    await _npgDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserDto> GetAsync(Guid userId)
        {
            try
            {
                User user = await _npgDbContext.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Desks)
                    .Include(u => u.Tasks)
                    .FirstOrDefaultAsync(x => x.Id == userId);

                user.Id = userId;

                UserDto userDto = user.ToDto();
                return userDto;
            }
            catch (Exception)
            {
                return new UserDto();
            }
        }

        public async Task<UserDto> GetAsync(string login)
        {
            try
            {
                User user = await _npgDbContext.Users
                    .Include(u => u.Projects)
                    .Include(u => u.Desks)
                    .Include(u => u.Tasks)
                    .FirstOrDefaultAsync(x => x.Email == login);

                IEnumerable<TaskDto> tasksCollection = new List<TaskDto>();

                var desksCollection = new List<Desk>();
                foreach (var project in user.Projects.ToList())
                {
                    var projectAdmins = await _npgDbContext.Projects.FirstOrDefaultAsync(p => p.AdminId == user.Id);

                    var currentProjectDesks = await _npgDbContext.Desks
                        .Where(d => (d.ProjectId == project.Id && d.AdminId != user.Id && d.IsPrivate != true))
                        .ToListAsync();
                    desksCollection.AddRange(currentProjectDesks);

                    if (!user.Projects.Contains(projectAdmins))
                    {
                        user.Projects.Add(projectAdmins);
                    }
                }

                foreach (var desk in desksCollection)
                {
                    if (!user.Desks.Contains(desk))
                    {
                        user.Desks.Add(desk);
                    }
                }

                foreach (var desk in user.Desks)
                {
                    var currentDeskTasks = await _npgDbContext.Tasks.Where(t => t.DeskId == desk.Id).ToListAsync();
                    user.Tasks.AddRange(currentDeskTasks);
                }

                return user.ToDto();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(UserDto dataToUpdate)
        {
            try
            {
                var existingUser = await _accountService.GetUser(dataToUpdate.Email);

                if (existingUser != null && dataToUpdate != null)
                {
                    string paswordHash = (dataToUpdate.Password == null || dataToUpdate.Password == string.Empty) ? existingUser.Password : _securityService.CreatePasswordHash(dataToUpdate.Password);

                    existingUser.Name = dataToUpdate.Name == null ? existingUser.Name : dataToUpdate.Name;
                    existingUser.Phone = dataToUpdate.Phone == null ? existingUser.Phone : dataToUpdate.Phone;
                    existingUser.LastName = dataToUpdate.LastName == null ? existingUser.LastName : dataToUpdate.LastName;
                    existingUser.Description = dataToUpdate.Description == null ? existingUser.Description : dataToUpdate.Description;
                    existingUser.Image = dataToUpdate.Image == null ? existingUser.Image : dataToUpdate.Image;

                    existingUser.Updated = DateTime.UtcNow;
                    existingUser.LastLoginDate = DateTime.UtcNow;

                    _npgDbContext.Users.Update(existingUser);

                    await _npgDbContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string login, string newPassword)
        {
            try
            {
                var currentUser = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Email == login);
                if (currentUser != null)
                {
                    string paswordHash = _securityService.CreatePasswordHash(newPassword);
                    currentUser.Password = paswordHash;
                    _npgDbContext.Users.Update(currentUser);
                    await _npgDbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public async Task<bool> ChangeEmailAsync(string currentEmail, string newEmail)
        {
            try
            {
                var currentUser = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Email == currentEmail);
                if (currentUser != null)
                {
                    currentUser.Email = newEmail;
                    _npgDbContext.Users.Update(currentUser);
                    await _npgDbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
