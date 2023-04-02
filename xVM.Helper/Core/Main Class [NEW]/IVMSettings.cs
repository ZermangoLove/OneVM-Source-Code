namespace xVM.Helper.Core
{
    public interface IVMSettings
    {
        bool IsVirtualized(dnlib.DotNet.MethodDef method);
        bool IsExported(dnlib.DotNet.MethodDef method);
    }
}