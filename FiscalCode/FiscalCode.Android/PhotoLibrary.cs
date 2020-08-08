using System.Threading.Tasks;

using Android.Media;
using Android.OS;
using Android.Util;
using FiscalCode.Utilities;
using Java.IO;
using Java.Lang;

[assembly: Xamarin.Forms.Dependency(typeof(FiscalCode.Droid.PhotoLibrary))]
namespace FiscalCode.Droid
{
    internal class PhotoLibrary : IPhotoLibrary
    {
        [System.Obsolete]
        public async Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            try
            {
                var picturesDirectory = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures);
                var folderDirectory = picturesDirectory;

                if (!string.IsNullOrEmpty(folder))
                {
                    folderDirectory = new File(picturesDirectory, folder);
                    folderDirectory.Mkdirs();
                }

                using var bitmapFile = new File(folderDirectory, filename);
                bitmapFile.CreateNewFile();

                using (var outputStream = new FileOutputStream(bitmapFile))
                {
                    await outputStream.WriteAsync(data);
                }

                MediaScannerConnection.ScanFile(MainActivity.Instance,
                                                new string[] { bitmapFile.Path },
                                                new string[] { "image/png", "image/jpeg" }, null);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, nameof(PhotoLibrary), ex.Message);
                return false;
            }

            return true;
        }
    }
}