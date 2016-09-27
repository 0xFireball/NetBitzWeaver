using System.Collections.Generic;

namespace NetBitz.Weaver.CmdLine
{
    internal class CommandLineParser
    {
        public CommandLineParser(string[] args)
        {
            foreach (string arg in args)
            {
                //argument
                if (arg.StartsWith("-", System.StringComparison.InvariantCulture))
                {
                    var inpArg = arg.Replace("\"", ""); //Strip quotes
                    inpArg = arg.Replace("-", ""); //Strip dashes
                    var argParts = inpArg.Split('=');
                    var argName = argParts[0];
                    var argValue = argParts[1];
                    Arguments[argName] = argValue;
                }
            }
        }

        public Dictionary<string, string> Arguments { get; } = new Dictionary<string, string>();

        public string this[string argumentName]
        {
            get
            {
                return Arguments[argumentName];
            }
        }
    }
}