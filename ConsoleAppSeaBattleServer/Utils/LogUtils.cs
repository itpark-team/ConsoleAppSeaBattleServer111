using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.Utils
{
    internal class LogUtils
    {
        public static void Log(string msg)
        {
            Console.WriteLine($"LOG: {DateTime.Now} --- {msg}");
        }
    }
}
