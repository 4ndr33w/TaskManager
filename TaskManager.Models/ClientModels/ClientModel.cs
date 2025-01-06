using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.Models.Abstractions;
using TaskManager.Models.Extensions;

namespace TaskManager.Models.ClientModels
{
    public class ClientModel<T> where T : ModelBase
    {
        public T Model { get; private set; }

        public ClientModel(T model) 
        { 
            Model = model; 
        }

        public Image Image 
        {
            get => Model?.LoadImage();
        }
    }
}
