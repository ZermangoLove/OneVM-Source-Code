using System;
using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class SindByte : IOpCode
    {
        public byte Code => Constants.OPCODELIST[62];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference)
            {
                ((IReference) adrSlot.O).SetValue(ctx, valSlot, PointerType.BYTE);
            }
            else
            {
                var value = valSlot.U1;
                var ptr = (byte*) adrSlot.U8;
                *ptr = value;
            }
            state = ExecutionState.Next;
        }
    }

    internal unsafe class SindWord : IOpCode
    {
        public byte Code => Constants.OPCODELIST[64];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference)
            {
                ((IReference) adrSlot.O).SetValue(ctx, valSlot, PointerType.WORD);
            }
            else
            {
                var value = valSlot.U2;
                var ptr = (ushort*) adrSlot.U8;
                *ptr = value;
            }
            state = ExecutionState.Next;
        }
    }

    internal unsafe class SindDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[66];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference)
            {
                ((IReference) adrSlot.O).SetValue(ctx, valSlot, PointerType.DWORD);
            }
            else
            {
                var value = valSlot.U4;
                var ptr = (uint*) adrSlot.U8;
                *ptr = value;
            }
            state = ExecutionState.Next;
        }
    }

    internal unsafe class SindQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[68];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference)
            {
                ((IReference) adrSlot.O).SetValue(ctx, valSlot, PointerType.QWORD);
            }
            else
            {
                var value = valSlot.U8;
                var ptr = (ulong*) adrSlot.U8;
                *ptr = value;
            }
            state = ExecutionState.Next;
        }
    }

    internal unsafe class SindObject : IOpCode
    {
        public byte Code => Constants.OPCODELIST[60];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference) ((IReference) adrSlot.O).SetValue(ctx, valSlot, PointerType.OBJECT);
            else throw new ExecutionEngineException();
            state = ExecutionState.Next;
        }
    }

    internal unsafe class SindPtr : IOpCode
    {
        public byte Code => Constants.OPCODELIST[58];

        public unsafe void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            var adrSlot = ctx.Stack[sp--];
            var valSlot = ctx.Stack[sp--];
            ctx.Stack.SetTopPosition(sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            if(adrSlot.O is IReference)
            {
                ((IReference) adrSlot.O).SetValue(ctx, valSlot, IntPtr.Size == 8 ? PointerType.QWORD : PointerType.DWORD);
            }
            else
            {
                if(IntPtr.Size == 8)
                {
                    var ptr = (ulong*) adrSlot.U8;
                    *ptr = valSlot.U8;
                }
                else
                {
                    var ptr = (uint*) adrSlot.U8;
                    *ptr = valSlot.U4;
                }
            }
            state = ExecutionState.Next;
        }
    }
}