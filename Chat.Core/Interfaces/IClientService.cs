using Chat.Shared;

namespace Chat.Clinet.Core.Interfaces
{
    public interface IClientService
    {
        void Initialize(int port, string ip);

        void SendMessage(Message message);

        Message GetMessage();

        string Username { get; set; }

        bool DataAvailable { get;  }
    }
}
