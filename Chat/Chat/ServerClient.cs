﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Chat
{
    class ServerClient
    {
        public static int BUFFER_SIZE = 4096;

        TcpListener socket;
        TcpClient client;
        NetworkStream stream;

        private byte[] receiveBuffer;

        bool running;
        public bool Connected { get; private set; }

        Action<NetMessage> receivedMessage;
        Action<NetMessage> connection;
        Action<NetMessage> disconnection;

        public List<string> ConnectedPeople { get; private set; }

        public ServerClient()
        {
            running = false;
            Connected = false;
        }

        public void InquireFunctions(Action<NetMessage> receivedMessage,
                                    Action<NetMessage> connection,
                                    Action<NetMessage> disconnection)
        {
            this.receivedMessage = receivedMessage;
            this.connection = connection;
            this.disconnection = disconnection;
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

        ~ServerClient()
        {
            if (running) socket.Stop();
        }

        public void TcpConnectionCallback(IAsyncResult ar)
        {
            client = socket.EndAcceptTcpClient(ar);
            Console.WriteLine("Connection from " + client.Client.RemoteEndPoint);
            connection(
                new NetMessage(
                    (client.Client.RemoteEndPoint + "").Split(':')[0]
                    + "#" + DateTime.Now + "#connection"
                    ));
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
                NetMessage message = new NetMessage(Encoding.UTF8.GetString(data, 0, byteLenght));

                if (message.MessageEquals("#!stop!#"))
                {
                    socket.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);
                    disconnection(message);
                    Connected = false;
                } else
                {
                    receivedMessage(message);
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

            NetMessage mes = new NetMessage("s#" + DateTime.Now + "#" + message);
            Console.WriteLine("send" + mes);
            byte[] data = mes.ToByteArrayUTF8();
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
