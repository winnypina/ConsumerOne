using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Responses;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;
        private readonly IMediaService _mediaService;
        private readonly ITranslationService _translationService;
        private readonly IUserInteractionService _popupService;
        private string _about;
        private string _addressAddon;
        private string _addressNumber;
        private string _businessPhone;
        private string _cep;
        private string _choosenLanguage;
        private string _city;
        private string _country;
        private string _email;
        private byte[] _encodedImage;
        private string _fullAddress;
        private ImageSource _image;
        private bool _isProvider;
        private string _name;
        private string _phone;
        private string _site;
        private string _state;
        private ImageSource _defaultImage;
        private DateTime _birthDate;
        private DateTime _maxDate;
        private DateTime _minDate;

        public EditProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, IUserInteractionService popupService, IMediaService mediaService,
            ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _popupService = popupService;
            _mediaService = mediaService;
            _translationService = translationService;
            Languages = new MvxObservableCollection<string>();
        }

        public MvxObservableCollection<string> Languages { get; }

        public bool IsProvider
        {
            get => _isProvider;
            set => SetProperty(ref _isProvider, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string About
        {
            get => _about;
            set => SetProperty(ref _about, value);
        }

        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
        }

        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }

        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string BusinessPhone
        {
            get => _businessPhone;
            set => SetProperty(ref _businessPhone, value);
        }

        public string FullAddress
        {
            get => _fullAddress;
            set => SetProperty(ref _fullAddress, value);
        }

        public string AddressNumber
        {
            get => _addressNumber;
            set => SetProperty(ref _addressNumber, value);
        }

        public string AddressAddon
        {
            get => _addressAddon;
            set => SetProperty(ref _addressAddon, value);
        }

        public string Cep
        {
            get => _cep;
            set => SetProperty(ref _cep, value);
        }

        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public string State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public string Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }

        public string ChosenLanguage
        {
            get => _choosenLanguage;
            set => SetProperty(ref _choosenLanguage, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public ImageSource DefaultImage
        {
            get => _defaultImage;
            set => SetProperty(ref _defaultImage, value);
        }

        public DateTime BirthDate

        {
            get => _birthDate;
            set => SetProperty(ref _birthDate, value);
        }

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;

            if (await _loginService.UpdateAccount(new AccountResponse
            {
                Address = FullAddress,
                Email = string.IsNullOrEmpty(Email) ? "no@email.com" : Email,
                Name = Name,
                AddressAddon = AddressAddon,
                State = State,
                About = About,
                AddressNumber = AddressNumber,
                BusinessPhone = BusinessPhone,
                Country = Country,
                Website = Site,
                DesiredLanguage = GetDesiredLanguage(),
                Id = _loginService.Account.Id,
                Type = _loginService.Account.Type,
                MobilePhone = Phone,
                BirthDate = IsProvider ? (DateTime?) null : BirthDate,
                Cep = Cep,
                City = City
            }).ConfigureAwait(false))
            {
                if (_encodedImage != null)
                {
                    await _mediaService.Upload(_encodedImage, $"{_loginService.Account.Id}.jpg").ConfigureAwait(false);
                }

                await NavigationService.Close(this);
                await NavigationService.Navigate<TabsMasterViewModel>();
            }

            IsLoading = false;
        });

        private string GetDesiredLanguage()
        {
            if (ChosenLanguage == Texts.SingleOrDefault(n => n.Key == "Língua Portugues")?.Value)
            {
                _translationService.SetCurrentLanguage(Services.Languages.PtBr);
                return "1";
            }

            if (ChosenLanguage == Texts.SingleOrDefault(n => n.Key == "Língua Inglês")?.Value)
            {
                _translationService.SetCurrentLanguage(Services.Languages.EnUs);
                return "2";
            }

            if (ChosenLanguage == Texts.SingleOrDefault(n => n.Key == "Língua Espanhol")?.Value)
            {
                _translationService.SetCurrentLanguage(Services.Languages.Es);
                return "3";
            }

            return null;
        }

        public MvxAsyncCommand ChangePasswordCommand => new MvxAsyncCommand(async () =>
        {
            await NavigationService.Navigate<ChangePasswordViewModel>();
        });

        public MvxAsyncCommand TakePictureCommand => new MvxAsyncCommand(ExecuteTakePictureCommand);

        private async Task ExecuteTakePictureCommand()
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
                    await _popupService.DisplayMessage("", ":( No camera available.");
                    return;
                }

                await _popupService.DisplayConfirmation("Camera","Deseja utilizar a camera ou escolher uma foto existente?","Camera","Biblioteca",
                    async result =>
                    {
                        if(!result)
                        {
                            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                            {
                                MaxWidthHeight = 100
                            });

                            if (file == null)
                                return;

                            Image = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                var data = new byte[stream.Length];
                                stream.Read(data, 0, data.Length);
                                stream.Seek(0, SeekOrigin.Begin);
                                _encodedImage = data;
                                return stream;
                            });
                        }
                        else
                        {
                            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Pictures",
                                Name = "picture.jpg",
                                AllowCropping = true,
                                MaxWidthHeight = 100,
                                RotateImage = false
                            });

                            if (file == null)
                                return;

                            Image = ImageSource.FromStream(() =>
                            {
                                var stream = file.GetStream();
                                var data = new byte[stream.Length];
                                stream.Read(data, 0, data.Length);
                                stream.Seek(0, SeekOrigin.Begin);
                                _encodedImage = data;
                                return stream;
                            });
                        }
                       
                    });

              
            }
            else
            {
                await _popupService.DisplayMessage("Permissão","Aplicativo sem permissão para fotos.");
            }
        }

        public override void Prepare()
        {
            SetScreenName("Editar Perfil");

            if(Languages.Count > 0)
            {
                Languages.RemoveRange(0, Languages.Count);
            }
            

            var list = new List<string>();
            list.Add(Texts.SingleOrDefault(n => n.Key == "Língua Portugues")?.Value);
            list.Add(Texts.SingleOrDefault(n => n.Key == "Língua Inglês")?.Value);
            list.Add(Texts.SingleOrDefault(n => n.Key == "Língua Espanhol")?.Value);

            list.OrderBy(n => n).ToList().ForEach(Languages.Add);

            if (_loginService.Account.Type == UserType.Provider) IsProvider = true;

            MinDate = DateTime.Now.AddYears(-100);
            MaxDate = DateTime.Now;

            BirthDate = _loginService.Account.BirthDate ?? DateTime.Now;
            DefaultImage = ImageSource.FromFile(IsProvider ? "defaultProvider.png" : "defaultUser.png");
            Image = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{_loginService.Account.Id}.jpg"));
            Name = _loginService.Account.Name;
            About = _loginService.Account.About;
            Site = _loginService.Account.Website;
            Email = _loginService.Account.Email == "no@email.com" ? "" : _loginService.Account.Email;
            Phone = _loginService.Account.MobilePhone;
            BusinessPhone = _loginService.Account.BusinessPhone;
            FullAddress = _loginService.Account.Address;
            AddressAddon = _loginService.Account.AddressAddon;
            AddressNumber = _loginService.Account.AddressNumber;
            State = _loginService.Account.State;
            City = _loginService.Account.City;
            Country = _loginService.Account.Country;
            if (string.IsNullOrEmpty(_loginService.Account.DesiredLanguage))
            {
                switch (_translationService.CurrentLanguage)
                {
                    case Services.Languages.PtBr:
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Portugues")?.Value;
                        break;
                    case Services.Languages.EnUs:
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Inglês")?.Value;
                        break;
                    case Services.Languages.Es:
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Espanhol")?.Value;
                        break;
                }
            }
            else
            {
                switch (_loginService.Account.DesiredLanguage)
                {
                    case "1":
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Portugues")?.Value;
                        break;
                    case "2":
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Inglês")?.Value;
                        break;
                    case "3":
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Espanhol")?.Value;
                        break;
                    default:
                        ChosenLanguage = Texts.SingleOrDefault(n => n.Key == "Língua Inglês")?.Value;
                        break;
                }

                base.Prepare();
            }
        }
    }
}