namespace DoenaSoft.PlayList
{
    using System;
    using System.IO;

    public static class Program
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

            Helper.ProcessFolder(args, new DirectoryInfo(args[0]), Settings.Default.MusicRootDirectoryName);

            Console.WriteLine();
            Console.WriteLine("Press <Enter> to exit.");
            Console.ReadLine();
        }
    }
}