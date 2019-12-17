using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile.ViewModels
{
    public abstract class BaseViewModel : MvxNavigationViewModel
    {
        private readonly ITranslationService _translationService;
        private bool _isLoading;
        public static BaseViewModel Current;

        protected BaseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(
            logProvider, navigationService)
        {
            _translationService = translationService;
            Current = this;
        }

        protected virtual void SetScreenName(string name)
        {
            Texts = new MvxObservableCollection<TranslationModel>(_translationService.TranslatedTexts[name]);
        }

        public MvxObservableCollection<TranslationModel> Texts { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading ,value);
        }
    }

    public abstract class BaseViewModel<TParameter> : MvxNavigationViewModel<TParameter>
    {
        private readonly ITranslationService _translationService;
        private bool _isLoading;
        private MvxObservableCollection<TranslationModel> _texts;

        protected BaseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(
            logProvider, navigationService)
        {
            _translationService = translationService;

        }

        protected virtual void SetScreenName(string name)
        {
            Texts = new MvxObservableCollection<TranslationModel>(_translationService.TranslatedTexts[name]);
        }

        public MvxObservableCollection<TranslationModel> Texts
        {
            get => _texts;
            set => _texts = value;
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
    }
}