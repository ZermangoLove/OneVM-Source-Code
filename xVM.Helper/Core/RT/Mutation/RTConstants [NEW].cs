using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.IO.Compression;
using System.Collections.Generic;
using System.Security.Cryptography;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.VM;
using xVM.Helper.Core.VMIL;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.RT.Mutation
{
    internal class RTConstants
    {
        private readonly Dictionary<string, byte> Constants = new Dictionary<string, byte>();

        public TypeDef Interpreter;
        public MethodDef Interpreter_Set_VMDATA;
        public MethodDef Interpreter_Set_Constants;

        public readonly static Int32 KeyA = new RandomGenerator().NextInt32();
        public readonly static Int32 MurmurKeyB = new RandomGenerator().NextInt32();

        private void AddField(string fieldName, byte fieldValue)
        {
            Constants[fieldName] = fieldValue;
        }

        public void ReadConstants(VMDescriptor desc, RuntimeHelpers helpers)
        {
            for (var i = 0; i < (int)VMRegisters.Max; i++)
            {
                var reg = (VMRegisters)i;
                var regId = desc.Architecture.Registers[reg];
                var regField = reg.ToString();
                AddField(regField, regId);
            }

            for (var i = 0; i < (int)VMFlags.Max; i++)
            {
                var fl = (VMFlags)i;
                var flId = desc.Architecture.Flags[fl];
                var flField = fl.ToString();
                AddField(flField, (byte)(1 << flId));
            }

            for (var i = 0; i < (int)ILOpCode.Max; i++)
            {
                var op = (ILOpCode)i;
                var opId = desc.Architecture.OpCodes[op];
                var opField = op.ToString();
                AddField(opField, opId);
            }

            for (var i = 0; i < (int)VMCalls.Max; i++)
            {
                var vc = (VMCalls)i;
                var vcId = desc.Runtime.VMCall[vc];
                var vcField = vc.ToString();
                AddField(vcField, (byte)vcId);
            }

            AddField(ConstantFields.E_CALL.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALL);
            AddField(ConstantFields.E_CALLVIRT.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALLVIRT);
            AddField(ConstantFields.E_NEWOBJ.ToString(), (byte)desc.Runtime.VCallOps.ECALL_NEWOBJ);
            AddField(ConstantFields.E_CALLVIRT_CONSTRAINED.ToString(), (byte)desc.Runtime.VCallOps.ECALL_CALLVIRT_CONSTRAINED);

            AddField(ConstantFields.INIT.ToString(), (byte)helpers.INIT);

            AddField(ConstantFields.INSTANCE.ToString(), desc.Runtime.RTFlags.INSTANCE);

            AddField(ConstantFields.CATCH.ToString(), desc.Runtime.RTFlags.EH_CATCH);
            AddField(ConstantFields.FILTER.ToString(), desc.Runtime.RTFlags.EH_FILTER);
            AddField(ConstantFields.FAULT.ToString(), desc.Runtime.RTFlags.EH_FAULT);
            AddField(ConstantFields.FINALLY.ToString(), desc.Runtime.RTFlags.EH_FINALLY);

            #region Set Constants Data
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #region Constants Data Write "const_stream"
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var const_stream = new MemoryStream();
            using (var const_writer = new BinaryWriter(const_stream))
            {
                List<byte> buffer = new List<byte>();
                buffer.AddRange(Constants.Values);
                buffer.RemoveAt(7); // Remove REG_R7 OpCode
                buffer.RemoveAt(23); // Remove FL_BEHAV3 OpCode

                // byte array to ushort
                foreach (var chunk in buffer.ToArray())
                {
                    const_writer.Write((ushort)chunk);
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Call Interpreter TypeDef and Interpreter.Set_Constants Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Interpreter = helpers.VMRT.RTModule.Find(RTMap.Interpreter, true);
            foreach (var setvdata in helpers.VMRT.RTModule.Find(RTMap.Interpreter, true).FindMethods(RTMap.Interpreter_Set_VMDATA))
            {
                Interpreter_Set_VMDATA = setvdata;
            }
            foreach (var setconst in helpers.VMRT.RTModule.Find(RTMap.Interpreter, true).FindMethods(RTMap.Interpreter_Set_Constants))
            {
                Interpreter_Set_Constants = setconst;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            byte[] Encrypted_const = const_stream.ToArray();

            #region "VeryFastEncryption" Algorithm
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            MutationHelper.InjectKey_String(Interpreter_Set_Constants, 1, Convert.ToString(KeyA)); // Write Array KeyA
            MutationHelper.InjectKey_String(Interpreter_Set_Constants, 2, Convert.ToString(MurmurKeyB)); // Write Murmur Key

            var hash = new System.Text.StringBuilder();
            var arrayKey = BitConverter.GetBytes(KeyA);
            var state = Murmur2.Hash(arrayKey, (ulong)arrayKey.LongLength, CRC32.CheckSum(arrayKey) ^ (ulong)MurmurKeyB);
            byte[] crypto = new SHA256Managed().ComputeHash(BitConverter.GetBytes(state));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            byte[] hashedPasswordBytes = System.Text.Encoding.ASCII.GetBytes(hash.ToString());
            int passwordShiftIndex = 0;
            bool shiftFlag = false;
            for (int i = 0; i < Encrypted_const.Length; i++)
            {
                int shift = hashedPasswordBytes[passwordShiftIndex];
                Encrypted_const[i] = shift <= 128
                    ? (byte)(Encrypted_const[i] + (shiftFlag
                        ? (byte)(((shift << 2)) % 255)
                        : (byte)(((shift << 4)) % 255)))
                    : (byte)(Encrypted_const[i] - (shiftFlag
                        ? (byte)(((shift << 4)) % 255)
                        : (byte)(((shift << 2)) % 255)));
                passwordShiftIndex = (passwordShiftIndex + 1) % 64;
                shiftFlag = !shiftFlag;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #region Write Encrypted OpCodes Length
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            MutationHelper.InjectKey_String(Interpreter_Set_VMDATA, 0, Convert.ToString(Encrypted_const.Length)); // Set_VMDATA
            MutationHelper.InjectKey_String(Interpreter_Set_Constants, 0, Convert.ToString(Encrypted_const.Length)); // Set_Constants
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            desc.Data.constantsMap = Encrypted_const;
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
        }
    }
}