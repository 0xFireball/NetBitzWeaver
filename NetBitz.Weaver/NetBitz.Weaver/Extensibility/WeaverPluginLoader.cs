using Platinum.PluginCore3;
using System.Reflection;

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