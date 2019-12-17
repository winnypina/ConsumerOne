using Akavache;
using ConsumerOne.Mobile.ViewModels;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace ConsumerOne.Mobile
{
    public class ConsumerOneApp : MvxApplication
    {
        public override void Initialize()
        {
            BlobCache.ApplicationName = "ConsumerOne";
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            RegisterAppStart<SplashViewModel>();
        }
    }
   
}
