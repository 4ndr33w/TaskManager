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
    public class TaskModel : ModelBase
    {
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public byte[]? File { get; set; }
        public string? Column { get; set; }
        [ForeignKey("DeskId")]
        public Guid DeskId { get; set; }
        public Desk? Desk { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        public User? Creator { get; set; }
        [ForeignKey("ExecutorId")]
        public Guid? ExecutorId { get; set; }
        public User? Executor { get; set; }
        public Priority Priority { get; set; }
        public string Color { get; set; }

        public TaskModel() { }
        public TaskModel(TaskDto task) : base(task)
        {
            this.StartDate = task.StartDate;
            this.EndDate = task.EndDate;
            this.File = task.File;
            this.Column = task.Column;
            this.DeskId = task.DeskId;
            this.CreatorId = task.CreatorId;
            this.ExecutorId = task.ExecutorId;
            this.Priority = task.Priority;
            this.Color = task.Color;
        }

        public TaskDto ToDto()
        {
            return new TaskDto
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Created = this.Created,
                Updated = this.Updated,
                Image = this.Image,

                StartDate = this.StartDate,
                EndDate = this.EndDate,
                File = this.File,
                DeskId = this.DeskId,
                Column = this.Column,
                CreatorId = this.CreatorId,
                ExecutorId = this.ExecutorId,

                Priority = this.Priority,
                Color = this.Color,
            };
        }
    }
}
