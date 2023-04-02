using System;
using xVM.Helper.Core.Services;
using xVM.Helper.DynCipher.AST;
using xVM.Helper.DynCipher.Generation;

namespace xVM.Helper.DynCipher {
	internal class DynCipherService {
		public void GenerateCipherPair(RandomGenerator random, out StatementBlock encrypt, out StatementBlock decrypt) {
			CipherGenerator.GeneratePair(random, out encrypt, out decrypt);
		}

		public void GenerateExpressionPair(RandomGenerator random, Expression var, Expression result, int depth, out Expression expression, out Expression inverse) {
			ExpressionGenerator.GeneratePair(random, var, result, depth, out expression, out inverse);
		}
	}
}