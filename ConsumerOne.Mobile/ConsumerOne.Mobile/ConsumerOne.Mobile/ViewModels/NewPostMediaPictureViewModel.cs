using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels
{
    public class NewPostMediaPictureViewModel : BaseViewModel<PostModel>
    {
        private readonly IUserInteractionService _popupService;
        private PostModel _current;
        private ImageSource _image1;
        private ImageSource _image2;
        private ImageSource _image3;
        private ImageSource _image4;
        private ImageSource _image5;

        public NewPostMediaPictureViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserInteractionService popupService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _popupService = popupService;
            Image1 = ImageSource.FromFile("plus.png");
            Image2 = ImageSource.FromFile("plus.png");
            Image3 = ImageSource.FromFile("plus.png");
            Image4 = ImageSource.FromFile("plus.png");
            Image5 = ImageSource.FromFile("plus.png");
        }

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(ExecuteNextCommand, () => Image1 != null || Image2 != null || Image3 != null || Image4 != null || Image5 != null);

        private async Task ExecuteNextCommand()
        {
            await NavigationService.Navigate<NewPostLocationViewModel, PostModel>(_current);
        }

        public MvxAsyncCommand SkipCommand => new MvxAsyncCommand(async ()=> await NavigationService.Navigate<NewPostReviewViewModel,PostModel>(_current));

        public ImageSource Image1
        {
            get => _image1;
            set
            {
                SetProperty(ref _image1, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public ImageSource Image2
        {
            get => _image2;
            set
            {
                SetProperty(ref _image2, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public ImageSource Image3
        {
            get => _image3;
            set
            {
                SetProperty(ref _image3, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public ImageSource Image4
        {
            get => _image4;
            set
            {
                SetProperty(ref _image4, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        public ImageSource Image5
        {
            get => _image5;
            set
            {
                SetProperty(ref _image5, value);
                NextCommand.RaiseCanExecuteChanged();
            }
        }


        public MvxAsyncCommand<int> TakePictureCommand => new MvxAsyncCommand<int>(ExecuteTakePictureCommand);

        private async Task ExecuteTakePictureCommand(int value)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results =
                    await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera, Permission.Storage);
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await _popupService.DisplayMessage("Erro", ":( No camera available.");
                    return;
                }

                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayConfirmation("Foto", "Deseja tirar a foto ou utilizar uma da biblioteca?", "Camera", "Biblioteca", async result =>
                {
                    if (result)
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                        {
                            SaveToAlbum = true,
                            MaxWidthHeight = 100,
                            AllowCropping = true,
                            SaveMetaData = false
                        });

                        if (file != null)
                        {
                            switch (value)
                            {
                                case 1:
                                    Image1 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return new MemoryStream(data);
                                    });
                                    break;
                                case 2:
                                    Image2 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 3:
                                    Image3 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 4:
                                    Image4 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 5:
                                    Image5 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                            }
                        }
                    }
                    else
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            MaxWidthHeight = 100,
                            SaveMetaData = false
                        });

                        if (file != null)
                        {
                            switch (value)
                            {
                                case 1:
                                    Image1 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return new MemoryStream(data);
                                    });
                                    break;
                                case 2:
                                    Image2 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 3:
                                    Image3 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 4:
                                    Image4 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                                case 5:
                                    Image5 = ImageSource.FromStream(() =>
                                    {
                                        var stream = file.GetStream();
                                        var data = new byte[stream.Length];
                                        stream.Read(data, 0, data.Length);
                                        stream.Seek(0, SeekOrigin.Begin);
                                        _current.Images.Add(data);
                                        return stream;
                                    });
                                    break;
                            }
                        }
                    }
                });
            }
            else
            {
                await _popupService.DisplayMessage("Erro","Aplicativo sem permissão para fotos.");
            }
        }

        public override void Prepare(PostModel parameter)
        {
            _current = parameter;
            _current.Video = null;
            if (parameter.Images != null && parameter.Images.Any())
            {
                int count = 1;
                foreach (var image in parameter.Images)
                {
                    switch (count)
                    {
                        case 1:
                            Image1 = ImageSource.FromStream(() => new MemoryStream(image));
                            break;
                        case 2:
                            Image2 = ImageSource.FromStream(() => new MemoryStream(image));
                            break;
                        case 3:
                            Image3 = ImageSource.FromStream(() => new MemoryStream(image));
                            break;
                        case 4:
                            Image4 = ImageSource.FromStream(() => new MemoryStream(image));
                            break;
                        case 5:
                            Image5 = ImageSource.FromStream(() => new MemoryStream(image));
                            break;
                    }

                    count++;
                }
            }
        }
    }
}