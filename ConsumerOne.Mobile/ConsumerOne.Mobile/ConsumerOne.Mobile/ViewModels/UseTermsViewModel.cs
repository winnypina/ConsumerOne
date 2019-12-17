using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class UseTermsViewModel : BaseViewModel
    {
        private readonly ITranslationService _translationService;
        private string _terms;

        public UseTermsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _translationService = translationService;
        }

        public override void Prepare()
        {
            SetScreenName("Termos de uso");
        }

        public string Terms
        {
            get => _terms;
            set => SetProperty(ref _terms,value);
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            var terms = await _translationService.GetUseTerms().ConfigureAwait(false);
            Terms = terms;
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

    }
}