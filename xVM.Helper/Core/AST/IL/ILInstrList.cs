using System.Collections.Generic;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.AST.IL
{
    public class ILInstrList : List<ILInstruction>
    {
        public void VisitInstrs<T>(VisitFunc<ILInstrList, ILInstruction, T> visitFunc, T arg)
        {
            for(var i = 0; i < Count; i++)
                visitFunc(this, this[i], ref i, arg);
        }
    }
}