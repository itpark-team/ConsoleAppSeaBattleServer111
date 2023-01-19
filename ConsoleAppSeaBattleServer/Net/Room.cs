using ConsoleAppSeaBattleServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Net
{
    internal class Room
    {
        private Player _player1;
        private Player _player2;

        private FieldsManager _player1Fields;
        private FieldsManager _player2Fields;

        public Room(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;

            _player1Fields = new FieldsManager();
            _player2Fields = new FieldsManager();

            _player1Fields.ClearFields();
            _player2Fields.ClearFields();

            _player1Fields.RandomShipsOnMyField();
            _player2Fields.RandomShipsOnMyField();
        }

        public void GameLoop()
        {
            while (true)
            {
                _player1.SendFields(_player1Fields.GetPlayerFields());
                _player2.SendFields(_player2Fields.GetPlayerFields());


            }
        }


    }
}
