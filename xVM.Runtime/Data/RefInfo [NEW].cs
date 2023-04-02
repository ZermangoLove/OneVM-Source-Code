using System.Reflection;

namespace xVM.Runtime.Data
{
    internal class RefInfo
    {
        private MemberInfo Resolved;
        public int Token;

        public MemberInfo Member()
		{
			MemberInfo result;
            if ((result = Resolved) == null)
            {
                result = Resolved = VMInstance.__ExecuteModule.ResolveMember(Token);
            }
			return result;
		}
    }
}
