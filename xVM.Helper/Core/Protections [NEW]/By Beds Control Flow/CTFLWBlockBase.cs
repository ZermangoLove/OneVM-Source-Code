using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Protections
{
	internal abstract class CTFLWBlockBase {
		public CTFLWBlockBase(BlockType type) {
			Type = type;
		}

		public CTFLWScopeBlock Parent { get; private set; }

		public BlockType Type { get; private set; }
		public abstract void ToBody(CilBody body);
	}

	internal enum BlockType {
		Normal,
		Try,
		Handler,
		Finally,
		Filter,
		Fault
	}

	internal class CTFLWScopeBlock : CTFLWBlockBase
    {
		public CTFLWScopeBlock(BlockType type, ExceptionHandler handler)
			: base(type) {
			Handler = handler;
			Children = new List<CTFLWBlockBase>();
		}

		public ExceptionHandler Handler { get; private set; }

		public List<CTFLWBlockBase> Children { get; set; }

		public override string ToString() {
			var ret = new StringBuilder();
			if (Type == BlockType.Try)
				ret.Append("try ");
			else if (Type == BlockType.Handler)
				ret.Append("handler ");
			else if (Type == BlockType.Finally)
				ret.Append("finally ");
			else if (Type == BlockType.Fault)
				ret.Append("fault ");
			ret.AppendLine("{");
			foreach (CTFLWBlockBase child in Children)
				ret.Append(child);
			ret.AppendLine("}");
			return ret.ToString();
		}

		public Instruction GetFirstInstr() {
            CTFLWBlockBase firstBlock = Children.First();
			if (firstBlock is CTFLWScopeBlock)
				return ((CTFLWScopeBlock)firstBlock).GetFirstInstr();
			return ((InstrBlock)firstBlock).Instructions.First();
		}

		public Instruction GetLastInstr() {
            CTFLWBlockBase firstBlock = Children.Last();
			if (firstBlock is CTFLWScopeBlock)
				return ((CTFLWScopeBlock)firstBlock).GetLastInstr();
			return ((InstrBlock)firstBlock).Instructions.Last();
		}

		public override void ToBody(CilBody body) {
			if (Type != BlockType.Normal) {
				if (Type == BlockType.Try) {
					Handler.TryStart = GetFirstInstr();
					Handler.TryEnd = GetLastInstr();
				}
				else if (Type == BlockType.Filter) {
					Handler.FilterStart = GetFirstInstr();
				}
				else {
					Handler.HandlerStart = GetFirstInstr();
					Handler.HandlerEnd = GetLastInstr();
				}
			}

			foreach (CTFLWBlockBase block in Children)
				block.ToBody(body);
		}
	}

	internal class InstrBlock : CTFLWBlockBase
    {
		public InstrBlock()
			: base(BlockType.Normal) {
			Instructions = new List<Instruction>();
		}

		public List<Instruction> Instructions { get; set; }

		public override string ToString() {
			var ret = new StringBuilder();
			foreach (Instruction instr in Instructions)
				ret.AppendLine(instr.ToString());
			return ret.ToString();
		}

		public override void ToBody(CilBody body) {
			foreach (Instruction instr in Instructions)
				body.Instructions.Add(instr);
		}
	}
}