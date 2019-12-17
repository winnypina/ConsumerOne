using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CarouselView.FormsPlugin.Android;
using MvvmCross.Forms.Platforms.Android.Views;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Rg.Plugins.Popup;
using Xamarin;
using Xamarin.Auth.Presenters.XamarinAndroid;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ConsumerOne.Mobile.ViewModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using ZXing.Mobile;

namespace ConsumerOne.Mobile.Droid
{
    [Activity(Label = "Consumer One", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, ConsumerOneApp, App>
    {
        public static readonly int PickImageId = 1000;

        public static MainActivity Current { private set; get; }
        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        protected override void OnCreate(Bundle bundle)
        {
            AppCenter.Start("fd3c4a08-7872-4aa9-83aa-69ad89cd15b9",
                   typeof(Analytics), typeof(Crashes));
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            MobileBarcodeScanner.Initialize(this.Application);
            base.OnCreate(bundle);
            Current = this;
        }

        public override void InitializeForms(Bundle bundle)
        {
            base.InitializeForms(bundle);
            AuthenticationConfiguration.Init(this, bundle);
            FormsGoogleMaps.Init(this, bundle);
            CarouselViewRenderer.Init();
            Popup.Init(this, bundle);
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImageId)
            {
                if (resultCode == Result.Ok && data != null)
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                else
                    PickImageTaskCompletionSource.SetResult(null);
            }
        }

        public override void OnBackPressed()
        {
            //Popup.SendBackPressed(base.OnBackPressed);
            if(BaseViewModel.Current.GetType() == typeof(MasterViewModel))
            {
                var viewModel = BaseViewModel.Current as MasterViewModel;
                viewModel.GoToMyProfileCommand.Execute();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}