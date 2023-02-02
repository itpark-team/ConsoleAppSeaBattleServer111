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
        private FieldsManager _myFields;
        private GameProcess _enemyGameProccess;
        private GameResult _gameResult;

        private Dictionary<string, Func<string, Response>> _methods;

        private int _playerNumber;

        public GameProcess(int playerNumber)
        {
            _playerNumber = playerNumber;

            _myFields = new FieldsManager();

            _myFields.ClearFields();
            _myFields.RandomShipsOnMyField();


            _methods = new Dictionary<string, Func<string, Response>>();
            _methods.Add(Commands.GetFields, GetFields);
            _methods.Add(Commands.Shoot, Shoot);
            _methods.Add(Commands.GetGameResult, GetGameResult);

        }

        public void SetEnemyGameProccess(GameProcess enemyGameProccess)
        {
            _enemyGameProccess = enemyGameProccess;
        }

        public void SetGameResult(GameResult gameResult)
        {
            _gameResult = gameResult;
        }

        public FieldsManager GetFieldsManager()
        {
            return _myFields;
        }

        public Response ProcessRequest(Request request)
        {
            var method = _methods[request.Command];

            return method.Invoke(request.JsonData);
        }

        private Response GetFields(string intputJsonData)
        {
            PlayerFields playerFields = _myFields.GetPlayerFields();
            string outputJsonData = JsonSerializer.Serialize(playerFields);

            return new Response()
            {
                Status = Statuses.Ok,
                JsonData = outputJsonData
            };
        }

        private Response Shoot(string intputJsonData)
        {
            ShootCoords shootCoords = JsonSerializer.Deserialize<ShootCoords>(intputJsonData);

            try
            {
                Cell resultCell = _enemyGameProccess.GetFieldsManager().TakeDamageOnMyField(shootCoords.I, shootCoords.J);

                _myFields.MarkShootOnShootField(shootCoords.I, shootCoords.J, resultCell);

                if (_enemyGameProccess.GetFieldsManager().HasAliveShips())
                {
                    if (resultCell == Cell.DeadShip)
                    {
                        if (_playerNumber == 1)
                        {
                            _gameResult.CurrentGameResult = GameResult.Turn1;
                        }
                        else if (_playerNumber == 2)
                        {
                            _gameResult.CurrentGameResult = GameResult.Turn2;
                        }
                    }

                    if (resultCell == Cell.Miss)
                    {
                        if (_playerNumber == 1)
                        {
                            _gameResult.CurrentGameResult = GameResult.Turn2;
                        }
                        else if (_playerNumber == 2)
                        {
                            _gameResult.CurrentGameResult = GameResult.Turn1;
                        }
                    }
                }
                else
                {
                    if (_playerNumber == 1)
                    {
                        _gameResult.CurrentGameResult = GameResult.Win1;
                    }
                    else if (_playerNumber == 2)
                    {
                        _gameResult.CurrentGameResult = GameResult.Win2;
                    }
                }

                return new Response()
                {
                    Status = Statuses.Ok
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Status = Statuses.Error,
                    JsonData = ex.Message
                };
            }
        }

        private Response GetGameResult(string intputJsonData)
        {
            return new Response()
            {
                Status = Statuses.Ok,
                JsonData = _gameResult.CurrentGameResult
            };
        }
    }
}
