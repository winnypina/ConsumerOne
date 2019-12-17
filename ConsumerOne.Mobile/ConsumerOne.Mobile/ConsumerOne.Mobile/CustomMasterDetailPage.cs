using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace ConsumerOne.Mobile
{
    public class CustomMasterDetailPage : MasterDetailPage
    {

        public CustomMasterDetailPage()
        {
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().AfterNavigate += CustomMasterDetailPage_AfterNavigate;
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().AfterClose += CustomMasterDetailPage_AfterClose;
        }

        private void CustomMasterDetailPage_AfterClose(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            IsPresented = false;
        }

        private void CustomMasterDetailPage_AfterNavigate(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            IsPresented = false;
        }
    }
}
