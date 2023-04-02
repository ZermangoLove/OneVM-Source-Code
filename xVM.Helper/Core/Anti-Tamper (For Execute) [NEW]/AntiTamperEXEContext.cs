using System.Collections.Generic;

using dnlib.DotNet;

namespace xVM.Helper.Core.AntiTamperEXEC
{
    internal struct AntiTamperEXEContext
    {
        public HashSet<MethodDef> Targets;
    }
}
