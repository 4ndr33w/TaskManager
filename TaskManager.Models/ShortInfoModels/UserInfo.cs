using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.Models.ShortInfoModels
{
    public class UserInfo : ModelBase
    {
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;
        public UserStatus UserStatus { get; set; } = UserStatus.User;

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
        public UserInfo() : base()
        {

        }
        public UserInfo(UserDto userDto) : base(userDto)
        {
            LastName = userDto.LastName;
            Email = userDto.Email;
            Phone = userDto.Phone;
            LastLoginDate = userDto.LastLoginDate;
            UserStatus = userDto.UserStatus;
        }
    }
}
