using dnlib.DotNet;
using System.Collections.Generic;

namespace NetBitz.Weaver.Types
{
    /// <summary>
    /// Represents a configuration for the NetBitz Weaver protection pipeline
    /// </summary>
    public class ProtectionConfiguration
    {
        public List<IWeaverPipelineProtection> Protections { get; } = new List<IWeaverPipelineProtection>();
        public List<ModuleDefMD> InputModules { get; } = new List<ModuleDefMD>();
    }
}