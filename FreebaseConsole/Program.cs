using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freebase;
namespace FreebaseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var thepolice = new Question { type = "/music/artist", name = "The Police" };
            thepolice.album.Add("name", null);
            thepolice.album.Add("limit", 2);
            thepolice.album.Add("track", new Object[5]);
            var response = ConnectToFreebase.Connect(thepolice);
            Console.ReadLine();
        }
    }
}
