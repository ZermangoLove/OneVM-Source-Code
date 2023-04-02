using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Emulator.Instructions {
    internal class Add : EmuInstruction {
        internal override OpCode OpCode => OpCodes.Add;

        internal override void Emulate(EmuContext context, Instruction instr) {
            var right = (int)context.Stack.Pop();
            var left = (int)context.Stack.Pop();

            context.Stack.Push(left + right);
        }
    }
}
