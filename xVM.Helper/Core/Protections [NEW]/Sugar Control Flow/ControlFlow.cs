using System;
using System.Linq;

using dnlib.DotNet;
using dnlib.DotNet.Pdb;
using dnlib.DotNet.Emit;

using static xVM.Helper.Core.Protections.SugarControlFlow.SugarBlockParser;

namespace xVM.Helper.Core.Protections.SugarControlFlow
{
    public class SugarControlFlow
    {
        public static void Execute(MethodDef method)
        {
            if (method.HasBody && method.Body.HasInstructions)
            {
                if (method.ReturnType != null)
                {
                    PhaseControlFlow(method);
                }
            }
        }

        private static void PhaseControlFlow(MethodDef method)
        {
            var body = method.Body;
            body.SimplifyBranches();

            SugarScopeBlock root = ParseBody(body);

            new SwitchMangler().Mangle(body, root, method, method.ReturnType);

            body.Instructions.Clear();
            root.ToBody(body);

            if (body.PdbMethod != null)
            {
                body.PdbMethod = new PdbMethod()
                {
                    Scope = new PdbScope()
                    {
                        Start = body.Instructions.First(),
                        End = body.Instructions.Last()
                    }
                };
            }

            method.CustomDebugInfos.RemoveWhere(cdi => cdi is PdbStateMachineHoistedLocalScopesCustomDebugInfo);

            foreach (ExceptionHandler eh in body.ExceptionHandlers)
            {
                var index = body.Instructions.IndexOf(eh.TryEnd) + 1;
                eh.TryEnd = index < body.Instructions.Count ? body.Instructions[index] : null;
                index = body.Instructions.IndexOf(eh.HandlerEnd) + 1;
                eh.HandlerEnd = index < body.Instructions.Count ? body.Instructions[index] : null;
            }
        }
    }
}