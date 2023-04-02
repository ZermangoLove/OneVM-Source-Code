using System;
using System.Collections.Generic;

using dnlib.DotNet.Emit;

using xVM.Helper.DynCipher.AST;
using xVM.Helper.DynCipher.Generation;
using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.Protections
{
	internal class ExpressionPredicate
	{
		private readonly CFContext ctx;
		private Func<int, int> expCompiled;
		private Expression expression;
		private bool inited;
		private List<Instruction> invCompiled;
		private Expression inverse;
		private Local stateVar;

		public ExpressionPredicate(CFContext ctx)
		{
			this.ctx = ctx;
		}

		private void Compile(CilBody body)
		{
			Variable variable = new Variable("{VAR}");
			Variable variable2 = new Variable("{RESULT}");
			VariableExpression var = new VariableExpression
			{
				Variable = variable
			};
			VariableExpression result = new VariableExpression
			{
				Variable = variable2
			};
			this.ctx.DynCipher.GenerateExpressionPair(this.ctx.Random, var, result, this.ctx.Depth, out this.expression, out this.inverse);
			this.expCompiled = new DMCodeGen(typeof(int), new Tuple<string, Type>[] { Tuple.Create<string, Type>("{VAR}", typeof(int)) }).GenerateCIL(this.expression).Compile<Func<int, int>>();
			this.invCompiled = new List<Instruction>();
			new CodeGen(this.stateVar, this.ctx, this.invCompiled).GenerateCIL(this.inverse);
			body.MaxStack = (ushort)(body.MaxStack + ((ushort)this.ctx.Depth));
		}

		public void EmitSwitchLoad(IList<Instruction> instrs)
		{
			instrs.Add(Instruction.Create(OpCodes.Stloc, this.stateVar));
			foreach (Instruction instruction in this.invCompiled)
			{
				instrs.Add(instruction.Clone());
			}
		}

		public int GetSwitchKey(int key) =>
						this.expCompiled(key);

		public void Init(CilBody body)
		{
			if (!this.inited)
			{
				this.stateVar = new Local(this.ctx.Method.Module.CorLibTypes.Int32);
				body.Variables.Add(this.stateVar);
				body.InitLocals = true;
				this.Compile(body);
				this.inited = true;
			}
		}

		private class CodeGen : CILCodeGen
		{
			private readonly Local state;

			public CodeGen(Local state, CFContext ctx, IList<Instruction> instrs) : base(ctx.Method, instrs)
			{
				this.state = state;
			}

			protected override Local Var(Variable var)
			{
				if (var.Name == "{RESULT}")
				{
					return this.state;
				}
				return base.Var(var);
			}
		}
	}
}

