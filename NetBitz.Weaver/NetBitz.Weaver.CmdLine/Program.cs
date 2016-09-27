using System;

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
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: NBWeaver [arguments]");
            Console.WriteLine();
        }
    }
}