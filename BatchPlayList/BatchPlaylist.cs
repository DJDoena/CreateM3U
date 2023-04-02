namespace DoenaSoft.PlayList
{
    using System;
    using System.IO;
    using System.Linq;

    public static class BatchPlaylist
    {
        public static void Main(string[] args)
        {
            var rootFolder = Helper.GetRootFolder(args);

            if (rootFolder == null)
            {
                Console.WriteLine($"'{args[0]}' does not exist. Press <Enter> to exit.");
                Console.ReadLine();

                return;
            }

            var folders = rootFolder.GetDirectories("*", SearchOption.AllDirectories).ToList();

            folders.Sort((left, right) => left.FullName.CompareTo(right.FullName));

            foreach (var folder in folders)
            {
                var files = folder.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

                if (files.Length > 0)
                {
                    Helper.ProcessFolder(args, folder, Settings.Default.MusicRootDirectoryName);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }
    }
}