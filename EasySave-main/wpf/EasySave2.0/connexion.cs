using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace test_projet
{
    public class connexion
    {
        public Socket server;
        public Socket client;
        Thread serverThread;
        public connexion()
        {
            serverThread = new Thread(Launch);
            serverThread.Start();
        }

        void Launch()
        {
            server = SeConnecter();

            client = AccepterConnexion(server);
        }


        public void SendProgress(int progress, string saveName)
        {
            EcouterReseau(client, progress, saveName);
        }
        private static Socket SeConnecter()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            serverSocket.Bind(endPoint);

            serverSocket.Listen(10);

            Console.WriteLine("Le serveur est en attente de connexion...");

            return serverSocket;
        }

        private static Socket AccepterConnexion(Socket serverSocket)
        {
            Socket clientSocket = serverSocket.Accept();

            IPEndPoint clientEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;
            Console.WriteLine($"Client connecté depuis {clientEndPoint.Address}:{clientEndPoint.Port}");

            return clientSocket;
        }


        private void RestartThread()
        {
            serverThread.Join();
            serverThread = new Thread(Launch);
            serverThread.Start();
        }
        private void EcouterReseau(Socket client, int progress, string saveName)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    byte[] message = Encoding.UTF8.GetBytes("ProgressUpdate:"+saveName+":"+progress);
                    client.Send(message);
                }
            }
            catch (SocketException ex)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                server.Close();
                RestartThread();


                

            }

        }
    }
}
