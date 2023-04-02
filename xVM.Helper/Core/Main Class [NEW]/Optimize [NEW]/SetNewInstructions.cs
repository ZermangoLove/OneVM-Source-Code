﻿using System;
using System.Collections.Generic;

using dnlib.DotNet.Emit;

namespace xVM.Helper.Core.Optimize
{
	internal static class MethodBodyExtension
	{
		public static void SetNewInstructions(this CilBody methodBody, List<Instruction> newInstructions, Dictionary<Instruction, Instruction> oldToNewInstructions)
		{
			List<ExceptionHandler> list = new List<ExceptionHandler>();
			foreach (var mdHandler in methodBody.ExceptionHandlers)
			{
				var exHandler = new ExceptionHandler(mdHandler.HandlerType);
				exHandler.CatchType = mdHandler.CatchType;

				if (mdHandler.FilterStart != null)
					exHandler.FilterStart = oldToNewInstructions[mdHandler.FilterStart];
				if (mdHandler.HandlerEnd != null)
					exHandler.HandlerEnd = oldToNewInstructions[mdHandler.HandlerEnd];
				if (mdHandler.HandlerStart != null)
					exHandler.HandlerStart = oldToNewInstructions[mdHandler.HandlerStart];
				if (mdHandler.TryEnd != null)
					exHandler.TryEnd = oldToNewInstructions[mdHandler.TryEnd];
				if (mdHandler.TryStart != null)
					exHandler.TryStart = oldToNewInstructions[mdHandler.TryStart];

				list.Add(exHandler);
			}
			methodBody.ExceptionHandlers.Clear();

			foreach (var item in list)
				methodBody.ExceptionHandlers.Add(item);

			newInstructions.FixJumps(oldToNewInstructions);
			methodBody.Instructions.Clear();

			foreach (var inst in newInstructions)
				methodBody.Instructions.Add(inst);
		}
	}
}
