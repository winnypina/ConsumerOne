using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using Foundation;
using UIKit;

namespace ConsumerOne.Mobile.iOS.Services
{
    public class ImageService : IImageService
    {
        public async Task SaveToGallery(string uri)
        {
            var httpClient = new HttpClient();

            Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(uri);

            // await! control returns to the caller and the task continues to run on another thread
            var contents = await contentsTask;
            var imageData = UIImage.LoadFromData(NSData.FromArray(contents));
            imageData.SaveToPhotosAlbum((retImage, error) =>
            {
                var o = retImage as UIImage;
                Console.WriteLine("error: " + error + "\nimage: " + o);
            });

        }
    }
}
