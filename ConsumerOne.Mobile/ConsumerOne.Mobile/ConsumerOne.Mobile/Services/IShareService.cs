using System.Threading.Tasks;
using ConsumerOne.Mobile.ViewModels.Models;

namespace ConsumerOne.Mobile.Services
{
    public interface IShareService
    {
        Task ShareQRCode(string message, string text);
        Task SharePost(PostListModel post);
    }
}