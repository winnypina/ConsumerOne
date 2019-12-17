using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public WelcomeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
        }

        public override void Prepare()
        {
            SetScreenName("Inicial");
        }

        public MvxAsyncCommand SignupCommand => new MvxAsyncCommand(ExecuteSignupCommand);

        public MvxAsyncCommand SigninCommand => new MvxAsyncCommand(ExecuteSigninCommand);

        private async Task ExecuteSigninCommand()
        {
            await NavigationService.Navigate<SigninViewModel>();
        }


        private async Task ExecuteSignupCommand()
        {
            await NavigationService.Navigate<SignupViewModel>();
        }
    }
}