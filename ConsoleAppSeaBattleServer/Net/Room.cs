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

        private GameResult _gameResult;

        public Room(Socket clientSocket1, Socket clientSocket2)
        {
            _clientSocket1 = clientSocket1;
            _clientSocket2 = clientSocket2;

            _gameResult = new GameResult();
            _gameResult.CurrentGameResult = GameResult.Turn1;

            _gameProcess1 = new GameProcess(1);
            _gameProcess2 = new GameProcess(2);

            _gameProcess1.SetEnemyGameProccess(_gameProcess2);
            _gameProcess2.SetEnemyGameProccess(_gameProcess1);

            _gameProcess1.SetGameResult(_gameResult);
            _gameProcess2.SetGameResult(_gameResult);
        }

        public void GameLoop()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Request request1 = ReceiveRequest(_clientSocket1);
                    LogUtils.Log($"MESSAGE FROM CLIENT 1 RECEIVED: {request1}");

                    if(request1.Command == Commands.ExitGame)
                    {
                        break;
                    }

                    Response response1 = _gameProcess1.ProcessRequest(request1);

                    SendResponse(_clientSocket1, response1);
                    LogUtils.Log($"MESSAGE TO CLIENT 1 SENT: {response1}");
                }

                _clientSocket1.Shutdown(SocketShutdown.Both);
                _clientSocket1.Close();

                LogUtils.Log($"CLIENT 1 DISCONNECTED");
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Request request2 = ReceiveRequest(_clientSocket2);
                    LogUtils.Log($"MESSAGE FROM CLIENT 2 RECEIVED: {request2}");

                    if (request2.Command == Commands.ExitGame)
                    {
                        break;
                    }

                    Response response2 = _gameProcess2.ProcessRequest(request2);

                    SendResponse(_clientSocket2, response2);
                    LogUtils.Log($"MESSAGE TO CLIENT 2 SENT: {response2}");
                }

                _clientSocket2.Shutdown(SocketShutdown.Both);
                _clientSocket2.Close();

                LogUtils.Log($"CLIENT 2 DISCONNECTED");
            });
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
