using System.Threading.Tasks;

using Foundation;
using UIKit;

namespace FiscalCode.iOS
{
    internal class PhotoLibrary
    {
#pragma warning disable IDE0060, IDE0067
        public Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            var nsData = NSData.FromArray(data);
            var image = new UIImage(nsData);
            var taskCompletionSource = new TaskCompletionSource<bool>();

            image.SaveToPhotosAlbum((UIImage img, NSError error) =>
            {
                taskCompletionSource.SetResult(error == null);
            });

            return taskCompletionSource.Task;
        }
    }
#pragma warning restore IDE0060, IDE0067
}