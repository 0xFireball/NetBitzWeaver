using NetBitz.Weaver.ProtectionPipeline;
using NetBitz.Weaver.Types;
using NetBitz.Weaver.Utilities;
using System;
using System.IO;

namespace NetBitz.Weaver.CmdLine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("NetBitz Weaver - (c) 2016 0xFireball, IridiumIon Software, ExaPhaser Industries.");
            Console.WriteLine("All Rights Reserved.");
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                ShowHelp();
                return;
            }
            CommandLineParser parsedArgs = null;
            try
            {
                parsedArgs = new CommandLineParser(args);
            }
            catch
            {
                Console.WriteLine("Invalid arguments provided!");
                ShowHelp();
                return;
            }
            if (parsedArgs["-f"] != null && parsedArgs["-o"] != null)
            {
                var inputFile = parsedArgs["-f"];
                var outputFile = parsedArgs["-o"];

                var loadedAssembly = AssemblyLoader.LoadAssembly(File.Open(inputFile, FileMode.Open, FileAccess.Read));
                var protectionConfiguration = new ProtectionConfiguration();

                protectionConfiguration.InputAssemblies.Add(loadedAssembly);
                var protector = new WeaverProtector(protectionConfiguration);
                protector.Run();
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: NBWeaver [arguments]");
            Console.WriteLine(" -h : Show help");
            Console.WriteLine(" -f : Input file containing a .NET assembly");
            Console.WriteLine(" -o : Output assembly (use with -f)");
            Console.WriteLine(" -p : Input file containing a NetBitz Weaver project file.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("NBWeaver -f=MyFile.exe -o=MyProtectedFile.exe");
        }
    }
}