using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class User : ModelBase
    {
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;
        public UserStatus UserStatus { get; set; } = UserStatus.User;

        public List<Project>? Projects { get; set; }
        public List<Desk>? Desks { get; set; }
        public List<TaskDto>? Tasks { get; set; }
        public User() { }

        public User(UserDto user) : base(user)
        {
            LastName = user.Name;
            Email = user.Email;
            Password = user.Password;
            Phone = user.Phone;
            LastLoginDate = user.LastLoginDate;
            UserStatus = user.UserStatus;
        }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Created = this.Created,
                Updated = this.Updated,
                Image = this.Image,

                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                LastLoginDate = this.LastLoginDate,
                UserStatus = this.UserStatus,

                ProjectsIds = Projects != null ? this.Projects.Select(p => p.Id).ToList() : new List<Guid>(),
                DesksIds = Desks != null ? this.Desks.Select(p => p.Id).ToList() : new List<Guid>(),
                TasksIds = Tasks != null ? this.Tasks.Select(p => p.Id).ToList() : new List<Guid>(),
            };
        }
    }
}
