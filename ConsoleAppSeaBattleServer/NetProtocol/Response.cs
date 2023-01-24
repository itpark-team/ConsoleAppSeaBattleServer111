using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSeaBattleServer.NetProtocol
{
    internal class Response
    {
        public int Status { get; set; }
        public string JsonData { get; set; }

        public override string ToString()
        {
            return $"Status:{Status} JsonData:{JsonData}";
        }
    }
}
