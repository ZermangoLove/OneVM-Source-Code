using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace xVM.GUI
{
	internal class Utils
	{
		public static Random random = new Random();

		public static bool IsDotNetAssembly(string assemblyPath)
		{
			bool result;
			try
			{
                System.Reflection.AssemblyName.GetAssemblyName(assemblyPath);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static List<string> GetAllMethodList(string assemblyPath)
		{
			var module = ModuleDefMD.Load(assemblyPath);
			var list = new List<string>();
			foreach (TypeDef typeDef in module.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
						if (OVMAnalyzer(methodDef))
							list.Add(methodDef.FullName);
				}
			}
			return list;
		}

		public string Random_VMProtect_HEX()
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < 4; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(32 + (decimal)random.Next('z') - 32)));
				builder.Append(ch);
			}

			/*/
             * HEX'e dönüştürülmesini istemiyorsan return kısmını builder.ToString() yap.
             * kaç sayıda random str üretmeyi seçmek için for (int i = 0; i < 4; i++) kısmındaki "4" kısmını değiştirebilirsin.
            /*/
			return string.Join(string.Empty, builder.ToString().Select(c => string.Format("{0:X2}", System.Convert.ToInt32(c))).ToArray());
		}

		public static bool OVMAnalyzer(MethodDef method)
		{
			if (!method.HasBody)
				return false;
			if (method.HasGenericParameters)
				return false;
			if (method.IsPinvokeImpl)
				return false;
			if (method.IsUnmanagedExport)
				return false;
			return true;
		}

		public static string ToHexString(string str)
		{
			var sb = new StringBuilder();
			var bytes = Encoding.UTF8.GetBytes(str);
			foreach (var t in bytes)
			{
				sb.Append(t.ToString("X2"));
			}
			return sb.ToString().ToUpper(); // returns: "48656C6C6F20776F726C64" for "Hello world"
		}
	}
}
