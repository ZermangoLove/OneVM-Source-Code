using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.Helpers {
	/// <summary>
	///     Provides methods to mutated injected methods.
	/// </summary>
	internal static class MutationHelper
    {
        #region Field2Index (Changed Version)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal static Dictionary<string, int> Field2IntIndex = new Dictionary<string, int> {
            { "IntKey0", 0 },
            { "IntKey1", 1 },
            { "IntKey2", 2 },
            { "IntKey3", 3 },
            { "IntKey4", 4 },
            { "IntKey5", 5 },
            { "IntKey6", 6 },
            { "IntKey7", 7 },
            { "IntKey8", 8 },
            { "IntKey9", 9 },
            { "IntKey10", 10 },
            { "IntKey11", 11 },
            { "IntKey12", 12 },
            { "IntKey13", 13 },
            { "IntKey14", 14 },
            { "IntKey15", 15 },
            { "IntKey16", 16 },
            { "IntKey17", 17 },
            { "IntKey18", 18 },
            { "IntKey19", 19 },
            { "IntKey20", 20 }
        };

        internal static Dictionary<string, int> Field2LongIndex = new Dictionary<string, int> {
			{ "LongKey0", 0 },
			{ "LongKey1", 1 },
			{ "LongKey2", 2 },
			{ "LongKey3", 3 },
			{ "LongKey4", 4 },
			{ "LongKey5", 5 },
			{ "LongKey6", 6 },
			{ "LongKey7", 7 },
			{ "LongKey8", 8 },
			{ "LongKey9", 9 },
			{ "LongKey10", 10 },
			{ "LongKey11", 11 },
			{ "LongKey12", 12 },
			{ "LongKey13", 13 },
			{ "LongKey14", 14 },
			{ "LongKey15", 15 },
            { "LongKey16", 16 },
            { "LongKey17", 17 },
            { "LongKey18", 18 },
            { "LongKey19", 19 },
            { "LongKey20", 20 }
        };

        internal static Dictionary<string, string> Field2LdstrIndex = new Dictionary<string, string> {
            { "LdstrKey0", Convert.ToString(0) },
            { "LdstrKey1", Convert.ToString(1) },
            { "LdstrKey2", Convert.ToString(2) },
            { "LdstrKey3", Convert.ToString(3) },
            { "LdstrKey4", Convert.ToString(4) },
            { "LdstrKey5", Convert.ToString(5) },
            { "LdstrKey6", Convert.ToString(6) },
            { "LdstrKey7", Convert.ToString(7) },
            { "LdstrKey8", Convert.ToString(8) },
            { "LdstrKey9", Convert.ToString(9) },
            { "LdstrKey10", Convert.ToString(10) },
        };
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Original Field2Index (No Changed)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal static readonly Dictionary<string, int> Original_Field2IntIndex = new Dictionary<string, int> {
            { "IntKey0", 0 },
            { "IntKey1", 1 },
            { "IntKey2", 2 },
            { "IntKey3", 3 },
            { "IntKey4", 4 },
            { "IntKey5", 5 },
            { "IntKey6", 6 },
            { "IntKey7", 7 },
            { "IntKey8", 8 },
            { "IntKey9", 9 },
            { "IntKey10", 10 },
            { "IntKey11", 11 },
            { "IntKey12", 12 },
            { "IntKey13", 13 },
            { "IntKey14", 14 },
            { "IntKey15", 15 },
            { "IntKey16", 16 },
            { "IntKey17", 17 },
            { "IntKey18", 18 },
            { "IntKey19", 19 },
            { "IntKey20", 20 }
        };

        internal static readonly Dictionary<string, int> Original_Field2LongIndex = new Dictionary<string, int> {
            { "LongKey0", 0 },
            { "LongKey1", 1 },
            { "LongKey2", 2 },
            { "LongKey3", 3 },
            { "LongKey4", 4 },
            { "LongKey5", 5 },
            { "LongKey6", 6 },
            { "LongKey7", 7 },
            { "LongKey8", 8 },
            { "LongKey9", 9 },
            { "LongKey10", 10 },
            { "LongKey11", 11 },
            { "LongKey12", 12 },
            { "LongKey13", 13 },
            { "LongKey14", 14 },
            { "LongKey15", 15 },
            { "LongKey16", 16 },
            { "LongKey17", 17 },
            { "LongKey18", 18 },
            { "LongKey19", 19 },
            { "LongKey20", 20 }
        };

        internal static readonly Dictionary<string, string> Original_Field2LdstrIndex = new Dictionary<string, string> {
            { "LdstrKey0", Convert.ToString(0) },
            { "LdstrKey1", Convert.ToString(1) },
            { "LdstrKey2", Convert.ToString(2) },
            { "LdstrKey3", Convert.ToString(3) },
            { "LdstrKey4", Convert.ToString(4) },
            { "LdstrKey5", Convert.ToString(5) },
            { "LdstrKey6", Convert.ToString(6) },
            { "LdstrKey7", Convert.ToString(7) },
            { "LdstrKey8", Convert.ToString(8) },
            { "LdstrKey9", Convert.ToString(9) },
            { "LdstrKey10", Convert.ToString(10) },
        };
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        /// <summary>
        ///     Replaces the mutation key placeholder in method with actual key.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyId">The mutation key ID.</param>
        /// <param name="key">The actual key.</param>
        internal static void InjectKey_Int(MethodDef method, int keyId, int key)
        {
            foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    int _keyId;
                    if (field.DeclaringType.FullName == RTMap.Mutation && Field2IntIndex.TryGetValue(field.Name, out _keyId) && _keyId == keyId)
                    {
                        instr.OpCode = OpCodes.Ldc_I4;
                        instr.Operand = key;
                    }
                }
            }
        }

        /// <summary>
        ///     Replaces the mutation key placeholder in method with actual key.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyId">The mutation key ID.</param>
        /// <param name="key">The actual key.</param>
        internal static void InjectKey_Long(MethodDef method, int keyId, long key) {
			foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    int _keyId;
                    if (field.DeclaringType.FullName == RTMap.Mutation && Field2LongIndex.TryGetValue(field.Name, out _keyId) && _keyId == keyId)
                    {
                        instr.OpCode = OpCodes.Ldc_I8;
                        instr.Operand = key;
                    }
                }
            }
		}

        /// <summary>
        ///     Replaces the mutation key placeholder in method with actual key.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyId">The mutation key ID.</param>
        /// <param name="key">The actual key.</param>
        internal static void InjectKey_String(MethodDef method, int keyId, string key)
        {
            foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    string _keyId;
                    if (field.DeclaringType.FullName == RTMap.Mutation && Field2LdstrIndex.TryGetValue(field.Name, out _keyId) && _keyId == keyId.ToString())
                    {
                        instr.OpCode = OpCodes.Ldstr;
                        instr.Operand = key;
                    }
                }
            }
        }

        /// <summary>
        ///     Replaces the mutation key placeholders in method with actual keys.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyIds">The mutation key IDs.</param>
        /// <param name="keys">The actual keys.</param>
        internal static void InjectKeys_Int(MethodDef method, int[] keyIds, int[] keys)
        {
            foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    int _keyIndex;
                    if (field.DeclaringType.FullName == RTMap.Mutation &&
                        Field2IntIndex.TryGetValue(field.Name, out _keyIndex) && (_keyIndex = Array.IndexOf(keyIds, _keyIndex)) != -1)
                    {
                        instr.OpCode = OpCodes.Ldc_I4;
                        instr.Operand = keys[_keyIndex];
                    }
                }
            }
        }

        /// <summary>
        ///     Replaces the mutation key placeholders in method with actual keys.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyIds">The mutation key IDs.</param>
        /// <param name="keys">The actual keys.</param>
        internal static void InjectKeys_Long(MethodDef method, int[] keyIds, long[] keys) {
			foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    int _keyIndex;
                    if (field.DeclaringType.FullName == RTMap.Mutation &&
                        Field2LongIndex.TryGetValue(field.Name, out _keyIndex) && (_keyIndex = Array.IndexOf(keyIds, _keyIndex)) != -1)
                    {
                        instr.OpCode = OpCodes.Ldc_I8;
                        instr.Operand = keys[_keyIndex];
                    }
                }
            }
		}

        /// <summary>
        ///     Replaces the mutation key placeholders in method with actual keys.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="keyIds">The mutation key IDs.</param>
        /// <param name="keys">The actual keys.</param>
        internal static void InjectKeys_String(MethodDef method, int[] keyIds, string[] keys)
        {
            foreach (Instruction instr in method.Body.Instructions)
            {
                if (instr.OpCode == OpCodes.Ldsfld)
                {
                    var field = instr.Operand as IField;
                    string _keyIndex;
                    if (field.DeclaringType.FullName == RTMap.Mutation &&
                        Field2LdstrIndex.TryGetValue(field.Name, out _keyIndex) && Convert.ToInt32(_keyIndex = Array.IndexOf(keyIds, int.Parse(_keyIndex)).ToString()) != -1)
                    {
                        instr.OpCode = OpCodes.Ldstr;
                        instr.Operand = keys[int.Parse(_keyIndex)];
                    }
                }
            }
        }

        /// <summary>
        ///     Replaces the Value<T> call in method with actual instruction.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="ret_inst">The function replacing the argument of Value<T> call with actual instruction.</param>
        internal static void ReplaceValue_T(MethodDef method, Instruction ret_inst)
        {
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                Instruction instr = method.Body.Instructions[i];
                var md = instr.Operand as IMethod;
                if (instr.OpCode == OpCodes.Call &&
                    md.DeclaringType.Name == RTMap.Mutation &&
                    md.Name == RTMap.Mutation_Value_T)
                {
                    method.Body.Instructions[i] = ret_inst;
                }
            }
        }


        /// <summary>
        ///     Replaces the placeholder call in method with byte array sequence.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="data">The function replacing the argument of placeholder call with actual byte array sequence.</param>
        internal static void ReplacePlaceholder_Inject_ByteArray(MethodDef method, byte[] data)
        {
            MutationHelper.ReplacePlaceholder(method, arg => 
            {
                var repl = new List<Instruction>();
                repl.AddRange(arg);

                for (var j = 0; j < data.Length; j++)
                {
                    repl.Add(Instruction.Create(OpCodes.Dup));
                    repl.Add(Instruction.Create(OpCodes.Ldc_I4, j));
                    repl.Add(Instruction.Create(OpCodes.Ldc_I4, (int)data[j]));
                    repl.Add(Instruction.Create(OpCodes.Stelem_Ref));
                }

                return repl.ToArray();
            });
        }

        /// <summary>
        ///     Replaces the placeholder call in method with actual instruction sequence.
        /// </summary>
        /// <param name="method">The method to process.</param>
        /// <param name="repl">The function replacing the argument of placeholder call with actual instruction sequence.</param>
        internal static void ReplacePlaceholder(MethodDef method, Func<Instruction[], Instruction[]> repl) {
			MethodTrace trace = new MethodTrace(method).Trace();
			for (int i = 0; i < method.Body.Instructions.Count; i++) {
				Instruction instr = method.Body.Instructions[i];
				if (instr.OpCode == OpCodes.Call) {
					var operand = (IMethod)instr.Operand;
					if (operand.DeclaringType.FullName == RTMap.Mutation &&
					    operand.Name == RTMap.Mutation_Placeholder) {
						int[] argIndexes = trace.TraceArguments(instr);
						if (argIndexes == null)
							throw new ArgumentException("Failed to trace placeholder argument.");

						int argIndex = argIndexes[0];
						Instruction[] arg = method.Body.Instructions.Skip(argIndex).Take(i - argIndex).ToArray();
						for (int j = 0; j < arg.Length; j++)
							method.Body.Instructions.RemoveAt(argIndex);
						method.Body.Instructions.RemoveAt(argIndex);
						arg = repl(arg);
						for (int j = arg.Length - 1; j >= 0; j--)
							method.Body.Instructions.Insert(argIndex, arg[j]);
						return;
					}
				}
			}
		}
	}
}