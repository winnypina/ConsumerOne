using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
       

        public SignupViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            
        }

        public override void Prepare()
        {
            SetScreenName("Cadastro Perfil");
        }

        public MvxAsyncCommand SignupProviderCommand => new MvxAsyncCommand(async()=> await NavigationService.Navigate<ChooseSigningViewModel,string>("Provider"));
        public MvxAsyncCommand SignupConsumerCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<ChooseSigningViewModel, string>("Consumer"));

        
    }
}
