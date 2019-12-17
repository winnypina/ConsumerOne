using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostLocationViewModel : BaseViewModel<PostModel>
    {
        private string _address;
        private PostModel _current;

        private CameraPosition _currentLocation;

        public NewPostLocationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
        }

        public CameraPosition CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand, CanExecuteNextCommand);

        public MvxAsyncCommand SearchAddressCommand =>
            new MvxAsyncCommand(ExecuteSearchAddressCommand, CanExecuteSearchAddressCommand);

        private async Task ExecuteNextCommand()
        {
            _current.Address = Address;
            await NavigationService.Navigate<NewPostReviewViewModel, PostModel>(_current);
        }

        private bool CanExecuteNextCommand()
        {
            return !string.IsNullOrEmpty(Address);
        }
        private string currentAddress;
        private async Task ExecuteSearchAddressCommand()
        {
            if (currentAddress == null)
            {
                IsLoading = true;
                var geocoder = new Geocoder();
                var positions = await geocoder.GetPositionsForAddressAsync(Address).ConfigureAwait(false);
                var first = positions.FirstOrDefault();
                CurrentLocation = new CameraPosition(new Position(first.Latitude, first.Longitude), 10);
                _current.Latitude = first.Latitude;
                _current.Longitude = first.Longitude;
                IsLoading = false;
            }
            else
            {
                currentAddress = null;
                Address = string.Empty;
            }
        }

        private bool CanExecuteSearchAddressCommand()
        {
            return !string.IsNullOrEmpty(Address);
        }

        public override void Prepare(PostModel parameter)
        {
            _current = parameter;
            Address = parameter.Address;
#if DEBUG
            //if (string.IsNullOrEmpty(Address))
            //{
            //    Address = "Rua Cuiabá, Sorocaba";
            //}
#endif
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
                if (status != PermissionStatus.Granted) return;
            }

            if (string.IsNullOrEmpty(_current.Address))
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    CurrentLocation = new CameraPosition(new Position(location.Latitude, location.Longitude), 10);
                    _current.Latitude = location.Latitude;
                    _current.Longitude = location.Longitude;
                }

                var placemarks = await Geocoding.GetPlacemarksAsync(_current.Latitude, _current.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    Address =
                        $"{placemark.Thoroughfare}, {placemark.SubAdminArea}, {placemark.AdminArea}, {placemark.CountryName}";
                }
            }
            else
            {
                var geocoder = new Geocoder();
                var positions = await geocoder.GetPositionsForAddressAsync(Address);
                var first = positions.FirstOrDefault();
                CurrentLocation = new CameraPosition(new Position(first.Latitude, first.Longitude), 10);
                _current.Latitude = first.Latitude;
                _current.Longitude = first.Longitude;
            }
        }
    }
}