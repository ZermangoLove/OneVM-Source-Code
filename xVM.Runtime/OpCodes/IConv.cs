using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class IConvPtr : IOpCode
    {
        public byte Code => Constants.OPCODELIST[170];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];

            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~Constants.OPCODELIST[30]);
            if(!(System.IntPtr.Size == 8 && valueSlot.U8 >> 32 != 0))
                fl |= Constants.OPCODELIST[30];
            ctx.Registers[Constants.OPCODELIST[20]].U1 = fl;

            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class IConvR64 : IOpCode
    {
        public byte Code => Constants.OPCODELIST[172];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            // coreclr/src/vm/jithelpers.cpp JIT_Dbl2ULngOvf & JIT_Dbl2LngOvf

            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var valueSlot = ctx.Stack[sp];

            const double two63 = 2147483648.0 * 4294967296.0;
            const double two64 = 4294967296.0 * 4294967296.0;

            var value = valueSlot.R8;
            valueSlot.U8 = (ulong) (long) value;
            var fl = (byte) (ctx.Registers[Constants.OPCODELIST[20]].U1 & ~Constants.OPCODELIST[30]);

            if((fl & Constants.OPCODELIST[38]) != 0)
            {
                if(!(value > -1.0 && value < two64))
                    fl |= Constants.OPCODELIST[30];

                if(!(value < two63))
                    valueSlot.U8 = (ulong) ((long) value - two63) + 0x8000000000000000UL;
            }
            else
            {
                if(!(value > -two63 - 0x402 && value < two63))
                    fl |= Constants.OPCODELIST[30];
            }

            ctx.Registers[Constants.OPCODELIST[20]].U1 = (byte) (fl & ~Constants.OPCODELIST[38]);

            ctx.Stack[sp] = valueSlot;

            state = ExecutionState.Next;
        }
    }
}