using TaskManager.Models.Abstractions;
using TaskManager.Models.Enums;

namespace TaskManager.Models.Dtos
{
   public class ProjectDto : ModelBase
    {
        public ProjectStatus ProjectStatus { get; set; } = ProjectStatus.InProgress;
        public Guid? AdminId { get; set; }
        public List<Guid>? UsersIds { get; set; } = new List<Guid>();
        public List<Guid>? DesksIds { get; set; } = new List<Guid>();

        public ProjectDto() { }
        public ProjectDto(ProjectDto projectModel) : base(projectModel)
        {
            ProjectStatus = projectModel.ProjectStatus;
            AdminId = projectModel.AdminId;
        }
    }
}
