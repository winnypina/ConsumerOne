using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConsumerOne.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashView
	{
		public SplashView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this,false);
		}
	}
}