using NetBitz.Weaver.Extensibility;
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
            if (parsedArgs["f"] != null && parsedArgs["o"] != null)
            {
                var inputFile = parsedArgs["f"];
                var outputFile = parsedArgs["o"];

                Console.WriteLine($"Preparing NetBitz Weaver: {inputFile} -> {outputFile}");

                var protectionConfiguration = new ProtectionConfiguration();

                Console.WriteLine("Opening assembly...");
                var loadedAssembly = AssemblyLoader.LoadAssembly(File.Open(inputFile, FileMode.Open, FileAccess.Read));
                protectionConfiguration.InputAssemblies.Add(loadedAssembly);

                Console.WriteLine("Loading protections...");

                //Load all the protections that can be found into the
                var pluginConfigurator = new PluginConfigurator(protectionConfiguration);
                pluginConfigurator.LoadAllAvailableProtections();

                var protections = pluginConfigurator.Configuration.Protections;

                Console.WriteLine($"{protections.Count} Protections loaded: ");
                foreach (var protection in protections)
                {
                    Console.WriteLine($"    {protection.Name} - {protection.Description} - {protection.ProtectionGuid}");
                }

                Console.WriteLine("Preparing protection pipeline...");

                var protector = new WeaverProtector(protectionConfiguration);

                Console.WriteLine("Running protectors...");
                protector.Run();

                Console.WriteLine("Writing modules...");

                //var outputModuleStreams = new List<MemoryStream>();

                //There should only be one
                foreach (var factory in protector.Factories)
                {
                    using (var outputMemStream = new MemoryStream())
                    {
                        factory.Module.Write(outputMemStream, factory.WriterOptions);
                        using (var outputFileStream = File.Open(outputFile, FileMode.Create, FileAccess.Write))
                        {
                            outputMemStream.Position = 0; //reset memstream
                            outputMemStream.CopyTo(outputFileStream); //copy to file
                        }
                    }
                }

                //outputModuleStreams.Add(outputStream);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: NBWeaver [arguments]");
            Console.WriteLine(" -h : Show help");
            Console.WriteLine(" -f : Input file containing a .NET assembly. By default, all protections will be run.\nUse a project to override this behavior.");
            Console.WriteLine(" -o : Output assembly (use with -f)");
            Console.WriteLine(" -p : Input file containing a NetBitz Weaver project file.");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("NBWeaver -f=MyFile.exe -o=MyProtectedFile.exe");
        }
    }
}