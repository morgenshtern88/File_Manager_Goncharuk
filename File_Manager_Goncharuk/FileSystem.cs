using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class FileSystem
    {
        private static string drive = "";
        private static bool newFolder = true;
        private static string directoryName = " ";
        private static bool isFile = true;
        static ChangeFile file = new ChangeFile();
        private static string sourcePath = " ";
        private static string destPath = " ";
        private static string fileName = " ";
        private static string cutSoursePath = " ";


        public void Render()
        {
            Console.CursorVisible = false;
            ListView[] view = new ListView[2];
            for (int i = 0; i < view.Length; i++)
            {
                view[i] = new ListView(3 + i * 60, 2, height: 20);
                view[i].columnWidth = new List<int> { 30, 10, 15 };
                view[i].Items = GetItems("D:\\");
                view[i].Selected += View_Selected;
                view[i].Previous += View_Previous;
                view[i].Copy += View_Copy;
                view[i].Cut += View_Cut;
                view[i].Paste += View_Paste;
                view[i].Root += View_Root;
                view[i].ListOfDisk += View_LisyOfDisk;
                view[i].Properties += View_Properties;
                view[i].Rename += View_Rename;
                view[i].Create += View_Create;
            }

            while (true)
            {
                for (int i = 0; i < view.Length; i++)
                {
                    view[0].Render();
                    view[1].Render();
                    bool changeDirectory = false;
                    while (!changeDirectory)
                    {
                        view[i].Render();
                        var key = Console.ReadKey();
                        view[i].Update(key);
                        if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                        {
                            changeDirectory = true;
                        }
                    }
                }
            }
        }

        private static void View_Selected(object sender, EventArgs e)
        {
            string denied = "Access denied";
            var view = (ListView)sender;
            var info = view.SelectedItem.state;
            try
            {
                if (info is FileInfo file)
                {
                    Process.Start(file.FullName);
                }
                else if (info is DirectoryInfo dir)
                {
                    view.Clean();
                    view.Items = GetItems(dir.FullName);
                    view.Current.Add(dir.FullName);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                file.Message(info, denied);
            }
        }

        private static void View_Previous(object sender, EventArgs e)
        {
            string lastElement = "";
            var view = (ListView)sender;
            view.Clean();
            for (int i = 0; i < view.Current.Count; i++)
            {
                drive = view.Current[view.Current.Count - 1];
                view.Items = GetItems(Path.GetDirectoryName(view.Current[view.Current.Count - 1]));
                lastElement = view.Current[view.Current.Count - 1];
            }
            view.Current.Remove(lastElement);
        }

        private static void View_Copy(object sender, EventArgs e)
        {
            string copy = "Copy";
            var view = (ListView)sender;
            var info = view.SelectedItem.state;
            fileName = view.SelectedItem.state.ToString();
            file.Message(info, copy);
            if (info is FileInfo files)
            {
                sourcePath = System.IO.Path.Combine(files.DirectoryName, fileName);
                isFile = true;
            }
            else if (info is DirectoryInfo dir)
            {
                sourcePath = dir.FullName.ToString();
                isFile = false;
            }
        }
        private static void View_Cut(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.state;
            if (info is FileInfo fileInfo)
            {
                cutSoursePath = fileInfo.FullName;
                isFile = true;
            }
            else if (info is DirectoryInfo dir)
            {
                isFile = false;
                cutSoursePath = dir.FullName;
            }
            string cut = "Cut";
            file.Message(view.SelectedItem.state.ToString(), cut);
        }

        private static void View_Paste(object sender, EventArgs e)
        {
            try
            {
                var view = (ListView)sender;
                string targetPath = " ";
                string dirName = " ";
                if (view.cut == false)
                {
                    string nestedDirectory = " ";
                    view.info = (FileSystemInfo)view.SelectedItem.state;
                    for (int i = 0; i < view.Current.Count; i++)
                    {
                        destPath = System.IO.Path.Combine(view.Current[view.Current.Count - 1], fileName);
                        targetPath = view.Current[view.Current.Count - 1];
                    }
                    if (targetPath == " ")
                    {
                        DirectoryInfo dir = (DirectoryInfo)view.SelectedItem.state;
                        destPath = System.IO.Path.Combine(dir.Root.ToString(), fileName);
                        targetPath = dir.Root.ToString();
                    }
                    string paste = $"Past in  {targetPath} ";
                    if (isFile == true)
                    {
                        System.IO.File.Copy(sourcePath, destPath, true);
                        file.Message(fileName, paste);
                        view.Clean();
                        view.Items = GetItems(targetPath);
                    }
                    else
                    {
                        if (System.IO.Directory.Exists(sourcePath))
                        {
                            string[] folders = sourcePath.Split('\\');
                            directoryName = folders[folders.Length - 1];
                            newFolder = false;
                            View_Create(sender, e);
                            string[] files = System.IO.Directory.GetFiles(sourcePath);
                            string[] directorys = System.IO.Directory.GetDirectories(sourcePath);
                            dirName = fileName + " Paste ";
                            foreach (string s in files)
                            {
                                fileName = System.IO.Path.GetFileName(s);
                                destPath = System.IO.Path.Combine(targetPath + '\\' + directoryName, fileName);
                                System.IO.File.Copy(s, destPath, true);
                                view.Clean();
                                view.Items = GetItems(targetPath);
                            }

                            foreach (string dir in directorys)
                            {
                                nestedDirectory = System.IO.Path.Combine(targetPath + '\\' + directoryName, dir.Split('\\').Last());
                                Directory.CreateDirectory(nestedDirectory);
                                NestedDirectory(sourcePath, dir, nestedDirectory);
                            }
                        }
                    }
                    view.Clean();
                    view.Items = GetItems(targetPath);
                    file.Message(sourcePath, " Paste " + targetPath);
                }
                else if (view.cut == true)
                {
                    string destPath = " ";
                    var info = view.SelectedItem.state;
                    for (int i = 0; i < view.Current.Count; i++)
                    {
                        destPath = view.Current[view.Current.Count - 1];
                    }
                    if (info is DirectoryInfo directory)
                    {
                        if (destPath == " ")
                        {
                            destPath = directory.Root.ToString();
                        }
                    }
                    else if (info is FileInfo file)
                    {
                        if (destPath == " ")
                        {
                            destPath = file.FullName.Split('\\').First();
                        }
                    }
                    if (isFile)
                    {
                        File.Move(cutSoursePath, destPath + '\\' + cutSoursePath.Split('\\').Last());
                        view.Clean();
                        view.Items = GetItems(destPath);
                        dirName = cutSoursePath.Split('\\').Last();
                    }
                    else if (!isFile)
                    {
                        Directory.Move(cutSoursePath, destPath + '\\' + cutSoursePath.Split('\\').Last());
                        view.Clean();
                        view.Items = GetItems(destPath);
                        dirName = cutSoursePath.Split('\\').Last();
                        file.Message(dirName + " Paste ", destPath);
                    }
                }
            }
            catch (System.IO.IOException)
            {
                file.Message("The file", "already exist");
            }
            catch (System.UnauthorizedAccessException)
            {
                file.Message("Access", "denaied");
            }
        }
        public static void NestedDirectory(string sourhtPath, string dir, string nestedDir)
        {
            int num = 0;
            sourhtPath += '\\' + dir.Split('\\').Last();
            string[] files = System.IO.Directory.GetFiles(sourhtPath);
            string[] directorys = System.IO.Directory.GetDirectories(sourhtPath);
            foreach (var file in files)
            {
                destPath = Path.Combine(nestedDir, file.Split('\\').Last());
                File.Copy(file, destPath);
            }
            foreach (var directory in directorys)
            {
                nestedDir = Path.Combine(nestedDir + '\\', directory.Split('\\').Last());
                num++;
                Directory.CreateDirectory(nestedDir);
                NestedDirectory(sourhtPath, directory, nestedDir);
                string[] sourhtPaths = sourhtPath.Split('\\');
                string[] nest = nestedDir.Split('\\');
                nestedDir = "";
                for (int i = 0; i < nest.Length; i++)
                {
                    if (i != nest.Length - 1 && i != nest.Length - 2)
                    {
                        nestedDir += nest[i] + '\\';
                    }
                    if (i == nest.Length - 2)
                    {
                        nestedDir += nest[i];
                    }
                }
            }
        }

        private static void View_Root(object sender, EventArgs e)
        {
            string root = " ";
            var view = (ListView)sender;
            var info = view.SelectedItem.state;
            if (info is FileInfo file)
            {
                root = file.FullName.Split('\\').First();
                view.Clean();
                view.Items = GetItems(root);
            }
            else if (info is DirectoryInfo dir)
            {
                root = dir.Root.ToString();
                view.Clean();
                view.Items = GetItems(root);
            }

        }
        private static void View_LisyOfDisk(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            view.Clean();
            view.Items = GetDrives();
        }
        private static List<ListNewItems> GetDrives()
        {
            string[] drives = Directory.GetLogicalDrives();
            List<ListNewItems> result = new List<ListNewItems>();
            foreach (var drive in drives)
            {
                result.Add(new ListNewItems(new DirectoryInfo(drive), drive, "<drive>", ""));
            }
            return result;
        }

        private static void View_Properties(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.state;
            try
            {
                if (info is FileInfo fileInfo)
                {
                    file.Information(fileInfo.Name,
                        fileInfo.Directory.Root.ToString(),
                        fileInfo.Directory.FullName,
                        File.GetLastAccessTime(fileInfo.FullName).ToString(),
                        File.GetLastWriteTime(fileInfo.FullName).ToString(),
                        fileInfo.Length.ToString());
                }
                else if (info is DirectoryInfo dir)
                {
                    file.Count(dir.FullName);
                    file.Information(dir.Name,
                        dir.Root.FullName,
                        dir.Parent.FullName,
                        Directory.GetLastAccessTime(dir.FullName).ToString(),
                        Directory.GetLastWriteTime(dir.FullName).ToString(),
                        dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length).ToString(),
                        file.countOfFiles,
                        file.countOfFolders
                        );
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                file.Message(info, "access denied");
            }
            file.countOfFiles = 0;
            file.countOfFolders = 0;
        }

        private static void View_Rename(object sender, EventArgs e)
        {
            try
            {
                var view = (ListView)sender;
                var info = view.SelectedItem.state;
                file.Clean();
                Console.CursorTop = Console.WindowHeight - 5;
                Console.CursorLeft = 0;
                Console.CursorVisible = true;
                Console.WriteLine("Input new name of folder");
                string folderName = Console.ReadLine();
                if (info is FileInfo files)
                {
                    string newFileFullPath = Path.Combine(files.DirectoryName, folderName);
                    File.Move(files.FullName, newFileFullPath);
                }

                else if (info is DirectoryInfo dir)
                {
                    string newDirFullPath = Path.Combine(dir.Parent.FullName, folderName);
                    Directory.Move(dir.FullName, newDirFullPath);
                    view.Clean();
                    view.Items = GetItems(dir.Parent.FullName);
                }

            }
            catch (Exception)
            {
                file.Message("The file same name", "already excist");
            }
        }

        private static void View_Create(object sender, EventArgs e)
        {
            try
            {
                var view = (ListView)sender;
                string folderName = " ";
                for (int i = 0; i < view.Current.Count; i++)
                {
                    folderName = view.Current[view.Current.Count - 1];
                }
                if (folderName == " ")
                {
                    DirectoryInfo dir = (DirectoryInfo)view.SelectedItem.state;
                    folderName = dir.Root.ToString();
                }
                if (!newFolder)
                {
                    string pathString = System.IO.Path.Combine(folderName, directoryName);
                    Directory.CreateDirectory(pathString);
                    newFolder = true;
                }
                else
                {
                    file.Clean();
                    Console.CursorTop = Console.WindowHeight - 5;
                    Console.CursorLeft = 0;
                    Console.WriteLine("Input name your new folder and push Enter");
                    string name = Console.ReadLine();
                    string pathString = System.IO.Path.Combine(folderName, name);
                    Directory.CreateDirectory(pathString);
                    view.Clean();
                    view.Items = GetItems(folderName);
                }
            }
            catch (System.IO.IOException)
            {
                file.Message("Access", "denaied");
            }
        }
        private static List<ListNewItems> GetItems(string v)
        {
            try
            {

                return new DirectoryInfo(v).GetFileSystemInfos()
                    .Select(f => new ListNewItems(
                        f,
                        f.Name,
                        f is DirectoryInfo dir ? "<dir>" : f.Extension,
                        f is FileInfo file ? file.Length.ToString() : "")).ToList();
            }
            catch (System.ArgumentNullException)
            {
                return new DirectoryInfo(drive).GetFileSystemInfos()
                .Select(f => new ListNewItems(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? file.Length.ToString() : "")).ToList();
            }
        }
    }
}