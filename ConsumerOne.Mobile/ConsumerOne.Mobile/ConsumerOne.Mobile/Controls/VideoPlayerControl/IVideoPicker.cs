using System.Threading.Tasks;

namespace ConsumerOne.Mobile.Controls.VideoPlayerControl
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}
