﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class TestClient
    {
        public static int BUFFER_SIZE = 4096;
        bool IsConnected;
        public TcpClient socket;
        private byte[] receiveBuffer;
        private NetworkStream stream;

        Action<string> receivedMessageMethod;
        Action serverDisconnect;

        public TestClient(Action<string> receivedMessageMethod, Action serverDisconnect)
        {
            IsConnected = false;
            this.receivedMessageMethod = receivedMessageMethod;
            this.serverDisconnect = serverDisconnect;
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

                string message = Encoding.UTF8.GetString(data, 0, byteLenght);

                if (message.Equals("#!stop!#"))
                {
                    serverDisconnect();
                    stream.Close();
                    socket.Close();
                    IsConnected = false;
                }
                else { 
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
            if (!IsConnected) return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Stop()
        {
            if (IsConnected)
            {
                SendMessage("#!stop!#");
                stream.Close();
                socket.Close();
                IsConnected = false;
            }
        }
    }
}
