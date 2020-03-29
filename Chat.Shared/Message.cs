using System;

namespace Chat.Shared
{
    [Serializable]
    public class Message
    {
        public Header Header { get; set; }

        public string Data { get; set; }

        public Message(Header header, string data)
        {
            Header = header;
            Data = data;
        }
    }
}
