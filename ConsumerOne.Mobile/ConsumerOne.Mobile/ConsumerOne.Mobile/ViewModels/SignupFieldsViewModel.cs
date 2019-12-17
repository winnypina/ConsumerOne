using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.Services.Requests;
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
    public class SignupFieldsViewModel : BaseViewModel<string>
    {
        private readonly ILoginService _loginService;
        private readonly IUserInteractionService _popupService;
        private readonly IMediaService _mediaService;
        private string _confirmPassword;
        private string _email;

        private byte[] _encodedImage;
        private ImageSource _image;
        private ImageSource _defaultImage;
        private bool _isEmail;
        private bool _isPhone;

        private string _loginLabel;
        private string _name;
        private string _password;
        private string _phone;

        private string _signupType;
        private string _title;

        public SignupFieldsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService,
            ILoginService loginService, IUserInteractionService popupService, IMediaService mediaService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _loginService = loginService;
            _popupService = popupService;
            _mediaService = mediaService;
            IsPhone = true;
            IsEmail = false;
            LoginLabel = "Telefone";
        }

        public override void Prepare()
        {
            SetScreenName("Campos Cadastro");
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

        public bool IsPhone
        {
            get => _isPhone;
            set => SetProperty(ref _isPhone, value);
        }

        public bool IsEmail
        {
            get => _isEmail;
            set => SetProperty(ref _isEmail, value);
        }

        public string LoginLabel
        {
            get => _loginLabel;
            set => SetProperty(ref _loginLabel, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                SetProperty(ref _phone, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxCommand ChangeModeCommand => new MvxCommand(ExecuteChangeModeCommand);

        public MvxAsyncCommand TakePictureCommand => new MvxAsyncCommand(ExecuteTakePictureCommand);

        public MvxAsyncCommand SendCommand => new MvxAsyncCommand(ExecuteSendCommand);

        public override void Prepare(string parameter)
        {
            _signupType = parameter;
            switch (_signupType)
            {
                case "Provider":
                    DefaultImage = "providerPic.png";
                    break;
                case "Consumer":

                    DefaultImage = "consumerPic.png";
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
            Title = Texts.SingleOrDefault(n => n.Key == "Titulo telefone")?.Value;
        }

        private async Task ExecuteTakePictureCommand()
        {
            IsLoading = true;
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayConfirmation("Foto", "Deseja tirar a foto ou utilizar uma da biblioteca?", "Camera", "Biblioteca", async result =>
                {
                    if (result)
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                        {
                            SaveToAlbum = true,
                            MaxWidthHeight = 100,
                            RotateImage = false
                        });

                        if (file != null)
                        {
                            Image = file?.Path;
                        }
                    }
                    else
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            MaxWidthHeight = 100
                        });

                        if (file != null)
                        {
                            Image = file?.Path;
                        }
                    }
                });
            }
            else
            {
                await Mvx.IoCProvider.Resolve<IUserInteractionService>().DisplayMessage("Permissão de camera negada", "Por favor verifique as configurações do aplicativo no dispositivo");
                CrossPermissions.Current.OpenAppSettings();
            }
            IsLoading = false;
        }

        private void ExecuteChangeModeCommand()
        {
            IsPhone = !IsPhone;
            IsEmail = !IsEmail;
            LoginLabel = IsPhone ? "Telefone" : "Email";
            Email = string.Empty;
            Phone = string.Empty;
            Title = IsPhone ? Texts.SingleOrDefault(n => n.Key == "Titulo telefone")?.Value : Texts.SingleOrDefault(n => n.Key == "Titulo email")?.Value;

#if DEBUG
            Name = _signupType == "Provider" ? "Ricardo Fornecedor" : "Ricardo Consumidor";
            if (IsPhone)
            {
                Phone = "15996703410";
            }
            else
            {
                Email = "ricardozas@gmail.com";
            }

            Password = "teste@123";
            ConfirmPassword = "teste@123";
#endif
        }


        private async Task ExecuteSendCommand()
        {
            IsLoading = true;

            if (string.IsNullOrEmpty(Name) || (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Phone)) || string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(ConfirmPassword))
            {
                await _popupService.DisplayMessage("Preenchimento",Texts.SingleOrDefault(n => n.Key == "Preencher campos")?.Value);
                IsLoading = false;
                return;
            }

            if (IsEmail)
            {
                var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var match = regex.Match(Email);
                if (!match.Success)
                {
                    await _popupService.DisplayMessage("Preenchimento",Texts.SingleOrDefault(n => n.Key == "Email invalido")?.Value);
                    IsLoading = false;
                    return;
                }
            }

            if (IsPhone)
            {
                var regex = new Regex(@"^\(\d{2}\)9\d{4}-\d{4}|\((?:1[2-9]|[2-9]\d)\) [5-9]\d{3}-\d{4}$");
                var match = regex.Match(Phone);
                if (!match.Success)
                {
                    await _popupService.DisplayMessage("Erro", Texts.SingleOrDefault(n => n.Key == "Telefone invalido")?.Value);
                    IsLoading = false;
                    return;
                }
            }

            if (Password != ConfirmPassword)
            {
                await _popupService.DisplayMessage("Erro", Texts.SingleOrDefault(n => n.Key == "Senhas diferentes")?.Value);
                IsLoading = false;
                return;
            }
            var login = !string.IsNullOrEmpty(Email) ? Email : Phone;
            var emailLogin = login.Contains("@") ? login : "no@email.com";
            var phoneLogin = !login.Contains("@") ? login : null;
            var account = new CreateAccountRequest
            {
                CpfCnpj = null,
                Type = _signupType,
                Name = Name,
                Password = Password,
                Email = emailLogin,
                BirthDate = null,
                Address = null,
                MobilePhone = phoneLogin
            };

            if (IsPhone)
            {
                await _popupService.DisplayConfirmation("Confirmação",
                    $"{Texts.SingleOrDefault(n => n.Key == "Confirmação Telefone")?.Value}: {login}",
                    Texts.SingleOrDefault(n => n.Key == "Confirmar")?.Value, Texts.SingleOrDefault(n => n.Key == "Cancelar")?.Value,
                    async result =>
                    {
                        if (result)
                        {
                            if (await _loginService.Signup(Name, login, Password, null, _signupType)
                                                        .ConfigureAwait(false))
                            {
                                if (_encodedImage != null)
                                {
                                    await _mediaService.Upload(_encodedImage, _loginService.Account.Id + ".jpg");
                                }

                                await NavigationService.Navigate<CodeVerificationViewModel, CreateAccountRequest>(account);
                            }
                            else
                            {
                                await _popupService.DisplayMessage("Erro",
                                    "Ocorreu um problema ao efetuar seu registro. Por favor tente novamente mais tarde");
                            }
                        }

                    });
            }
            else
            {
                await _popupService.DisplayConfirmation("Confirmação",
                    $"{Texts.SingleOrDefault(n => n.Key == "Confirmação Email")?.Value}: {login}",
                    Texts.SingleOrDefault(n => n.Key == "Confirmar")?.Value, Texts.SingleOrDefault(n => n.Key == "Cancelar")?.Value,
                    async result =>
                    {
                        if (result)
                        {
                            if (await _loginService.Signup(Name, login, Password, null, _signupType)
                            .ConfigureAwait(false))
                            {
                                if (_encodedImage != null)
                                {
                                    await _mediaService.Upload(_encodedImage, _loginService.Account.Id + ".jpg");
                                }

                                await NavigationService.Navigate<CodeVerificationViewModel, CreateAccountRequest>(account);
                            }
                            else
                            {
                                await _popupService.DisplayMessage("Erro",
                                    "Ocorreu um problema ao efetuar seu registro. Por favor tente novamente mais tarde");
                            }
                        }
                    });
            }

            IsLoading = false;
        }

        private async Task VerifyCode(string code)
        {
            var login = !string.IsNullOrEmpty(Email) ? Email : Phone;
            if (await _loginService.VerifyCode(login, Convert.ToInt32(code)))
            {
                if (await _loginService.Login(login, Password))
                {
                    await _popupService.DisplayConfirmation(Texts.SingleOrDefault(n => n.Key == "Sucesso Titulo")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Descrição")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Confirmação")?.Value, Texts.SingleOrDefault(n => n.Key == "Sucesso Cancelar")?.Value,
                        async result =>
                        {
                            if (result)
                            {
                                await NavigationService.Navigate<EditProfileViewModel>();
                            }
                            else
                            {
                                await NavigationService.Navigate<TabsMasterViewModel>();
                            }
                        }
                    );
                }
                else
                {
                    await _popupService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de verificação do código. Tente novamente mais tarde!");
                }


            }
            else
            {
                await _popupService.DisplayMessage("Erro", "Ocorreu um erro na tentiva de verificação do código. Tente novamente mais tarde!");
            }
        }
    }


}