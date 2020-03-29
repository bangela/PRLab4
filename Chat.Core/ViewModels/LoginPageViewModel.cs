using Acr.UserDialogs;
using Chat.Clinet.Core.Interfaces;
using Chat.Shared;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;

namespace Chat.Clinet.Core.ViewModels
{
    public class LoginPageViewModel : MvxNavigationViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IClientService _clientService;
        public LoginPageViewModel(IMvxLogProvider provider, IMvxNavigationService navigationService, IUserDialogs userDialogs, IClientService clientService)
           : base(provider, navigationService)
        {
            _userDialogs = userDialogs;
            _clientService = clientService;
            _clientService.Initialize(5000, "127.0.0.1");
        }

        #region Properties
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        #endregion

        #region Commands
        private IMvxCommand _loginCommand;
        public IMvxCommand LoginCommand => _loginCommand ?? (_loginCommand = new MvxCommand(Login));
        #endregion

        #region Private Methods
        private void Login()
        {
            try
            {
                _clientService.SendMessage(new Message(Header.JOIN, Username));
            }
            catch(Exception e)
            {
                _userDialogs.Alert("Error on connecting with server", "Error");
            }
            try
            {
                var message = _clientService.GetMessage();
                if (message.Header == Header.USER_EXIST)
                {
                    _userDialogs.Alert($"User with username {Username} already exist", "Error");
                }
                else
                {
                    _clientService.Username = Username;
                    NavigationService.Navigate<ChatPageViewModel>();
                }
            }
            catch (Exception e)
            {
                _userDialogs.Alert("Error on getting message from server", "Error");
            }
        }
        #endregion
    }
}
