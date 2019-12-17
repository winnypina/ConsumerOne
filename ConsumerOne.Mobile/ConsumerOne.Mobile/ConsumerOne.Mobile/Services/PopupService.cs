using System.Threading.Tasks;
using ConsumerOne.Mobile.Popups;
using ConsumerOne.Mobile.ViewModels;
using MvvmCross;
using MvvmCross.Base;
using Rg.Plugins.Popup.Services;

namespace ConsumerOne.Mobile.Services
{
    public class PopupService : IPopupService
    {
        public async Task Dismiss()
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                await PopupNavigation.Instance.PopAllAsync();
            });
        }

        public async Task ShowPostOptions(IPostPopupViewModel viewModel)
        {
            await Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(async () =>
            {
                await PopupNavigation.Instance.PushAsync(new PostOptionsPopup(viewModel));
            });
        }
    }
}
