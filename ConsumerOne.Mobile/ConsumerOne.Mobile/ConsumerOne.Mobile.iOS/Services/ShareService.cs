using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using Foundation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.iOS.Services
{
    public class ShareService : IShareService
    {
        public async Task SharePost(PostListModel post)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(post.Image.ToString().Replace("Uri: ", ""));
            var response = await client.GetAsync("");
            var data = await response.Content.ReadAsByteArrayAsync();
            var path = Path.Combine(FileSystem.CacheDirectory, "share.jpg");
            File.WriteAllBytes(path, data);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Olha o que eu encontrei no Consumer One",
                File = new ShareFile(path)
            });
        }

        public async Task ShareQRCode(string message, string text)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(text);
            var response = await client.GetAsync("");
            var data = await response.Content.ReadAsByteArrayAsync();
            var path = Path.Combine(FileSystem.CacheDirectory, "share.jpg");
            File.WriteAllBytes(path, data);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Me acompanhe no Consumer One",
                File = new ShareFile(path)
            });
        }
    }
}
