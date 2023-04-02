using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Text;
using static xVM.Helper.Core.Protections.SugarControlFlow.SugarBlockParser;

namespace xVM.Helper.Core.Protections.SugarControlFlow
{
    internal abstract class ManglerBase
    {
        protected static IEnumerable<SugarInstrBlock> GetAllBlocks(SugarScopeBlock scope)
        {
            foreach (BlockBase child in scope.Children)
            {
                if (child is SugarInstrBlock)
                    yield return (SugarInstrBlock)child;
                else
                {
                    foreach (SugarInstrBlock block in GetAllBlocks((SugarScopeBlock)child))
                        yield return block;
                }
            }
        }

        public abstract void Mangle(CilBody body, SugarScopeBlock root, MethodDef method, TypeSig retType);
    }

}
