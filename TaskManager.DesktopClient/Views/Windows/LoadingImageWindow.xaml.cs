using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using Microsoft.Win32.SafeHandles;

using TaskManager.DesktopClient.Resources;
using TaskManager.DesktopClient.Services.ViewServices;

using static System.Net.Mime.MediaTypeNames;

namespace TaskManager.DesktopClient.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoadingImageWindow.xaml
    /// </summary>
    public partial class LoadingImageWindow : Window
    {
        public LoadingImageWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _source = GetSource();
            loadingImageForm.Source = _source;

            ImageAnimator.Animate(_bitmap, OnFrameChanged);
        }
        private Bitmap _bitmap;

        private BitmapSource _source;

        private BitmapSource GetSource()
        {
            //TaskManager.DesktopClient.Resources
            var imageLoadService = new ImageLoadSaveService();
            if (_bitmap == null)
            {
                string gifPath = TextData.LoadingGifFilePath;
                _bitmap = new Bitmap(gifPath);
            }

            IntPtr handle = IntPtr.Zero;

            handle = _bitmap.GetHbitmap();

            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames();
            if (_source != null)
                _source.Freeze();
            _source = GetSource();
            loadingImageForm.Source = _source;
            InvalidateVisual();
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(FrameUpdatedCallback));
        }
    }
}
