using dnlib.DotNet;
using dnlib.DotNet.Emit;
using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.AST.IR;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.VM;
using xVM.Helper.Core.VMIL;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core.RT.Mutation
{
    internal class RuntimeHelpers
    {
        private RTConstants constants;

        private MethodDef methodINIT;
        public readonly VMRuntime VMRT;
        private readonly ModuleDef rtModule;

        public RuntimeHelpers(RTConstants constants, VMRuntime rt)
        {
            this.VMRT = rt;
            this.rtModule = rt.RTModule;
            this.constants = constants;
            AllocateHelpers();
        }

        public ulong INIT
        {
            get;
            private set;
        }

        private MethodDef CreateHelperMethod(string name)
        {
            var helper = new MethodDefUser(name, MethodSig.CreateStatic(rtModule.CorLibTypes.Void));
            helper.Body = new CilBody();
            return helper;
        }

        private void AllocateHelpers()
        {
            methodINIT = CreateHelperMethod("INIT");
            INIT = VMRT.Descriptor.Data.GetExportUlongID(methodINIT);
        }

        public void AddHelpers()
        {
            var scope = new ScopeBlock();

            var initBlock = new BasicBlock<IRInstrList>(1, new IRInstrList
            {
                new IRInstruction(IROpCode.RET)
            });
            scope.Content.Add(initBlock);

            var retnBlock = new BasicBlock<IRInstrList>(0, new IRInstrList
            {
                new IRInstruction(IROpCode.VCALL, IRConstant.FromI4(VMRT.Descriptor.Runtime.VMCall[VMCalls.EXIT]))
            });
            scope.Content.Add(initBlock);

            CompileHelpers(methodINIT, scope);

            var info = VMRT.Descriptor.Data.LookupInfo(methodINIT);
            scope.ProcessBasicBlocks<ILInstrList>(block =>
            {
                if(block.Id == 1)
                {
                    AddHelper(null, methodINIT, (ILBlock) block);
                    var blockKey = info.BlockKeys[block];
                    info.EntryKey = blockKey.EntryKey;
                    info.ExitKey = blockKey.ExitKey;
                }
                VMRT.AddBlock(methodINIT, (ILBlock) block);
            });
        }

        private void AddHelper(VMMethodInfo info, MethodDef method, ILBlock block)
        {
            var helperScope = new ScopeBlock();
            block.Id = 0;
            helperScope.Content.Add(block);
            if(info != null)
            {
                var helperInfo = new VMMethodInfo();
                var keys = info.BlockKeys[block];
                helperInfo.RootScope = helperScope;
                helperInfo.EntryKey = keys.EntryKey;
                helperInfo.ExitKey = keys.ExitKey;
                VMRT.Descriptor.Data.SetInfo(method, helperInfo);
            }
            VMRT.AddHelper(method, helperScope, block);
        }

        private void CompileHelpers(MethodDef method, ScopeBlock scope)
        {
            var methodCtx = new IRContext(method, method.Body);
            methodCtx.IsRuntime = true;
            var irTransformer = new IRTransformer(scope, methodCtx, VMRT);
            irTransformer.Transform();

            var ilTranslator = new ILTranslator(VMRT);
            var ilTransformer = new ILTransformer(method, scope, VMRT);
            ilTranslator.Translate(scope);
            ilTransformer.Transform();

            var postTransformer = new ILPostTransformer(method, scope, VMRT);
            postTransformer.Transform();
        }
    }
}