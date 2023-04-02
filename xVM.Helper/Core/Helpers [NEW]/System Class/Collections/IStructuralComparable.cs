using System;
using System.Collections;

namespace xVM.Helper.Core.Helpers.System.Collections
{
    public interface IStructuralComparable
    {
        Int32 CompareTo(Object other, IComparer comparer);
    }
}