using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Enums;

namespace TaskManager.Models.Dtos
{
    public class TaskDto : ModelBase
    {
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public byte[]? File { get; set; }
        public string? Column { get; set; }
        public Guid DeskId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid ExecutorId { get; set; }
        public Priority Priority { get; set; }
        public string? Color { get; set; }
        public string? FileName { get; set; }

        public TaskDto() { }
        public TaskDto(TaskDto taskDto) : base(taskDto)
        {
            StartDate = taskDto.StartDate;
            EndDate = taskDto.EndDate;
            File = taskDto.File;
            DeskId = taskDto.DeskId;
            Column = taskDto.Column;
            CreatorId = taskDto.CreatorId;
            ExecutorId = taskDto.ExecutorId;
            Priority = taskDto.Priority;
            Color = taskDto.Color;
            FileName = taskDto.FileName;
        }
    }
}
