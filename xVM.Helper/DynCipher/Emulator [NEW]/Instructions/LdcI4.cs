﻿using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator.Instructions {
    internal class LdcI4 : EmuInstruction {
        internal override OpCode OpCode => OpCodes.Ldc_I4;

        internal override void Emulate(EmuContext context, Instruction instr) {
            context.Stack.Push(instr.Operand);
        }
    }
}
