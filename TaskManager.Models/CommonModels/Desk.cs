using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Dtos;

namespace TaskManager.Models
{
    public class Desk : ModelBase
    {
        public bool IsPrivate { get; set; } = true;
        public string[]? Columns { get; set; }

        [ForeignKey("AdminId")]
        public Guid AdminId { get; set; }
        public User Admin { get; set; }

        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public List<TaskDto>? Tasks { get; set; }
        public string Color { get; set; }

        public Desk() { }

        public Desk(DeskDto desk) : base(desk)
        {
            IsPrivate = desk.IsPrivate;
            Columns = desk.Columns;
            AdminId = desk.AdminId;
            ProjectId = desk.ProjectId;
            Color = desk.Color;
        }

        public DeskDto ToDto()
        {
            return new DeskDto
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Created = this.Created,
                Updated = DateTime.UtcNow,
                Image = this.Image,

                IsPrivate = this.IsPrivate,
                AdminId = this.AdminId,
                TaskIds = this.Tasks != null ? this.Tasks.Select(t => t.Id).ToList() : new List<Guid>(),
                Columns = this.Columns,
                ProjectId = this.ProjectId,

                Color = this.Color,
            };
        }
    }
}
