using ConsoleAppSeaBattleServer.Game;
using ConsoleAppSeaBattleServer.NetProtocol;
using ConsoleAppSeaBattleServer.NetUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Net
{
    internal class Player
    {
        private Socket _clientSocket;
        
        public Player(Socket clientSocket)
        {
            _clientSocket = clientSocket;
        }

        public void SendFields(PlayerFields playerFields)
        {
            Response response = new Response()
            {
                Status = Statuses.ShowFields,
                JsonData = JsonSerializer.Serialize(playerFields)
            };

            string messageToClient = JsonSerializer.Serialize(response);

            SocketUtils.SendMessage(_clientSocket, messageToClient);
        }
    }
}
