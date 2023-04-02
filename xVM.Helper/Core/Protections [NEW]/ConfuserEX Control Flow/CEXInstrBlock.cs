using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace xVM.Helper.Core.Protections
{
	internal class CEXInstrBlock : CEXBlockBase
	{
		public CEXInstrBlock() : base(CEXBlockType.Normal)
		{
			this.Instructions = new List<Instruction>();
		}

		public override void ToBody(CilBody body)
		{
			foreach (Instruction instruction in this.Instructions)
			{
				body.Instructions.Add(instruction);
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (Instruction instruction in this.Instructions)
			{
				builder.AppendLine(instruction.ToString());
			}
			return builder.ToString();
		}

		public List<Instruction> Instructions { get; set; }
	}
}

