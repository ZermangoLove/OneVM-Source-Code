using dnlib.DotNet.Emit;
using System;
using System.Runtime.CompilerServices;

namespace xVM.Helper.Core.Protections
{
	internal abstract class CEXBlockBase
	{
		public CEXBlockBase(CEXBlockType type)
		{
			this.Type = type;
		}

		public abstract void ToBody(CilBody body);

		public CEXScopeBlock Parent { get; private set; }

		public CEXBlockType Type { get; private set; }
	}
}

