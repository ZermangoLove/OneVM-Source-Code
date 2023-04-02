using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator.Instructions {
    internal class Stloc : EmuInstruction {
        internal override OpCode OpCode => OpCodes.Stloc_S;

        internal override void Emulate(EmuContext context, Instruction instr) {
            var val = context.Stack.Pop();
            context.SetLocalValue((Local)instr.Operand, val);
        }
    }
}
