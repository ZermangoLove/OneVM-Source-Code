// dnlib: See LICENSE.txt for more info

using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dnlib.DotNet.Emit
{
	/// <summary>
	/// Converts instructions to strings
	/// </summary>
	public static class InstructionPrinterCustom
	{
		public static string GetOperandString(Instruction instr)
		{
			var sb = new RichTextBox();
			AddOperandString(sb, instr, string.Empty);
			return sb.ToString();
		}

		public static void AddOperandString(RichTextBox sb, Instruction instr) => AddOperandString(sb, instr, string.Empty);

		public static void AddOperandString(RichTextBox sb, Instruction instr, string extra)
		{
			var op = instr.Operand;
			switch (instr.OpCode.OperandType)
			{
				case OperandType.InlineBrTarget:
				case OperandType.ShortInlineBrTarget:
					sb.AppendText(extra);
					AddInstructionTarget(sb, op as Instruction);
					break;

				case OperandType.InlineField:
				case OperandType.InlineMethod:
				case OperandType.InlineTok:
				case OperandType.InlineType:
					sb.AppendText(extra);
					if (op is IFullName)
						sb.AppendText((op as IFullName).FullName, Color.FromArgb(0x64C800));
					else if (op != null)
						sb.AppendText(op.ToString(), Color.FromArgb(0x64C800));
					else
						sb.AppendText("null", Color.FromArgb(0x64C800));
					break;

				case OperandType.InlineI:
				case OperandType.InlineI8:
				case OperandType.InlineR:
				case OperandType.ShortInlineI:
				case OperandType.ShortInlineR:
					sb.AppendText($"{extra}{op}", Color.FromArgb(0xB5CEA8));
					break;

				case OperandType.InlineSig:
					sb.AppendText(extra);
					sb.AppendText(FullNameFactory.MethodFullName(null, (UTF8String)null, op as MethodSig, null, null, null, null));
					break;

				case OperandType.InlineString:
					sb.AppendText(extra);
					EscapeString(sb, op as string, true);
					break;

				case OperandType.InlineSwitch:
					var targets = op as IList<Instruction>;
					if (targets is null)
						sb.AppendText("null");
					else
					{
						sb.AppendText("(");
						for (int i = 0; i < targets.Count; i++)
						{
							if (i != 0)
								sb.AppendText(",");
							AddInstructionTarget(sb, targets[i]);
						}
						sb.AppendText(")");
					}
					break;

				case OperandType.InlineVar:
				case OperandType.ShortInlineVar:
					sb.AppendText(extra);
					if (op is null)
						sb.AppendText("null", Color.FromArgb(0xD9E0E6));
					else
						sb.AppendText(op.ToString(), Color.FromArgb(0xD9E0E6));
					break;

				case OperandType.InlineNone:
				case OperandType.InlinePhi:
				default:
					break;
			}
		}

		static void AddInstructionTarget(RichTextBox sb, Instruction targetInstr)
		{
			if (targetInstr is null)
				sb.AppendText("null", Color.FromArgb(0x806F4D));
			else
				sb.AppendText($"IL_{targetInstr.Offset:X4}", Color.FromArgb(0x806F4D));
		}

		static void EscapeString(RichTextBox sb, string s, bool addQuotes)
		{
			if (s is null)
			{
				sb.AppendText("null", Color.FromArgb(0xD69D85));
				return;
			}

			if (addQuotes)
				sb.AppendText("\"", Color.FromArgb(0xD69D85));

			foreach (var c in s)
			{
				if ((int)c < 0x20)
				{
					switch (c)
					{
						case '\a': sb.AppendText(@"\a", Color.FromArgb(0xD69D85)); break;
						case '\b': sb.AppendText(@"\b", Color.FromArgb(0xD69D85)); break;
						case '\f': sb.AppendText(@"\f", Color.FromArgb(0xD69D85)); break;
						case '\n': sb.AppendText(@"\n", Color.FromArgb(0xD69D85)); break;
						case '\r': sb.AppendText(@"\r", Color.FromArgb(0xD69D85)); break;
						case '\t': sb.AppendText(@"\t", Color.FromArgb(0xD69D85)); break;
						case '\v': sb.AppendText(@"\v", Color.FromArgb(0xD69D85)); break;
						default:
							sb.AppendText($@"\u{(int)c:X4}", Color.FromArgb(0xD69D85));
							break;
					}
				}
				else if (c == '\\' || c == '"')
				{
					sb.AppendText("\\", Color.FromArgb(0xD69D85));
					sb.AppendText(c.ToString(), Color.FromArgb(0xD69D85));
				}
				else
					sb.AppendText(c.ToString(), Color.FromArgb(0xD69D85));
			}

			if (addQuotes)
				sb.AppendText("\"", Color.FromArgb(0xD69D85));
		}

		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(text);
			box.SelectionColor = box.ForeColor;
		}

		private static void CheckKeyword(this RichTextBox ILBox, string word, Color color, int ekstra)
		{
			if (ILBox.Text.Contains(word))
			{
				int index = -1;
				int selectStart = ILBox.SelectionStart;

				while ((index = ILBox.Text.IndexOf(word, (index + 1))) != -1)
				{
					ILBox.Select(index, word.Length + ekstra);
					ILBox.SelectionColor = color;
					ILBox.Select(selectStart, 0);
					ILBox.SelectionColor = Color.Black;
				}
			}
		}
	}
}
