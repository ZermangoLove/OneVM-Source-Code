using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
	internal class CEXScopeBlock : CEXBlockBase
	{
		public CEXScopeBlock(CEXBlockType type, ExceptionHandler handler) : base(type)
		{
			this.Handler = handler;
			this.Children = new List<CEXBlockBase>();
		}

		public Instruction GetFirstInstr()
		{
			CEXBlockBase base2 = this.Children.First<CEXBlockBase>();
			if (base2 is CEXScopeBlock)
			{
				return ((CEXScopeBlock)base2).GetFirstInstr();
			}
			return ((CEXInstrBlock)base2).Instructions.First<Instruction>();
		}

		public Instruction GetLastInstr()
		{
			CEXBlockBase base2 = this.Children.Last<CEXBlockBase>();
			if (base2 is CEXScopeBlock)
			{
				return ((CEXScopeBlock)base2).GetLastInstr();
			}
			return ((CEXInstrBlock)base2).Instructions.Last<Instruction>();
		}

		public override void ToBody(CilBody body)
		{
			if (base.Type != CEXBlockType.Normal)
			{
				if (base.Type == CEXBlockType.Try)
				{
					this.Handler.TryStart = this.GetFirstInstr();
					this.Handler.TryEnd = this.GetLastInstr();
				}
				else if (base.Type == CEXBlockType.Filter)
				{
					this.Handler.FilterStart = this.GetFirstInstr();
				}
				else
				{
					this.Handler.HandlerStart = this.GetFirstInstr();
					this.Handler.HandlerEnd = this.GetLastInstr();
				}
			}
			foreach (CEXBlockBase base2 in this.Children)
			{
				base2.ToBody(body);
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (base.Type == CEXBlockType.Try)
			{
				builder.Append("try ");
			}
			else if (base.Type == CEXBlockType.Handler)
			{
				builder.Append("handler ");
			}
			else if (base.Type == CEXBlockType.Finally)
			{
				builder.Append("finally ");
			}
			else if (base.Type == CEXBlockType.Fault)
			{
				builder.Append("fault ");
			}
			builder.AppendLine("{");
			foreach (CEXBlockBase base2 in this.Children)
			{
				builder.Append(base2);
			}
			builder.AppendLine("}");
			return builder.ToString();
		}

		public List<CEXBlockBase> Children { get; set; }

		public ExceptionHandler Handler { get; private set; }
	}
}

