using xVM.Runtime.Dynamic;
using xVM.Runtime.Execution;

namespace xVM.Runtime.OpCodes
{
    internal unsafe class PushRByte : IOpCode
    {
        public byte Code => Constants.OPCODELIST[74];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);

            var regId = ctx.ReadByte();
            var slot = ctx.Registers[regId];
            ctx.Stack[sp] = new VMSlot {U1 = slot.U1};

            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushRWord : IOpCode
    {
        public byte Code => Constants.OPCODELIST[76];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);

            var regId = ctx.ReadByte();
            var slot = ctx.Registers[regId];
            ctx.Stack[sp] = new VMSlot {U2 = slot.U2};

            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushRDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[78];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);

            var regId = ctx.ReadByte();
            var slot = ctx.Registers[regId];
            if(regId == Constants.OPCODELIST[16] || regId == Constants.OPCODELIST[14])
                ctx.Stack[sp] = new VMSlot {O = new StackRef(slot.U4)};
            else
                ctx.Stack[sp] = new VMSlot {U4 = slot.U4};

            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushRQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[80];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);

            var regId = ctx.ReadByte();
            var slot = ctx.Registers[regId];
            ctx.Stack[sp] = new VMSlot {U8 = slot.U8};

            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushRObject : IOpCode
    {
        public byte Code => Constants.OPCODELIST[72];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);

            var regId = ctx.ReadByte();
            var slot = ctx.Registers[regId];
            ctx.Stack[sp] = slot;

            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushIDword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[82];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            ulong imm = ctx.ReadByte();
            imm |= (ulong) ctx.ReadByte() << 8;
            imm |= (ulong) ctx.ReadByte() << 16;
            imm |= (ulong) ctx.ReadByte() << 24;
            var sx = (imm & 0x80000000) != 0 ? 0xffffffffUL << 32 : 0;
            ctx.Stack[sp] = new VMSlot {U8 = sx | imm};
            state = ExecutionState.Next;
        }
    }

    internal unsafe class PushIQword : IOpCode
    {
        public byte Code => Constants.OPCODELIST[84];

        public void Run(VMContext ctx, out ExecutionState state)
        {
            var sp = ctx.Registers[Constants.OPCODELIST[16]].U4;
            ctx.Stack.SetTopPosition(++sp);
            ctx.Registers[Constants.OPCODELIST[16]].U4 = sp;

            ulong imm = ctx.ReadByte();
            imm |= (ulong) ctx.ReadByte() << 8;
            imm |= (ulong) ctx.ReadByte() << 16;
            imm |= (ulong) ctx.ReadByte() << 24;
            imm |= (ulong) ctx.ReadByte() << 32;
            imm |= (ulong) ctx.ReadByte() << 40;
            imm |= (ulong) ctx.ReadByte() << 48;
            imm |= (ulong) ctx.ReadByte() << 56;
            ctx.Stack[sp] = new VMSlot {U8 = imm};
            state = ExecutionState.Next;
        }
    }
}