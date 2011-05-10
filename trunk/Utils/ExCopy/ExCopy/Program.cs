
using System;
using System.IO;
namespace ExCopy
{
    public class Program
    {
        public static int Main(string[] args)
        {
            /*
             * --base <dir> --extension <ex> --output <dir>
             */
            var parser = new CommandLineParser();
            if (!parser.Parse(args))
            {
                PrintUsage();
                return -1;
            }
            else
            {
                var outDirName = parser.OutDir.FullName;
                var filesToCopy = parser.BaseDir.EnumerateFiles("*." + parser.Extension, SearchOption.AllDirectories);
                foreach (var f in filesToCopy)
                {
                    var outPath = Path.Combine(outDirName, f.Name);
                    File.Copy(f.FullName, outPath, true);
                }

                return 0;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("--base <dir> --extension <ex> --output <dir>");
        }
    }

    public class CommandLineParser
    {
        public DirectoryInfo BaseDir { get; private set; }

        public string Extension { get; private set; }

        public DirectoryInfo OutDir { get; private set; }

        public bool Parse(string[] args)
        {
            if (args.Length != 6)
                return false;

            if (args[0] != "--base" && args[2] != "--extension" && args[4] != "output")
                return false;

            try
            {
                var baseDirStr = args[1].TrimEnd(new char[] { '\\', '\"' });
                var extensionStr = args[3];
                var outDirStr = args[5].TrimEnd(new char[] { '\\', '\"' });

                Console.WriteLine(baseDirStr);
                Console.WriteLine(outDirStr);

                var baseDir = new DirectoryInfo(baseDirStr);
                var outDir = new DirectoryInfo(outDirStr);

                if (!baseDir.Exists || !outDir.Exists || String.IsNullOrEmpty(extensionStr))
                {
                    Console.WriteLine("Base or output dir doesn't exist or empty extension");

                    return false;
                }

                BaseDir = baseDir;
                Extension = extensionStr;
                OutDir = outDir;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}
