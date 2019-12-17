using System;
using System.IO;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Controls.VideoPlayerControl;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostMediaVideoViewModel : BaseViewModel<PostModel>
    {
        private readonly IUserInteractionService _popupService;

        private PostModel _current;
        private bool _hasVideo;
        private VideoSource _videoSource;

        public NewPostMediaVideoViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserInteractionService popupService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _popupService = popupService;
        }

        public override void Prepare(PostModel parameter)
        {
            _current = parameter;
            _current.Images.Clear();
            if (_current.Video != null)
            {
                HasVideo = true;
                VideoSource = VideoSource.FromStream(new MemoryStream(_current.Video));
            }
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand, () => HasVideo);

        private async Task ExecuteNextCommand()
        {
            await NavigationService.Navigate<NewPostLocationViewModel, PostModel>(_current);
        }

        public MvxAsyncCommand SkipCommand => new MvxAsyncCommand(async () => await NavigationService.Navigate<NewPostReviewViewModel, PostModel>(_current));

        public bool HasVideo
        {
            get => _hasVideo;
            set
            {
                SetProperty(ref _hasVideo, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public VideoSource VideoSource
        {
            get => _videoSource;
            set => SetProperty(ref _videoSource, value);
        }

        public MvxAsyncCommand RecordVideoCommand => new MvxAsyncCommand(ExecuteRecordVideoCommand);

        private async Task ExecuteRecordVideoCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakeVideoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await _popupService.DisplayMessage("Camera", "Nenhuma câmera detectada.");
                return;
            }

            await _popupService.DisplayConfirmation("Camera", "Deseja utilizar a camera ou escolher um vídeo existente?", "Camera", "Biblioteca",
                async result =>
                {
                    if (result)
                    {
                        var file = await CrossMedia.Current.TakeVideoAsync(
                        new StoreVideoOptions
                        {
                            SaveToAlbum = true,
                            Directory = "Demo",
                            Quality = VideoQuality.Medium
                        });

                        if (file == null)
                            return;

                        var stream = file.GetStream();
                        var data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                        _current.Video = data;

                        HasVideo = true;

                        VideoSource = VideoSource.FromFile(file.Path);
                    }
                    else
                    {
                        var file = await CrossMedia.Current.PickVideoAsync();

                        if (file == null)
                            return;

                        var stream = file.GetStream();
                        var data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                        _current.Video = data;

                        HasVideo = true;

                        VideoSource = VideoSource.FromFile(file.Path);
                    }
                });

        }
    }
}