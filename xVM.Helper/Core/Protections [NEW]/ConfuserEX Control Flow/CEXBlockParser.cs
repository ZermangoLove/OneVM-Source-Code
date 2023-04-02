using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using dnlib.DotNet.Emit;

using xVM.Helper.Core.Helpers.System;

namespace xVM.Helper.Core.Protections
{
	internal static class CEXBlockParser
	{
        public static CEXScopeBlock ParseBody(CilBody body)
        {
            var ehScopes = new Dictionary<ExceptionHandler, Tuple<CEXScopeBlock, CEXScopeBlock, CEXScopeBlock>>();
            foreach (ExceptionHandler eh in body.ExceptionHandlers)
            {
                var tryBlock = new CEXScopeBlock(CEXBlockType.Try, eh);

                var handlerType = CEXBlockType.Handler;

                if (eh.HandlerType == ExceptionHandlerType.Finally)
                    handlerType = CEXBlockType.Finally;
                else if (eh.HandlerType == ExceptionHandlerType.Fault)
                    handlerType = CEXBlockType.Fault;

                var handlerBlock = new CEXScopeBlock(handlerType, eh);

                if (eh.FilterStart != null)
                {
                    var filterBlock = new CEXScopeBlock(CEXBlockType.Filter, eh);
                    ehScopes[eh] = Tuple.Create(tryBlock, handlerBlock, filterBlock);
                }
                else
                    ehScopes[eh] = Tuple.Create(tryBlock, handlerBlock, (CEXScopeBlock)null);
            }

            var root = new CEXScopeBlock(CEXBlockType.Normal, null);
            var scopeStack = new Stack<CEXScopeBlock>();

            scopeStack.Push(root);
            foreach (Instruction instr in body.Instructions)
            {
                foreach (ExceptionHandler eh in body.ExceptionHandlers)
                {
                    Tuple<CEXScopeBlock, CEXScopeBlock, CEXScopeBlock> ehScope = ehScopes[eh];

                    if (instr == eh.TryEnd)
                        scopeStack.Pop();

                    if (instr == eh.HandlerEnd)
                        scopeStack.Pop();

                    if (eh.FilterStart != null && instr == eh.HandlerStart)
                    {
                        // Filter must precede handler immediately
                        Debug.Assert(scopeStack.Peek().Type == CEXBlockType.Filter);
                        scopeStack.Pop();
                    }
                }
                foreach (ExceptionHandler eh in body.ExceptionHandlers.Reverse())
                {
                    Tuple<CEXScopeBlock, CEXScopeBlock, CEXScopeBlock> ehScope = ehScopes[eh];
                    CEXScopeBlock parent = scopeStack.Count > 0 ? scopeStack.Peek() : null;

                    if (instr == eh.TryStart)
                    {
                        if (parent != null)
                            parent.Children.Add(ehScope.Item1);
                        scopeStack.Push(ehScope.Item1);
                    }

                    if (instr == eh.HandlerStart)
                    {
                        if (parent != null)
                            parent.Children.Add(ehScope.Item2);
                        scopeStack.Push(ehScope.Item2);
                    }

                    if (instr == eh.FilterStart)
                    {
                        if (parent != null)
                            parent.Children.Add(ehScope.Item3);
                        scopeStack.Push(ehScope.Item3);
                    }
                }

                CEXScopeBlock scope = scopeStack.Peek();
                var block = scope.Children.LastOrDefault() as CEXInstrBlock;
                if (block == null)
                    scope.Children.Add(block = new CEXInstrBlock());
                block.Instructions.Add(instr);
            }
            foreach (ExceptionHandler eh in body.ExceptionHandlers)
            {
                if (eh.TryEnd == null)
                    scopeStack.Pop();
                if (eh.HandlerEnd == null)
                    scopeStack.Pop();
            }
            Debug.Assert(scopeStack.Count == 1);
            return root;
        }
    }
}

