using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight = 35;
            Console.BufferWidth = Console.WindowWidth = 120;
            FileSystem file = new FileSystem();
            ChangeFile change = new ChangeFile();
            change.Menu();
            file.Render();
        }
    }
}
