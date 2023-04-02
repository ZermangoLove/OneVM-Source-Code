using System;
using System.Collections;

namespace xVM.Helper.Core.Helpers.System.Collections
{
    public interface IStructuralEquatable
    {
        Boolean Equals(Object other, IEqualityComparer comparer);
        int GetHashCode(IEqualityComparer comparer);
    }
}