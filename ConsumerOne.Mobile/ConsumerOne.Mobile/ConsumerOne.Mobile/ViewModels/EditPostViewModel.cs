using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class EditPostViewModel : BaseViewModel<PostModel>
    {
        private string _title;
        private string _description;
        private string _tags;

        private PostModel _current;

        public EditPostViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
#if DEBUG
            Title = "teste";
            Description = "teste";
            Tags = "teste";
#endif
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand, CanExecuteNextCommand);

        private async Task ExecuteNextCommand()
        {
            _current.Title = Title;
            _current.Description = Description;
            _current.Tags = Tags;
            await NavigationService.Navigate<NewPostPriceViewModel, PostModel>(_current);
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
            if (_current != null)
            {
                Prepare(_current);
            }
        }

        public override void Prepare(PostModel parameter)
        {
            _current = parameter;
            if (!string.IsNullOrEmpty(parameter.Title))
            {
                Title = parameter.Title;
            }

            if (!string.IsNullOrEmpty(parameter.Description))
            {
                Description = parameter.Description;
            }

            if (!string.IsNullOrEmpty(parameter.Tags))
            {
                Tags = parameter.Tags;
            }
        }
    }
}
