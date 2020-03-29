using Chat.Clinet.Core.Interfaces;
using Chat.Shared;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chat.Clinet.Core.Services
{
    public class ClientService : IClientService
    {
        private int port;
        private string ip;
        private TcpClient tcpClient;

        public string Username { get; set; }

        public bool DataAvailable
        {
            get => tcpClient.GetStream().DataAvailable;
        }
        public void Initialize(int port, string ip)
        {
            try
            {
                tcpClient = new TcpClient(ip, port);
                tcpClient.Connect(ip, port);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Connection refused {e.Message}");
            }
        }

        public void SendMessage(Message message)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                NetworkStream strm = tcpClient.GetStream();
                formatter.Serialize(strm, message);
            }
            catch(Exception e)
            {
                Debug.WriteLine($"Error on sending message: {e.Message}");
                throw e;
            }
        }

        public Message GetMessage()
        {
            try
            {
                NetworkStream strm = tcpClient.GetStream();
                IFormatter formatter = new BinaryFormatter();
                Message message = (Message)formatter.Deserialize(strm);
                return message;
            }
            catch (Exception e)
            {
                Debug.WriteLine("sendMessage exception: " + e.Message);
                throw e;
            }
        }
    }
}
