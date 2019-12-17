using System;
using System.Text;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly ILoginService loginService;
        private readonly IUserInteractionService userInteractionService;
        private string user;

        public ForgotPasswordViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService, ILoginService loginService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            this.loginService = loginService;
            this.userInteractionService = userInteractionService;
        }

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public string User { get => user; set => SetProperty(ref user,value); }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            if (await loginService.ResetPassword(User,CreatePassword(8)))
            {
                await userInteractionService.DisplayMessage("Sucesso", "Uma nova senha foi enviada para " + User);
                await NavigationService.Navigate<WelcomeViewModel>();
            }
            else
            {
                await userInteractionService.DisplayMessage("Erro", "Usuário não encontrado. Por favor tente novamente");
            }
        });
    }
}