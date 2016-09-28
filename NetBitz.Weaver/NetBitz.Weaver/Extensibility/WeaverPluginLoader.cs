using NetBitz.Weaver.Common.Extensibility;
using Platinum.PluginCore3;

namespace NetBitz.Weaver.Extensibility
{
    public class WeaverPluginLoader
    {
        public PluginLoader<IWeaverPlugin> CoreLoader { get; }

        public WeaverPluginLoader()
        {
            CoreLoader = new PluginLoader<IWeaverPlugin>();
        }
    }
}