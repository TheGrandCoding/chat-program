using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public class Connection
    {
        public Connection()
        {
            Client = new TcpClient(AddressFamily.InterNetwork);
        }
        public TcpClient Client { get; set; }

        public event EventHandler<string> Receieved;

        public void Send(string message)
        {
            message = $"%{message}`";
            var stream = Client.GetStream();
            var bytes = Encoding.UTF8.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
        }

        public void Listen()
        {
            Listening = true;
            if(listenThread == null)
            {
                listenThread = new Thread(listenLoop);
                listenThread.Start();
            }
        }

        public bool Listening { get; set; }
        Thread listenThread;
        void listenLoop()
        {
            do
            {
                var stream = Client.GetStream();
                var bytes = new Byte[Client.ReceiveBufferSize];
                string data;
                stream.Read(bytes, 0, Client.ReceiveBufferSize);
                data = Encoding.UTF8.GetString(bytes).Trim().Replace("\0", "");
                if (string.IsNullOrWhiteSpace(data))
                    continue;

                foreach(var tempMsg in data.Split('%'))
                {
                    if (string.IsNullOrWhiteSpace(tempMsg))
                        continue;

                    var message = tempMsg.Substring(0, tempMsg.LastIndexOf('`'));
                    if (string.IsNullOrWhiteSpace(message))
                        continue;
                    Receieved?.Invoke(this, message);
                }
            } while (Listening);
        }
    }
}
