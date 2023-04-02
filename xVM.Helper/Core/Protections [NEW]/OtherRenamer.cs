using System;
using System.Collections.Generic;

using dnlib.DotNet;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.Services;

namespace xVM.Helper.Core.Protections
{
	public class OtherRenamer
	{
		public OtherRenamer()
		{
			next = new RandomGenerator().NextInt32(1, int.MaxValue);
		}

		Dictionary<string, string> nameMap = new Dictionary<string, string>();
		int next;

		string ToString(int id)
		{
			return id.ToString("X");
		}

		string NewName(string name)
		{
			string newName;
			if (!nameMap.TryGetValue(name, out newName))
			{
				nameMap[name] = newName = ToString(next);
				next = next * 0x19660D + 0x3C6EF35F;
			}
			return newName;
		}

		public void Process(ModuleDef module)
		{
			foreach (var type in module.GetTypes())
			{
                if (!type.IsPublic && type.Name != VMRuntime.Watermark_Class_Name)
                //if (type.Name != VMRuntime.Watermark_Class_Name)
                {
					type.Namespace = string.Empty;
					type.Name = NewName(CRC32.CheckSumStr(type.FullName));
				}

				foreach (var genParam in type.GenericParameters)
					genParam.Name = NewName(CRC32.CheckSumStr(genParam.Name));

				bool isDelegate = type.BaseType != null &&
								  (type.BaseType.FullName == "System.Delegate" ||
								   type.BaseType.FullName == "System.MulticastDelegate");

				foreach (var method in type.Methods)
				{
					if (method.HasBody)
					{
						foreach (var instr in method.Body.Instructions)
						{
							var memberRef = instr.Operand as MemberRef;
							if (memberRef != null)
							{
								var typeDef = memberRef.DeclaringType.ResolveTypeDef();

								if (memberRef.IsMethodRef && typeDef != null)
								{
									var target = typeDef.ResolveMethod(memberRef);
									if (target != null && target.IsRuntimeSpecialName)
										typeDef = null;
								}

								if (typeDef != null && typeDef.Module == module)
									memberRef.Name = NewName(CRC32.CheckSumStr(memberRef.FullName));
							}
						}
					}
					foreach (var param in method.Parameters)
						param.Name = NewName(CRC32.CheckSumStr(param.Name));

					if (isDelegate || method.IsVirtual || method.IsRuntimeSpecialName ||
						method.IsSpecialName || method.DeclaringType.IsDelegate ||
						method.HasOverrides || method.IsNewSlot)
						continue;
					method.Name = NewName(CRC32.CheckSumStr(method.Name));
				}
				for (int i = 0; i < type.Fields.Count; i++)
				{
					var field = type.Fields[i];
					if (field.IsLiteral)
					{
						type.Fields.RemoveAt(i--);
						continue;
					}
					if (field.IsRuntimeSpecialName)
						continue;
					field.Name = NewName(CRC32.CheckSumStr(field.Name));
				}
				type.Properties.Clear();
				type.Events.Clear();
			}
		}
	}
}
