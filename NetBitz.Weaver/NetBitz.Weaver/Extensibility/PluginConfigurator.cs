using NetBitz.Weaver.Types;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetBitz.Weaver.Extensibility
{
    /// <summary>
    /// Adds protections from plugins to the protection pipeline of a configuration object
    /// </summary>
    public class PluginConfigurator
    {
        public ProtectionConfiguration Configuration { get; }
        public WeaverPluginLoader PluginLoader { get; }

        public PluginConfigurator(ProtectionConfiguration configuration)
        {
            Configuration = configuration;
            PluginLoader = new WeaverPluginLoader();
        }

        public void LoadAllAvailableProtections()
        {
            LoadAllAvailablePlugins();
            var pluginInstances = PluginLoader.CoreLoader.Factory.AvailablePlugins.Select(plg => plg.Instance);
            foreach (var plugin in pluginInstances)
            {
                //Prepare plugin
                plugin.LoadComponents();

                //Add all protections to pipeline
                Configuration.Protections.AddRange(plugin.Protections);
            }
        }

        private void LoadAllAvailablePlugins()
        {
            //Base directory
            PluginLoader.CoreLoader.Factory.LoadPluginsInBaseDirectory();

            //Plugins subdirectory of executable paths
            var executableDirectoryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            PluginLoader.CoreLoader.Factory.LoadPluginsFromDirectory(executableDirectoryPath);

            //Plugins folder in AppData
            var appDataPluginPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NetBitz Weaver", "plugins");
            Directory.CreateDirectory(appDataPluginPath); //create if not exist
            PluginLoader.CoreLoader.Factory.LoadPluginsFromDirectory(appDataPluginPath);
        }
    }
}