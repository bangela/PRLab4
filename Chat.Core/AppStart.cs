using Chat.Clinet.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Chat.Clinet.Core
{
    class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication application,
                       IMvxNavigationService navigationService) : base(application, navigationService)
        {

        }
        protected override async Task NavigateToFirstViewModel(object hint = null)
        {
            await NavigationService.Navigate<LoginPageViewModel>();
        }
    }
}
