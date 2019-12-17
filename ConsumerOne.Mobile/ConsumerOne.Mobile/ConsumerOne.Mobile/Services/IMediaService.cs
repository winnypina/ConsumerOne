using System.Threading.Tasks;

namespace ConsumerOne.Mobile.Services
{
    public interface IMediaService
    {
        Task Upload(byte[] data, string fileName);
    }
}