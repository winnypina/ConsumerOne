using ConsumerOne.Api.Models;

namespace ConsumerOne.Api.ViewModels
{
    public class FacebookAuthViewModel
    {
        public string AccessToken { get; set; }
        public UserType UserType { get; set; }
    }
}