using System;
using System.Threading;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;
        private readonly ITranslationService _translationService;

        public SplashViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, ITranslationService translationService) : base(logProvider,
            navigationService, translationService)
        {
            _loginService = loginService;
            _translationService = translationService;
        }


        public override void ViewAppeared()
        {
            Task.Factory.StartNew(LoadLogin);
        }


        private async Task LoadLogin()
        {
            await _translationService.GetTranslations().ConfigureAwait(false);

            if (!await _loginService.LoadCachedUser().ConfigureAwait(false))
            {
                var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
                if (currentCulture.Contains("pt-BR"))
                {
                    _translationService.SetCurrentLanguage(Languages.PtBr);
                }
                else if (currentCulture.Contains("en-US"))
                {
                    _translationService.SetCurrentLanguage(Languages.EnUs);
                }
                else if (currentCulture.Contains("es-ES"))
                {
                    _translationService.SetCurrentLanguage(Languages.Es);
                }
                else
                {
                    _translationService.SetCurrentLanguage(Languages.EnUs);
                }

                await NavigationService.Navigate<WelcomeViewModel>();
            }
            else
            {
                if (!string.IsNullOrEmpty(_loginService.Account.DesiredLanguage))
                {
                    if (_loginService.Account.DesiredLanguage == "1")
                    {
                        _translationService.SetCurrentLanguage(Languages.PtBr);
                    }
                    else if (_loginService.Account.DesiredLanguage == "2")
                    {
                        _translationService.SetCurrentLanguage(Languages.EnUs);
                    }
                    else if (_loginService.Account.DesiredLanguage == "3")
                    {
                        _translationService.SetCurrentLanguage(Languages.Es);
                    }
                    else
                    {
                        _translationService.SetCurrentLanguage(Languages.EnUs);
                    }
                }
                else
                {
                    _translationService.SetCurrentLanguage(Languages.EnUs);
                }
                await NavigationService.Navigate<TabsMasterViewModel>();
                
               
            }

            
            //await NavigationService.Navigate<NewPostMediaPictureViewModel,PostModel>(new PostModel
            //{
            //    Address = "Rua Natal, 321, Sorocaba",
            //    Currency = "BRL",
            //    Description = "teste",
            //    Id = Guid.NewGuid(),
            //    StoreLink = "http://google.com.br",
            //    Title = "teste",
            //    Tags = "teste",
            //    Value = 30
            //});
        }
    }
}