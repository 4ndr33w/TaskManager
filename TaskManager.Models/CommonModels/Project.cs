using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Dtos;
using TaskManager.Models.Enums;

namespace TaskManager.Models
{
    public class Project : ModelBase
    {
        [ForeignKey("AdminId")]
        public Guid? AdminId { get; set; }
        public User Admin { get; set; }
        public ProjectStatus ProjectStatus { get; set; } = ProjectStatus.InProgress;

        public List<User>? Users { get; set; }
        public List<Desk>? Desks { get; set; }

        public Project() { }
        public Project(ProjectDto project) : base(project)
        {
            ProjectStatus = project.ProjectStatus;
            AdminId = project.AdminId;
        }

        public ProjectDto ToDto()
        {
            return new ProjectDto
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Created = this.Created,
                Updated = this.Updated,
                Image = this.Image,

                ProjectStatus = this.ProjectStatus,
                AdminId = this.AdminId,
                UsersIds = Users != null ? this.Users.Select(u => u.Id).ToList() : new List<Guid>(),
                DesksIds = Desks != null ? this.Desks.Select(d => d.Id).ToList() : new List<Guid>(),
            };
        }
    }
}
