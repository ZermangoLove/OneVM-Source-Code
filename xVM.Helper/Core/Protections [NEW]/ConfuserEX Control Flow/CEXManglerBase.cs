using dnlib.DotNet.Emit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace xVM.Helper.Core.Protections
{
	internal abstract class CEXManglerBase
	{
		protected static IEnumerable<CEXInstrBlock> GetAllBlocks(CEXScopeBlock scope)
		{
			foreach (var iteratorVariable0 in scope.Children)
			{
				if (iteratorVariable0 is CEXInstrBlock)
				{
					yield return (CEXInstrBlock)iteratorVariable0;
				}
				else
				{
					foreach (var iteratorVariable1 in GetAllBlocks((CEXScopeBlock)iteratorVariable0))
					{
						yield return iteratorVariable1;
					}
				}
			}
		}

		public abstract void Mangle(CilBody body, CEXScopeBlock root, CFContext ctx);
	}
}

