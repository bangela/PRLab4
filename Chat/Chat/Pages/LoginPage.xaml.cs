using Chat.Clinet.Core.ViewModels;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Chat.Client.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : MvxContentPage<LoginPageViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();
        }
    }
}