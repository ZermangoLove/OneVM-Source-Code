using System;
using System.Collections.Generic;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using xVM.Helper.Core.Services;
using xVM.Helper.DynCipher;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.Protections
{
    public class Multiply
    {
		public static Random rnd = new Random();
		public static void Execute(MethodDef method)
        {
            if (method.HasBody)
            {
				var instr = method.Body.Instructions;
				for (int i = 0; i < method.Body.Instructions.Count; i++)
				{
					if (instr[i].OpCode == OpCodes.Ldstr)
					{
						Random rn = new Random();
						for (int j = 1; j < rn.Next(16); j++)
						{
							if (j != 1) j += 1;
							//Create a new local 
							Local new_local = new Local(method.Module.CorLibTypes.String);
							//Create another new local
							Local new_local2 = new Local(method.Module.CorLibTypes.String);
							//add them in the method
							method.Body.Variables.Add(new_local);
							method.Body.Variables.Add(new_local2);
							//set ldstr value to the local
							instr.Insert(i + j, Instruction.Create(OpCodes.Stloc_S, new_local));
							instr.Insert(i + (j + 1), Instruction.Create(OpCodes.Ldloc_S, new_local));
						}
					}
					if (instr[i].IsLdcI4())
					{
						Random rn = new Random();
						for (int j = 1; j < rn.Next(16); j++)
						{
							if (j != 1) j += 1;
							Local new_local = new Local(method.Module.CorLibTypes.Int32);
							Local new_local2 = new Local(method.Module.CorLibTypes.Int32);
							method.Body.Variables.Add(new_local);
							method.Body.Variables.Add(new_local2);
							instr.Insert(i + j, Instruction.Create(OpCodes.Stloc_S, new_local));
							instr.Insert(i + (j + 1), Instruction.Create(OpCodes.Ldloc_S, new_local));
						}
					}
				}
			}
        }

		public static CilBody body;
		private static void Mutate(int i, int sub, int calculado, ModuleDef module, MethodDef method)
		{
			switch (sub)
			{
				case 1:
					{
						Local local = new Local(module.CorLibTypes.Object);
						Local local2 = new Local(module.CorLibTypes.Object);
						Local local3 = new Local(module.CorLibTypes.Object);
						Local local4 = new Local(module.CorLibTypes.Object);
						method.Body.Variables.Add(local);
						method.Body.Variables.Add(local2);
						method.Body.Variables.Add(local3);
						method.Body.Variables.Add(local4);
						body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldc_I4, sizeof(GCNotificationStatus)));
						body.Instructions.Insert(i + 2, new Instruction(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 3, new Instruction(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 4, OpCodes.Add.ToInstruction());
						body.Instructions.Insert(i + 5, new Instruction(OpCodes.Ldc_I4, sizeof(sbyte)));
						body.Instructions.Insert(i + 6, new Instruction(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 7, new Instruction(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 8, OpCodes.Sub.ToInstruction());
						body.Instructions.Insert(i + 9, new Instruction(OpCodes.Ldc_I4, sizeof(sbyte)));
						body.Instructions.Insert(i + 10, new Instruction(OpCodes.Stloc_S, local3));
						body.Instructions.Insert(i + 11, new Instruction(OpCodes.Ldloc_S, local3));
						body.Instructions.Insert(i + 12, OpCodes.Sub.ToInstruction());
						body.Instructions.Insert(i + 13, new Instruction(OpCodes.Ldc_I4, sizeof(sbyte)));
						body.Instructions.Insert(i + 14, new Instruction(OpCodes.Stloc_S, local4));
						body.Instructions.Insert(i + 15, new Instruction(OpCodes.Ldloc_S, local4));
						body.Instructions.Insert(i + 16, OpCodes.Sub.ToInstruction());
						break;
					}
				case 2:
					{
						Local local = new Local(module.CorLibTypes.Object);
						method.Body.Variables.Add(local);
						body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldc_I4, sizeof(char)));
						body.Instructions.Insert(i + 2, new Instruction(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 3, new Instruction(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 4, OpCodes.Add.ToInstruction());
						break;
					}
				case 3:
					{
						Local local = new Local(module.CorLibTypes.Object);
						Local local2 = new Local(module.CorLibTypes.Object);
						method.Body.Variables.Add(local);
						method.Body.Variables.Add(local2);
						body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldc_I4, sizeof(int)));
						body.Instructions.Insert(i + 2, new Instruction(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 3, new Instruction(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 4, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 5, new Instruction(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 6, new Instruction(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Sub));
						body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Add));
						break;
					}
				case 4:
					{
						Local local = new Local(module.CorLibTypes.Object);
						Local local2 = new Local(module.CorLibTypes.Object);
						Local local3 = new Local(module.CorLibTypes.Object);
						Local local4 = new Local(module.CorLibTypes.Object);
						method.Body.Variables.Add(local);
						method.Body.Variables.Add(local2);
						method.Body.Variables.Add(local3);
						method.Body.Variables.Add(local4);
						body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Add));
						body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Ldc_I4, sizeof(decimal)));
						body.Instructions.Insert(i + 3, new Instruction(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 4, new Instruction(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 5, Instruction.Create(OpCodes.Ldc_I4, sizeof(GCCollectionMode)));
						body.Instructions.Insert(i + 6, new Instruction(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 7, new Instruction(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 8, Instruction.Create(OpCodes.Sub));
						body.Instructions.Insert(i + 9, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 10, new Instruction(OpCodes.Stloc_S, local3));
						body.Instructions.Insert(i + 11, new Instruction(OpCodes.Ldloc_S, local3));
						body.Instructions.Insert(i + 12, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 13, new Instruction(OpCodes.Stloc_S, local4));
						body.Instructions.Insert(i + 14, new Instruction(OpCodes.Ldloc_S, local4));
						body.Instructions.Insert(i + 15, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 16, new Instruction(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 17, new Instruction(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 18, Instruction.Create(OpCodes.Sub));
						body.Instructions.Insert(i + 19, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 20, new Instruction(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 21, new Instruction(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 22, Instruction.Create(OpCodes.Ldc_I4, sizeof(byte)));
						body.Instructions.Insert(i + 23, new Instruction(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 24, new Instruction(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 25, Instruction.Create(OpCodes.Add));
						break;
					}
				case 5:
					{
						Local local = new Local(module.CorLibTypes.Object);
						Local local2 = new Local(module.CorLibTypes.Object);
						method.Body.Variables.Add(local);
						method.Body.Variables.Add(local2);
						body.Instructions.Insert(i + 1, new Instruction(OpCodes.Ldc_I4, sizeof(EnvironmentVariableTarget)));
						body.Instructions.Insert(i + 2, Instruction.Create(OpCodes.Stloc_S, local));
						body.Instructions.Insert(i + 3, Instruction.Create(OpCodes.Ldloc_S, local));
						body.Instructions.Insert(i + 4, OpCodes.Add.ToInstruction());
						body.Instructions.Insert(i + 5, new Instruction(OpCodes.Ldc_I4, sizeof(sbyte)));
						body.Instructions.Insert(i + 6, Instruction.Create(OpCodes.Stloc_S, local2));
						body.Instructions.Insert(i + 7, Instruction.Create(OpCodes.Ldloc_S, local2));
						body.Instructions.Insert(i + 9, OpCodes.Add.ToInstruction());
						break;
					}
			}
		}
	}
}
