using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace ConsumerOne.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(Position = TabbedPosition.Root, WrapInNavigationPage =false, NoHistory = true)]
    public partial class TabsMasterView
    {

        public TabsMasterView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        protected override void OnAppearing()
        {
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            base.OnAppearing();
        }
    }
}