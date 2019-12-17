using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class PrivacyPolicyViewModel : BaseViewModel
    {

        private readonly ITranslationService _translationService;
        private string _privacyPolicyText;

        public PrivacyPolicyViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _translationService = translationService;
        }

        public override void Prepare()
        {
            SetScreenName("Privacidade");
        }

        public string PrivacyPolicyText
        {
            get => _privacyPolicyText;
            set => SetProperty(ref _privacyPolicyText, value);
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            var terms = await _translationService.GetPrivacyPolicy().ConfigureAwait(false);
            PrivacyPolicyText = terms;
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));
    }
}