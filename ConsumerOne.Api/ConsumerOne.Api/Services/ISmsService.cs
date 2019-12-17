using System.Threading.Tasks;

namespace ConsumerOne.Api.Services
{
    public interface ISmsService
    {
        Task Send(string number, string message);
    }
}