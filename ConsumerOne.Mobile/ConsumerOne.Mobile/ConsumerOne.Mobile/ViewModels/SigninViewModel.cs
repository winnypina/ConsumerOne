using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace ConsumerOne.Mobile.ViewModels
{
    public class SigninViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;
        private readonly IUserInteractionService _popupService;
        private string _username;
        private string _password;

        public SigninViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, IUserInteractionService popupService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _popupService = popupService;
#if DEBUG
            Username = "ricardozas@gmail.com";
            Password = "teste@123";
#endif
        }

        public override void Prepare()
        {
            SetScreenName("Login");
        }

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxAsyncCommand ForgotPasswordCommand => new MvxAsyncCommand(ExecuteForgotPasswordCommand);

        public MvxAsyncCommand LoginWithFacebookCommand => new MvxAsyncCommand(ExecuteLoginWithFacebookCommand);

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(ExecuteSendCommand, CanExecuteSendCommand);

        private async Task ExecuteLoginWithFacebookCommand()
        {
            IsLoading = true;
            await Task.Run(async () =>
            {
                var auth = new OAuth2Authenticator(
                    "210182943241238",
                    "email",
                    new Uri("https://m.facebook.com/dialog/oauth/"),
                    new Uri("https://www.facebook.com/connect/login_success.html"));

                var presenter = new OAuthLoginPresenter();
                presenter.Completed += async (sending, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        
                        await _loginService.LoginWithFacebook(eventArgs.Account.Properties["access_token"], Services.Responses.UserType.Consumer)
                            .ConfigureAwait(false);
                        await NavigationService.Navigate<TabsMasterViewModel>();
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                    }
                };

                await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
                {
                    presenter.Login(auth);
                });

               
            });
        }

        private async Task ExecuteForgotPasswordCommand()
        {
            await NavigationService.Navigate<ForgotPasswordViewModel>();
        }

        private bool CanExecuteSendCommand()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private async Task ExecuteSendCommand()
        {
            IsLoading = true;
            if (await _loginService.Login(Username,Password).ConfigureAwait(false))
            {
                await NavigationService.Navigate<TabsMasterViewModel>();
            }
            else
            {
                await _popupService.DisplayMessage("Erro","Login e/ou senha inválidos");
            }
            IsLoading = false;
        }
    }
}