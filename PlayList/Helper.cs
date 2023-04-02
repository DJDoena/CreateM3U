namespace DoenaSoft.PlayList
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class Helper
    {
        internal static DirectoryInfo GetRootFolder(string[] args)
        {
            DirectoryInfo rootFolder;
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No path provided. Using current directory.");

                rootFolder = new DirectoryInfo(".");
            }
            else if (CheckFolderExists(args[0]))
            {
                rootFolder = new DirectoryInfo(args[0]);
            }
            else
            {
                rootFolder = null;
            }

            return rootFolder;
        }

        internal static void ProcessFolder(string[] args, DirectoryInfo folder, string musicRootDirectoryName)
        {
            Console.WriteLine("Processing \"" + folder.FullName + "\"");

            var fileName = GetFileName(args, folder, musicRootDirectoryName);

            var output = new StringBuilder();

            var files = folder.GetFiles("*.mp3", SearchOption.TopDirectoryOnly).ToList();

            files.Sort((left, right) => left.FullName.CompareTo(right.FullName));

            foreach (var file in files)
            {
                output.AppendLine(file.Name);
            }

            if (output.Length > 0)
            {
                using (var sw = new StreamWriter(folder.FullName + @"\!" + fileName + ".m3u", false, Encoding.GetEncoding(1252)))
                {
                    sw.Write(output.ToString());
                }
            }
        }

        private static bool CheckFolderExists(string arg)
        {
            bool exists;
            try
            {
                exists = Directory.Exists(arg);
            }
            catch
            {
                exists = false;
            }

            return exists;
        }

        private static string GetFileName(string[] args, DirectoryInfo folder, string musicRootDirectoryName)
        {
            string fileName;
            if (CheckForIndividualNames(args))
            {
                var fileNameBuilder = new StringBuilder();

                var split = folder.FullName.Split('\\');

                if (split[split.Length - 1].StartsWith("Disc ") && split.Length > 2)
                {
                    if (split[split.Length - 3] != musicRootDirectoryName)
                    {
                        fileNameBuilder = new StringBuilder(split[split.Length - 3] + " - ");
                    }
                }

                if (split.Length > 1)
                {
                    if (split[split.Length - 2] != musicRootDirectoryName)
                    {
                        fileNameBuilder.Append(split[split.Length - 2] + " - ");
                    }
                }

                fileNameBuilder.Append(split[split.Length - 1]);

                fileName = fileNameBuilder.ToString();
            }
            else
            {
                fileName = "playlist";
            }

            return fileName;
        }

        private static bool CheckForIndividualNames(string[] args)
            => args?.Any(arg => arg?.ToLower() == "/individualnames" || arg?.ToLower() == "/in") == true;
    }
}