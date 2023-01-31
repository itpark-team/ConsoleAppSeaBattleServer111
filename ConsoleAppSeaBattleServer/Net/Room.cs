using ConsoleAppSeaBattleServer.Game;
using ConsoleAppSeaBattleServer.Gameplay;
using ConsoleAppSeaBattleServer.NetProtocol;
using ConsoleAppSeaBattleServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Net
{
    internal class Room
    {
        private Socket _clientSocket1;
        private Socket _clientSocket2;

        private GameProcess _gameProcess1;
        private GameProcess _gameProcess2;

        public Room(Socket clientSocket1, Socket clientSocket2)
        {
            _clientSocket1 = clientSocket1;
            _clientSocket2 = clientSocket2;

            _gameProcess1 = new GameProcess();
            _gameProcess2 = new GameProcess();

            _gameProcess1.SetEnemyGameProccess(_gameProcess2);
            _gameProcess2.SetEnemyGameProccess(_gameProcess1);
        }

        public void GameLoop()
        {
            //первоначальная выдача полей клиентам

            //-----
            //client1

            Request request1 = ReceiveRequest(_clientSocket1);
            LogUtils.Log($"MESSAGE FROM CLIENT 1 RECEIVED: {request1}");

            Response response1 = _gameProcess1.ProcessRequest(request1);

            SendResponse(_clientSocket1, response1);
            LogUtils.Log($"MESSAGE TO CLIENT 1 SENT: {response1}");

            //-----
            //client2

            Request request2 = ReceiveRequest(_clientSocket2);
            LogUtils.Log($"MESSAGE FROM CLIENT 2 RECEIVED: {request2}");

            Response response2 = _gameProcess2.ProcessRequest(request2);

            SendResponse(_clientSocket2, response2);
            LogUtils.Log($"MESSAGE TO CLIENT 2 SENT: {response2}");


            while (true)
            {
                Request request11 = ReceiveRequest(_clientSocket1);
                LogUtils.Log($"MESSAGE FROM CLIENT 1 RECEIVED: {request11}");

                Response response11 = _gameProcess1.ProcessRequest(request11);

                SendResponse(_clientSocket1, response11);
                LogUtils.Log($"MESSAGE TO CLIENT 1 SENT: {response11}");
            }
        }

        private void SendResponse(Socket clientSocket, Response response)
        {
            string messageToClient = JsonSerializer.Serialize(response);

            SocketUtils.SendMessage(clientSocket, messageToClient);
        }

        private Request ReceiveRequest(Socket clientSocket)
        {
            string messageFromClient = SocketUtils.ReceiveMessage(clientSocket);

            Request request = JsonSerializer.Deserialize<Request>(messageFromClient);

            return request;
        }
    }
}
