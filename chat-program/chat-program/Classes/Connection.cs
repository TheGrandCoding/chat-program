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
        public string Reference;

        public Func<Connection, Exception, Task> DisconnectCallback;
        public Connection(string name, Func<Connection, Exception, Task> callback)
        {
            Reference = name;
            DisconnectCallback = callback;
            Client = new TcpClient(AddressFamily.InterNetwork);
        }
        public TcpClient Client { get; set; }

        public event EventHandler<string> Receieved;

        public void Send(string message)
        {
            try
            {
                message = $"%{message}`";
                var stream = Client.GetStream();
                var bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            } catch (Exception ex)
            {
                Close(ex);
            }
        }

        public void Close(Exception ex = null)
        {
            Listening = false;
            try
            {
                Client.Close();
            } catch { }
            DisconnectCallback.Invoke(this, ex);
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
                try
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
                } // TODO: See if we can only look at IO / Socket errors
                catch (Exception ex)
                {
                    this.Close(ex);
                }
            } while (Listening);
        }
    }
}
