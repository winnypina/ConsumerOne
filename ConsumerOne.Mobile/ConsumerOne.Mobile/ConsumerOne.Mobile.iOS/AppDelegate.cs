using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;
using Xamarin;

namespace ConsumerOne.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, ConsumerOneApp, App>
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppCenter.Start("9a05e161-3ed9-409c-8881-dfca88085d45",
                   typeof(Analytics), typeof(Crashes));
            IQKeyboardManager.SharedManager.Enable = true;


            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(65, 105, 225);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            return base.FinishedLaunching(app, options);
        }

        protected override void LoadFormsApplication()
        {
            base.LoadFormsApplication();
            Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            Xamarin.FormsGoogleMaps.Init("AIzaSyD6GwDzvu-0GZ43akLynY6-LMvwncZkZeU");
            Xamarin.FormsGoogleMapsBindings.Init();
            Rg.Plugins.Popup.Popup.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
        }
    }
}
