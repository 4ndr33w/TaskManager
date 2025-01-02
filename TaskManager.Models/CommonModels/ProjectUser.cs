using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models.CommonModels
{
    //[PrimaryKey("ProjectId, UserId")]
    public class ProjectUser
    {
        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public ProjectUser() { }

        public ProjectUser(Guid taskId, Guid deskId)
        {
            ProjectId = taskId;
            UserId = deskId;
        }
    }
}
