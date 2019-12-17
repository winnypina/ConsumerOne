using System;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Requests;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class CodeVerificationViewModel : BaseViewModel<CreateAccountRequest>
    {
        private readonly ILoginService _loginService;
        private readonly IUserInteractionService _popupService;

        private CreateAccountRequest _currentRequest;
        private string _title;
        private string code;

        public CodeVerificationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, IUserInteractionService popupService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _popupService = popupService;
        }

        public override void Prepare()
        {
            SetScreenName("Codigo Verificacao");
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Code
        {
            get => code;
            set
            {
                SetProperty(ref code, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(ExecuteSendCommand, CanExecuteSendCommand);

        public MvxAsyncCommand ResendCommand => new MvxAsyncCommand(ExecuteResendCommand);

        private async Task ExecuteResendCommand()
        {
            IsLoading = true;
            var login = _currentRequest.MobilePhone ?? _currentRequest.Email;

            if (await _loginService.ResendCode(login))
            {
                await _popupService.DisplayMessage("", Texts.SingleOrDefault(n => n.Key == "Reenvio")?.Value);
            }
            else
            {
                await _popupService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de envio do código. Tente novamente mais tarde!");
            }

            IsLoading = false;
        }

        private async Task ExecuteSendCommand()
        {
            IsLoading = true;
            var login = _currentRequest.MobilePhone ?? _currentRequest.Email;

            if (await _loginService.VerifyCode(login, Convert.ToInt32(Code)))
            {
                if (await _loginService.Login(login, _currentRequest.Password))
                {
                    await _popupService.DisplayConfirmation(Texts.SingleOrDefault(n => n.Key == "Sucesso Titulo")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Descrição")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Confirmação")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Cancelar")?.Value, async result =>
                    {
                        if (result)
                        {
                            await NavigationService.Navigate<EditProfileViewModel>();
                        }
                        else
                        {
                            await NavigationService.Navigate<TabsMasterViewModel>();
                        }
                    });

                }
                else
                {
                    await _popupService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de verificação do código. Tente novamente mais tarde!");
                }
            }
            else
            {
                await _popupService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de verificação do código. Tente novamente mais tarde!");
            }

            IsLoading = false;
        }

        private bool CanExecuteSendCommand()
        {
            return !string.IsNullOrEmpty(Code) && Code.Length == 4;
        }

        public override void Prepare(CreateAccountRequest parameter)
        {
            _currentRequest = parameter;
            if (!string.IsNullOrEmpty(parameter.MobilePhone))
            {
                Title = Texts.SingleOrDefault(n => n.Key == "Titulo telefone")?.Value;
            }
            else
            {
                Title = Texts.SingleOrDefault(n => n.Key == "Titulo email")?.Value;
            }
        }
    }
}