using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostPriceViewModel : BaseViewModel<PostModel>
    {

        private PostModel _current;
        private double _price;
        private string _storeWebsite;
        private string _currency;

        public NewPostPriceViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            Currencies = new MvxObservableCollection<string>();
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand);

        private async Task ExecuteNextCommand()
        {
            _current.Value = Price;
            _current.StoreLink = StoreWebsite;
            _current.Currency = Currency;
            await NavigationService.Navigate<NewPostMediaViewModel, PostModel>(_current);
        }

        public override void ViewAppeared()
        {
            Currencies.Clear();
            Currencies.Add("BRL(R$)");
            if (_current.Value > 0)
            {
                Price = _current.Value;
            }
            else
            {
#if DEBUG
                Price = 30;
#endif

            }

            if (!string.IsNullOrEmpty(_current.StoreLink))
            {
                StoreWebsite = _current.StoreLink;
            }
            else
            {
#if DEBUG
                StoreWebsite = "https://google.com.br";
#endif
            }

            if (!string.IsNullOrEmpty(_current.Currency))
            {
                Currency = _current.Currency;
            }
            else
            {
                Currency = Currencies.First();
            }
        }

        public override void Prepare(PostModel parameter)
        {
            
            _current = parameter;
            

        }

        public double Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string StoreWebsite
        {
            get => _storeWebsite;
            set
            {
                if (!value.StartsWith("https"))
                {
                    value = "https://" + value;
                }
                SetProperty(ref _storeWebsite, value);
            }
        }

        public MvxObservableCollection<string> Currencies { get; }

        public string Currency
        {
            get => _currency;
            set => SetProperty(ref _currency,value);
        }
    }
}