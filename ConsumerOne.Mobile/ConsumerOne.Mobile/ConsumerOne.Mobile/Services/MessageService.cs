using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConsumerOne.Mobile.ViewModels.Models;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace ConsumerOne.Mobile.Services
{
    public class MessageService : IMessageService
    {
        private readonly IApiService apiService;

        public MessageService(IApiService apiService)
        {
            this.apiService = apiService;
        }

        public async Task<IEnumerable<MessageModel>> GetMessages()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = apiService.UserInitiated.GetMessages().ConfigureAwait(false);
            try
            {
                var posts = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return posts.ToList().Select(n=> new MessageModel
                {
                    Id = n.Id,
                    ToId = n.ToId,
                    FromId = n.FromId,
                    ToName = n.ToName,
                    FromName = n.FromName,
                    SentDate = n.SentDate,
                    Message = n.Message
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> SendMessage(MessageModel message)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = apiService.UserInitiated.SendMessage(message).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                return true;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
