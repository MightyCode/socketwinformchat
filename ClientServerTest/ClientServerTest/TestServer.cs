using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ClientServerTest
{
    class TestServer
    {
        public static int BUFFER_SIZE = 4096;
        TcpListener socket;
        readonly int PORT = 25255;
        bool running;
        public bool Connected { get; private set; }
        TcpClient client;
        NetworkStream stream;

        private byte[] receiveBuffer;

        public TestServer()
        {
            running = false;
            Connected = false;
        }

        public void InitNetworkAndStart()
        {
            socket = new TcpListener(IPAddress.Any, PORT);
            socket.Start();
            socket.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);

            Console.WriteLine("Server start on port : " + PORT);

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
            stream = client.GetStream();

            byte[] data = Encoding.ASCII.GetBytes("");

            stream.Write(data, 0, data.Length);
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
                Console.WriteLine(">" + Encoding.ASCII.GetString(data, 0, byteLenght));
                stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, MessageReceived, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when receive a message\n" + ex);
            }
        }

        public void SendMessage(string message)
        {
            if (!running) return;

            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Stop()
        {
            socket.Stop();

            running = false;
        }
    }
}
