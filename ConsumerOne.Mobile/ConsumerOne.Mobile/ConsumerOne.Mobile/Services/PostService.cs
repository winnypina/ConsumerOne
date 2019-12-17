using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.ViewModels.Models;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace ConsumerOne.Mobile.Services
{
    public class PostService : IPostService
    {
        private readonly IApiService _apiService;

        public PostService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> Publish(PostRequest model)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.PublishPost(model).ConfigureAwait(false);
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

        public async Task<bool> Like(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.LikePost(postId).ConfigureAwait(false);
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

        public async Task<bool> CreateComment(CommentRequest request)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.CreateComment(request).ConfigureAwait(false);
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

        public async Task<bool> DeleteComment(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.DeleteComment(postId).ConfigureAwait(false);
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

        public async Task<IEnumerable<PostRequest>> DoSearch(PostFilterModel request, int page)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.DoSearch(request,page).ConfigureAwait(false);
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
                return posts;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<CommentRequest>> GetComments(Guid postId)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetComments(postId).ConfigureAwait(false);
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
                return posts;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<PostRequest>> GetPosts(int page)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetPostsPaged(page).ConfigureAwait(false);
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
                return posts;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<IEnumerable<PostRequest>> GetPostsForUser(string userId, int page)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetPostsForUserPaged(userId,page).ConfigureAwait(false);
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
                return posts;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.DeletePost(id).ConfigureAwait(false);
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

        public async Task<bool> PauseUnpausePost(Guid id)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            var signupTask = _apiService.UserInitiated.PauseUnpausePost(id).ConfigureAwait(false);
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