using dnlib.DotNet;
using dnlib.DotNet.Emit;
using NetBitz.Weaver.Common.Helpers;
using NetBitz.Weaver.Types;
using NetBitzWeaver.Protections.Professional.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var entryPointMethodDef = factory.Module.EntryPoint;
                var entryPointBody = entryPointMethodDef.Body;

                //Inject a check to test if debugger is there
                var debuggerTypeRef = new TypeRefUser(factory.Module, "System.Diagnostics", "Debugger", factory.Module.CorLibTypes.AssemblyRef);

                var isAttachedPropertyGetSig = MethodSig.CreateStatic(factory.Module.CorLibTypes.Boolean);

                var debuggerAttachedPropertyGetterRef = new MemberRefUser(factory.Module, "get_IsAttached", isAttachedPropertyGetSig, debuggerTypeRef);

                var originalFirstInstruction = entryPointBody.Instructions[0]; //Get first instruction to jump to
                
                //inject into entry point function
                entryPointBody.Instructions.Insert(0, OpCodes.Call.ToInstruction(debuggerAttachedPropertyGetterRef)); //System.Diagnostics.Debugger.IsAttached_get()
                entryPointBody.Instructions.Insert(1, OpCodes.Brfalse_S.ToInstruction(originalFirstInstruction)); //if (!result) goto instr_3
                entryPointBody.Instructions.Insert(2, OpCodes.Ret.ToInstruction()); //return

                //inject antidebug helper into .cctor

                TypeDef antiDebugHelperType = RuntimeTypeLoader.GetRuntimeType(typeof(SafeAntiDebugHelper).FullName);
                List<IDnlibDef> members = InjectHelper.Inject(antiDebugHelperType, factory.Module.GlobalType, factory.Module).ToList();
                
                MethodDef cctor = factory.Module.GlobalType.FindOrCreateStaticConstructor();
                var init = (MethodDef)members.Single(method => method.Name == nameof(SafeAntiDebugHelper.Initialize));
                cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Call, init));
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