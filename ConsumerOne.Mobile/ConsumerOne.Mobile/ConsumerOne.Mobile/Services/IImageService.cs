using System;
using System.Threading.Tasks;

namespace ConsumerOne.Mobile.Services
{
    public interface IImageService
    {
        Task SaveToGallery(string url);
    }
}
