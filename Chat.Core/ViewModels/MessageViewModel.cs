using MvvmCross.ViewModels;

namespace Chat.Clinet.Core.ViewModels
{
    public class MessageViewModel : MvxNotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}
