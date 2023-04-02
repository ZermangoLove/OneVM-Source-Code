using dnlib.DotNet;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.RT;

namespace xVM.Helper.Core.AST.IL
{
    public class ILBlock : BasicBlock<ILInstrList>
    {
        public ILBlock(int id, ILInstrList content) : base(id, content) { }

        public virtual IVMChunk CreateChunk(VMRuntime rt, MethodDef method)
        {
            return new BasicBlockChunk(rt, method, this);
        }
    }
}