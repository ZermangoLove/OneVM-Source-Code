using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator.Instructions {
    internal class Br : EmuInstruction {
        internal override OpCode OpCode => OpCodes.Br;

        internal override void Emulate(EmuContext context, Instruction instr) {
            context.InstructionPointer = context.Instructions.IndexOf((Instruction)instr.Operand);
        }
    }
}
