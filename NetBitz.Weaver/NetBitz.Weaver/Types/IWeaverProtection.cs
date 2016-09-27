using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace NetBitz.Weaver.Types
{
    public interface IWeaverProtection
    {
        string Guid { get; }
        string Name { get; }
        string Description { get; }

        void RunProtection(ProtectedModuleFactory factory);
    }
}