using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Controls.VideoPlayerControl;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Requests;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extended;

namespace ConsumerOne.Mobile.ViewModels
{

    public class PostWindowViewModel : BaseViewModel<Guid>, IPostPopupViewModel
    {
        private readonly ILoginService _loginService;
        private readonly IUserInteractionService userInteractionService;
        private readonly IPostService _postService;
        private readonly IShareService _shareService;
        private readonly IPopupService popupService;
        private int _currentPage;
        private bool _hasCoords;
        private bool _isLoadingItems;
        public static PostFilterModel Filter;
        private Location _location;

        public PostWindowViewModel(IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            IUserInteractionService userInteractionService,
            ITranslationService translationService, IPostService postService, IShareService shareService, IPopupService popupService,
            ILoginService loginService) : base(logProvider, navigationService,
            translationService)
        {
            this.userInteractionService = userInteractionService;
            _postService = postService;
            _shareService = shareService;
            this.popupService = popupService;
            _loginService = loginService;
            Posts = new MvxObservableCollection<PostListModel>();
        }

        public MvxCommand<PostListModel> BuyNewCommand => new MvxCommand<PostListModel>(post =>
        {
            Device.OpenUri(new Uri(post.StoreLink));
        });

        public MvxAsyncCommand<string> GoToProfileCommand => new MvxAsyncCommand<string>(async post =>
        {
            await NavigationService.Navigate<ProfileViewModel, ProfileModel>(new ProfileModel { Id = post });
        });


        public MvxAsyncCommand<PostListModel> SendMessageCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            await NavigationService.Navigate<SendMessageViewModel, string>(post.UserId);
        });

        public MvxAsyncCommand<PostListModel> MoreCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            currentPost = post;
            OwnPost = post.UserId == _loginService.Account.Id;
            await popupService.ShowPostOptions(this);
        });

        public MvxAsyncCommand RefreshCommand => new MvxAsyncCommand(async () =>
        {
            if (Filter != null)
            {
                await DoSearch();
            }
            else
            {
                await GetPosts();
            }
        });

        public MvxAsyncCommand<PostListModel> LikeCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            if (post.IsLikedByUser)
            {
                post.IsLikedByUser = false;
                post.LikeCount = post.LikeCount - 1;
            }
            else
            {
                post.IsLikedByUser = true;
                post.LikeCount = post.LikeCount + 1;
            }

            await _postService.Like(post.Id).ConfigureAwait(false);
        });

        public MvxAsyncCommand<PostListModel> ShareCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            IsLoading = true;
            await _shareService.SharePost(post);
            IsLoading = false;
        });

        public MvxAsyncCommand<PostListModel> CommentsCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            await NavigationService.Navigate<CommentsViewModel, PostListModel>(post);
        });

        public bool HasCoords
        {
            get => _hasCoords;
            set => SetProperty(ref _hasCoords, value);
        }

        public bool IsLoadingItems
        {
            get => _isLoadingItems;
            set => SetProperty(ref _isLoadingItems, value);
        }

        public MvxObservableCollection<PostListModel> Posts { get; }


        private async Task DoSearch()
        {
            IsLoading = true;
            _currentPage = 0;

            if (Posts.Count > 0)
            {
                Posts.RemoveRange(0, Posts.Count);
            }

            var posts = await _postService.DoSearch(Filter, _currentPage).ConfigureAwait(false);
            if (posts != null)
            {
                var postList = new List<PostListModel>();
                foreach (var post in posts.OrderByDescending(n => n.PublishDate))
                {
                    var localPost = new PostListModel
                    {
                        Address = post.Address,
                        Title = post.Title,
                        VideoSource = VideoSource.FromUri(ApiService.MediaBaseAddress + $"{post.Video}.mp4"),
                        Price = $"R$ {post.Value:N2}",
                        HasVideo = !string.IsNullOrEmpty(post.Video),
                        StoreLink = post.StoreLink,
                        Description = post.Description,
                        Id = post.Id,
                        UserId = post.UserId,
                        PostId = $"CO|Post|{post.Id}",
                        QrCodeImageSource = ImageSource.FromUri(new Uri($"{ApiService.ApiBaseAddress}/post/CO|Post|{post.Id}/qrcode")),
                        Username = post.Username,
                        Distance = HasCoords ? GetDistance(post.Latitude, post.Longitude) : string.Empty,
                        Latitude = post.Latitude,
                        Longitude = post.Longitude,
                        HasImage = post.Images.Any(),
                        UserPicture = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{post.UserId}.jpg")),
                        LikeCount = post.LikeCount,
                        IsLikedByUser = post.IsLikedByUser
                    };
                    localPost.Images.AddRange(post.Images.Select(image =>
                        ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{image}.jpg"))).ToList());
                    localPost.Image = localPost.Images?.FirstOrDefault();
                    postList.Add(localPost);
                }
                Posts.AddRange(postList);
            }
            else
            {
                await _loginService.Logout();
                await NavigationService.Navigate<SplashViewModel>();
            }


            IsLoading = false;
        }

        private async Task GetPosts()
        {
            IsLoading = true;
            _currentPage = 0;
            if (Posts.Count > 0)
            {
                Posts.RemoveRange(0, Posts.Count);
            }
            var posts = await _postService.GetPosts(_currentPage).ConfigureAwait(false);
            if (posts != null)
            {
                var postList = new List<PostListModel>();
                foreach (var post in posts.Where(n=>n.Id == id).OrderByDescending(n => n.PublishDate))
                {
                    var localPost = new PostListModel
                    {
                        Address = post.Address,
                        Title = post.Title,
                        VideoSource = VideoSource.FromUri(ApiService.MediaBaseAddress + $"{post.Video}.mp4"),
                        Price = $"R$ {post.Value:N2}",
                        HasVideo = !string.IsNullOrEmpty(post.Video),
                        StoreLink = post.StoreLink,
                        Description = post.Description,
                        UserId = post.UserId,
                        Id = post.Id,
                        PostId = $"CO|Post|{post.Id}",
                        QrCodeImageSource = ImageSource.FromUri(new Uri($"{ApiService.ApiBaseAddress}/post/CO|Post|{post.Id}/qrcode")),
                        Username = post.Username,
                        Distance = HasCoords ? GetDistance(post.Latitude, post.Longitude) : string.Empty,
                        Latitude = post.Latitude,
                        Longitude = post.Longitude,
                        HasImage = post.Images.Any(),
                        UserPicture = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{post.UserId}.jpg")),
                        LikeCount = post.LikeCount,
                        IsLikedByUser = post.IsLikedByUser
                    };
                    localPost.Images.AddRange(post.Images.Select(image =>
                        ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{image}.jpg"))).ToList());
                    localPost.Image = localPost.Images?.FirstOrDefault();
                    postList.Add(localPost);
                }
                Posts.AddRange(postList);
            }
            else
            {
                await _loginService.Logout();
                await NavigationService.Navigate<SplashViewModel>();
            }


            IsLoading = false;
        }

        public override async void ViewAppeared()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
                if (status != PermissionStatus.Granted) return;
            }

            HasCoords = true;
            _location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);

            if (Filter != null)
            {
                await DoSearch();
            }
            else
            {
                await GetPosts();
            }
        }

        public int ScreenHeight { get { return (int)Application.Current.MainPage.Height; } }


        private bool ownPost;
        public bool OwnPost { get => ownPost; set => SetProperty(ref ownPost, value); }

        private PostListModel currentPost;

        public MvxAsyncCommand EditPostCommand => new MvxAsyncCommand(async () =>
        {
            NewPostViewModel.Current = new PostModel
            {
                Address = currentPost.Address,
                Description = currentPost.Description,
                Id = currentPost.Id,
                Latitude = currentPost.Latitude ?? 0,
                Longitude = currentPost.Longitude ?? 0,
                StoreLink = currentPost.StoreLink,
                Title = currentPost.Title,
                Value = Convert.ToDouble(currentPost.Price.Replace("R$", ""))
            };
            await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(NewPostViewModel)));
            await popupService.Dismiss();
        });

        public MvxAsyncCommand PausePostCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;

            if (await _postService.PauseUnpausePost(currentPost.Id))
            {
                await userInteractionService.DisplayMessage("Sucesso", "Post pausado com sucesso");

            }
            else
            {
                await userInteractionService.DisplayMessage("Erro", "Não foi possível completar sua solicitação. Tente novamente mais tarde");
            }

            await popupService.Dismiss();
            await GetPosts();

            IsLoading = false;
        });

        public MvxAsyncCommand DeletePostCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;

            if (await _postService.DeletePost(currentPost.Id))
            {
                await userInteractionService.DisplayMessage("Sucesso", "Post removido com sucesso");

            }
            else
            {
                await userInteractionService.DisplayMessage("Erro", "Não foi possível completar sua solicitação. Tente novamente mais tarde");
            }

            await popupService.Dismiss();
            await GetPosts();

            IsLoading = false;
        });

        public MvxAsyncCommand TraceRouteCommand => new MvxAsyncCommand(async () =>
        {
            // Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
            var name = currentPost.Address.Replace("&", "and"); // var name = Uri.EscapeUriString(place.Name);
            var loc = $"{currentPost.Latitude},{currentPost.Longitude}";
            var addr = Uri.EscapeUriString(currentPost.Address);

            string request = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    request = $"http://maps.apple.com/maps?q={name.Replace(' ', '+')}";
                    break;
                case Device.Android:
                    request = $"geo:0,0?q={(string.IsNullOrWhiteSpace(addr) ? loc : addr)}({name})";
                    break;
            }

            Device.OpenUri(new Uri(request));
            await popupService.Dismiss();
        });

        public MvxAsyncCommand ReportPostCommand => new MvxAsyncCommand(async () =>
        {
            await userInteractionService.DisplayMessage("Sucesso", "Sua denúncia foi enviada para revisão");
            await popupService.Dismiss();
        });

        public MvxAsyncCommand DismissPopupCommand => new MvxAsyncCommand(async () =>
        {
            await popupService.Dismiss();
        });

        private async Task<IEnumerable<PostListModel>> LoadMore()
        {
            IsLoadingItems = true;
            _currentPage++;
            List<PostRequest> posts;
            if (Filter != null)
            {
                posts = (await _postService.DoSearch(Filter, _currentPage).ConfigureAwait(false)).ToList();
            }
            else
            {
                posts = (await _postService.GetPosts(_currentPage).ConfigureAwait(false)).ToList();
            }
            if (!posts.Any()) _currentPage--;
            IsLoadingItems = false;
            return posts.Select(post =>
            {
                var postModel = new PostListModel
                {
                    Address = post.Address,
                    Title = post.Title,
                    VideoSource = VideoSource.FromUri(ApiService.MediaBaseAddress + $"{post.Video}.mp4"),
                    Price = $"R$ {post.Value:N2}",
                    HasVideo = !string.IsNullOrEmpty(post.Video),
                    StoreLink = post.StoreLink,
                    Description = post.Description,
                    UserId = post.UserId,
                    Id = post.Id,
                    PostId = $"CO|Post|{post.Id}",
                    QrCodeImageSource = ImageSource.FromUri(new Uri($"{ApiService.ApiBaseAddress}/post/CO|Post|{post.Id}/qrcode")),
                    Username = post.Username,
                    Distance = HasCoords ? GetDistance(post.Latitude, post.Longitude) : string.Empty,
                    Latitude = post.Latitude,
                    Longitude = post.Longitude,
                    HasImage = post.Images.Any(),
                    UserPicture = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{post.UserId}.jpg")),
                    LikeCount = post.LikeCount,
                    IsLikedByUser = post.IsLikedByUser
                };
                postModel.Images.AddRange(post.Images.Select(image =>
                    ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{image}.jpg"))).ToList());
                postModel.Image = postModel.Images?.FirstOrDefault();
                return postModel;
            }
            );
        }

        private string GetDistance(double? postLatitude, double? postLongitude)
        {
            if (!postLatitude.HasValue || !postLongitude.HasValue || _location == null) return string.Empty;
            var km = new Coordinates(postLatitude.Value, postLongitude.Value).DistanceTo(
                new Coordinates(_location.Latitude, _location.Longitude), UnitOfLength.Kilometers);
            return $"{km:N1} km";
        }

        private Guid id;

        public override void Prepare(Guid parameter)
        {
            id = parameter;
        }
    }
}
