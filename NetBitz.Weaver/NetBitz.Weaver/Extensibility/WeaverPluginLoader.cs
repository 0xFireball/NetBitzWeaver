using NetBitz.Weaver.Common.Extensibility;
using Platinum.PluginCore3;

namespace NetBitz.Weaver.Extensibility
{
    public class WeaverPluginLoader
    {
        public PluginLoader<IWeaverPlugin> PluginLoader { get; }

        public WeaverPluginLoader()
        {
            PluginLoader = new PluginLoader<IWeaverPlugin>();
        }
    }
}