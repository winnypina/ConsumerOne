using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services.Responses;
using ConsumerOne.Mobile.ViewModels;

namespace ConsumerOne.Mobile.Services
{
    public interface ILoginService
    {

        Task<AccountResponse> GetProfile(string id);
        string AccessToken { get; set; }
        AccountResponse Account { get; set; }
        Task<bool> Login(string username, string password);
        Task<bool> ResetPassword(string newPassword);
        Task<bool> ResetPassword(string user,string newPassword);
        Task<bool> Follow(string id);

        Task<IEnumerable<RatingResponse>> GetRating();

        Task<IEnumerable<AccountResponse>> GetFollows();
        Task<IEnumerable<AccountResponse>> GetFollowers();

        Task<IEnumerable<RatingResponse>> GetRatingForUser(string id);

        Task<bool> PostRating(RatingResponse request);

        Task<bool> VerifyCode(string login, int code);

        Task<bool> ResendCode(string login);

        Task<bool> Signup(string name, string login, string password, string picture, string type);
        Task<string> GetUsageTerms();
        Task<bool> LoginWithFacebook(string accessToken, UserType userType);
        Task<bool> UpdateAccount(AccountResponse account);
        Task<bool> LoadCachedUser();
        Task Logout();
        Task<bool> Delete();
    }
}