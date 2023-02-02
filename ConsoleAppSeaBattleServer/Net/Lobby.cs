using ConsoleAppSeaBattleServer.NetModel;
using ConsoleAppSeaBattleServer.NetProtocol;
using ConsoleAppSeaBattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Net
{
    internal class Lobby
    {
        private Socket _serverSocket;
        private IPEndPoint _ipEndPoint;

        public Lobby(string ip, int port)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public void StartLobby()
        {
            _serverSocket.Bind(_ipEndPoint);
            _serverSocket.Listen(10);

            LogUtils.Log("SERVER STARTED");
        }

        public void AwaitTwoClients()
        {
            while (true)
            {
                Socket _clientSocket1 = _serverSocket.Accept();
                LogUtils.Log($"CLIENT 1 ACCEPT FROM {_clientSocket1.RemoteEndPoint}");

                string messageToClient1 = JsonSerializer.Serialize(new Response()
                {
                    Status = Statuses.Ok,
                    JsonData = "1"
                });
                SocketUtils.SendMessage(_clientSocket1, messageToClient1);


                Socket _clientSocket2 = _serverSocket.Accept();
                LogUtils.Log($"CLIENT 2 ACCEPT FROM {_clientSocket2.RemoteEndPoint}");

                string messageToClient2 = JsonSerializer.Serialize(new Response()
                {
                    Status = Statuses.Ok,
                    JsonData = "2"
                });
                SocketUtils.SendMessage(_clientSocket2, messageToClient2);

                Thread.Sleep(1000);

                messageToClient1 = JsonSerializer.Serialize(new Response()
                {
                    Status = Statuses.Ok,
                });
                SocketUtils.SendMessage(_clientSocket1, messageToClient1);

                messageToClient2 = JsonSerializer.Serialize(new Response()
                {
                    Status = Statuses.Ok,
                });
                SocketUtils.SendMessage(_clientSocket2, messageToClient2);

                Task.Run(() =>
                {
                    Room room = new Room(_clientSocket1, _clientSocket2);
                    room.GameLoop();
                });

            }
        }
    }
}
