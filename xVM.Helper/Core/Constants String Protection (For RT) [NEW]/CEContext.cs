using System;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

using xVM.Helper.Core.RT;
using xVM.Helper.DynCipher;
using xVM.Helper.Core.Services;
using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.Constants
{
	internal class CEContext
	{
        public VMRuntime VMRuntime;
		public ModuleDef Module;
		public MethodDef InitMethod;
		public CompressionService Compressor;
		public ModuleWriterOptions Options;

		public int DecoderCount;
		public List<Tuple<MethodDef, DecoderDesc>> Decoders;

		public EncodeElements Elements;
		public List<uint> EncodedBuffer;

		public DynamicMode ModeHandler;

		public DynCipherService DynCipher;
		public RandomGenerator Random;

		public Dictionary<MethodDef, List<Tuple<Instruction, uint, IMethod>>> ReferenceRepl;
	}

	internal class DecoderDesc
	{
		public object Data;
		public byte InitializerID;
		public byte NumberID;
		public byte StringID;
	}
}
