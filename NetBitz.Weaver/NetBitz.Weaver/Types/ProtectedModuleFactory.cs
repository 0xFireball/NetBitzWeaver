using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBitz.Weaver.Types
{
    public class ProtectedModuleFactory
    {
        public ModuleDef Module { get; }

        public ProtectedModuleFactory(ModuleDef module)
        {
            Module = module;
            CreateModuleObjects();
        }

        private void CreateModuleObjects()
        {
            throw new NotImplementedException();
        }
    }
}
