using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerOne.Mobile.iOS.Services;
using ConsumerOne.Mobile.Services;
using ConsumerOne.Mobile.ViewModels;
using ConsumerOne.Mobile.Views;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.IoC;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.iOS
{
    public class Setup : MvxFormsIosSetup<ConsumerOneApp, App>
    {
        protected override IMvxIoCProvider CreateIocProvider()
        {
            var provider = base.CreateIocProvider();
            provider.RegisterSingleton<IShareService>(new ShareService());
            provider.RegisterSingleton<IImageService>(new ImageService());
            provider.RegisterSingleton<IQrCodeScanningService>(new QrCodeScanningService());
            return provider;
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPagePresenter = new CustomMvxFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton<IMvxFormsPagePresenter>(formsPagePresenter);
            return formsPagePresenter;
        }

        private Xamarin.Forms.Application _formsApplication;

        public override Xamarin.Forms.Application FormsApplication
        {
            get
            {
                if (!Xamarin.Forms.Forms.IsInitialized)
                {
                    Xamarin.Forms.Forms.Init();
                }

                if (_formsApplication == null)
                {
                    _formsApplication = CreateFormsApplication();
                }
                if (Xamarin.Forms.Application.Current != _formsApplication)
                {
                    Xamarin.Forms.Application.Current = _formsApplication;
                }
                return _formsApplication;
            }
        }
    }

    public class CustomMvxFormsPagePresenter : MvxFormsPagePresenter
    {
        public CustomMvxFormsPagePresenter(IMvxFormsViewPresenter platformPresenter) : base(platformPresenter)
        {
        }

        protected override async Task<Page> CloseAndCreatePage(Type view,
            MvxViewModelRequest request,
            MvxPagePresentationAttribute attribute,
            bool closeModal = true,
            bool closePlatformViews = true,
            bool showPlatformViews = true)
        {
            if (closeModal)
                await CloseAllModals();

            if (closePlatformViews)
                PlatformPresenter.ClosePlatformViews();

            if (showPlatformViews)
                PlatformPresenter.ShowPlatformHost(attribute.HostViewModelType);

            return CreatePage(view, request, attribute);
        }

        public override async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            var navigation = GetPageOfType<NavigationPage>()?.Navigation;
            if (hint is MvxPopToRootPresentationHint popToRootHint)
            {
                // Make sure all modals are closed
                await CloseAllModals(popToRootHint.Animated);

                // Double check we have a navigation page, otherwise
                // we can just return as we must be already at the root page
                if (navigation == null)
                    return true;

                // Close all pages back to the root
                await navigation.PopToRootAsync(popToRootHint.Animated);
                return true;
            }
            if (hint is MvxPopPresentationHint popHint)
            {
                var matched = await PopModalToViewModel(navigation, popHint);
                if (matched) return true;


                await PopToViewModel(navigation, popHint.ViewModelToPopTo, popHint.Animated);
                return true;
            }
            if (hint is MvxRemovePresentationHint removeHint)
            {
                foreach (var modal in navigation.ModalStack)
                {
                    var removed = RemoveByViewModel(modal.Navigation, removeHint.ViewModelToRemove);
                    if (removed)
                        return true;
                }

                RemoveByViewModel(navigation, removeHint.ViewModelToRemove);
                return true;
            }
            if (hint is MvxPagePresentationHint pageHint)
            {
                var pageType = ViewsContainer.GetViewType(pageHint.ViewModel);
                if (GetPageOfTypeByType(pageType) is Page page)
                {
                    if(page.Parent is CustomNavigationPage navPage)
                    {
                        if (navPage.Parent is TabbedPage tabbedPage)
                        {
                            tabbedPage.CurrentPage = navPage;
                        }   
                        else if (navPage.Parent is CarouselPage carouselPage && page is ContentPage contentPage)
                            carouselPage.CurrentPage = contentPage;
                    } else
                    {
                        if (page.Parent is TabbedPage tabbedPage)
                            tabbedPage.CurrentPage = page;
                        else if (page.Parent is CarouselPage carouselPage && page is ContentPage contentPage)
                            carouselPage.CurrentPage = contentPage;
                    }
                    
                }
                return true;
            }
            if (hint is MvxPopRecursivePresentationHint popRecursiveHint)
            {
                var levels = popRecursiveHint.LevelsDeep;
                if (levels > navigation.NavigationStack.Count())
                    levels = navigation.NavigationStack.Count();
                for (int i = 0; i < levels; i++)
                {
                    await navigation.PopAsync(popRecursiveHint.Animated);
                }

                return true;
            }
            return true;
        }

        protected override NavigationPage CreateNavigationPage(Page pageRoot = null)
        {
            return new CustomNavigationPage(pageRoot);
        }

        public override async Task<bool> ShowContentPage(
             Type view,
             MvxContentPagePresentationAttribute attribute,
             MvxViewModelRequest request)
        {
            // FIX: the in-tab navigation fix appears to need special treatment for android with regard to showPlatformViews param
            var showPlatformViews = ShowContentPagePlatformViews(request, attribute);

            var page = await CloseAndCreatePage(view, request, attribute, showPlatformViews: showPlatformViews);
            await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
            return true;
        }

        public override async Task PushOrReplacePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            // Make sure we always have a rootPage
            if (rootPage == null)
            {
                rootPage = FormsApplication.MainPage;
            }

            var navigationRootPage = GetPageOfType<NavigationPage>(rootPage);

            if(!(page is SplashView))
            {
                // FIX: if our root page is a tabbed page then we need to check the different tabs for the appropriate navigation stack
                navigationRootPage = FindNavigationPageInTabbedPageByAttribute(navigationRootPage, rootPage, attribute);

            }

            // Step down through any nested navigation pages to make sure we're pushing to the
            // most nested navigation page
            if (attribute.WrapInNavigationPage && !(page is SplashView) &&
                navigationRootPage?.CurrentPage is NavigationPage navigationNestedPage)
            {
                await PushOrReplacePage(navigationNestedPage, page, attribute);
                return;
            }

            // Handle the case where the page should be wrapped in a navigation page
            if (attribute.WrapInNavigationPage && !(page is SplashView))
            {
                // Look at parent and see whether it's a navigation page,
                // if it is, then use it to navigate to the new page
                if (navigationRootPage == null && rootPage?.Parent is NavigationPage parentNavigation)
                {
                    navigationRootPage = parentNavigation;
                }

                // If the root isn't a navigation page, we need to wrap the new page
                // in a navigation wrapper.
                if (navigationRootPage == null || attribute.NoHistory)
                {
                    var navpage = CreateNavigationPage(page);
                    ReplacePageRoot(rootPage, navpage, attribute);
                }
                else
                {
                    await navigationRootPage.PushAsync(page, attribute.Animated);
                }
            }
            else
            {
                ReplacePageRoot(rootPage, page, attribute);
            }
        }

        public override async Task<bool> ShowTabbedPage(
            Type view,
            MvxTabbedPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            
            var page = await CloseAndCreatePage(view, request, attribute);

            if (attribute.Position == TabbedPosition.Root)
            {
                if (page is TabbedPage tabbedPageRoot)
                {
                    await PushOrReplacePage(FormsApplication.MainPage, page, attribute);
                }
                else
                    throw new MvxException($"A root page should be of type {nameof(TabbedPage)}");
            }
            else
            {
                var tabHost = GetPageOfType<TabbedPage>();
                if (tabHost == null)
                {
                    // can't use internal class MvxFormsLog here - fallback to System.Diagnosticss.Trace for developer visibility
                    System.Diagnostics.Trace.WriteLine($"Current root is not a TabbedPage show your own first to use custom Host. Assuming we need to create one.");
                    tabHost = new TabbedPage();
                    await PushOrReplacePage(FormsApplication.MainPage, tabHost, attribute);
                }

                // FIX: when attribute indicates page should be wrapped in a NavigationPage, do so
                if (attribute.WrapInNavigationPage)
                {
                    page = CreateNavigationPage(page).Build(tp =>
                    {
                        tp.Title = page.Title;
                        tp.Icon = page.Icon;
                    });
                }
                // END OF: FIX

                tabHost.Children.Add(page);
            }
            return true;
        }

        protected override async Task<bool> ClosePage(Page rootPage, Page page, MvxPagePresentationAttribute attribute)
        {
            var root = TopNavigationPage();

            // FIX: if our root page is a tabbed page then we need to check the different tabs for the correct navigation stack
            //      supply navPage=null here so we only take this into account when a match is found.
            var tabbedNavPage = FindNavigationPageInTabbedPageByAttribute(null, root, attribute);
            if (tabbedNavPage != null)
            {
                // double-check that the found page is currently displaying the page we're trying to close.
                if (tabbedNavPage.CurrentPage == page)
                {
                    root = tabbedNavPage;
                }
            }

            if (page != null)
            {
                root?.Navigation?.RemovePage(page);
            }
            else
            {
                var nav = root?.Navigation;
                if (nav != null)
                {
                    await nav.PopAsync(attribute.Animated);
                }
            }

            return true;
        }

        protected override Task<bool> FindAndCloseViewFromViewModel(IMvxViewModel mvxViewModel, Page rootPage, MvxPagePresentationAttribute attribute)
        {
            var root = TopNavigationPage();
            Page pageToClose = null;

            // finding the view from viewmodel in navigation stack
            pageToClose = root?.Navigation?.NavigationStack?
                .OfType<IMvxPage>()
                .FirstOrDefault(x => x.ViewModel == mvxViewModel) as Page;

            if (pageToClose == null)
            {
                // FIX: try finding the view by checking the attribute - which will refer to its nav parent
                // if it's contained within a TabbedPage.
                // this call to FindNavigationPageInTabbedPageByAttribute is somewhat redundant (as we have to repeat
                // it in the overriden ClosePage method).  We still have to execute it here to ensure pageToClose is still set
                // and allowing the error handling Task.FromResult(false) below to work as before.
                var tabbedNavPage = FindNavigationPageInTabbedPageByAttribute(null, rootPage, attribute);
                if (tabbedNavPage != null)
                {
                    // double-check that the found page matches the view model we're trying to close.
                    if (((IMvxPage)tabbedNavPage.CurrentPage).ViewModel == mvxViewModel)
                    {
                        pageToClose = tabbedNavPage.CurrentPage;
                    }
                }
            }

            if (pageToClose == null)
            {
                // can't use internal class MvxFormsLog here - fallback to System.Diagnosticss.Debug for developer visibility
                System.Diagnostics.Debug.WriteLine("Ignoring close for ViewModel - Matching View for ViewModel instance failed");
                return Task.FromResult(false);
            }

            return ClosePage(rootPage, pageToClose, attribute);
        }

        private NavigationPage FindNavigationPageInTabbedPageByAttribute(NavigationPage navPage, Page rootPage, MvxPagePresentationAttribute attribute)
        {
            if (!attribute.WrapInNavigationPage || attribute.HostViewModelType == null) return navPage;
            var tabbedPage = FindTabbedPage(rootPage);

            if (tabbedPage == null) return navPage;

            // find the child that is a navigation stack with a view using the HostViewModelType as its view model
            foreach (var tab in tabbedPage.Children)
            {
                if (!(tab is NavigationPage tabbedNavPage)) continue;

                // does this stack have the view with the view model type we want
                if (tabbedNavPage?.Navigation?.NavigationStack?.OfType<IMvxPage>().FirstOrDefault(x => x.ViewModel.GetType() == attribute.HostViewModelType) is Page)
                {
                    return tabbedNavPage;
                }
            }

            // if no match was found, return the nav page supplied to the method
            return navPage;
        }

        private class TabNavigationViewModelMapping
        {
            public Type Presented { get; set; }
            public Type TabParent { get; set; }
        }

        private static bool ShowContentPagePlatformViews(
            MvxViewModelRequest request,
            MvxContentPagePresentationAttribute attribute)
        {
            if (Device.RuntimePlatform != Device.Android || attribute.HostViewModelType == null)
            {
                return true;
            }

            // TODO: find a better way other than maintaining a hard-coded mapping of types..
            // To get inner-tab navigation working on Android it seems we need to not show platform views.
            // This mapping maintains a list of associations between the viewmodel being presented
            // and the tab navigation parent where this must be AVOIDED.
            var tabMappings = new List<TabNavigationViewModelMapping>
             {
                 new TabNavigationViewModelMapping { TabParent = typeof(TabsMasterViewModel), Presented = typeof(NewPostViewModel) }
             };
            return !tabMappings.Any(m => ((MvxViewModelInstanceRequest)request)?.ViewModelInstance?.GetType() == m.Presented
                                           && attribute.HostViewModelType == m.TabParent);
        }

        private TabbedPage FindTabbedPage(Page page)
        {
            switch (page)
            {
                case TabbedPage tabbedPage:
                    return tabbedPage;
                case NavigationPage navPage:
                    return FindTabbedPage(navPage.CurrentPage);
                default:
                    return null;
            }
        }
    }
}