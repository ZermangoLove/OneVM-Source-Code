using System.Collections.Generic;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.AST.IR
{
    public class IRInstrList : List<IRInstruction>
    {
        public void VisitInstrs<T>(VisitFunc<IRInstrList, IRInstruction, T> visitFunc, T arg)
        {
            for(var i = 0; i < Count; i++)
                visitFunc(this, this[i], ref i, arg);
        }
    }
}