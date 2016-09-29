using NetBitz.Weaver.Types;
using System.Linq;

namespace NetBitz.Weaver.ProtectionPipeline
{
    public class WeaverProtector
    {
        private ProtectionConfiguration Configuration { get; }
        public ProtectedModuleFactoryCollection Factories { get; } = new ProtectedModuleFactoryCollection();

        public WeaverProtector(ProtectionConfiguration protectionConfiguration)
        {
            Configuration = protectionConfiguration;
        }

        public void Run()
        {
            //Create factories
            var inputModuleFactories = Configuration.InputAssemblies.Select(asm => new ProtectedModuleFactory(asm.ManifestModule)).ToList();
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

        public void UnloadProtectors()
        {
            Configuration.Protections.ForEach(pr => pr.Unload());
        }
    }
}