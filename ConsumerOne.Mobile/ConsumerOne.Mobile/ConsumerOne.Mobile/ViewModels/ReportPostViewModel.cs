using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class ReportPostViewModel : BaseViewModel<PostListModel>
    {
        public ReportPostViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserInteractionService userInteractionService,
            ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            this.userInteractionService = userInteractionService;
        }

        private PostListModel parameter;
        private readonly IUserInteractionService userInteractionService;

        public override void Prepare(PostListModel parameter)
        {
            this.parameter = parameter;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () => {

            await userInteractionService.DisplayMessage("Obrigado", "Sua denúncia foi enviada com sucesso");
            await NavigationService.Close(this);
        });

        public override Task Initialize()
        {
            return base.Initialize();
        }
    }
}
