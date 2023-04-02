using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator
{
    internal abstract class EmuInstruction {
        internal abstract OpCode OpCode { get; }

        internal abstract void Emulate(EmuContext context, Instruction instr);
    }
}
