using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
    internal class CTFLWBlock
    {
        public int ID = 0;
        public int nextBlock = 0;
        public List<Instruction> instructions = new List<Instruction>();
    }
}
