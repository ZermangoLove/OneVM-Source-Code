using System;

namespace xVM.Helper.Core.Protections
{
	internal enum CEXBlockType
	{
		Normal,
		Try,
		Handler,
		Finally,
		Filter,
		Fault
	}
}

