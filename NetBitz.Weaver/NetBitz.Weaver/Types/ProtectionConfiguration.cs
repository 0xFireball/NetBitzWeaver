using dnlib.DotNet;
using NetBitz.Weaver.Protections;
using System.Collections.Generic;

namespace NetBitz.Weaver.Types
{
    public class ProtectionConfiguration
    {
        public List<IWeaverProtection> Protections { get; } = new List<IWeaverProtection>();
        public List<AssemblyDef> InputAssemblies { get; } = new List<AssemblyDef>();

        public static ProtectionConfiguration GetDefault()
        {
            var defaultConfig = new ProtectionConfiguration();
            defaultConfig.Protections.Add(new InvalidMetadataProtection());
            return defaultConfig;
        }
    }
}