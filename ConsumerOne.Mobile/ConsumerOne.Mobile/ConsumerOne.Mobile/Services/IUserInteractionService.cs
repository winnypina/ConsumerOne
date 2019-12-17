using System;
using System.Threading.Tasks;

namespace ConsumerOne.Mobile.Services
{
    public interface IUserInteractionService
    {
        Task DisplayMessage(string title, string message);
        Task DisplayConfirmation(string title, string message, string accept, string cancel, Action<bool> resultAction);
    }
}
