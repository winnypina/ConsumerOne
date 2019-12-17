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
    public class RatingViewModel : BaseViewModel<ProfileModel>
    {
        private readonly ILoginService _loginService;

        private ProfileModel _parameter;
        private bool _isOneStar;
        private bool _isTwoStar;
        private bool _isThreeStar;
        private bool _isFourStar;
        private bool _isFiveStar;
        private string _description;
        private bool _hasRating;

        public RatingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ITranslationService translationService, ILoginService loginService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            Ratings = new MvxObservableCollection<RatingResponse>();
        }
        
        public override void Prepare(ProfileModel parameter)
        {
            _parameter = parameter;
            HasRating = _parameter.HasRating;
        }

        public bool IsOneStar
        {
            get => _isOneStar;
            set => SetProperty(ref _isOneStar, value);
        }

        public bool IsTwoStar
        {
            get => _isTwoStar;
            set => SetProperty(ref _isTwoStar, value);
        }

        public bool IsThreeStar
        {
            get => _isThreeStar;
            set => SetProperty(ref _isThreeStar, value);
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public bool IsFourStar
        {
            get => _isFourStar;
            set => SetProperty(ref _isFourStar, value);
        }

        public bool IsFiveStar
        {
            get => _isFiveStar;
            set { SetProperty(ref _isFiveStar, value); }
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description,value);
        }

        public bool HasRating
        {
            get => _hasRating;
            set => SetProperty(ref _hasRating,value);
        }

        public MvxCommand<int> SetRatingCommand => new MvxCommand<int>(rating =>
        {
            if (rating == 1)
            {
                IsOneStar = true;
                IsTwoStar = false;
                IsThreeStar = false;
                IsFourStar = false;
                IsFiveStar = false;
            }
            if (rating == 2)
            {
                IsOneStar = true;
                IsTwoStar = true;
                IsThreeStar = false;
                IsFourStar = false;
                IsFiveStar = false;
            }
            if (rating == 3)
            {
                IsOneStar = true;
                IsTwoStar = true;
                IsThreeStar = true;
                IsFourStar = false;
                IsFiveStar = false;
            }
            if (rating == 4)
            {
                IsOneStar = true;
                IsTwoStar = true;
                IsThreeStar = true;
                IsFourStar = true;
                IsFiveStar = false;
            }
            if (rating == 5)
            {
                IsOneStar = true;
                IsTwoStar = true;
                IsThreeStar = true;
                IsFourStar = true;
                IsFiveStar = true;
            }
        });

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;

            var rating = 0;
            if (IsFiveStar)
            {
                rating = 5;
            }
            else
            {
                if (IsFourStar)
                {
                    rating = 4;
                }
                else
                {
                    if (IsThreeStar)
                    {
                        rating = 3;
                    }
                    else
                    {
                        if (IsTwoStar)
                        {
                            rating = 2;
                        }
                        else
                        {
                            if (IsOneStar)
                            {
                                rating = 1;
                            }
                            else
                            {
                                rating = 0;
                            }
                        }
                    }
                }
            }

            await _loginService.PostRating(new RatingResponse
            {
                FromId = _loginService.Account.Id,
                Message = Description,
                Score = rating,
                SentDate = DateTime.UtcNow,
                ToId = _parameter.Id                
            }).ConfigureAwait(false);

            HasRating = true;

            await LoadRatings();
            
            IsLoading = false;
        });

        public override async Task Initialize()
        {
            await LoadRatings();
        }

        public MvxObservableCollection<RatingResponse> Ratings { get; }

        private async Task LoadRatings()
        {
            IsLoading = true;

            IEnumerable<RatingResponse> ratings;

            if (string.IsNullOrEmpty(_parameter.Id))
            {
                ratings = await _loginService.GetRating().ConfigureAwait(false);
            }
            else
            {
                ratings = await _loginService.GetRatingForUser(_parameter.Id).ConfigureAwait(false);
            }
            if(Ratings.Count > 0)
            {
                Ratings.RemoveRange(0, Ratings.Count);
            }
            Ratings.AddRange(ratings.OrderByDescending(n => n.SentDate).ToList());
            IsLoading = false;
        }
    }
}