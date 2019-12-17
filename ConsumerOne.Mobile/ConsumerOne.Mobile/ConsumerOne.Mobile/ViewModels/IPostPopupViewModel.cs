using System;
using MvvmCross.Commands;

namespace ConsumerOne.Mobile.ViewModels
{
    public interface IPostPopupViewModel
    {
        bool OwnPost { get; set; }
        MvxAsyncCommand EditPostCommand { get; }
        MvxAsyncCommand PausePostCommand { get; }
        MvxAsyncCommand DeletePostCommand { get; }
        MvxAsyncCommand TraceRouteCommand { get; }
        MvxAsyncCommand ReportPostCommand { get; }
        MvxAsyncCommand DismissPopupCommand { get; }
    }
}
