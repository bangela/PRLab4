using Chat.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Chat.Server
{
    public class Server
    {
        private TcpListener tcpListener;
        private List<Session> sessions;
        private bool isRunning;
        public volatile Object readLock;

        public Server(int port)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(ipAddress, port);
            sessions = new List<Session>();
            isRunning = false;
            readLock = new Object();
        }

        public void Start()
        {
            if (!isRunning)
            {
                try
                {
                    tcpListener.Start();
                    isRunning = true;
                    Console.WriteLine($"Server start with succes");
                    var checkThread = new Thread(new ThreadStart(checkClients));
                    checkThread.Start();
                    var listenThread = new Thread(new ThreadStart(listen));
                    listenThread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error on server start: {e.Message}");
                }
            }
        }
        public void Stop()
        {
            if(isRunning)
            {
                try
                {
                    tcpListener.Stop();
                    isRunning = false;
                    Console.WriteLine("Server is stoped");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error server stop: {e.Message}");
                }
            }
        }

        private void listen()
        {
            while(isRunning)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Session session = new Session();
                session.tcpClient = client;
                sessions.Add(session);
                Console.WriteLine($"New client is added {session.Id}");
            }
        }

        private void checkClients()
        {
            while(isRunning)
            {
                try
                {
                    lock (readLock)
                    {
                        foreach (var session in sessions)
                        {
                            if (session.tcpClient.GetStream().DataAvailable)
                            {
                                var message = getMessage(session.tcpClient.Client);
                                if (message != null)
                                {
                                    Thread processData = new Thread(() => processMessage(session, message));
                                    processData.Start();
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine($"Error on get message from client {e.Message}");
                }
            }
        }
        private Message getMessage(Socket socket)
        {
            try
            {
                NetworkStream strm = new NetworkStream(socket);
                IFormatter formatter = new BinaryFormatter();
                Message message = (Message)formatter.Deserialize(strm);
                Console.WriteLine("- message header: " + message.Header);
                return message;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error on getting message {e.Message}");
            }
            return null;
        }

        private void processMessage(Session session,  Message message)
        {
            switch(message.Header)
            {
                case Header.JOIN:
                    if (sessions.Any(x => x.Username == message.Data))
                    {
                        sendMessage(new Message(Header.USER_EXIST, null), session.tcpClient.Client);
                    }
                    else
                    {
                        session.Username = message.Data;
                        Console.WriteLine($"Session: {session.Id} set username in {session.Username}");
                        foreach (var memessageSession in sessions)
                        {
                            sendMessage(new Message(Header.ACCEPTED, session.Username), memessageSession.tcpClient.Client);
                        }
                    }
                    break;
                case Header.POST:
                    Console.WriteLine($"Session {session.Id} send message {message.Data}");
                    foreach(var messageSession in sessions)
                    {
                        sendMessage(new Message(Header.POST, session.Username + ":" + message.Data), messageSession.tcpClient.Client);
                    }
                    break;
            }
        }

        private void sendMessage(Message message, Socket socket)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                NetworkStream strm = new NetworkStream(socket);
                formatter.Serialize(strm, message);
            }
            catch (Exception e)
            {
                Console.WriteLine("sendMessage exception: " + e.Message);
            }
        }
    }
}
