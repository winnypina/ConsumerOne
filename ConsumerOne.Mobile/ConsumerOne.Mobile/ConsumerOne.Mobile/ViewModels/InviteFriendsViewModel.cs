using System;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms.MultiSelectListView;

namespace ConsumerOne.Mobile.ViewModels
{
    public class InviteFriendsViewModel : BaseViewModel
    {
        private readonly IUserInteractionService _popupService;

        public InviteFriendsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserInteractionService popupService, ITranslationService translationService) : base(logProvider, navigationService, translationService)
        {
            _popupService = popupService;
            Contacts = new MultiSelectObservableCollection<Contact>();
        }

        public override void Prepare()
        {
            SetScreenName("Convidar amigos");
        }

        public MultiSelectObservableCollection<Contact> Contacts { get; }

        public MvxAsyncCommand InviteCommand => new MvxAsyncCommand(async () =>
        {
            IsLoading = true;
            if (Contacts.SelectedItems != null && Contacts.SelectedItems.Any())
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                await _popupService.DisplayMessage("", Texts.SingleOrDefault(n => n.Key == "Sucesso")?.Value);
                await NavigationService.Close(this);
            }
            IsLoading = false;
        });

        public MvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this));

        public override async Task Initialize()
        {

            IsLoading = true;

            var contactsStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

            if (contactsStatus != PermissionStatus.Granted)
            {
                var results =
                    await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                contactsStatus = results[Permission.Contacts];
            }

            if (contactsStatus == PermissionStatus.Granted)
            {
                Contacts.Clear();
                var contacts = (await Plugin.ContactService.CrossContactService.Current.GetContactListAsync()).OrderBy(n => n.Name).ToList();
                contacts.ForEach(contact =>
                {
                    if (string.IsNullOrEmpty(contact.PhotoUri))
                    {
                        contact.PhotoUri = "defaultUser.png";
                    }
                    Contacts.Add(contact);
                });
            }
            else
            {
                await NavigationService.Close(this);
            }
            IsLoading = false;
        }
    }
}