using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.ViewModels;

namespace ConsumerOne.Mobile.Services
{
    public interface IPopupService
    {

        Task ShowPostOptions(IPostPopupViewModel viewModel);
        Task Dismiss();
    }
}
