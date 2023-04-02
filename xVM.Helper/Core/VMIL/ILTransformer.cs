using System;
using System.Collections.Generic;
using dnlib.DotNet;
using xVM.Helper.Core.AST.IL;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.RT;
using xVM.Helper.Core.VM;
using xVM.Helper.Core.VMIL.Transforms;

namespace xVM.Helper.Core.VMIL
{
    public class ILTransformer
    {
        private ITransform[] pipeline;

        public ILTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
        {
            RootScope = rootScope;
            Method = method;
            Runtime = runtime;

            Annotations = new Dictionary<object, object>();
            pipeline = InitPipeline();
        }

        public VMRuntime Runtime
        {
            get;
        }

        public MethodDef Method
        {
            get;
        }

        public ScopeBlock RootScope
        {
            get;
        }

        public VMDescriptor VM => Runtime.Descriptor;

        internal Dictionary<object, object> Annotations
        {
            get;
        }

        internal ILBlock Block
        {
            get;
            private set;
        }

        internal ILInstrList Instructions => Block.Content;

        private ITransform[] InitPipeline()
        {
            return new ITransform[]
            {
                //new Protections.SMC.SMCILTransform(),
                new ReferenceOffsetTransform(),
                new EntryExitTransform(),
                new SaveInfoTransform()
            };
        }

        public void Transform()
        {
            if(pipeline == null)
                throw new InvalidOperationException("Transformer already used.");

            foreach(var handler in pipeline)
            {
                handler.Initialize(this);

                RootScope.ProcessBasicBlocks<ILInstrList>(block =>
                {
                    Block = (ILBlock) block;
                    handler.Transform(this);
                });
            }

            pipeline = null;
        }
    }
}