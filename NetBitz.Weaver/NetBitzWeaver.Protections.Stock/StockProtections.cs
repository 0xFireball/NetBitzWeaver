using NetBitz.Weaver.Common.Extensibility;
using NetBitz.Weaver.Types;
using NetBitzWeaver.Protections.Stock.Protections.AntiDebug;
using NetBitzWeaver.Protections.Stock.Protections.InvalidMetadata;
using Platinum.PluginCore3;
using Platinum.PluginCore3.Classes;
using System;
using System.Collections.Generic;

namespace NetBitzWeaver.Protections.Stock
{
    public class StockProtections : IWeaverPlugin, IPlatinumPlugin
    {
        public string Author => "0xFireball";

        public string Description => "The standard set of protections for NetBitz Weaver";

        public string Guid { get; set; } = "8d8f9a7b-7f79-4212-8e2c-907a12a24b7c";

        public IPlatinumPluginHost Host { get; set; }

        public string Name => "Stock Protections";

        public string PreferencesKey => "stockprotections";

        public List<IWeaverPipelineProtection> Protections { get; } = new List<IWeaverPipelineProtection>();

        public Version Version => typeof(StockProtections).Assembly.GetName().Version;

        public void ConfigurePlugin(PluginSettings pluginSettings)
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
            Protections.Add(new InvalidMetadataProtection());
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