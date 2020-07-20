using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chat
{
    class TestServer
    {
        public static int BUFFER_SIZE = 4096;

        TcpListener socket;
        TcpClient client;
        NetworkStream stream;

        private byte[] receiveBuffer;

        bool running;
        public bool Connected { get; private set; }

        Action<string> receivedMessageMethod;
        Action<string> connection;

        public TestServer(Action<string> receivedMessageMethod, Action<string> connection)
        {
            running = false;
            Connected = false;

            this.receivedMessageMethod = receivedMessageMethod;
            this.connection = connection;
        }

        public void InitNetworkAndStart(int port)
        {
            socket = new TcpListener(IPAddress.Any, port);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);

            Console.WriteLine("Server start on port : " + port);

            receiveBuffer = new byte[BUFFER_SIZE];

            running = true;
        }

        ~TestServer()
        {
            if (running) socket.Stop();
        }

        public void TcpConnectionCallback(IAsyncResult ar)
        {
            client = socket.EndAcceptTcpClient(ar);
            Console.WriteLine("Connection from " + client.Client.RemoteEndPoint);
            connection(client.Client.RemoteEndPoint + "");
            stream = client.GetStream();


            stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, MessageReceived, null);

            Connected = true;
        }

        public void MessageReceived(IAsyncResult result)
        {
            try
            {
                int byteLenght = stream.EndRead(result);
                if (byteLenght <= 0)
                {
                    return;
                }

                byte[] data = new byte[byteLenght];
                Array.Copy(receiveBuffer, data, byteLenght);

                string message = Encoding.UTF8.GetString(data, 0, byteLenght);

                if (message.Equals("#!stop!#"))
                {
                    socket.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);
                    Connected = false;
                } else
                {
                    receivedMessageMethod(message);
                    stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, MessageReceived, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when receive a message\n" + ex);
            }
        }

        public void SendMessage(string message)
        {
            if (!running || !Connected) return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Stop()
        {
            if (Connected)
            {
                SendMessage("#!stop!#");
            }

            if (running)
            {
                if (stream != null) stream.Close();

                socket.Stop();
                running = false;
            }
        }
    }
}
