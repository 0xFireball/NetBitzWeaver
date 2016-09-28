using dnlib.DotNet;
using System.Collections.Generic;

namespace NetBitz.Weaver.Types
{
    /// <summary>
    /// Represents a configuration for the NetBitz Weaver protection pipeline
    /// </summary>
    public class ProtectionConfiguration
    {
        public List<IWeaverProtection> Protections { get; } = new List<IWeaverProtection>();
        public List<AssemblyDef> InputAssemblies { get; } = new List<AssemblyDef>();
    }
}