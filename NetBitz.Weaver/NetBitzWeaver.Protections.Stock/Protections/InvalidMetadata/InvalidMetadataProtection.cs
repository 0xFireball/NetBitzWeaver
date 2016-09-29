using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using NetBitz.Weaver.Common.Services;
using NetBitz.Weaver.Common.Utilities;
using NetBitz.Weaver.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetBitzWeaver.Protections.Stock.Protections.InvalidMetadata
{
    public class InvalidMetadataProtection : IWeaverPipelineProtection
    {
        public string ProtectionGuid => "3558fd78-cc6a-4b0b-a0a7-22f559b62b01";
        public string Name => "Invalid Metadata";
        public string Description => "Injects invalid metadata into the assembly";
        public bool RequiresBatchProtection => false;

        private List<ProtectedModuleFactory> listenerBoundFactories = new List<ProtectedModuleFactory>();
        private Random sysRandom = new Random();
        private RandomGenerator randomSvc;

        public void Initialize()
        {
            //Generate a random 32-byte seed
            byte[] seedBuffer = new byte[32];
            sysRandom.NextBytes(seedBuffer);
            randomSvc = new RandomGenerator(seedBuffer);
        }

        public void RunProtection(ProtectedModuleFactory factory)
        {
            //Confuse metadata readers
            factory.WriterOptions.PEHeadersOptions.NumberOfRvaAndSizes = 8;

            //add junk extra data

            uint thirtyBits = (uint)sysRandom.Next(1 << 30);
            uint twoBits = (uint)sysRandom.Next(1 << 2);
            uint randomUintBits = (thirtyBits << 2) | twoBits;
            factory.WriterOptions.MetaDataOptions.TablesHeapOptions.ExtraData = randomUintBits; //0xC0FFEE43;

            //register events
            factory.CurrentModuleWriterListener.OnWriterEvent += OnWriterEvent;
            listenerBoundFactories.Add(factory);
        }

        private void Randomize<T>(MDTable<T> table) where T : IRawRow
        {
            List<T> rows = table.ToList();
            randomSvc.Shuffle(rows);
            table.Reset();
            foreach (T row in rows)
                table.Add(row);
        }

        private void OnWriterEvent(object sender, ModuleWriterListenerEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;
            if ((ProtectionGuid != null) && (e.WriterEvent == ModuleWriterEvent.MDEndCreateTables))
            {
                // These hurts reflection

                /*
                uint methodLen = (uint)writer.MetaData.TablesHeap.MethodTable.Rows + 1;
                uint fieldLen = (uint)writer.MetaData.TablesHeap.FieldTable.Rows + 1;

                var root = writer.MetaData.TablesHeap.TypeDefTable.Add(new RawTypeDefRow(
                        0, 0x7fff7fff, 0, 0x3FFFD, fieldLen, methodLen));
                writer.MetaData.TablesHeap.NestedClassTable.Add(new RawNestedClassRow(root, root));

                var namespaces = writer.MetaData.TablesHeap.TypeDefTable
                    .Select(row => row.Namespace)
                    .Distinct()
                    .ToList();
                foreach (var ns in namespaces)
                {
                    if (ns == 0) continue;
                    var type = writer.MetaData.TablesHeap.TypeDefTable.Add(new RawTypeDefRow(
                        0, 0, ns, 0x3FFFD, fieldLen, methodLen));
                    writer.MetaData.TablesHeap.NestedClassTable.Add(new RawNestedClassRow(root, type));
                }

                foreach (var row in writer.MetaData.TablesHeap.ParamTable)
                    row.Name = 0x7fff7fff;
                */

                writer.MetaData.TablesHeap.ModuleTable.Add(new RawModuleRow(0, 0x7fff7fff, 0, 0, 0));
                writer.MetaData.TablesHeap.AssemblyTable.Add(new RawAssemblyRow(0, 0, 0, 0, 0, 0, 0, 0x7fff7fff, 0));
                int r = randomSvc.NextInt32(8, 16);
                for (int i = 0; i < r; i++)
                    writer.MetaData.TablesHeap.ENCLogTable.Add(new RawENCLogRow(randomSvc.NextUInt32(), randomSvc.NextUInt32()));
                r = randomSvc.NextInt32(8, 16);
                for (int i = 0; i < r; i++)
                    writer.MetaData.TablesHeap.ENCMapTable.Add(new RawENCMapRow(randomSvc.NextUInt32()));

                //Randomize(writer.MetaData.TablesHeap.NestedClassTable);
                Randomize(writer.MetaData.TablesHeap.ManifestResourceTable);
                //Randomize(writer.MetaData.TablesHeap.GenericParamConstraintTable);

                writer.TheOptions.MetaDataOptions.TablesHeapOptions.ExtraData = randomSvc.NextUInt32();
                writer.TheOptions.MetaDataOptions.TablesHeapOptions.UseENC = false;
                writer.TheOptions.MetaDataOptions.MetaDataHeaderOptions.VersionString += "\0\0\0\0";

                /*
                We are going to create a new specific '#GUID' Heap to avoid UnConfuserEX to work.
                <sarcasm>UnConfuserEX is so well coded, it relies on static cmp between values</sarcasm>
                If you deobfuscate this tool, you can see that it check for #GUID size and compare it to
                '16', so we have to create a new array of byte wich size is exactly 16 and put it into
                our brand new Heap
                */
                //
                writer.TheOptions.MetaDataOptions.OtherHeapsEnd.Add(new RawHeap("#GUID", Guid.NewGuid().ToByteArray()));
                //
                writer.TheOptions.MetaDataOptions.OtherHeapsEnd.Add(new RawHeap("#Strings", new byte[1]));
                writer.TheOptions.MetaDataOptions.OtherHeapsEnd.Add(new RawHeap("#Blob", new byte[1]));
                writer.TheOptions.MetaDataOptions.OtherHeapsEnd.Add(new RawHeap("#Schema", new byte[1]));
            }
            else if (e.WriterEvent == ModuleWriterEvent.MDOnAllTablesSorted)
            {
                writer.MetaData.TablesHeap.DeclSecurityTable.Add(new RawDeclSecurityRow(
                                                                     unchecked(0x7fff), 0xffff7fff, 0xffff7fff));
                /*
                writer.MetaData.TablesHeap.ManifestResourceTable.Add(new RawManifestResourceRow(
                    0x7fff7fff, (uint)ManifestResourceAttributes.Private, 0x7fff7fff, 2));
                */
            }
        }

        public void RunProtection(ProtectedModuleFactoryCollection factoryCollection)
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            //unregister events
            listenerBoundFactories.ForEach(f => f.CurrentModuleWriterListener.OnWriterEvent -= OnWriterEvent);
        }

        /// <summary>
        /// A raw heap
        /// </summary>
        private class RawHeap : HeapBase
        {
            private readonly byte[] content;
            private readonly string name;

            public RawHeap(string name, byte[] content)
            {
                this.name = name;
                this.content = content;
            }

            public override string Name
            {
                get { return name; }
            }

            public override uint GetRawLength()
            {
                return (uint)content.Length;
            }

            protected override void WriteToImpl(BinaryWriter writer)
            {
                writer.Write(content);
            }
        }
    }
}