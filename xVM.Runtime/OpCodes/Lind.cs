using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class LindByte : IOpCode
    {
        public byte Code => Constants.OPCODELIST[50];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference)
            {
                valSlot = ((IReference) adrSlot.O).GetValue(ctx, PointerType.BYTE);
            }
            else
            {
                var ptr = (byte*) adrSlot.U8;
                valSlot = new VMSlot {U1 = *ptr};
            }
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class LindWord : IOpCode
    {
        public byte Code => Constants.OPCODELIST[52];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference)
            {
                valSlot = ((IReference) adrSlot.O).GetValue(ctx, PointerType.WORD);
            }
            else
            {
                var ptr = (ushort*) adrSlot.U8;
                valSlot = new VMSlot {U2 = *ptr};
            }
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class LindDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[54];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference)
            {
                valSlot = ((IReference) adrSlot.O).GetValue(ctx, PointerType.DWORD);
            }
            else
            {
                var ptr = (uint*) adrSlot.U8;
                valSlot = new VMSlot {U4 = *ptr};
            }
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class LindQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[56];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference)
            {
                valSlot = ((IReference) adrSlot.O).GetValue(ctx, PointerType.QWORD);
            }
            else
            {
                var ptr = (ulong*) adrSlot.U8;
                valSlot = new VMSlot {U8 = *ptr};
            }
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class LindObject : IOpCode
    {
        public byte Code => Constants.OPCODELIST[48];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference) valSlot = ((IReference) adrSlot.O).GetValue(ctx, PointerType.OBJECT);
            else throw new ExecutionEngineException();
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }

    internal unsafe class LindPtr : IOpCode
    {
        public byte Code => Constants.OPCODELIST[46];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp];

            VMSlot valSlot;
            if(adrSlot.O is IReference)
            {
                valSlot = ((IReference) adrSlot.O).GetValue(ctx, IntPtr.Size == 8 ? PointerType.QWORD : PointerType.DWORD);
            }
            else
            {
                if(IntPtr.Size == 8)
                {
                    var ptr = (ulong*) adrSlot.U8;
                    valSlot = new VMSlot {U8 = *ptr};
                }
                else
                {
                    var ptr = (uint*) adrSlot.U8;
                    valSlot = new VMSlot {U4 = *ptr};
                }
            }
            ctx.Stack[sp] = valSlot;

            state = ExecutionState.Next;
        }
    }
}