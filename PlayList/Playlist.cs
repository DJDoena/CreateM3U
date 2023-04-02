namespace DoenaSoft.PlayList
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

#if !BatchPlayList

    using System.Collections.Generic;

    public static class Playlist
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

            ProcessDirectory(CheckForIndividualNames(args), new DirectoryInfo(args[0]), Settings.Default.MusicRootDirectoryName);

            Console.WriteLine();
            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }

#else

    internal static class Playlist
    {

#endif

        internal static void ProcessDirectory(bool individualNames, DirectoryInfo di, string musicRootDirectoryName)
        {
            Console.WriteLine("Processing \"" + di.FullName + "\"");

            var fileName = "playlist";

            if (individualNames)
            {
                fileName = string.Empty;

                var split = di.FullName.Split('\\');

                if (split[split.Length - 1].StartsWith("Disc ") && split.Length > 2)
                {
                    if (split[split.Length - 3] != musicRootDirectoryName)
                    {
                        fileName = split[split.Length - 3] + " - ";
                    }
                }

                if (split.Length > 1)
                {
                    if (split[split.Length - 2] != musicRootDirectoryName)
                    {
                        fileName += split[split.Length - 2] + " - ";
                    }
                }

                fileName += split[split.Length - 1];
            }

            var output = new StringBuilder();

            var list = di.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).ToList();

            list.Sort((left, right) => left.FullName.CompareTo(right.FullName));

            foreach (var fi in list)
            {
                output.AppendLine(fi.Name);
            }

            if (output.Length > 0)
            {
                using (var sw = new StreamWriter(di.FullName + @"\!" + fileName + ".m3u", false, Encoding.GetEncoding("Windows-1252")))
                {
                    sw.Write(output.ToString());
                }
            }
        }

        internal static bool CheckForIndividualNames(string[] args)
        {
            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "/individualnames" || arg.ToLower() == "/in")
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}