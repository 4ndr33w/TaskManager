using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

using TaskManager.Models.Abstractions;

namespace TaskManager.Models.Extensions
{
    public static class ModelBaseExtensions
    {
        public static System.Drawing.Image LoadImage(this ModelBase model)
        {
            if(model?.Image == null || model.Image.Length == 0)
            {
                return null;
            }

            //using (var mStream = new MemoryStream(model.Image))
            //{
            //    System.Drawing.Image image = System.Drawing.Image.FromStream(mStream);
            //    return image = image == null? new System.Drawing.Image() : image;
            //}
            return null;
        }

        public static byte[] SaveImage(this Image image)
        {
            if (image == null)
            {
                return null;
            }

            using (var mStream = new MemoryStream())
            {
                image.Save(mStream, image.RawFormat);
                return mStream.ToArray();
            }
        }
    }
}
