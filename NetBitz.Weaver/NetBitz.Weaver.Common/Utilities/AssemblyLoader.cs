using dnlib.DotNet;
using System.IO;

namespace NetBitz.Weaver.Utilities
{
    public class AssemblyLoader
    {
        public static AssemblyDef LoadAssembly(Stream inputStream)
        {
            AssemblyDef loadedAssembly = AssemblyDef.Load(inputStream);
            return loadedAssembly;
        }
    }
}