﻿using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator.Instructions {
    internal class And : EmuInstruction {
        internal override OpCode OpCode => OpCodes.And;

        internal override void Emulate(EmuContext context, Instruction instr) {
            var right = (int)context.Stack.Pop();
            var left = (int)context.Stack.Pop();

            context.Stack.Push(left & right);
        }
    }
}
