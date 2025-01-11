using Microsoft.EntityFrameworkCore;

using System.Windows.Media.Imaging;

namespace TaskManager.Models.Abstractions
{
    [PrimaryKey("Id")]
    public abstract class ModelBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public byte[]? Image { get; set; }

        public ModelBase()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

        public ModelBase(ModelBase modelBase)
        {
            Id = modelBase.Id;
            Name = modelBase.Name;
            Description = modelBase.Description;
            Created = modelBase.Created;
            Updated = DateTime.UtcNow;
            Image = modelBase.Image;
        }
    }
}
