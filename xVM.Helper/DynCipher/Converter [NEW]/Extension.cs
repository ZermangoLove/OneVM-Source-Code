using System;
using System.IO;
using System.Linq;
using System.Text;

using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.DynCipher.Converter
{
    public static class Extension
    {
        public static void ConvertToBytes(this BinaryWriter writer, MethodDef method) => new Converter(method, writer).ConvertToBytes();
    }
}
