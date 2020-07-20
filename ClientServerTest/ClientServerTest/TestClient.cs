using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerTest
{
    class TestClient
    {
        public static int BUFFER_SIZE = 1000000;
        bool IsConnected;
        public TcpClient socket;
        private byte[] receiveBuffer;
        private NetworkStream stream;

        public TestClient()
        {
            IsConnected = false;
        }

        ~TestClient()
        {
            if (IsConnected) Stop();
        }

        public void StartClient(string stringHost, int port)
        {
            socket = new TcpClient(stringHost, port);
            socket.ReceiveBufferSize = BUFFER_SIZE;
            socket.SendBufferSize = BUFFER_SIZE;
            receiveBuffer = new byte[BUFFER_SIZE];

            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, BUFFER_SIZE, MessageReceived, null);

            IsConnected = true;
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
            if (!IsConnected) return;

            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Stop()
        {

            IsConnected = false;
        }
    }
}
