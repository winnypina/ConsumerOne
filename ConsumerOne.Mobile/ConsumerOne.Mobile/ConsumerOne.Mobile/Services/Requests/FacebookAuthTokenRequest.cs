using ConsumerOne.Mobile.Services.Responses;

namespace ConsumerOne.Mobile.Services.Requests
{
    public class FacebookAuthTokenRequest
    {
        public string AccessToken { get; set; }
        public UserType UserType { get; internal set; }
    }
}