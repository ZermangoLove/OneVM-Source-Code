using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class Jmp : IOpCode
    {
        public byte Code => Constants.OPCODELIST[114];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var slot = ctx.Stack[sp];
            ctx.Stack.SetTopPosition(--sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            ctx.Registers[Constants.OPCODELIST[18]].U8 = slot.U8;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class Jz : IOpCode
    {
        public byte Code => Constants.OPCODELIST[110];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];
            var valSlot = ctx.Stack[sp - 1];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(valSlot.U8 == 0)
                ctx.Registers[Constants.OPCODELIST[18]].U8 = adrSlot.U8;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class Jnz : IOpCode
    {
        public byte Code => Constants.OPCODELIST[112];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];
            var valSlot = ctx.Stack[sp - 1];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(valSlot.U8 != 0)
                ctx.Registers[Constants.OPCODELIST[18]].U8 = adrSlot.U8;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class Swt : IOpCode
    {
        public byte Code => Constants.OPCODELIST[116];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var tblSlot = ctx.Stack[sp];
            var valSlot = ctx.Stack[sp - 1];
            sp -= 2;
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            var index = valSlot.U4;
            var len = *(ushort*) (tblSlot.U8 - 2);
            if(index < len)
                ctx.Registers[Constants.OPCODELIST[18]].U8 += (ulong) (int) ((uint*) tblSlot.U8)[index];
            state = ExecutionState.Next;
        }
    }
}