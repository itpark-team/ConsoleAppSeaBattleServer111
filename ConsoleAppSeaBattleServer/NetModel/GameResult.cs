using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.NetModel
{
    internal class GameResult
    {
        public string CurrentGameResult { get; set; }

        public const string Turn1 = "Turn1";
        public const string Turn2 = "Turn2";
        public const string Win1 = "Win1";
        public const string Win2 = "Win2";
    }
}
