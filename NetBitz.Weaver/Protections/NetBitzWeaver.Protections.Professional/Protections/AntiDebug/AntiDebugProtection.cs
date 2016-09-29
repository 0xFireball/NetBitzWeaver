using dnlib.DotNet;
using dnlib.DotNet.Emit;
using NetBitz.Weaver.Types;
using System;

namespace NetBitzWeaver.Protections.Professional.Protections.AntiDebug
{
    internal class AntiDebugProtection : IWeaverPipelineProtection
    {
        public string ProtectionGuid => "4fcd52ee-0f19-4c08-bdab-8aa6515cb6cd";
        public string Name => "Anti-Debug";
        public string Description => "Protects the module from being debugged by a managed debugger";
        public bool RequiresBatchProtection => false;

        public void Initialize()
        {
            //Nothing to do here
        }

        public void RunProtection(ProtectedModuleFactory factory)
        {
            //Prevent module from being debugged
            //TODO: Actually implement
            if (factory.Module.Kind == ModuleKind.Console || factory.Module.Kind == ModuleKind.Windows)
            {
                //This is only useful on executables
                //Crash if debugger is present:
                /*
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // ...
                }
                */

                //Load entry point CIL body

                var entryPointRef = factory.Module.EntryPoint;
                var entryPointBody = entryPointRef.Body;

                //Inject a check to test if debugger is there
                var debuggerTypeRef = new TypeRefUser(factory.Module, "System.Diagnostics", "Debugger", factory.Module.CorLibTypes.AssemblyRef);

                var isAttachedPropertyGetSig = MethodSig.CreateStatic(factory.Module.CorLibTypes.Boolean);

                var debuggerAttachedPropertyGetterRef = new MemberRefUser(factory.Module, "get_IsAttached", isAttachedPropertyGetSig, debuggerTypeRef);

                var originalFirstInstruction = entryPointBody.Instructions[0]; //Get first instruction to jump to
                entryPointBody.Instructions.Insert(0, OpCodes.Call.ToInstruction(debuggerAttachedPropertyGetterRef)); //System.Diagnostics.Debugger.IsAttached_get()
                entryPointBody.Instructions.Insert(1, OpCodes.Brfalse_S.ToInstruction(originalFirstInstruction)); //if (!result) goto instr_3
                entryPointBody.Instructions.Insert(2, OpCodes.Ret.ToInstruction()); //return
            }
        }

        public void RunProtection(ProtectedModuleFactoryCollection factoryCollection)
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            //Nothing to do here
        }
    }
}