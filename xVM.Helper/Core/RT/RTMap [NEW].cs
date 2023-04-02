namespace xVM.Helper.Core.RT
{
    internal class RTMap
    {
        public static string IDAttribute = "xVM.Runtime.IDAttribute";

        public static string AntiTamperEXEC = "xVM.Runtime.Protection.AntiTamperEXEC";
        public static string AntiTamperEXEC_Initialize = "Initialize";
        public static string AntiTamperEXEC_AntiTamperRTIsRunning_Field = "AntiTamperRTIsRunning";

        public static string ConstantsProt = "xVM.Runtime.Protection.Constant";
        public static string ConstantsProt_Initialize = "Initialize";
        public static string ConstantsProt_Get = "Get";

        public static string Murmur2 = "xVM.Runtime.Services.Murmur2";
        public static string Murmur2_Hash = "Hash";

        public static string VMData = "xVM.Runtime.Data.VMData";

        public static string Utils = "xVM.Runtime.Utils";
        public static string Utils_ReadCompressedULong = "ReadCompressedULong";
        public static string Utils_FromCodedToken = "FromCodedToken";

        public static string VMEntry = "xVM.Runtime.VMEntry";
        public static string VMEntry_Invoke = "Invoke";
        public static string VMEntry_ConfigureRT = "ConfigureRT";

        public static string VMInstance = "xVM.Runtime.VMInstance";
        public static string VMInstance_Invoke = "Invoke";

        public static string VMDispatcher = "xVM.Runtime.Execution.VMDispatcher";
        public static string VMDispatcher_DoThrow = "DoThrow";
        public static string VMDispatcher_Throw = "Throw";

        public static string VMTrampoline = "xVM.Runtime.Execution.Internal.VMTrampoline";

        public static string Interpreter = "xVM.Runtime.Interpreter";
        public static string Interpreter_GetInternalVData = "GetInternalVData";
        public static string Interpreter_Set_VMDATA = "Set_VMDATA";
        public static string Interpreter_Set_Constants = "Set_Constants";

        public static string Mutation = "Mutation";
        public static string Mutation_Placeholder = "Placeholder";
        public static string Mutation_Value_T = "Value";
        public static string Mutation_Value_T_Arg0 = "Value";
        public static string Mutation_Crypt = "Crypt";

        public static string MutationCore = "MutationCore";
        public static string MutationCore_Placeholder = "Placeholder";
        public static string MutationCore_Value_T = "Value";
        public static string MutationCore_Value_T_Arg0 = "Value";
        public static string MutationCore_Crypt = "Crypt";

        public static string AnyCtor = ".ctor";
    }
}