using NetBitz.Weaver.Types;
using System.Linq;

namespace NetBitz.Weaver.ProtectionPipeline
{
    public class WeaverProtector
    {
        private ProtectionConfiguration Configuration { get; }

        public WeaverProtector(ProtectionConfiguration protectionConfiguration)
        {
            Configuration = protectionConfiguration;
        }

        public void Run()
        {
            var inputModules = Configuration.InputAssemblies.Select(asm=>asm.ManifestModule);
            //Protect each module
        }
    }
}