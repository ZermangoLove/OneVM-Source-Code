using System;
using System.Collections.Generic;

using dnlib.DotNet;

namespace xVM.Helper.Core.Optimize
{
	internal class MethodReferenceEqualityComparer : IEqualityComparer<IMethod>
	{
		private static MethodReferenceEqualityComparer _singleton = new MethodReferenceEqualityComparer();

		public static MethodReferenceEqualityComparer Singleton
		{
			get
			{
				return _singleton;
			}
		}
		public bool Equals(IMethod mrefA, IMethod mrefB)
		{
			return mrefA.Equals(mrefB);
		}

		public int GetHashCode(IMethod mref)
		{
			return mref.FullName.GetHashCode();
		}
	}
}
