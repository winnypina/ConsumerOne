using System;
using System.Threading.Tasks;

namespace ConsumerOne.Mobile.Services
{
	public interface IQrCodeScanningService
	{
		Task<string> ScanAsync();
	}
}
