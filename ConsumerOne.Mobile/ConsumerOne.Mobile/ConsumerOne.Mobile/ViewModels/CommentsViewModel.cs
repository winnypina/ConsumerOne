using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class CommentsViewModel : BaseViewModel<PostListModel>
    {
        private readonly IUserInteractionService userInteractionService;
        private readonly IPostService _postService;

        private PostListModel _currentPost;
        private string _commentText;
        private bool _canPublish;

        public CommentsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserInteractionService userInteractionService,
            ITranslationService translationService, IPostService postService) : base(logProvider, navigationService,
            translationService)
        {
            this.userInteractionService = userInteractionService;
            _postService = postService;
            Comments = new MvxObservableCollection<CommentModel>();
        }

        public override void Prepare(PostListModel parameter)
        {
            _currentPost = parameter;
        }

        public MvxObservableCollection<CommentModel> Comments { get; }

        public MvxAsyncCommand RefreshCommand => new MvxAsyncCommand(LoadComments);

        public MvxAsyncCommand<CommentModel> DeleteCommentCommand => new MvxAsyncCommand<CommentModel>(async comment => {
            IsLoading = true;
            await userInteractionService.DisplayConfirmation("Confirmar", "Deseja realmente excluir este comentário", "Sim", "Não", async result =>
             {
                 if(result)
                 {
                     await _postService.DeleteComment(comment.Id);
                     await LoadComments();
                 }
             });
           
            IsLoading = false;
        });

        public MvxAsyncCommand CreateCommentCommand => new MvxAsyncCommand(ExecuteCreateCommentCommand, CanExecuteCreateCommentCommand);


        public string CommentText
        {
            get => _commentText;
            set
            {
                SetProperty(ref _commentText, value);
                if (!string.IsNullOrEmpty(value))
                {
                    CanPublish = true;
                }
                else
                {
                    CanPublish = false;
                }
                CreateCommentCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanExecuteCreateCommentCommand()
        {
            return !string.IsNullOrEmpty(CommentText);
        }

        private async Task ExecuteCreateCommentCommand()
        {
            IsLoading = true;

            await _postService.CreateComment(new CommentRequest
            {
                Description = CommentText,
                PostId = _currentPost.Id
            }).ConfigureAwait(false);

            await LoadComments();
            CommentText = string.Empty;
            IsLoading = false;
        }

        public bool CanPublish
        {
            get => _canPublish;
            set => SetProperty(ref _canPublish,value);
        }

        public override async Task Initialize()
        {
            await LoadComments();
        }

        private async Task LoadComments()
        {
            IsLoading = true;

            if(Comments.Count > 0)
            {
                Comments.RemoveRange(0,Comments.Count);
            }
            var commentList = new List<CommentModel>();

            var comments = await _postService.GetComments(_currentPost.Id).ConfigureAwait(false);
            if (comments != null)
            {
                foreach (var commentRequest in comments.OrderByDescending(n => n.PublishDate))
                {
                    var span = (DateTime.UtcNow - commentRequest.PublishDate.ToUniversalTime());

                    var since = string.Empty;

                    if (span.Days > 0)
                    {
                        since += $"{span.Days}d";
                    }
                    else
                    {
                        if (span.Hours > 0)
                        {
                            since += $"{span.Hours}h";
                        }
                        else
                        {
                            if (span.Minutes > 0)
                            {
                                since += $"{span.Minutes}m";
                            }
                            else
                            {
                                since += $"{span.Seconds}s";
                            }
                        }
                    }

                    commentList.Add(new CommentModel
                    {
                        Description = commentRequest.Description,
                        AppUserId = commentRequest.AppUserId,
                        Id = commentRequest.Id,
                        PostId = commentRequest.PostId,
                        PublishDate = commentRequest.PublishDate,
                        AppUsername = commentRequest.AppUsername,
                        Since = since,
                        UserPicture = ApiService.MediaBaseAddress + $"{commentRequest.AppUserId}.jpg"
                    });
                }
            }

            Comments.AddRange(commentList);

            IsLoading = false;
        }
    }
}
