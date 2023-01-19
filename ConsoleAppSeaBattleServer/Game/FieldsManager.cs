using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Game
{
    internal class FieldsManager
    {
        private Random _random;

        private const int Rows = 10;
        private const int Columns = 10;

        private Cell[,] _myField;
        private Cell[,] _shootField;

        private int countAliveShips;

        public FieldsManager()
        {
            _random = new Random();
            _myField = new Cell[Rows, Columns];
            _shootField = new Cell[Rows, Columns];

            countAliveShips = 10;
        }

        public void ClearFields()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    _myField[i, j] = Cell.Empty;
                    _shootField[i, j] = Cell.Empty;
                }
            }
        }

        public void RandomShipsOnMyField()
        {
            for (int k = 0; k < countAliveShips; k++)
            {
                int iShip, jShip;

                do
                {
                    iShip = _random.Next(0, Rows);
                    jShip = _random.Next(0, Columns);
                } while (_myField[iShip, jShip] != Cell.Empty);

                _myField[iShip, jShip] = Cell.AliveShip;
            }
        }

        public Cell TakeDamageOnMyField(int iShoot, int jShoot)
        {
            if (_myField[iShoot, jShoot] == Cell.Empty)
            {
                _myField[iShoot, jShoot] = Cell.Miss;
                return Cell.Miss;
            }
            else if (_myField[iShoot, jShoot] == Cell.AliveShip)
            {
                _myField[iShoot, jShoot] = Cell.DeadShip;
                countAliveShips--;
                return Cell.DeadShip;
            }

            throw new Exception("Ошибка. Выстрел в эту клетку невозможен");
        }

        public void MarkShootOnShootField(int iShoot, int jShoot, Cell newValue)
        {
            _shootField[iShoot, jShoot] = newValue;
        }

        public bool HasAliveShips()
        {
            return countAliveShips > 0;
        }

        private char[,] ConvertFieldToCharArray(Cell[,] field)
        {
            char[,] charField = new char[Rows, Columns];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    charField[i, j] = (char)field[i, j];
                }
            }

            return charField;
        }

        public PlayerFields GetPlayerFields()
        {
            return new PlayerFields()
            {
                MyField = ConvertFieldToCharArray(_myField),
                ShootField = ConvertFieldToCharArray(_shootField)
            };
        }
    }
}
