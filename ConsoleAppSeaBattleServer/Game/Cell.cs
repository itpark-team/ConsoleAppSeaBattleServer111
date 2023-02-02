using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.NetModel
{
    internal enum Cell
    {
        AliveShip = 'K',
        DeadShip = 'X',
        Miss = 'O',
        Empty = '.'
    }
}
