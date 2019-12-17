using System;
using System.Net.Http;
using Fusillade;
using Refit;
using ZonaAzul.Mobile.Core;

namespace ConsumerOne.Mobile.Services
{
    public class ApiService : IApiService
    {
        //public const string ApiBaseAddress = "http://192.168.0.13:5000/api";
        public const string ApiBaseAddress = "https://admin.consumer.one/api";
        public const string MediaBaseAddress = "https://s3.amazonaws.com/consumerone/";

        private readonly Lazy<IConsumerOneService> _background;
        private readonly Lazy<IConsumerOneService> _speculative;
        private readonly Lazy<IConsumerOneService> _userInitiated;
        
        public ApiService(string apiBaseAddress = null)
        {
            IConsumerOneService CreateClient(HttpMessageHandler messageHandler)
            {
                var client = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(apiBaseAddress ?? ApiBaseAddress)
                };


                return RestService.For<IConsumerOneService>(client);
            }

            _background = new Lazy<IConsumerOneService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.Background)));

            _userInitiated = new Lazy<IConsumerOneService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.UserInitiated)));

            _speculative = new Lazy<IConsumerOneService>(() => CreateClient(
                new AuthenticatedHttpClientHandler(new HttpClientHandler(), Priority.Speculative)));
        }

        public IConsumerOneService Background => _background.Value;

        public IConsumerOneService UserInitiated => _userInitiated.Value;

        public IConsumerOneService Speculative => _speculative.Value;
        
    }
}