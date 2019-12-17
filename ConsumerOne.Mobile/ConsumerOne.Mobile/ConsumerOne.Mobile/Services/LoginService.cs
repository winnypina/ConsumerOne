using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.Services.Responses;
using ConsumerOne.Mobile.ViewModels;
using Plugin.Connectivity;
using Polly;
using Refit;

namespace ConsumerOne.Mobile.Services
{
    public class LoginService : ILoginService
    {
        private readonly IApiService _apiService;

        public LoginService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public string AccessToken { get; set; }

        public AccountResponse Account { get; set; }

        public async Task<bool> ResetPassword(string newPassword)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.ResetPassword(new PasswordResetRequest { NewPassword = newPassword }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> ResetPassword(string user,string newPassword)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.NewPassword(new PasswordResetRequest { NewPassword = newPassword }).ConfigureAwait(false);

            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> Login(string username, string password)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.Login(new AuthTokenRequest { Password = password, UserName = username}).ConfigureAwait(false);
            try
            {
                var loginResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                if (loginResponse.AccessToken == null) return false;
                AccessToken = loginResponse.AccessToken;
                var cache = BlobCache.UserAccount;
                await cache.InsertObject("token", AccessToken);
                var accountTask = _apiService.UserInitiated.GetAccount().ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);
                Account = accountResponse;
                
                await cache.InsertObject("account", Account);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> Follow(string id)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.Follow(id).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<IEnumerable<RatingResponse>> GetRating()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetRatings().ConfigureAwait(false);
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

        public async Task<IEnumerable<AccountResponse>> GetFollows()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetFollows().ConfigureAwait(false);
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

        public async Task<IEnumerable<AccountResponse>> GetFollowers()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetFollowers().ConfigureAwait(false);
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

        public async Task<IEnumerable<RatingResponse>> GetRatingForUser(string id)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;
            var signupTask = _apiService.UserInitiated.GetRatingsForUser(id).ConfigureAwait(false);
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

        public async Task<bool> PostRating(RatingResponse request)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.PostRating(request).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<AccountResponse> GetProfile(string id)
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            try
            {
                var accountTask = _apiService.UserInitiated.GetProfile(id).ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);

                return accountResponse;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;

        }

        public async Task<bool> LoginWithFacebook(string accessToken, UserType type)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var loginTask = _apiService.UserInitiated.LoginWithFacebook(new FacebookAuthTokenRequest { AccessToken = accessToken, UserType = type}).ConfigureAwait(false);
            try
            {
                var loginResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await loginTask);
                if (loginResponse.AccessToken == null) return false;
                AccessToken = loginResponse.AccessToken;
                var cache = BlobCache.UserAccount;
                await cache.InsertObject("token", AccessToken);
                var accountTask = _apiService.UserInitiated.GetAccount().ConfigureAwait(false);
                var accountResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await accountTask);
                Account = accountResponse;
                
                await cache.InsertObject("account", Account);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;

        }

        public async Task<bool> UpdateAccount(AccountResponse updatedAccount)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;
            
            var signupTask = _apiService.UserInitiated.UpdateAccount(updatedAccount).ConfigureAwait(false);
            try
            {
                var account = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                Account = account;
                var cache = BlobCache.UserAccount;
                await cache.InsertObject("account", Account);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> LoadCachedUser()
        {
            var cache = BlobCache.UserAccount;

            var keys = (await cache.GetAllKeys()).ToList();

            if (keys.Contains("account") && keys.Contains("token"))
            {
                AccessToken = await cache.GetObject<string>("token");
                Account = await cache.GetObject<AccountResponse>("account");
                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            Account = null;
            AccessToken = null;
            var cache = BlobCache.UserAccount;
            await cache.InvalidateAll();
        }



        public async Task<bool> Signup(string name, string login, string password, string picture, string type)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var emailLogin = login.Contains("@") ? login : "no@email.com";
            var phoneLogin = !login.Contains("@") ? login : null;

            var signupTask = _apiService.UserInitiated.CreateAccount(new CreateAccountRequest {CpfCnpj = null,Type=type, Picture = picture, Name = name, Password = password, Email = emailLogin, BirthDate = null,Address = null,MobilePhone = phoneLogin}).ConfigureAwait(false);
            try
            {
                var account = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await signupTask);
                Account = account;
                var cache = BlobCache.UserAccount;
                await cache.InsertObject("account", Account);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> VerifyCode(string login,int code)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.VerifyCode(new CodeVerificationRequest() { UserName = login, Code = code}).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> ResendCode(string login)
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.ResendCode(new CodeVerificationRequest() { UserName = login }).ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<string> GetUsageTerms()
        {
            if (!CrossConnectivity.Current.IsConnected) return null;

            var usTermTask = _apiService.UserInitiated.GetUseTerms().ConfigureAwait(false);
            try
            {
                var useTermResponse = await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await usTermTask);
                return useTermResponse.Text;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public async Task<bool> Delete()
        {
            if (!CrossConnectivity.Current.IsConnected) return false;

            var verifyTask = _apiService.UserInitiated.DeleteAccount().ConfigureAwait(false);
            try
            {
                await Policy
                    .Handle<WebException>()
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await verifyTask);
                return true;
            }
            catch (Refit.ApiException e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}