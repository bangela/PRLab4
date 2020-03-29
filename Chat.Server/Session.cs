using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Chat.Server
{
    public class Session
    {
        public string Username { get; set; }

        public Guid Id { get; set; }

        public TcpClient tcpClient { get; set; }

        public Session()
        {
            Id = Guid.NewGuid();
        }
    }
}
