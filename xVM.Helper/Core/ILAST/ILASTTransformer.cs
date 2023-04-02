using System;
using System.Collections.Generic;
using dnlib.DotNet;
using xVM.Helper.Core.AST.ILAST;
using xVM.Helper.Core.CFG;
using xVM.Helper.Core.ILAST.Transformation;
using xVM.Helper.Core.RT;
using xVM.Helper.Core.VM;

namespace xVM.Helper.Core.ILAST
{
    public class ILASTTransformer
    {
        private ITransformationHandler[] pipeline;

        public ILASTTransformer(MethodDef method, ScopeBlock rootScope, VMRuntime runtime)
        {
            RootScope = rootScope;
            Method = method;
            Runtime = runtime;

            Annotations = new Dictionary<object, object>();
            InitPipeline();
        }

        public MethodDef Method
        {
            get;
        }

        public ScopeBlock RootScope
        {
            get;
        }

        public VMRuntime Runtime
        {
            get;
        }

        public VMDescriptor VM => Runtime.Descriptor;

        internal Dictionary<object, object> Annotations
        {
            get;
        }

        internal BasicBlock<ILASTTree> Block
        {
            get;
            private set;
        }

        internal ILASTTree Tree => Block.Content;

        #region Burda OpCodeleri Dönüştürüyor
        ///////////////////////////////////////////////////////////////
        private void InitPipeline()
        {
            pipeline = new ITransformationHandler[]
            {
                new VariableInlining(),
                new StringTransform(),
                new ArrayTransform(),
                new IndirectTransform(),
                new ILASTTypeInference(),
                new NullTransform(),
                new BranchTransform()
            };
        }
        ///////////////////////////////////////////////////////////////
        #endregion

        public void Transform()
        {
            if(pipeline == null)
                throw new InvalidOperationException("Transformer already used.");

            foreach(var handler in pipeline)
            {
                handler.Initialize(this);

                RootScope.ProcessBasicBlocks<ILASTTree>(block =>
                {
                    Block = block;
                    handler.Transform(this);
                });
            }

            pipeline = null;
        }
    }
}