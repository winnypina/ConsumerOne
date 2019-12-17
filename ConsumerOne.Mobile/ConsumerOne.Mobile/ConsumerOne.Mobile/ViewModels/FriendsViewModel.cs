using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Responses;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public class FriendsViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;

        private IEnumerable<AccountResponse> _people;
        private string _term;
        private string _title;

        public FriendsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ITranslationService translationService, ILoginService loginService) : base(logProvider, navigationService,
            translationService)
        {
            _loginService = loginService;
            People = new MvxObservableCollection<AccountResponse>();
        }

        public MvxAsyncCommand<AccountResponse> SelectCommand => new MvxAsyncCommand<AccountResponse>(async item =>
            {
                await NavigationService.Navigate<ProfileViewModel, ProfileModel>(new ProfileModel {Id = item.Id});
            });

        public MvxObservableCollection<AccountResponse> People { get; }

        public string Term
        {
            get => _term;
            set
            {
                if (SetProperty(ref _term, value)) Filter();
            }
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public override void Prepare()
        {
            Title = _loginService.Account.Type == UserType.Consumer ? "Seguindo" : "Seguidores";
            base.Prepare();
        }


        private void Filter()
        {
            if(People.Count > 0)
            {
                People.RemoveRange(0, People.Count);
            }
            
            if (_people != null)
            {
                if (string.IsNullOrEmpty(Term))
                    _people.ToList().ForEach(People.Add);
                else
                    _people.Where(n => n.Name.IndexOf(Term, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList()
                        .ForEach(People.Add);
            }
        }

        public override async void ViewAppeared()
        {
            IsLoading = true;

            if (People.Count > 0)
            {
                People.RemoveRange(0, People.Count);
            }
            if (_loginService.Account.Type == UserType.Consumer)
                _people = await _loginService.GetFollows();
            else
                _people = await _loginService.GetFollowers();

            _people.ToList().ForEach(People.Add);

            IsLoading = false;
        }
    }
}