using ConsoleAppSeaBattleServer.Game;
using ConsoleAppSeaBattleServer.NetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Gameplay
{
    internal class GameProcess
    {
        private FieldsManager _clientFields;

        private Dictionary<string, Func<string, Response>> _methods;

        public GameProcess()
        {
            _clientFields = new FieldsManager();

            _clientFields.ClearFields();
            _clientFields.RandomShipsOnMyField();


            _methods = new Dictionary<string, Func<string, Response>>();
            _methods.Add(Commands.GetFields, GetFields);
        }


        public Response ProcessRequest(Request request)
        {
            var method = _methods[request.Command];

            return method.Invoke(request.JsonData);
        }

        private Response GetFields(string intputJsonData)
        {
            PlayerFields playerFields = _clientFields.GetPlayerFields();
            string outputJsonData = JsonSerializer.Serialize(playerFields);

            return new Response() { 
                Status = Statuses.Ok,
                JsonData = outputJsonData
            };
        }
    }
}
