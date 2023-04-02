using System;
using System.Collections.Generic;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using xVM.Helper.Core.Services;
using xVM.Helper.DynCipher;
using xVM.Helper.Core.Helpers;

namespace xVM.Helper.Core.Protections
{
    public static class CEXControlFlow
    {
        public static void Execute(MethodDef method, int repeat)
        {
            var ret = new CFContext();
            ret.Intensity = 1;
            ret.Depth = 1;
            ret.Method = method;
            ret.DynCipher = new DynCipherService();
            ret.Random = new RandomGenerator(32);

            if (method.HasBody && method.Body.Instructions.Count > 0)
            {
                for (int a = 0; a < repeat; a++) //1x repeat
                {
                    ret.ProcessMethod(method.Body, ret);
                }
                method.Body.SimplifyBranches();
            }
        }
    }
}
