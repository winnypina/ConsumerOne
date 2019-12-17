using System;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class SendMessageViewModel : BaseViewModel<string>
    {

        private string parameter;
        private string message;
        private readonly IMessageService messageService;
        private readonly ILoginService loginService;
        private readonly IUserInteractionService userInteractionService;

        public SendMessageViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ITranslationService translationService, IMessageService messageService, ILoginService loginService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            this.messageService = messageService;
            this.loginService = loginService;
            this.userInteractionService = userInteractionService;
            Messages = new MvxObservableCollection<MessageModel>();
        }

        public override void Prepare(string parameter)
        {
            this.parameter = parameter;
        }

        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            var messageToBeSent = new MessageModel
            {
                Message = Message,
                ToId = parameter,
                FromId = loginService.Account.Id
            };
            if(await messageService.SendMessage(messageToBeSent))
            {
                await userInteractionService.DisplayMessage("Sucesso", "Mensagem enviada com sucesso");
                Message = string.Empty;
            }
            else
            {
                await userInteractionService.DisplayMessage("Erro","Ocorreu um erro no envio da mensagem, por favor tente novamente mais tarde.");
            }
            await LoadMessages();
            IsLoading = false;
        }, () => !string.IsNullOrEmpty(Message));

        public MvxObservableCollection<MessageModel> Messages { get; }

        public override async Task Initialize()
        {
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            IsLoading = true;

            var serverMessages = (await messageService.GetMessages()).ToList();
            var userMessages = serverMessages.Where(n => n.ToId == parameter || n.FromId == parameter).OrderByDescending(n => n.SentDate);
            if(Messages.Count > 0)
            {
                Messages.RemoveRange(0, Messages.Count);
            }
            Messages.AddRange(userMessages);
            IsLoading = false;
        }
    }
}
