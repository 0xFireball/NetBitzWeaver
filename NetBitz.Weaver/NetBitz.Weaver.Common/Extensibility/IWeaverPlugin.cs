using NetBitz.Weaver.Types;
using Platinum.PluginCore3;
using System.Collections.Generic;

namespace NetBitz.Weaver.Common.Extensibility
{
    public interface IWeaverPlugin : IPlatinumPlugin
    {
        List<IWeaverPipelineProtection> Protections { get; }
    }
}