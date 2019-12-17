using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Provider;
using ConsumerOne.Mobile.Services;
using Java.Util;
using Plugin.CurrentActivity;

namespace ConsumerOne.Mobile.Droid.Services
{
    public class ImageService : IImageService
    {
        public async Task SaveToGallery(string url)
        {
            var httpClient = new HttpClient();

            Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(url);

            // await! control returns to the caller and the task continues to run on another thread
            var contents = await contentsTask;

            var currentPicture = BitmapFactory.DecodeByteArray(contents, 0, contents.Length);

            MediaStore.Images.Media.InsertImage(CrossCurrentActivity.Current.Activity.ContentResolver, currentPicture, UUID.RandomUUID() + ".png",
                "Consumer One");

        }


    }
}
