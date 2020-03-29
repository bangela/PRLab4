using Chat.Clinet.Core.ViewModels;
using MvvmCross.Forms.Views;
using Xamarin.Forms.Xaml;

namespace Chat.Client.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : MvxContentPage<ChatPageViewModel>
    {
        public ChatPage()
        {
            InitializeComponent();
        }
    }
}