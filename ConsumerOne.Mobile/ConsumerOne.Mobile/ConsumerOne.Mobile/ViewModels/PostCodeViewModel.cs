using System;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.ViewModels
{
    public class PostCodeViewModel : BaseViewModel<PostListModel>
    {
        public PostCodeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ILoginService loginService, IImageService imageService, ITranslationService translationService, IShareService shareService, IUserInteractionService userInteractionService) : base(logProvider, navigationService, translationService)
        {
            this.loginService = loginService;
            this.imageService = imageService;
            this.shareService = shareService;
            this.userInteractionService = userInteractionService;
        }


        private PostListModel post;
        private ImageSource userImage;
        private string username;
        private string userAddress;
        private string qrCodeImage;
        private string postTitle;
        private readonly ILoginService loginService;
        private readonly IImageService imageService;
        private readonly IShareService shareService;
        private readonly IUserInteractionService userInteractionService;

        public ImageSource UserImage { get => userImage; set => SetProperty(ref userImage, value); }
        public string Username { get => username; set => SetProperty(ref username,value); }
        public string UserAddress { get => userAddress; set => SetProperty(ref userAddress,value); }
        public string QrCodeImage { get => qrCodeImage; set => SetProperty(ref qrCodeImage ,value); }
        public string PostTitle { get => postTitle; set => SetProperty(ref postTitle,value); }

        public MvxAsyncCommand ShareImageCommand => new MvxAsyncCommand(async () =>
        {
            await shareService.ShareQRCode("Compartilhando meu perfil ConsumerOne", QrCodeImage);
        });

        public MvxAsyncCommand SaveImageCommand => new MvxAsyncCommand(async () =>
        {
            await imageService.SaveToGallery(QrCodeImage);
            await userInteractionService.DisplayMessage("Salvar", "Salvo na galeria.");
        });

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public override async Task Initialize()
        {
            IsLoading = true;
            QrCodeImage = $"{ApiService.ApiBaseAddress}/post/CO|Post|{post.Id}/qrcode";
            var user = await loginService.GetProfile(post.UserId);
            UserImage = ImageSource.FromUri(new Uri(ApiService.MediaBaseAddress + $"{user.Id}.jpg"));
            Username = user.Name;
            if (!string.IsNullOrEmpty(user.Address))
            {
                UserAddress = $"{user.Address}, {user.AddressNumber}";
                if (!string.IsNullOrEmpty(user.AddressAddon))
                {
                    UserAddress += $"{user.AddressAddon}";
                }
                UserAddress += $", {user.City}";
                UserAddress += $", {user.State}";
            }
            PostTitle = post.Title;
            IsLoading = false;
        }

        public override void Prepare(PostListModel parameter)
        {
            post = parameter;
        }
    }
}
