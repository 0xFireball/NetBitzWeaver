using dnlib.DotNet;
using System.Collections.Generic;

namespace NetBitz.Weaver.Common.Types
{
    /// <summary>
    /// Represents a configuration for the NetBitz Weaver layering pipeline
    /// </summary>
    public class LayeringConfiguration
    {
        public List<IWeaverLayeringPipelineProtection> Protections { get; } = new List<IWeaverLayeringPipelineProtection>();
        public List<ModuleDefMD> InputModules { get; } = new List<ModuleDefMD>();
    }
}