using System;
using System.Linq;
using System.Collections.Generic;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace xVM.Helper.Core
{
    internal static class SwitchFixer
    {
        #region SwitchFixerA
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal static void SwitchFixerA(MethodDef method)
		{
			try
			{
				for (var i = 0; i < method.Body.Instructions.Count; i++)
				{
					if (method.Body.Instructions[i].OpCode == OpCodes.Switch)
					{
						Instruction[] array = new Instruction[10];
						int[] array2 = new int[10];
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						array[0] = method.Body.Instructions[i];
						num++;
						for (int j = i - 1; j > 0; j--)
						{
							if (method.Body.Instructions[j].OpCode == OpCodes.Nop || method.Body.Instructions[j].OpCode == OpCodes.Ldloc || method.Body.Instructions[j].OpCode == OpCodes.Ldloc_0 || method.Body.Instructions[j].OpCode == OpCodes.Ldloc_1 || method.Body.Instructions[j].OpCode == OpCodes.Ldloc_2 || method.Body.Instructions[j].OpCode == OpCodes.Ldloc_3 || method.Body.Instructions[j].OpCode == OpCodes.Ldloc_S || method.Body.Instructions[j].OpCode == OpCodes.Stloc || method.Body.Instructions[j].OpCode == OpCodes.Stloc_0 || method.Body.Instructions[j].OpCode == OpCodes.Stloc_1 || method.Body.Instructions[j].OpCode == OpCodes.Stloc_2 || method.Body.Instructions[j].OpCode == OpCodes.Stloc_3 || method.Body.Instructions[j].OpCode == OpCodes.Stloc_S || method.Body.Instructions[j].OpCode == OpCodes.Ldc_I4_0)
							{
								array[num] = method.Body.Instructions[j];
								num++;
							}
							else if (method.Body.Instructions[j].OpCode == OpCodes.Sub)
							{
								array[num] = method.Body.Instructions[j];
								num++;
								num2 = 1;
							}
							else if (method.Body.Instructions[j].OpCode == OpCodes.Add)
							{
								array[num] = method.Body.Instructions[j];
								num++;
								num2 = 2;
							}
							else
							{
								if (SwitchFixerA_Helper(method.Body.Instructions[j]) == 0)
								{
									break;
								}
								array[num] = method.Body.Instructions[j];
								num++;
								if (num3 == 0)
								{
									num3 = SwitchFixerA_Helper(method.Body.Instructions[j]);
								}
							}
							if (num == array2.Length)
							{
								break;
							}
						}
						for (int k = 0; k < method.Body.Instructions.Count; k++)
						{
							if (method.Body.Instructions[k].OpCode == OpCodes.Br || method.Body.Instructions[k].OpCode == OpCodes.Br_S)
							{
								if (method.Body.Instructions[k].Operand is Instruction)
								{
									Instruction instruction = method.Body.Instructions[k].Operand as Instruction;
									for (int j = 0; j < num; j++)
									{
										if (array[j] == instruction)
										{
											array2[j]++;
										}
									}
								}
							}
						}
						if (num > 0)
						{
							int num4 = array2[0];
							int num5 = 0;
							for (int j = 0; j < num; j++)
							{
								if (array2[j] > num4)
								{
									num4 = array2[j];
									num5 = j;
								}
							}
							Instruction[] array3 = method.Body.Instructions[i].Operand as Instruction[];
							if (num4 > 0 && array2[num5] >= array3.Length - 1)
							{
								bool flag = false;
								CaseSwitches[] array4 = new CaseSwitches[array3.Length];
								int num6 = 0;
								for (int j = 0; j < method.Body.Instructions.Count; j++)
								{
									if (method.Body.Instructions[j].OpCode == OpCodes.Br || method.Body.Instructions[j].OpCode == OpCodes.Br_S)
									{
										if (method.Body.Instructions[j].Operand is Instruction)
										{
											Instruction instruction = method.Body.Instructions[j].Operand as Instruction;
											if (instruction == array[num5])
											{
												for (int k = j - 1; k > 0; k--)
												{
													if (!(method.Body.Instructions[k].OpCode == OpCodes.Nop) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc_0) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc_1) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc_2) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc_3) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc_S) && !(method.Body.Instructions[k].OpCode == OpCodes.Stloc))
													{
														if (SwitchFixerA_Helper(method.Body.Instructions[k]) != 0)
														{
															int num7 = SwitchFixerA_Helper(method.Body.Instructions[k]);
															if (num2 != 0 && num3 != 0)
															{
																if (num2 == 1)
																{
																	num7 -= num3;
																}
																else if (num2 == 2)
																{
																	num7 += num3;
																}
															}
															if (num7 >= 0 && num7 < array3.Length && num6 < array3.Length)
															{
																array4[num6].loadeval = num7;
																array4[num6].Start = k;
																array4[num6].End = j;
																num6++;
															}
															else
															{
																flag = true;
															}
															break;
														}
													}
												}
											}
										}
									}
								}
								if (!flag && num6 > 0)
								{
									for (int j = 0; j < num6; j++)
									{
										method.Body.Instructions[array4[j].End] = new Instruction(OpCodes.Br, array3[array4[j].loadeval]);
										for (int l = array4[j].Start; l < array4[j].End; l++)
										{
											method.Body.Instructions[l] = new Instruction(OpCodes.Nop, null);
										}
									}
									method.Body.Instructions[i] = new Instruction(OpCodes.Nop, null);
									for (int j = 0; j < method.Body.Instructions.Count; j++)
									{
										if (method.Body.Instructions[j] == array[num5])
										{
											for (int l = j; l < i; l++)
											{
												method.Body.Instructions[l] = new Instruction(OpCodes.Nop, null);
											}
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			catch
			{
				System.Console.WriteLine("SwitchFixerA Failed!");
			}
		}

		private static int SwitchFixerA_Helper(Instruction input)
		{
			int result;
			if (input.OpCode == OpCodes.Ldc_I4_1)
			{
				result = 1;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_2)
			{
				result = 2;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_3)
			{
				result = 3;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_4)
			{
				result = 4;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_5)
			{
				result = 5;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_6)
			{
				result = 6;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_7)
			{
				result = 7;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_8)
			{
				result = 8;
			}
			else if (input.OpCode == OpCodes.Ldc_I4_S)
			{
				sbyte b = (sbyte)input.Operand;
				result = b;
			}
			else if (input.OpCode == OpCodes.Ldc_I4)
			{
				int num = (int)input.Operand;
				result = num;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private struct CaseSwitches
		{
			public int Start;
			public int End;
			public int loadeval;
		}
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
