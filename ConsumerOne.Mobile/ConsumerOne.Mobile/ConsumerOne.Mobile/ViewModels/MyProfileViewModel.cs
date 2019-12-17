using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Responses;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels
{

    public class ProfileModel
    {
        public string Id { get; set; }
        public bool HasRating { get; set; }
    }

    public class MyProfileViewModel : BaseViewModel<ProfileModel>, IPostPopupViewModel
    {
        private readonly IPostService postService;
        private readonly IPopupService popupService;
        private readonly ILoginService _loginService;
        private readonly IShareService _shareService;
        private readonly IImageService imageService;
        private readonly IUserInteractionService userInteractionService;
        private string _address;
        private string _description;
        private string _id;
        private ImageSource _image;
        private int _numberOfFriends;
        private string _phone;
        private double _rating;
        private string _website;
        private ImageSource _defaultImage;
        private string _addressLine1;
        private string _addressLine2;

        public MyProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IPostService postService, IPopupService popupService,
            ILoginService loginService, IShareService shareService, ITranslationService translationService, IImageService imageService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            this.postService = postService;
            this.popupService = popupService;
            _loginService = loginService;
            _shareService = shareService;
            this.imageService = imageService;
            this.userInteractionService = userInteractionService;
            Posts = new MvxObservableCollection<PostListModel>();
        }

        public MvxObservableCollection<PostListModel> Posts { get; }

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(async () =>
       {
           await _shareService.ShareQRCode("Meu perfil no Consumer One", $"{ApiService.ApiBaseAddress}/post/CO|Profile|{_loginService.Account.Id}/qrcode");
       });

        public MvxCommand CallCommand => new MvxCommand(() =>
        {
            try
            {
                PhoneDialer.Open(Phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        });

        public MvxCommand MapCommand => new MvxCommand(() =>
        {
            // Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
            var name = Address.Replace("&", "and"); // var name = Uri.EscapeUriString(place.Name);
            var addr = Uri.EscapeUriString(Address);

            string request = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    request = $"http://maps.apple.com/maps?q={name.Replace(' ', '+')}";
                    break;
                case Device.Android:
                    request = $"geo:0,0?q={(string.IsNullOrWhiteSpace(addr) ? "" : addr)}({name})";
                    break;
            }

            Device.OpenUri(new Uri(request));
        });

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public MvxAsyncCommand ShareImageCommand => new MvxAsyncCommand(async () =>
        {
            await _shareService.ShareQRCode("Compartilhando meu perfil ConsumerOne", $"{ApiService.ApiBaseAddress}/post/CO|Profile|{_loginService.Account.Id}/qrcode");
        });

        public MvxAsyncCommand SaveImageCommand => new MvxAsyncCommand(async () =>
        {
            await imageService.SaveToGallery($"{ApiService.ApiBaseAddress}/post/CO|Profile|{_loginService.Account.Id}/qrcode");
            await userInteractionService.DisplayMessage("Salvar", "Salvo na galeria.");
        });

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string Website
        {
            get => _website;
            set => SetProperty(ref _website, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public int NumberOfFriends
        {
            get => _numberOfFriends;
            set => SetProperty(ref _numberOfFriends, value);
        }

        public double Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        public ImageSource DefaultImage
        {
            get => _defaultImage;
            set => SetProperty(ref _defaultImage, value);
        }

        public string AddressLine1
        {
            get => _addressLine1;
            set => SetProperty(ref _addressLine1, value);
        }

        public string AddressLine2
        {
            get => _addressLine2;
            set => SetProperty(ref _addressLine2, value);
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
        public MvxAsyncCommand GoToMenuCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<MasterViewModel>());

        private ProfileModel _parameter;
        private bool _isCurrentUser;
        private bool _isFollowing;
        private bool _hasRated;
        private double? _myRating;
        private string name;
        private ImageSource qrCodeImageSource;
        private bool isConsumer;

        public override void Prepare()
        {
            SetScreenName("Meu Perfil");
        }

        public bool IsCurrentUser
        {
            get => _isCurrentUser;
            set => SetProperty(ref _isCurrentUser, value);
        }

        public bool IsFollowing
        {
            get => _isFollowing;
            set => SetProperty(ref _isFollowing, value);
        }

        public bool HasRated
        {
            get => _hasRated;
            set => SetProperty(ref _hasRated, value);
        }

        public ImageSource QrCodeImageSource { get => qrCodeImageSource; set => SetProperty(ref qrCodeImageSource, value); }

        public double? MyRating
        {
            get => _myRating;
            set => SetProperty(ref _myRating, value);
        }

        public MvxAsyncCommand RateCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<RatingViewModel, ProfileModel>(_parameter);
        });

        public MvxAsyncCommand MyRatingCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<MyRatingViewModel>();
        });

        public MvxCommand OpenWebsiteCommand => new MvxCommand(() =>
        {
            Device.OpenUri(new Uri(Website));
        });

        public MvxAsyncCommand FollowCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            IsFollowing = true;
            await _loginService.Follow(_parameter.Id).ConfigureAwait(false);
            IsLoading = false;
        });

        public MvxAsyncCommand UnfollowCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            IsFollowing = false;
            await _loginService.Follow(_parameter.Id).ConfigureAwait(false);
            IsLoading = false;
        });

        public bool IsConsumer { get => isConsumer; set => SetProperty(ref isConsumer,value); }

        private bool ownPost;
        public bool OwnPost { get => ownPost; set => SetProperty(ref ownPost, value); }

        public MvxAsyncCommand<PostListModel> MoreCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            currentPost = post;
            OwnPost = post.UserId == _loginService.Account.Id;
            await popupService.ShowPostOptions(this);
        });

        public MvxAsyncCommand<PostListModel> SharePostCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            IsLoading = true;
            await _shareService.SharePost(post);
            IsLoading = false;
        });

        public MvxAsyncCommand<PostListModel> CommentsCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            await NavigationService.Navigate<CommentsViewModel, PostListModel>(post);
        });

        public MvxCommand<PostListModel> BuyNewCommand => new MvxCommand<PostListModel>(post =>
        {
            Device.OpenUri(new Uri(post.StoreLink));
        });

        public MvxAsyncCommand<string> GoToProfileCommand => new MvxAsyncCommand<string>(async post =>
        {
            await NavigationService.Navigate<ProfileViewModel, ProfileModel>(new ProfileModel { Id = post });
        });

        public MvxAsyncCommand<PostListModel> GoToQrCodeCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            await NavigationService.Navigate<PostCodeViewModel, PostListModel>(post);
        });


        public MvxAsyncCommand<PostListModel> SendMessageCommand => new MvxAsyncCommand<PostListModel>(async post =>
        {
            await NavigationService.Navigate<SendMessageViewModel, string>(post.UserId);
        });

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

            if (await postService.PauseUnpausePost(currentPost.Id))
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

            if (await postService.DeletePost(currentPost.Id))
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

        private PostListModel currentPost;

        public MvxAsyncCommand ReportPostCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<ReportPostViewModel, PostListModel>(currentPost);
            await popupService.Dismiss();
        });

        public MvxAsyncCommand DismissPopupCommand => new MvxAsyncCommand(async () =>
        {
            await popupService.Dismiss();
        });

        public override async Task Initialize()
        {
            if (_parameter != null)
            {
                IsLoading = true;
                IsCurrentUser = _parameter.Id == _loginService.Account.Id;
                var account = await _loginService.GetProfile(_parameter.Id);
                if (account == null) return;
                IsFollowing = account.IsFollowing;
                HasRated = account.MyRating.HasValue;
                _parameter.HasRating = HasRated;
                MyRating = account.MyRating;
                IsConsumer = account.Type == UserType.Consumer;
                DefaultImage = ImageSource.FromFile(account.Type == UserType.Provider ? "defaultProvider.png" : "defaultUser.png");
                Image = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{account.Id}.jpg"));
                Name = account.Name;
                Description = account.About;
                QrCodeImageSource = ImageSource.FromUri(new Uri($"{ApiService.ApiBaseAddress}/post/CO|Account|{account.Id}/qrcode"));
                Website = account.Website;
                if (!string.IsNullOrEmpty(account.Address))
                {
                    Address = $"{account.Address}, {account.AddressNumber}";
                    if (!string.IsNullOrEmpty(account.AddressAddon))
                    {
                        Address += $"{account.AddressAddon}";
                    }
                    AddressLine1 = account.City;
                    AddressLine2 = $"{account.State}, {account.Country} - {account.Cep}";
                }
                Phone = account.MobilePhone;
                IsLoading = false;
                await GetPosts();
            }
        }

        private async Task GetPosts()
        {
            IsLoading = true;
            if (Posts.Count > 0)
            {
                Posts.RemoveRange(0, Posts.Count);
            }
            if (_loginService.Account == null) return;
            var posts = await postService.GetPostsForUser(_loginService.Account.Id,0).ConfigureAwait(false);
            if (posts != null)
            {
                var postList = new List<PostListModel>();
                foreach (var post in posts.OrderByDescending(n => n.PublishDate))
                {
                    var localPost = new PostListModel
                    {
                        Address = post.Address,
                        Title = post.Title,
                        Price = $"R$ {post.Value:N2}",
                        HasVideo = !string.IsNullOrEmpty(post.Video),
                        StoreLink = post.StoreLink,
                        Description = post.Description,
                        UserId = post.UserId,
                        Id = post.Id,
                        PostId = $"CO|Post|{post.Id}",
                        QrCodeImageSource = ImageSource.FromUri(new Uri($"{ApiService.ApiBaseAddress}/post/CO|Post|{post.Id}/qrcode")),
                        Username = post.Username,
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


            IsLoading = false;
        }

        public override async void ViewAppeared()
        {
            await GetPosts();
        }

        public override void Prepare(ProfileModel parameter)
        {

            if (string.IsNullOrEmpty(parameter.Id))
            {
                IsCurrentUser = true;
                NumberOfFriends = _loginService.Account.Followers;
                DefaultImage = ImageSource.FromFile(_loginService.Account.Type == UserType.Provider ? "defaultProvider.png" : "defaultUser.png");
                Image = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{_loginService.Account.Id}.jpg"));
                Name = _loginService.Account.Name;
                Description = _loginService.Account.About;
                Id = "CO|Account|" + _loginService.Account.Id;
                Website = _loginService.Account.Website;
                Address = $"{_loginService.Account.Address}, {_loginService.Account.AddressNumber}";
                if (!string.IsNullOrEmpty(_loginService.Account.AddressAddon))
                {
                    Address += $"{_loginService.Account.AddressAddon}";
                }

                AddressLine1 = _loginService.Account.City;
                AddressLine2 = $"{_loginService.Account.State}, {_loginService.Account.Country} - {_loginService.Account.Cep}";
                Phone = _loginService.Account.MobilePhone;
            }
            else
            {
                _parameter = parameter;
            }

        }
    }
}