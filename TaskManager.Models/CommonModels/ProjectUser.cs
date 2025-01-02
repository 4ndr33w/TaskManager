using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models.CommonModels
{
    [PrimaryKey("Id")]
    public class ProjectUser
    {
        public Guid Id { get; set; }

        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }

        public ProjectUser() { }

        public ProjectUser(Guid taskId, Guid deskId)
        {
            ProjectId = taskId;
            UserId = deskId;
        }
    }
}
