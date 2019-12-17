using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostMediaViewModel : BaseViewModel<PostModel>
    {

        private PostModel _current;
        private bool _isImage;
        private bool _isVideo;

        public NewPostMediaViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            
        }

        public override void Prepare(PostModel parameter)
        {
            _current = parameter;
            IsImage = parameter.Images != null && parameter.Images.Any();
            IsVideo = parameter.Video != null;

            if (!IsImage && !IsVideo)
            {
                IsImage = true;
            }
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand);

        private async Task ExecuteNextCommand()
        {
            if (IsImage)
            {
                await NavigationService.Navigate<NewPostMediaPictureViewModel, PostModel>(_current);
            }
            else
            {
                await NavigationService.Navigate<NewPostMediaVideoViewModel, PostModel>(_current);
            }
        }

        public bool IsImage
        {
            get => _isImage;
            set => SetProperty(ref _isImage,value);
        }

        public bool IsVideo
        {
            get => _isVideo;
            set => SetProperty(ref _isVideo, value);
        }

        public MvxCommand ChooseImageCommand => new MvxCommand(() =>
        {
            IsImage = true;
            IsVideo = false;
        });

        public MvxCommand ChooseVideoCommand => new MvxCommand(() =>
        {
            IsImage = false;
            IsVideo = true;
        });
    }
}
