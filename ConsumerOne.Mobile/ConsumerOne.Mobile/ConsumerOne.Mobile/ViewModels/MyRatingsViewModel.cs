using System;
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
    public class MyRatingViewModel : BaseViewModel
    {
        private readonly ILoginService loginService;
        private int totalRatings;

        public MyRatingViewModel(IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            ILoginService loginService,
            ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            this.loginService = loginService;
            Ratings = new MvxObservableCollection<RatingResponse>();
        }

        public MvxObservableCollection<RatingResponse> Ratings { get; }
        public int TotalRatings { get => totalRatings; set => SetProperty(ref totalRatings,value); }

        public override async Task Initialize()
        {
            IsLoading = true;
            if(Ratings. Count > 0)
            {
                Ratings.RemoveRange(0, Ratings.Count);
            }
            var ratings = (await loginService.GetRatingForUser(loginService.Account.Id)).OrderByDescending(n=>n.SentDate).ToList();
            Ratings.AddRange(ratings);
            TotalRatings = Ratings.Count;
            IsLoading = false;
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

    }
}
