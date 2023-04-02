using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
    internal class NullBlock
    {
        public NullBlock()
        {
            Instructions = new List<Instruction>();
        }

        public List<Instruction> Instructions { get; set; }

        public int Number { get; set; }
    }
}
