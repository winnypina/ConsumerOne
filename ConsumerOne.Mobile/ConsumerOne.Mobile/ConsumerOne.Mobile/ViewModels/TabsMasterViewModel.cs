using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Responses;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class TabsMasterViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;

        private bool _hasRun;

        public TabsMasterViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService, ILoginService loginService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
        }

        public override async void ViewAppearing()
        {
            await ShowInitialViewModels();
            base.ViewAppearing();
        }

        private async Task ShowInitialViewModels()
        {
            if (!_hasRun)
            {
                _hasRun = true;
                if(_loginService != null && _loginService.Account != null)
                {
                    if (_loginService.Account.Type == UserType.Provider)
                    {
                        var tasks = new List<Task>
                    {
                        NavigationService.Navigate<HomeViewModel>(),
                        NavigationService.Navigate<SearchViewModel>(),
                        NavigationService.Navigate<NewPostViewModel,PostModel>(new PostModel()),
                        NavigationService.Navigate<MessagesViewModel>(),
                        NavigationService.Navigate<MyProfileViewModel, ProfileModel>( new ProfileModel { Id = _loginService.Account.Id })
                    };
                        await Task.WhenAll(tasks);
                    }
                    else
                    {
                        var tasks = new List<Task>
                    {
                        NavigationService.Navigate<HomeViewModel>(),
                        NavigationService.Navigate<SearchViewModel>(),
                        NavigationService.Navigate<MessagesViewModel>(),
                        NavigationService.Navigate<MyProfileViewModel, ProfileModel>( new ProfileModel { Id = _loginService.Account.Id })
                    };
                        await Task.WhenAll(tasks);
                    }
                } else
                {
                    await NavigationService.Navigate<SplashViewModel>();
                }
               
            }
            
        }
    }
}
