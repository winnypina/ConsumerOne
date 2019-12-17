using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostViewModel : BaseViewModel<PostModel>
    {
        private string _title;
        private string _description;
        private string _tags;

        public static PostModel Current;

        public NewPostViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
#if DEBUG
            Title = "teste";
            Description = "teste";
            Tags = "teste";
#endif
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand,CanExecuteNextCommand);

        private async Task ExecuteNextCommand()
        {
            Current.Title = Title;
            Current.Description = Description;
            Current.Tags = Tags;   
            await NavigationService.Navigate<NewPostPriceViewModel, PostModel>(Current);
        }

        private bool CanExecuteNextCommand()
        {
            return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Tags);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        public override void ViewAppeared()
        {
            if(Current != null)
            {
                Prepare(Current);
            }
        }

        public override void Prepare(PostModel parameter)
        {
            if(parameter != null)
            {
                Current = parameter;
            }
            
            if (!string.IsNullOrEmpty(Current.Title))
            {
                Title = Current.Title;
            }

            if (!string.IsNullOrEmpty(Current.Description))
            {
                Description = Current.Description;
            }

            if (!string.IsNullOrEmpty(Current.Tags))
            {
                Tags = Current.Tags;
            }  
        }
    }
}
