using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ZXing.Mobile;

namespace ConsumerOne.Mobile.Droid.Services
{
    public class QrCodeScanningService : IQrCodeScanningService
	{
		public async Task<string> ScanAsync()
		{
			var optionsCustom = new MobileBarcodeScanningOptions()
			{
				//UseFrontCameraIfAvailable = true
			};
			var scanner = new MobileBarcodeScanner()
			{
				TopText = "Aproxime a câmera do código de barra",
				BottomText = "Toque na tela para focar"
			};

			var scanResults = await scanner.Scan(optionsCustom);

			return (scanResults != null) ? scanResults.Text : string.Empty;
		}
	}
}
