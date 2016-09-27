using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace NetBitz.Weaver.Types
{
    public class ProtectedModuleFactory
    {
        public ModuleDef Module { get; }
        public ModuleWriterOptions WriterOptions { get; private set; }

        public ProtectedModuleFactory(ModuleDef module)
        {
            Module = module;
            CreateModuleObjects();
        }

        private void CreateModuleObjects()
        {
            WriterOptions = new ModuleWriterOptions(Module);
        }
    }
}