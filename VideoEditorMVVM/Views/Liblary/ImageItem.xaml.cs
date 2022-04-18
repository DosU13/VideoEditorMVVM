using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace VideoEditorMVVM.Views.Liblary
{
    public sealed partial class ImageItem : UserControl
    {
        public string MediaPath
        {
            get { return (String)GetValue(MediaPathProperty); }
            set { SetValue(MediaPathProperty, value); UpdateImageSource(); }
        }

        public static readonly DependencyProperty MediaPathProperty =
            DependencyProperty.Register("MediaPath", typeof(string), typeof(ImageItem), null);

        public ImageItem()
        {
            this.InitializeComponent();
        }

        private bool hovered = false;
        public bool Hovered
        {
            get { return hovered; }
            set
            {
                hovered = value;
                Bindings.Update();
            }
        }

        private bool isVideo = false;
        public bool IsVideo
        {
            get { return isVideo; }
            set
            {
                isVideo = value;
                Bindings.Update();
            }
        }

        private ImageSource imageSource = null;
        private ImageSource ImageSource {
            get {
                return imageSource ?? new BitmapImage(ResourceManager.Current.MainResourceMap[@"Files/Assets/StoreLogo.png"].Uri);
            } 
        }

        private async void ImageChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mkv");
            picker.FileTypeFilter.Add(".mpeg");

            var file = await picker.PickSingleFileAsync();
            MediaPath = file?.Path;
            Bindings.Update();
        }

        private async void UpdateImageSource()
        {
            if (MediaPath != null && MediaPath != "")
            {
                try
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(MediaPath);
                    if (file == null) return;
                    if (IsVideo) IsVideo = false;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.DecodePixelHeight = 224;
                    bitmap.DecodePixelType = DecodePixelType.Logical;
                    string t = file.FileType;
                    if (t == ".jpg" || t == ".jpeg" || t == ".png")
                    {
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            await bitmap.SetSourceAsync(fileStream);
                        }
                    }else if(t == ".mp4" || t == ".mkv" || t == "..mpeg")
                    {
                        IsVideo = true;
                        await bitmap.SetSourceAsync(await file.GetThumbnailAsync(ThumbnailMode.SingleItem));
                    }
                    else
                    {
                        bitmap = null;
                        MainPage.Status = "file type " + t + " is not recognized";
                    }
                    imageSource = bitmap;
                    Bindings.Update();
                }
                catch (Exception) { }
            }
        }

        private void Image_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Hovered = true;
        }

        private void Image_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Hovered = false;
        }
    }
}
