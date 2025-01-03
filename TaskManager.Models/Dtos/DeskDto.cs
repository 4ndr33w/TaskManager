using TaskManager.Models.Abstractions;

namespace TaskManager.Models.Dtos
{
    public class DeskDto : ModelBase
    {
        public bool IsPrivate { get; set; } = true;
        public string[]? Columns { get; set; }
        public Guid AdminId { get; set; }
        public Guid ProjectId { get; set; }
        public List<Guid>? TaskIds { get; set; } = new List<Guid>();
        public string Color { get; set; } = "Green";

        public DeskDto() : base() { }
        public DeskDto(DeskDto desk) : base(desk)
        {
            IsPrivate = desk.IsPrivate;
            Columns = desk.Columns;
            AdminId = desk.AdminId;
            ProjectId = desk.ProjectId;
            Color = desk.Color;
        }
    }
}
