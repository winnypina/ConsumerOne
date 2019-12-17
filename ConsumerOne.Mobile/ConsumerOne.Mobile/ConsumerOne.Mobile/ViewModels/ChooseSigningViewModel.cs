using System;
using System.Json;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;

namespace ConsumerOne.Mobile.ViewModels
{
    public class ChooseSigningViewModel : BaseViewModel<string>
    {
        private readonly ILoginService _loginService;

        public ChooseSigningViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
        }

        private string _parameter;

        public MvxAsyncCommand SigningCommand =>
            new MvxAsyncCommand(async () => await NavigationService.Navigate<SignupFieldsViewModel,string>(_parameter));

        public MvxAsyncCommand LoginWithFacebookCommand => new MvxAsyncCommand(ExecuteLoginWithFacebookCommand);

        public override void Prepare()
        {
            SetScreenName("Cadastro Tipo Login");
        }

        private async Task ExecuteLoginWithFacebookCommand()
        {
            IsLoading = true;
           
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

                        await _loginService.LoginWithFacebook(eventArgs.Account.Properties["access_token"], _parameter == "Provider" ? Services.Responses.UserType.Provider : Services.Responses.UserType.Consumer)
                            .ConfigureAwait(false);
                        await NavigationService.Navigate<TabsMasterViewModel>();
                        IsLoading = false;
                    }
                    else
                    {
                        IsLoading = false;
                    }
                };
                presenter.Login(auth);
          
        }

        public override void Prepare(string parameter)
        {
            _parameter = parameter;
        }
    }
}