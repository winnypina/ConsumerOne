using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly ILoginService loginService;
        private readonly IUserInteractionService userInteractionService;
        private string newPassword;
        private string newPasswordConfirmation;

        public ChangePasswordViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService, ILoginService loginService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            this.loginService = loginService;
            this.userInteractionService = userInteractionService;
        }

        public string NewPassword { get => newPassword; set => SetProperty(ref newPassword,value); }
        public string NewPasswordConfirmation { get => newPasswordConfirmation; set => SetProperty(ref newPasswordConfirmation,value); }

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(ExecuteSaveCommand);

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        private async Task ExecuteSaveCommand()
        {
            IsLoading = true;
            if (string.IsNullOrEmpty(NewPassword) || NewPassword != NewPasswordConfirmation)
            {
                await userInteractionService.DisplayMessage("Dados inválidos","Senhas não conferem");
                IsLoading = false;
                return;
            }

            if(await loginService.ResetPassword(NewPassword))
            {
                await userInteractionService.DisplayMessage("Sucesso", "Senha alterada com sucesso.");
                await NavigationService.Close(this);
            }
            else
            {
                await userInteractionService.DisplayMessage("Erro", "Não foi possível alterar sua senha nesse momento. Por favor tente mais tarde.");
            }

            IsLoading = false;
        }
    }
}