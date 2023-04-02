using System;
using dnlib.DotNet;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.ILAST;
using xVM.Helper.Core.RT;
using xVM.Helper.Core.VMIL;
using xVM.Helper.Core.VMIR;

namespace xVM.Helper.Core
{
    internal class MethodVirtualizer
    {
        public MethodVirtualizer(VMRuntime runtime)
        {
            Runtime = runtime;
        }

        protected VMRuntime Runtime
        {
            get;
        }

        protected MethodDef Method
        {
            get;
            private set;
        }

        protected ModuleDef Module
        {
            get;
            private set;
        }

        protected ScopeBlock RootScope
        {
            get;
            private set;
        }

        protected IRContext IRContext
        {
            get;
            private set;
        }

        protected bool IsExport
        {
            get;
            private set;
        }

        public ScopeBlock Run(MethodDef method, ModuleDef module, bool isExport)
        {
            try
            {
                Method = method;
                Module = module;
                IsExport = isExport;

                Init();
                BuildILAST();
                TransformILAST();
                BuildVMIR();
                TransformVMIR();
                BuildVMIL();
                TransformVMIL();
                Deinitialize();

                var scope = RootScope;
                RootScope = null;
                Method = null;
                return scope;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Failed Translate]: {0} LOG[{1}]", method.FullName, ex.ToString());

                var scope = RootScope;
                RootScope = null;
                Method = null;
                return scope;
            }
        }

        protected virtual void Init()
        {
            RootScope = BlockParser.Parse(Method, Method.Body);
            IRContext = new IRContext(Method, Method.Body);
        }

        protected virtual void BuildILAST()
        {
            ILASTBuilder.BuildAST(Method, Method.Body, RootScope);
        }

        protected virtual void TransformILAST()
        {
            var transformer = new ILASTTransformer(Method, RootScope, Runtime);
            transformer.Transform();
        }

        protected virtual void BuildVMIR()
        {
            var translator = new IRTranslator(IRContext, Runtime);
            translator.Translate(RootScope);
        }

        protected virtual void TransformVMIR()
        {
            var transformer = new IRTransformer(RootScope, IRContext, Runtime);
            transformer.Transform();
        }

        protected virtual void BuildVMIL()
        {
            var translator = new ILTranslator(Runtime);
            translator.Translate(RootScope);
        }

        protected virtual void TransformVMIL()
        {
            var transformer = new ILTransformer(Method, RootScope, Runtime);
            transformer.Transform();
        }

        protected virtual void Deinitialize()
        {
            IRContext = null;
            Runtime.AddMethod(Method, RootScope);
            if(IsExport)
                Runtime.ExportMethod(Method, Module);
        }
    }
}