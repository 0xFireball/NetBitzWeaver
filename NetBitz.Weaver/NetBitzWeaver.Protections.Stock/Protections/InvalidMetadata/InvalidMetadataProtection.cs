using NetBitz.Weaver.Types;
using System;

namespace NetBitzWeaver.Protections.Stock.Protections.InvalidMetadata
{
    public class InvalidMetadataProtection : IWeaverPipelineProtection
    {
        public string Guid => "3558fd78-cc6a-4b0b-a0a7-22f559b62b01";
        public string Name => "Invalid Metadata";
        public string Description => "Injects invalid metadata into the assembly";
        public bool RequiresBatchProtection => false;

        public void RunProtection(ProtectedModuleFactory factory)
        {
            //Confuse metadata readers
            factory.WriterOptions.PEHeadersOptions.NumberOfRvaAndSizes = 8;

            //add junk extra data
            factory.WriterOptions.MetaDataOptions.TablesHeapOptions.ExtraData = 0xC0FEED93;
        }

        public void RunProtection(ProtectedModuleFactoryCollection factoryCollection)
        {
            throw new NotImplementedException();
        }
    }
}