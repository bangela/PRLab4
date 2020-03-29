using Acr.UserDialogs;
using Chat.Clinet.Core.Interfaces;
using Chat.Shared;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Diagnostics;
using System.Threading;

namespace Chat.Clinet.Core.ViewModels
{
    public class ChatPageViewModel : MvxNavigationViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IClientService _clientService;
        public ChatPageViewModel(
            IMvxLogProvider provider, 
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs, 
            IClientService clientService)
           : base(provider, navigationService)
        {
            _userDialogs = userDialogs;
            _clientService = clientService;
            Messages = new MvxObservableCollection<MessageViewModel>();
            Thread checkMessagesThread = new Thread(new ThreadStart(CheckMessages));
            checkMessagesThread.Start();
        }

        #region Properties
        private string _data;
        public string Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private MvxObservableCollection<MessageViewModel> _messages;
        public MvxObservableCollection<MessageViewModel> Messages
        {
            get => _messages;
            set => SetProperty(ref _messages, value);
        }
        #endregion

        #region Commands
        private IMvxCommand _sendMessageCommand;
        public IMvxCommand SendMessageCommand => _sendMessageCommand ?? (_sendMessageCommand = new MvxCommand(SendMessage));
        #endregion

        #region Private Methods
        private void SendMessage()
        {
            try
            {
                _clientService.SendMessage(new Message(Header.POST, _data));
            }
            catch
            {
                _userDialogs.Alert("Error on sending message", "Error");
            }
        }

        private void CheckMessages()
        {
            try
            {
                while (true)
                {
                    if (_clientService.DataAvailable)
                    {
                        var message = _clientService.GetMessage();
                        if (message.Header == Header.POST)
                        {
                            var data = message.Data.Split(':');
                            Messages.Add(new MessageViewModel
                            {
                                Username = data[0],
                                Message = data[1]
                            });
                        }
                        else if(message.Header == Header.ACCEPTED)
                        {
                            var data = message.Data;
                            Messages.Add(new MessageViewModel
                            {
                                Username = "System",
                                Message = $"User {data} enter in chat"
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error on getting message {e.Message}");
            }
        }
        #endregion
    }
}
