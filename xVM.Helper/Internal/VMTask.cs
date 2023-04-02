using System;
using System.IO;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Writer;

using xVM.Helper.Core;
using xVM.Helper.Core.RT;
using xVM.Helper.Internal;

namespace xVM.Helper.Internal
{
    public class VMTask
    {
        public void Exceute(ModuleDef module, MemoryStream xmlFile)
        {
            InitializePhase init = new InitializePhase()
            {
                ModuleDFMD = (ModuleDefMD)module,
                XMLSettings = xmlFile
            };
            init.Run();
        }
    }
}
