using NetBitz.Weaver.Common.Types;
using System.Linq;

namespace NetBitz.Weaver.LayeringPipeline
{
    public class WeaverLayerer
    {
        private LayeringConfiguration Configuration { get; }
        public LayeredModuleFactoryCollection Factories { get; } = new LayeredModuleFactoryCollection();

        public WeaverLayerer(LayeringConfiguration layeringConfiguration)
        {
            Configuration = layeringConfiguration;
        }

        public void Run()
        {
            //Create factories
            var inputModuleFactories = Configuration.InputModules.Select(mod => new LayeredModuleFactory(mod)).ToList();
            //Create a factory collection
            Factories.AddRange(inputModuleFactories);

            foreach (var protection in Configuration.Protections)
            {
                if (!protection.RequiresBatchProtection)
                {
                    foreach (var moduleFactory in Factories)
                    {
                        protection.Initialize(); //prepare for new module
                        //Run the protection on the module's factory
                        protection.RunProtection(moduleFactory);
                    }
                }
                else
                {
                    //TODO: Batch protection
                }
            }
        }

        public void UnloadProviders()
        {
            Configuration.Protections.ForEach(pr => pr.Unload());
        }
    }
}