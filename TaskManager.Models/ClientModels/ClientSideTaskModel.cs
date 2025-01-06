using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Extensions;

namespace TaskManager.Models.ClientModels
{
    public class ClientSideTaskModel : TaskModel
    {
        public ClientSideTaskModel(TaskModel task) : base(task.ToDto()) 
        {
            //Picture = this.LoadImage();
        }
        public Image Picture
        {
            get => this.LoadImage();

            
        }
    }
}
