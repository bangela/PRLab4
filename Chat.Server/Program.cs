using System;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(5000);
            server.Start();
        }
    }
}
