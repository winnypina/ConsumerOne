using System.Linq;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;
        private readonly ITranslationService _translationService;
        private readonly IUserInteractionService _popupService;
        private bool isConsumer;

        public MasterViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, ITranslationService translationService, IUserInteractionService popupService) : base(
            logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _translationService = translationService;
            _popupService = popupService;
            IsConsumer = _loginService.Account.Type == Services.Responses.UserType.Consumer;
        }

        public bool IsConsumer { get => isConsumer; set => SetProperty(ref isConsumer,value); }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public override void Prepare()
        {
            SetScreenName("Menu");
        }

        public MvxAsyncCommand GoToScanCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<ScanViewModel>();
        });

        public MvxAsyncCommand GoToInviteFriendsAsyncCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<InviteFriendsViewModel>();
        });

        public MvxAsyncCommand GoToMyProfileCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Close(this);
        });

        public MvxAsyncCommand GoToEditProfileCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<EditProfileViewModel>();
        });

        public MvxAsyncCommand GoToUseTermsCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<UseTermsViewModel>();
        });

        public MvxAsyncCommand GoToPrivacyPolicyCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<PrivacyPolicyViewModel>();
        });

        public MvxAsyncCommand EraseAccountCommand => new MvxAsyncCommand(async () =>
        {
            await _popupService.DisplayConfirmation("",
                _translationService.TranslatedTexts["Deletar conta"].SingleOrDefault(n => n.Key == "Mensagem")?.Value, _translationService.TranslatedTexts["Deletar conta"].SingleOrDefault(n => n.Key == "Confirmar")?.Value, _translationService.TranslatedTexts["Deletar conta"].SingleOrDefault(n => n.Key == "Cancelar")?.Value,
                async result =>
                {
                    if(result)
                    {
                        if (await _loginService.Delete())
                        {
                            await _loginService.Logout();
                            await NavigationService.Navigate<SplashViewModel>();
                        }
                    }
                   
                });
        });

        public MvxAsyncCommand LogoutCommand => new MvxAsyncCommand(async () =>
        {
            await _popupService.DisplayConfirmation("",
                _translationService.TranslatedTexts["Sair conta"].SingleOrDefault(n => n.Key == "Mensagem")?.Value,
                _translationService.TranslatedTexts["Sair conta"].SingleOrDefault(n => n.Key == "Confirmar")?.Value,
                _translationService.TranslatedTexts["Sair conta"].SingleOrDefault(n => n.Key == "Cancelar")?.Value,
                async result =>
                {
                    await _loginService.Logout();
                    await NavigationService.Navigate<SplashViewModel>();
                });
        });
    }
}