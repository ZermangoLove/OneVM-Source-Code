using System;

namespace xVM.Helper.DynCipher.AST {
	public abstract class Statement {
		public object Tag { get; set; }
		public abstract override string ToString();
	}
}