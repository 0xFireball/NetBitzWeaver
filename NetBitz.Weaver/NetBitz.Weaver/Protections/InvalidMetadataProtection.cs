using dnlib.DotNet;
using dnlib.DotNet.Writer;
using NetBitz.Weaver.Types;

namespace NetBitz.Weaver.Protections
{
    public class InvalidMetadataProtection : IWeaverProtection
    {
        public string Guid => "3558fd78-cc6a-4b0b-a0a7-22f559b62b01";
        public string Name => "Invalid Metadata";
        public string Description => "Injects invalid metadata into the assembly";

        public void RunProtection(ModuleDef[] inputModule, ModuleWriterOptions writerOpts)
        {
            //Confuse metadata readers
            writerOpts.PEHeadersOptions.NumberOfRvaAndSizes = 8;

            //add junk extra data
            writerOpts.MetaDataOptions.TablesHeapOptions.ExtraData = 0xC0FEED93;
        }
    }
}