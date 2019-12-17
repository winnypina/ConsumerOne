using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Xamarin.Forms.GoogleMaps;

namespace ConsumerOne.Mobile.ViewModels
{

    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    public static class CoordinatesDistanceExtensions
    {
        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates)
        {
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }

    public class UnitOfLength
    {
        public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
        public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
        public static UnitOfLength Miles = new UnitOfLength(1);

        private readonly double _fromMilesFactor;

        private UnitOfLength(double fromMilesFactor)
        {
            _fromMilesFactor = fromMilesFactor;
        }

        public double ConvertFromMiles(double input)
        {
            return input * _fromMilesFactor;
        }
    }

    public class NewPostReviewViewModel : BaseViewModel<PostModel>
    {
        private readonly ILoginService _loginService;
        private readonly IPostService _postService;
        private readonly IUserInteractionService _popupService;
        private readonly IMediaService _mediaService;
        private PostModel _postModel;
        private bool _hasImage;
        private bool _hasVideo;
        private bool _hasLocation;
        private double _distance;
        private ImageSource _imageTest;
        private MvxObservableCollection<ImageSource> _images;
        private string _address;
        private PostModel _current;
        private string _title;
        private string _description;
        private string _username;
        private ImageSource _userPicture;
        private string _postId;
        private VideoSource _videoSource;


        public NewPostReviewViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService, 
            ILoginService loginService, IPostService postService, IUserInteractionService popupService, IMediaService mediaService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _postService = postService;
            _popupService = popupService;
            _mediaService = mediaService;

            Images = new MvxObservableCollection<ImageSource>();
        }

        public VideoSource VideoSource
        {
            get => _videoSource;
            set => SetProperty(ref _videoSource, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address,value);
        }

        public bool HasImage
        {
            get => _hasImage;
            set => SetProperty(ref _hasImage,value);
        }

        public bool HasVideo
        {
            get => _hasVideo;
            set => SetProperty(ref _hasVideo,value);
        }

        public bool HasLocation
        {
            get => _hasLocation;
            set => SetProperty(ref _hasLocation,value);
        }

        public ImageSource UserPicture
        {
            get => _userPicture;
            set => SetProperty(ref _userPicture,value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username,value);
        }

        public string PostId
        {
            get => _postId;
            set => SetProperty(ref _postId,value);
        }

        private Guid _newPostId;
        private string _price;

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price,value);
        }

        public override void Prepare(PostModel parameter)
        {
            
            _current = parameter;

            if (_current.Id != Guid.Empty)
            {
                PostId = $"CO|Post|{_current.Id}";
            }
            else
            {
                _newPostId = Guid.NewGuid();
                PostId = $"CO|Post|{_newPostId}";
            }

            Price = $"R$ {_current.Value:N2}";
            Address = parameter.Address;
            Title = parameter.Title;
            Username = _loginService.Account.Name;
            
            UserPicture = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{_loginService.Account.Id}.jpg"));
            Description = parameter.Description;
            if (parameter.Video != null)
            {
                HasVideo = true;
                HasImage = false;
            }
            else
            {
                HasVideo = false;
                HasImage = true;
            }
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public double Distance
        {
            get => _distance;
            set => SetProperty(ref _distance,value);
        }

        public MvxObservableCollection<ImageSource> Images
        {
            get => _images;
            set => SetProperty(ref _images,value);
        }

        public override async Task Initialize()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
                if (status != PermissionStatus.Granted) return;
            }

            HasLocation = true;

            var geocoder = new Geocoder();
            
            var positions = await geocoder.GetPositionsForAddressAsync(Address).ConfigureAwait(false);
            var postLocation = positions.FirstOrDefault();

            var location = await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);

            Images.Clear();
            _current.Images.ForEach(n =>
            {
                Images.Add(ImageSource.FromStream(() => new MemoryStream(n)));    
            });

            Distance = new Coordinates(location.Latitude,location.Longitude).DistanceTo(new Coordinates(postLocation.Latitude,postLocation.Longitude), UnitOfLength.Kilometers);
        }

        public MvxAsyncCommand PublishCommand => new MvxAsyncCommand(ExecutePublishCommand);

        public MvxAsyncCommand EditAgainCommand => new MvxAsyncCommand(ExecuteEditAgainCommand);

        private async Task ExecuteEditAgainCommand()
        {
            await NavigationService.Navigate<EditPostViewModel, PostModel>(_current);
        }

        private async Task ExecutePublishCommand()
        {
            IsLoading = true;

            List<Guid> images = null;

            if (_current.Images != null && _current.Images.Any())
            {
                images = new List<Guid>();
                foreach (var image in _current.Images)
                {
                    var id = Guid.NewGuid();
                    images.Add(id);
                    await _mediaService.Upload(image, id+ ".jpg");
                }
            }

            string video = null;

            if (_current.Video != null)
            {
                var id = Guid.NewGuid();
                video = id.ToString();
                await _mediaService.Upload(_current.Video, id + ".mp4");
            }

            var request = new PostRequest
            {
                Address = _current.Address,
                Images = images,
                Video = video,
                Description = _current.Description,
                Tags = _current.Tags,
                Title = _current.Title,
                Currency = _current.Currency,
                Value = _current.Value,
                Longitude = _current.Longitude,
                Latitude = _current.Latitude,
                StoreLink = _current.StoreLink,
                Id = _current.Id
            };

            if (await _postService.Publish(request))
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(HomeViewModel)));
               
            }
            else
            {
                await _popupService.DisplayMessage("Erro","Erro ao publicar o post");
            }

            IsLoading = false;
        }
    }
}