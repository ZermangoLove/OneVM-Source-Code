using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.DynCipher;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.Protections
{
	internal class CFContext
	{
        public double Intensity;
        public int Depth;
        public MethodDef Method;
        public DynCipherService DynCipher;
		public RandomGenerator Random;

        public void ProcessMethod(CilBody nop, CFContext ctx)
        {
            var blocks = ControlFlowGraph.Construct(nop);
            var root = CEXBlockParser.ParseBody(blocks.Body);
            new SwitchMangler().Mangle(blocks.Body, root, ctx);
            blocks.Body.Instructions.Clear();
            root.ToBody(blocks.Body);
            foreach (var handler in blocks.Body.ExceptionHandlers)
            {
                int num2 = blocks.Body.Instructions.IndexOf(handler.TryEnd) + 1;
                handler.TryEnd = (num2 < blocks.Body.Instructions.Count) ? blocks.Body.Instructions[num2] : null;
                num2 = blocks.Body.Instructions.IndexOf(handler.HandlerEnd) + 1;
                handler.HandlerEnd = (num2 < blocks.Body.Instructions.Count) ? blocks.Body.Instructions[num2] : null;
            }
            blocks.Body.KeepOldMaxStack = true;
            nop = blocks.Body;
        }

        public void AddJump(IList<Instruction> instrs, Instruction target)
        {
            if (((!this.Method.Module.IsClr40 && false) && (!this.Method.DeclaringType.HasGenericParameters && !this.Method.HasGenericParameters)) && ((instrs[0].OpCode.FlowControl == FlowControl.Call) || (instrs[0].OpCode.FlowControl == FlowControl.Next)))
            {
                switch (this.Random.NextInt32(3))
                {
                    case 0:
                        instrs.Add(Instruction.Create(OpCodes.Ldc_I4_0));
                        instrs.Add(Instruction.Create(OpCodes.Brtrue, instrs[0]));
                        break;

                    case 1:
                        instrs.Add(Instruction.Create(OpCodes.Ldc_I4_1));
                        instrs.Add(Instruction.Create(OpCodes.Brfalse, instrs[0]));
                        break;

                    case 2:
                        {
                            bool flag = false;
                            if (this.Random.NextBoolean())
                            {
                                TypeDef def = this.Method.Module.Types[this.Random.NextInt32(this.Method.Module.Types.Count)];
                                if (def.HasMethods)
                                {
                                    instrs.Add(Instruction.Create(OpCodes.Ldtoken, def.Methods[this.Random.NextInt32(def.Methods.Count)]));
                                    instrs.Add(Instruction.Create(OpCodes.Box, (ITypeDefOrRef)this.Method.Module.CorLibTypes.GetTypeRef("System", "RuntimeMethodHandle")));
                                    flag = true;
                                }
                            }
                            if (!flag)
                            {
                                instrs.Add(Instruction.Create(OpCodes.Ldc_I4, this.Random.NextBoolean() ? 0 : 1));
                                instrs.Add(Instruction.Create(OpCodes.Box, this.Method.Module.CorLibTypes.Int32.TypeDefOrRef));
                            }
                            Instruction item = Instruction.Create(OpCodes.Pop);
                            instrs.Add(Instruction.Create(OpCodes.Brfalse, instrs[0]));
                            instrs.Add(Instruction.Create(OpCodes.Ldc_I4, this.Random.NextBoolean() ? 0 : 1));
                            instrs.Add(item);
                            break;
                        }
                }
            }
            instrs.Add(Instruction.Create(OpCodes.Br, target));
        }

        public void AddJunk(IList<Instruction> instrs)
        {
            if (!this.Method.Module.IsClr40 && false)
            {
                switch (this.Random.NextInt32(6))
                {
                    case 0:
                        instrs.Add(Instruction.Create(OpCodes.Pop));
                        return;

                    case 1:
                        instrs.Add(Instruction.Create(OpCodes.Dup));
                        return;

                    case 2:
                        instrs.Add(Instruction.Create(OpCodes.Throw));
                        return;

                    case 3:
                        instrs.Add(Instruction.Create(OpCodes.Ldarg, new Parameter(0xff)));
                        return;

                    case 4:
                        {
                            Local local = new Local(null, null, 0xff);
                            instrs.Add(Instruction.Create(OpCodes.Ldloc, local));
                            return;
                        }
                    case 5:
                        instrs.Add(Instruction.Create(OpCodes.Ldtoken, (IMethod)this.Method));
                        return;
                }
            }
        }
    }
}

