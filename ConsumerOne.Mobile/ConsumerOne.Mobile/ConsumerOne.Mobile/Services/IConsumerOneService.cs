using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.Services.Responses;
using ConsumerOne.Mobile.ViewModels;
using ConsumerOne.Mobile.ViewModels.Models;
using Refit;

namespace ConsumerOne.Mobile.Services
{
    public interface IConsumerOneService
    {
        [Post("/auth/login")]
        Task<LoginResponse> Login([Body] AuthTokenRequest request);

        [Post("/externalauth/facebook")]
        Task<LoginResponse> LoginWithFacebook([Body] FacebookAuthTokenRequest request);

        [Get("/accounts")]
        Task<AccountResponse> GetAccount();

        [Get("/accounts/profile/{id}")]
        Task<AccountResponse> GetProfile(string id);

        [Get("/TextTranslation")]
        Task<IEnumerable<TranslationResponse>> GetTranslation();

        [Get("/UseTerms")]
        Task<UseTermsResponse> GetUseTerms();

        [Get("/PrivacyPolicy")]
        Task<UseTermsResponse> GetPrivacyPolicy();

        [Post("/accounts")]
        Task<AccountResponse> CreateAccount([Body] CreateAccountRequest request);

        [Post("/accounts/verify")]
        Task VerifyCode([Body] CodeVerificationRequest request);

        [Post("/accounts/resend")]
        Task ResendCode([Body] CodeVerificationRequest request);

        [Put("/accounts")]
        Task<AccountResponse> UpdateAccount(AccountResponse request);
        
        [Multipart]
        [Post("/media")]
        Task UploadPicture([AliasAs("files")] IEnumerable<StreamPart> streams);

        [Post("/post")]
        Task PublishPost([Body] PostRequest request);

        [Post("/post/like/{id}")]
        Task LikePost(Guid id);

        [Get("/post/paged/{page}")]
        Task<IEnumerable<PostRequest>> GetPostsPaged(int page);

        [Get("/post/user/{userId}/paged/{page}")]
        Task<IEnumerable<PostRequest>> GetPostsForUserPaged(string userId, int page);

        [Get("/post/comment/{id}")]
        Task<IEnumerable<CommentRequest>> GetComments(Guid id);

        [Post("/post/comment")]
        Task CreateComment(CommentRequest request);

        [Post("/post/comment/{id}")]
        Task DeleteComment(Guid id);

        [Post("/post/{id}")]
        Task DeletePost(Guid id);

        [Post("/post/pause/{id}")]
        Task PauseUnpausePost(Guid id);

        [Post("/post/search/paged/{page}")]
        Task<IEnumerable<PostRequest>> DoSearch([Body] PostFilterModel request, int page);

        [Get("/accounts/followers")]
        Task<IEnumerable<AccountResponse>> GetFollowers();

        [Get("/accounts/follows")]
        Task<IEnumerable<AccountResponse>> GetFollows();

        [Post("/accounts/resetPassword")]
        Task ResetPassword(PasswordResetRequest model);

        [Post("/accounts/newPassword")]
        Task NewPassword(PasswordResetRequest model);

        [Get("/accounts/rating")]
        Task<IEnumerable<RatingResponse>> GetRatings();

        [Get("/accounts/rating/{id}")]
        Task<IEnumerable<RatingResponse>> GetRatingsForUser(string id);

        [Post("/accounts/rating")]
        Task PostRating([Body] RatingResponse request);

        [Post("/accounts/follow/{id}")]
        Task Follow(string id);

        [Delete("/accounts")]
        Task DeleteAccount();

        [Get("/message")]
        Task<IEnumerable<MessageResponse>> GetMessages();

        [Post("/message")]
        Task SendMessage(MessageModel message);
    }
}