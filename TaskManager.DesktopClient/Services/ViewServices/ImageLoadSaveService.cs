using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using Prism.Mvvm;

namespace TaskManager.DesktopClient.Services.ViewServices
{
    public class ImageLoadSaveService
    {
        public ImageLoadSaveService() { }

        public byte[] EncodingImage(string imagePath)
        {
            string outputFilePath = "";
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                byte[] imageBytes = ImageToByteArray(image);

                return imageBytes;
            }
        }

        public byte[] GetByteArrayFromBitmapSource(BitmapSource bitmapSource)
        {
            BitmapImage bitmapImage = new BitmapImage();

            byte[] data = null;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public async Task<byte[]> SearchImageFile()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var result = dialog.ShowDialog();
                dialog.Filter = "Изображения (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|Все файлы (*.*)|*.*";
                dialog.Title = "Выбор изображения";

                if (result.Value == true)
                {
                    var byteResult = EncodingImage(dialog.FileName);
                    return byteResult;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        //public Bitmap ConvertBitmapFromBitmapImage(BitmapImage bitmapImage)
        //{
        //    using (MemoryStream mStream = new MemoryStream())
        //    {
        //        BitmapEncoder encoder = new GifBitmapEncoder();
        //        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        //        encoder.Save(mStream);

        //        System.Drawing.Bitmap bitmap = new Bitmap(mStream);

        //        return new Bitmap(bitmap);
        //    }
        //}

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                image.Save(mStream, image.RawFormat);

                return mStream.ToArray();
            }
        }

        //public byte[] GetByteArrayFromBitmapImage(BitmapSource bitmapImage)
        //{
        //    byte[] data = null;
        //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        encoder.Save(ms);
        //        data = ms.ToArray();
        //    }
        //    return data;
        //}

        //public BitmapSource GetBitmapSource(byte[] imageBytes)
        //{
        //    try
        //    {
        //        using (MemoryStream mStream = new MemoryStream(imageBytes))
        //        {
        //            System.Drawing.Image imageFromBytes = System.Drawing.Image.FromStream(mStream);

        //            mStream.Position = 0;
        //            BitmapDecoder decoder = BitmapDecoder.Create(
        //                mStream,
        //                BitmapCreateOptions.PreservePixelFormat,
        //                BitmapCacheOption.OnLoad);

        //            return decoder.Frames[0];
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

    }
}
