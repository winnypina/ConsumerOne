using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class MessagesViewModel : BaseViewModel
    {
        private readonly IMessageService messageService;

        public MessagesViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService, IMessageService messageService) : base(logProvider, navigationService, translationService)
        {
            Messages = new MvxObservableCollection<MessageModel>();
            this.messageService = messageService;
        }

        public MvxObservableCollection<MessageModel> Messages { get; }

        public override async Task Initialize()
        {
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            IsLoading = true;

            var allmessages = await messageService.GetMessages();

            
            
            IsLoading = false;
        }
    }
}