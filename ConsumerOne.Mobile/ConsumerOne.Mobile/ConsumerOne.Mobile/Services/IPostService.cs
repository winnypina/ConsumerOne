using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.ViewModels.Models;

namespace ConsumerOne.Mobile.Services
{
    public interface IPostService
    {
        Task<bool> Publish(PostRequest model);
        Task<IEnumerable<PostRequest>> GetPosts(int page);
        Task<bool> Like(Guid postId);
        Task<IEnumerable<CommentRequest>> GetComments(Guid id);
        Task<bool> CreateComment(CommentRequest request);
        Task<bool> DeleteComment(Guid id);
        Task<IEnumerable<PostRequest>> GetPostsForUser(string userId, int page);

        Task<bool> DeletePost(Guid id);
        Task<bool> PauseUnpausePost(Guid id);

        Task<IEnumerable<PostRequest>> DoSearch(PostFilterModel request,int page);
    }
}