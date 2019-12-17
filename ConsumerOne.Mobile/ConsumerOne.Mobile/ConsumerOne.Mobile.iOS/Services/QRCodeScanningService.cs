using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using UIKit;
using ZXing.Mobile;

namespace ConsumerOne.Mobile.iOS.Services
{
    public class QrCodeScanningService : IQrCodeScanningService
    {
        public async Task<string> ScanAsync()
        {

            var scanner = new MobileBarcodeScanner(GetTopViewController())
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar"
            };
            var scanResults = await scanner.Scan();
            //Fix by Ale
            return (scanResults != null) ? scanResults.Text : string.Empty;

        }

        private UIViewController GetTopViewController()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }
            return viewController;
        }
    }
}
