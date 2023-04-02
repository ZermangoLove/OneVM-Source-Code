using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    public class IntConfusion
    {
        public void Execute(ModuleDef md)
        {
            foreach (var type in md.GetTypes())
            {
                if (type.IsGlobalModuleType) continue;
                foreach (var meth in type.Methods)
                {
                    if (!meth.HasBody) continue;
                    {
                        for (var i = 0; i < meth.Body.Instructions.Count; i++)
                        {
                            if (!meth.Body.Instructions[i].IsLdcI4()) continue;
                            var numorig = new Random(Guid.NewGuid().GetHashCode()).Next();
                            var div = new Random(Guid.NewGuid().GetHashCode()).Next();
                            var num = numorig ^ div;

                            var nop = OpCodes.Nop.ToInstruction();

                            var local = new Local(meth.Module.ImportAsTypeSig(typeof(int)));
                            meth.Body.Variables.Add(local);

                            meth.Body.Instructions.Insert(i + 1, OpCodes.Stloc.ToInstruction(local));
                            meth.Body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, meth.Body.Instructions[i].GetLdcI4Value() - sizeof(float)));
                            meth.Body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldc_I4, num));
                            meth.Body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, div));
                            meth.Body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Xor));
                            meth.Body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Ldc_I4, numorig));
                            meth.Body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Bne_Un, nop));
                            meth.Body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Ldc_I4, 2));
                            meth.Body.Instructions.Insert(i + 9, OpCodes.Stloc.ToInstruction(local));
                            meth.Body.Instructions.Insert(i + 10, Instruction.Create(OpCodes.Sizeof, meth.Module.Import(typeof(float))));
                            meth.Body.Instructions.Insert(i + 11, Instruction.Create(OpCodes.Add));
                            meth.Body.Instructions.Insert(i + 12, nop);
                            i += 12;
                        }
                        meth.Body.SimplifyBranches();
                    }
                }
            }
        }
    }
}
