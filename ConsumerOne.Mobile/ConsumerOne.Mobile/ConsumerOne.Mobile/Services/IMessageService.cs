using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerOne.Mobile.ViewModels.Models;

namespace ConsumerOne.Mobile.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageModel>> GetMessages();
        Task<bool> SendMessage(MessageModel message);
    }
}
