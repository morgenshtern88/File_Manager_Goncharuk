using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp5
{
    class ChangeFile
    {
        public int countOfFiles;
        public int countOfFolders;
        public void Message(object state, string message)
        {
            Clean();
            Console.CursorTop = Console.WindowHeight - 5;
            Console.CursorLeft = 0;
            Console.WriteLine(state.ToString() + " is " + message.PadRight(Console.WindowWidth, ' '));
        }
        public void Menu()
        {
            Console.CursorTop = Console.WindowHeight - 2;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Yellow;
            const string menu = "F1 - copy  F2 - cut  F3 - paste  F4 - root  F5 - list of disks  F6 - properties  F7 - rename  F9 - new folder";
            Console.Write(menu.PadRight(Console.WindowWidth, ' '));
            Console.ResetColor();
        }

        internal void Clean()
        {
            Console.CursorTop = Console.WindowHeight - 10;
            Console.CursorLeft = 0;
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("  ".PadRight(Console.WindowWidth, ' '));
            }
            Console.CursorTop = Console.WindowHeight - 9;
            Console.CursorLeft = 0;
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("  ".PadRight(Console.WindowWidth, ' '));
            }
        }

        internal void Information(string name, string fullName, string root, string accessTime, string writeTime, string lenght)
        {
            Console.CursorTop = Console.WindowHeight - 8;
            Console.CursorLeft = 0;
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Parent directory: {fullName}");
            Console.WriteLine($"Root directory: {root}");
            Console.WriteLine($"Last read time: {accessTime}");
            Console.WriteLine($"Last write time: {writeTime}");
            Console.WriteLine($"Size: {lenght} bytes");
        }

        internal void Information(string name, string fullName, string root, string accessTime, string writeTime, string lenght, int countfiles, int countFolders)
        {
            Console.CursorTop = Console.WindowHeight - 10;
            Console.CursorLeft = 0;
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Parent directory: {root}");
            Console.WriteLine($"Root drectory: {fullName}");
            Console.WriteLine($"Last read time: {accessTime}");
            Console.WriteLine($"Last write time: {writeTime}");
            Console.WriteLine($"Size: {lenght} bytes");
            Console.WriteLine($"Files: {countOfFiles}");
            Console.WriteLine($"Folders: {countOfFolders}");
        }

        internal void Count(string fullName)
        {
            string[] files = Directory.GetFiles(fullName);
            string[] folders = Directory.GetDirectories(fullName);
            countOfFiles += files.Length;
            countOfFolders += folders.Length;
            for (int i = 0; i < folders.Length; i++)
            {
                Count(folders[i]);
            }
        }
    }
}

