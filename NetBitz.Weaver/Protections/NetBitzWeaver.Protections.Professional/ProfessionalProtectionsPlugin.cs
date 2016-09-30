using NetBitz.Weaver.Common.Extensibility;
using NetBitz.Weaver.Types;
using NetBitzWeaver.Protections.Professional.Protections.AntiDebug;
using Platinum.PluginCore3;
using Platinum.PluginCore3.Classes;
using System;
using System.Collections.Generic;

namespace NetBitzWeaver.Protections.Stock
{
    public class StockProtections : IWeaverPlugin, IPlatinumPlugin
    {
        public string Author => "0xFireball";

        public string Description => "A set of protections for NetBitz Weaver Professional Edition";

        public string PluginGuid { get; set; } = "8de7e395-6831-47e0-8bbd-6ffe108b765b";

        public IPlatinumPluginHost Host { get; set; }

        public string Name => "Professoinal Protections";

        public string PreferencesKey => "professionalprotections";

        public List<IWeaverPipelineProtection> Protections { get; } = new List<IWeaverPipelineProtection>();

        public Version Version => typeof(StockProtections).Assembly.GetName().Version;

        public void ConfigurePlugin(PluginPreferences pluginSettings)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //Success
        }

        public void Initialize()
        {
            //Nothing really
        }

        public void LoadComponents()
        {
            //Build protections list
            Protections.Add(new AntiDebugProtection());
        }

        public void Shutdown()
        {
            //Nothing much here
        }

        public void UnloadComponents()
        {
            //Nothing really here
        }
    }
}