using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client / Server   - Write one of both mode :");
            string answer = Console.ReadLine();

            if (answer.Equals("Client") || answer.Equals("client")) Client();
            else if (answer.Equals("Server") || answer.Equals("server")) Server();
            else
            {
                Console.WriteLine("No mode choose");
                Console.ReadLine();
            }
        }

        public static void Server()
        {
            Console.WriteLine("\nMode Server\n\n");
            TestServer server = new TestServer();
            server.InitNetworkAndStart();

            bool exit = false;
            string message;

            while (!exit)
            {
                Console.WriteLine("Entrer un message :");
                message = Console.ReadLine();

                if (server.Connected)
                    server.SendMessage(message);
                else
                {
                    Console.WriteLine("! Aucune personne connectée !");
                }

                if (message.Equals("Exit") || message.Equals("exit"))
                {
                    server.Stop();
                    exit = true;
                }
            }


            Console.WriteLine("End of programme");
            Console.ReadLine();
        }

        public static void Client()
        {
            Console.WriteLine("\nMode Client\n\n");
            string stringAdress, stringPort;

            Console.WriteLine("Adress ? :");
            stringAdress = Console.ReadLine();

            Console.WriteLine("Port ? :");
            stringPort = Console.ReadLine();

            TestClient client = new TestClient();
            client.StartClient(stringAdress, Convert.ToInt32(stringPort));

            bool exit = false;
            string message;

            while (!exit)
            {
                Console.WriteLine("Entrer un message :");
                message = Console.ReadLine();

                client.SendMessage(message);

                if (message.Equals("Exit") || message.Equals("exit"))
                {
                    client.Stop();
                    exit = true;
                }
            }

            Console.WriteLine("End of programme");
            Console.ReadLine();
        }
    }
}
