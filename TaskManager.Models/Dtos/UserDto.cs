using System.ComponentModel.DataAnnotations;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Enums;

namespace TaskManager.Models.Dtos
{
    public class UserDto : ModelBase
    {
        public string? LastName { get; set; }

        [Required(ErrorMessage ="Email field is empty!")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Password field is empty!")]
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;
        public UserStatus UserStatus { get; set; } = UserStatus.User;

        public List<Guid>? ProjectsIds { get; set; }
        public List<Guid>? DesksIds { get; set; }
        public List<Guid>? TasksIds { get; set; }
        public UserDto() : base() { LastLoginDate = DateTime.UtcNow; }
        public UserDto(User user) : base(user)
        {
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            Phone = user.Phone;
            LastLoginDate = user.LastLoginDate;
            UserStatus = user.UserStatus;
        }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}
