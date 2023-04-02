using System.IO;
using System.Linq;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Core.AST.ILAST;

namespace xVM.Helper.Core.ILAST.Transformation
{
    public class StringTransform : ITransformationHandler
    {
        public void Initialize(ILASTTransformer tr) { }

        public void Transform(ILASTTransformer tr)
        {
            tr.Tree.TraverseTree(Transform, tr);
        }

        private static void Transform(ILASTExpression expr, ILASTTransformer tr)
        {
            if (expr.ILCode != Code.Ldstr)
                return;

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write((string)expr.Operand);
            stream.Flush();
            writer.Flush();

            var StrDat = stream.ToArray();
            stream.Close();
            writer.Close();

            expr.ILCode = Code.Box;
            expr.Operand = tr.Method.Module.CorLibTypes.String.ToTypeDefOrRef();
            expr.Arguments = new IILASTNode[]
            {
                new ILASTExpression
                {
                    ILCode = Code.Ldc_I4,
                    Operand = (int) tr.VM.Data.GetId(StrDat),
                    Arguments = new IILASTNode[0]
                }
            };
        }
    }
}