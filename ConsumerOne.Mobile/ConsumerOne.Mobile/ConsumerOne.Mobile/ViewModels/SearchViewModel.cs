using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace ConsumerOne.Mobile.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        private string _address;
        private int _distance;
        private bool _isPosts;
        private bool _isProvider;
        private double? _latitude;
        private double? _longitude;
        private string _selectedOrder;
        private string _term;

        public SearchViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            Distance = 1;
            OrderTypes = new MvxObservableCollection<string> {"Data", "Distância"};
            SelectedOrder = OrderTypes.First();
            IsPosts = true;
        }

        public string Term
        {
            get => _term;
            set => SetProperty(ref _term, value);
        }

        public bool IsPosts
        {
            get => _isPosts;
            set => SetProperty(ref _isPosts, value);
        }

        public bool IsProvider
        {
            get => _isProvider;
            set => SetProperty(ref _isProvider, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public int Distance
        {
            get => _distance;
            set => SetProperty(ref _distance, value);
        }

        public string SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public MvxObservableCollection<string> OrderTypes { get; }

        public MvxAsyncCommand FilterCommand => new MvxAsyncCommand(ExecuteFilterCommand);

        public MvxCommand SwitchTypeCommand => new MvxCommand(() =>
        {
            if (IsPosts)
            {
                IsPosts = false;
                IsProvider = true;
            }
            else
            {
                IsPosts = true;
                IsProvider = false;
            }
        });

        public MvxAsyncCommand SearchAddressCommand =>
            new MvxAsyncCommand(ExecuteSearchAddressCommand, CanExecuteSearchAddressCommand);

        private async Task ExecuteSearchAddressCommand()
        {
            if(currentAddress == null)
            {
                currentAddress = Address;
                IsLoading = true;
                var geocoder = new Geocoder();
                var positions = await geocoder.GetPositionsForAddressAsync(Address).ConfigureAwait(false);
                var first = positions.FirstOrDefault();
                _latitude = first.Latitude;
                _longitude = first.Longitude;
                IsLoading = false;
            } else
            {
                currentAddress = null;
                Address = string.Empty;
            }
           
        }

        private bool CanExecuteSearchAddressCommand()
        {
            return !string.IsNullOrEmpty(Address);
        }

        private string currentAddress;

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

            if (string.IsNullOrEmpty(Address))
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    Address =
                        $"{placemark.Thoroughfare}, {placemark.SubAdminArea}, {placemark.AdminArea}, {placemark.CountryName}";
                    currentAddress = Address;
                }
            }
        }

        private async Task ExecuteFilterCommand()
        {

            PostListViewModel.Filter = new PostFilterModel
            {
                Address = Address,
                Distance = Distance,
                Longitude = _longitude,
                Latitude = _latitude,
                SearchType = IsPosts ? SearchType.Posts : SearchType.Providers,
                Term = Term,
                OrderType = SelectedOrder == "Data" ? OrderType.Date : OrderType.Distance
            };

            await NavigationService.Navigate<PostListViewModel>();

        }
    }
}