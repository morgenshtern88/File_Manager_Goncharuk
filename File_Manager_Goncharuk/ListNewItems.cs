using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class ListNewItems
    {
        public readonly string[] columns;
        public object state { get; set; }
        private string currentDirectory = " ";
        public ListNewItems(object state, params string[] columns)
        {
            this.state = state;
            this.columns = columns;
        }
        internal void Render(List<int> columnsWidth, int elementIndex, int ListViewX, int ListviewY)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                Console.CursorTop = elementIndex + ListviewY; ;
                Console.CursorLeft = ListViewX + columnsWidth.Take(i).Sum();
                Console.Write(GetStringWith(columns[i], columnsWidth[i]));
            }
        }
        internal void Clean(List<int> columnsWidht, int i, int x, int y)
        {
            Console.CursorTop = i + y;
            Console.CursorLeft = x;
            Console.Write(new string(' ', columnsWidht.Sum()));
        }
        public void Location(List<string> Current, int x, int y)
        {
            var info = state;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.CursorTop = y - 2;
            Console.CursorLeft = x;
            if (Current.Count != 0)
            {
                for (int i = 0; i < Current.Count; i++)
                {
                    currentDirectory = Current[Current.Count - 1];
                }
            }
            else
            {
                if (info is DirectoryInfo dir)
                {
                    currentDirectory = dir.Root.ToString();
                }
                else if (info is FileInfo file)
                {
                    currentDirectory = file.DirectoryName;
                }
            }
            Console.WriteLine(currentDirectory.PadRight(Console.WindowWidth / 2 - 5, ' '));
        }

        private string GetStringWith(string v1, int maxLenght)
        {
            if (v1.Length < maxLenght)
            {
                return v1.PadRight(maxLenght, ' ');
            }
            else
            {
                return v1.Substring(0, maxLenght - 5) + "[  ]";
            }
        }
    }
}
