namespace DoenaSoft.PlayList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class BatchPlaylist
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0 || !Directory.Exists(args[0]))
            {
                Console.WriteLine("No path provided. Using current directory.");

                if (args == null || args.Length == 0)
                {
                    args = new[] { "." };
                }
                else
                {
                    var temp = new List<string>(args);

                    temp.Insert(0, ".");

                    args = temp.ToArray();
                }
            }

            var list = (new DirectoryInfo(args[0])).GetDirectories("*", SearchOption.AllDirectories).ToList();

            list.Sort((left, right) => left.FullName.CompareTo(right.FullName));

            foreach (var di in list)
            {
                var fis = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly);

                if (fis.Length > 0)
                {
                    Playlist.ProcessDirectory(Playlist.CheckForIndividualNames(args), di, Settings.Default.MusicRootDirectoryName);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }
    }
}