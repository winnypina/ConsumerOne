using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Responses;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace ConsumerOne.Mobile.ViewModels
{
    public class ProfileWindowViewModel : BaseViewModel<ProfileModel>
    {
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

        public ProfileWindowViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, IShareService shareService, ITranslationService translationService, IImageService imageService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _shareService = shareService;
            this.imageService = imageService;
            this.userInteractionService = userInteractionService;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

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

        public MvxCommand OpenWebsiteCommand => new MvxCommand(() =>
        {
            Device.OpenUri(new Uri(Website));
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
            await _shareService.ShareQRCode("Compartilhando meu perfil ConsumerOne", $"{ApiService.ApiBaseAddress}/post/CO|Profile|{Id}/qrcode");
        });

        public MvxAsyncCommand SaveImageCommand => new MvxAsyncCommand(async () =>
        {
            await imageService.SaveToGallery($"{ApiService.ApiBaseAddress}/post/CO|Profile|{Id}/qrcode");
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

        
        public MvxAsyncCommand GoToMenuCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<MasterViewModel>());

        private ProfileModel _parameter;
        private bool _isCurrentUser;
        private bool _isFollowing;
        private bool _hasRated;
        private double? _myRating;
        private string name;
        private ImageSource qrCodeImageSource;

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

        public override async Task Initialize()
        {
            if (_parameter != null)
            {
                IsLoading = true;
                IsCurrentUser = _parameter.Id == _loginService.Account.Id;
                var account = await _loginService.GetProfile(_parameter.Id);
                IsFollowing = account.IsFollowing;
                HasRated = account.MyRating.HasValue;
                _parameter.HasRating = HasRated;
                MyRating = account.MyRating;
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
            }
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

