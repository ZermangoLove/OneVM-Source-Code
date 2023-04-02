using System;
using System.Text;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Optimize
{
	internal class AutoMethodBodySimplifyOptimize : IDisposable
	{
		public AutoMethodBodySimplifyOptimize(MethodDef methodBody, bool optimizeOnDispose)
		{
			_methodBody = methodBody.Body;
			_optimizeOnDispose = optimizeOnDispose;
			_methodBody.SimplifyMacros(methodBody.Parameters);
		}
		public void Dispose()
		{
			if (_optimizeOnDispose)
			{
				_methodBody.OptimizeMacros();
			}
		}

		private CilBody _methodBody;
		private bool _optimizeOnDispose;
	}
}
