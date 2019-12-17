using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using ZXing;

namespace ConsumerOne.Mobile.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        private readonly IUserInteractionService userInteractionService;
        private readonly IQrCodeScanningService qrCodeScanningService;
        private Result _result;
        private bool _isAnalyzing;
        private bool _isScanning;

        public ScanViewModel(IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            ITranslationService translationService,
            IUserInteractionService userInteractionService,
            IQrCodeScanningService qrCodeScanningService) : base(logProvider, navigationService, translationService)
        {
            this.userInteractionService = userInteractionService;
            this.qrCodeScanningService = qrCodeScanningService;
        }

        public override async Task Initialize()
        {
            IsLoading = true;
            
        }

        public override async void ViewAppeared()
        {
            var result = await qrCodeScanningService.ScanAsync();
            if (string.IsNullOrEmpty(result))
            {
                await NavigationService.Close(this);
                return;
            }
            if(!result.StartsWith("CO|"))
            {
                await userInteractionService.DisplayMessage("Desculpe", "Este QRCode não parece ter sido gerado pelo Consumer One. Por favor tente novamente");
                await NavigationService.Close(this);
                return;
            }
            var data = result.Split('|');
            var area = data[1];
            var id = data[2];
            if(area == "Profile")
            {
                await NavigationService.Close(this);
                await NavigationService.Navigate<ProfileWindowViewModel, ProfileModel>(new ProfileModel { Id = id });
                return;
            }
            if (area == "Post")
            {
                await NavigationService.Close(this);
                await NavigationService.Navigate<PostWindowViewModel, Guid>(Guid.Parse(id));
            }
        }
    }
}