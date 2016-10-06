using dnlib.DotNet;
using dnlib.DotNet.Writer;
using NetBitz.Weaver.Common.Utilities;

namespace NetBitz.Weaver.Common.Types
{
    public class LayeredModuleFactory
    {
        public ModuleDef Module { get; }
        public ModuleWriterOptions WriterOptions { get; private set; }
        public ModuleWriterListener CurrentModuleWriterListener => WriterOptions.Listener as ModuleWriterListener;

        public LayeredModuleFactory(ModuleDef module)
        {
            Module = module;
            CreateModuleObjects();
        }

        private void CreateModuleObjects()
        {
            WriterOptions = new ModuleWriterOptions(Module);

            //Add custom listener
            WriterOptions.Listener = new ModuleWriterListener();
        }
    }
}