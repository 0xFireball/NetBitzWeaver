using dnlib.DotNet;
using System.Reflection;

namespace NetBitzWeaver.Protections.Professional.Utilities
{
    internal static class RuntimeTypeLoader
    {
        static ModuleDef currentRtModule;
        
        public static TypeDef GetRuntimeType(string fullName)
        {
            if (currentRtModule == null)
            {
                
                Module module = typeof(RuntimeTypeLoader).Assembly.ManifestModule;
                currentRtModule = ModuleDefMD.Load(module);
                currentRtModule.EnableTypeDefFindCache = true;
            }
            return currentRtModule.Find(fullName, true);
        }
    }
}