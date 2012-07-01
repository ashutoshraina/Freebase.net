using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreebaseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var thepolice = new Question { type = "/music/artist" };
            var response = ConnectToFreebase.Connect(thepolice);
            Console.ReadLine();
        }
    }
}
