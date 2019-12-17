using System.Threading.Tasks;

namespace ConsumerOne.Api.Services
{
    public interface IEmailSender
    {
        Task Send(string toAddress, string subject, string message);
    }
}