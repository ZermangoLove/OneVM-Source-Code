using xVM.Runtime.Protection;

namespace xVM.Runtime.Dynamic
{
    internal unsafe class Constants
    {
        [VMProtect.BeginUltra]
        static Constants()
        {
            if (AntiDumpV2.AntiDumpIsRunning == true && OPCODELIST == null && Utils.AntiTamperChecker == null)
                OPCODELIST = Interpreter.__CONSTDATA;
        }

        public static byte[] OPCODELIST = null;

        #region Old OpCodes System
        //public static byte REG_R0 = OPCODELIST[0];
        //public static byte REG_R1 = OPCODELIST[2];
        //public static byte REG_R2 = OPCODELIST[4];
        //public static byte REG_R3 = OPCODELIST[6];
        //public static byte REG_R4 = OPCODELIST[8];
        //public static byte REG_R5 = OPCODELIST[10];
        //public static byte REG_R6 = OPCODELIST[12];
        //public static byte REG_R7 = OPCODELIST[14]; //Removed
        //public static byte REG_BP = OPCODELIST[14];
        //public static byte REG_SP = OPCODELIST[16];
        //public static byte REG_IP = OPCODELIST[18];
        //public static byte REG_FL = OPCODELIST[20];
        //public static byte REG_K1 = OPCODELIST[22];
        //public static byte REG_K2 = OPCODELIST[24];
        //public static byte REG_M1 = OPCODELIST[26];
        //public static byte REG_M2 = OPCODELIST[28];

        //public static byte FL_OVERFLOW = OPCODELIST[30];
        //public static byte FL_CARRY = OPCODELIST[32];
        //public static byte FL_ZERO = OPCODELIST[34];
        //public static byte FL_SIGN = OPCODELIST[36];
        //public static byte FL_UNSIGNED = OPCODELIST[38];
        //public static byte FL_BEHAV1 = OPCODELIST[40];
        //public static byte FL_BEHAV2 = OPCODELIST[42];
        //public static byte FL_BEHAV3 = OPCODELIST[44]; //Removed

        //public static byte OP_NOP = OPCODELIST[44];
        //public static byte OP_LIND_PTR = OPCODELIST[46];
        //public static byte OP_LIND_OBJECT = OPCODELIST[48];
        //public static byte OP_LIND_BYTE = OPCODELIST[50];
        //public static byte OP_LIND_WORD = OPCODELIST[52];
        //public static byte OP_LIND_DWORD = OPCODELIST[54];
        //public static byte OP_LIND_QWORD = OPCODELIST[56];
        //public static byte OP_SIND_PTR = OPCODELIST[58];
        //public static byte OP_SIND_OBJECT = OPCODELIST[60];
        //public static byte OP_SIND_BYTE = OPCODELIST[62];
        //public static byte OP_SIND_WORD = OPCODELIST[64];
        //public static byte OP_SIND_DWORD = OPCODELIST[66];
        //public static byte OP_SIND_QWORD = OPCODELIST[68];
        //public static byte OP_POP = OPCODELIST[70];
        //public static byte OP_PUSHR_OBJECT = OPCODELIST[72];
        //public static byte OP_PUSHR_BYTE = OPCODELIST[74];
        //public static byte OP_PUSHR_WORD = OPCODELIST[76];
        //public static byte OP_PUSHR_DWORD = OPCODELIST[78];
        //public static byte OP_PUSHR_QWORD = OPCODELIST[80];
        //public static byte OP_PUSHI_DWORD = OPCODELIST[82];
        //public static byte OP_PUSHI_QWORD = OPCODELIST[84];
        //public static byte OP_SX_BYTE = OPCODELIST[86];
        //public static byte OP_SX_WORD = OPCODELIST[88];
        //public static byte OP_SX_DWORD = OPCODELIST[90];
        //public static byte OP_CALL = OPCODELIST[92];
        //public static byte OP_RET = OPCODELIST[94];
        //public static byte OP_NOR_DWORD = OPCODELIST[96];
        //public static byte OP_NOR_QWORD = OPCODELIST[98];
        //public static byte OP_CMP = OPCODELIST[100];
        //public static byte OP_CMP_DWORD = OPCODELIST[102];
        //public static byte OP_CMP_QWORD = OPCODELIST[104];
        //public static byte OP_CMP_R32 = OPCODELIST[106];
        //public static byte OP_CMP_R64 = OPCODELIST[108];
        //public static byte OP_JZ = OPCODELIST[110];
        //public static byte OP_JNZ = OPCODELIST[112];
        //public static byte OP_JMP = OPCODELIST[114];
        //public static byte OP_SWT = OPCODELIST[116];
        //public static byte OP_ADD_DWORD = OPCODELIST[118];
        //public static byte OP_ADD_QWORD = OPCODELIST[120];
        //public static byte OP_ADD_R32 = OPCODELIST[122];
        //public static byte OP_ADD_R64 = OPCODELIST[124];
        //public static byte OP_SUB_R32 = OPCODELIST[126];
        //public static byte OP_SUB_R64 = OPCODELIST[128];
        //public static byte OP_MUL_DWORD = OPCODELIST[130];
        //public static byte OP_MUL_QWORD = OPCODELIST[132];
        //public static byte OP_MUL_R32 = OPCODELIST[134];
        //public static byte OP_MUL_R64 = OPCODELIST[136];
        //public static byte OP_DIV_DWORD = OPCODELIST[138];
        //public static byte OP_DIV_QWORD = OPCODELIST[140];
        //public static byte OP_DIV_R32 = OPCODELIST[142];
        //public static byte OP_DIV_R64 = OPCODELIST[144];
        //public static byte OP_REM_DWORD = OPCODELIST[146];
        //public static byte OP_REM_QWORD = OPCODELIST[148];
        //public static byte OP_REM_R32 = OPCODELIST[150];
        //public static byte OP_REM_R64 = OPCODELIST[152];
        //public static byte OP_SHR_DWORD = OPCODELIST[154];
        //public static byte OP_SHR_QWORD = OPCODELIST[156];
        //public static byte OP_SHL_DWORD = OPCODELIST[158];
        //public static byte OP_SHL_QWORD = OPCODELIST[160];
        //public static byte OP_FCONV_R32_R64 = OPCODELIST[162];
        //public static byte OP_FCONV_R64_R32 = OPCODELIST[164];
        //public static byte OP_FCONV_R32 = OPCODELIST[166];
        //public static byte OP_FCONV_R64 = OPCODELIST[168];
        //public static byte OP_ICONV_PTR = OPCODELIST[170];
        //public static byte OP_ICONV_R64 = OPCODELIST[172];
        //public static byte OP_VCALL = OPCODELIST[174];
        //public static byte OP_TRY = OPCODELIST[176];
        //public static byte OP_LEAVE = OPCODELIST[178];

        //public static byte VCALL_EXIT = OPCODELIST[180];
        //public static byte VCALL_BREAK = OPCODELIST[182];
        //public static byte VCALL_ECALL = OPCODELIST[184];
        //public static byte VCALL_CAST = OPCODELIST[186];
        //public static byte VCALL_CKFINITE = OPCODELIST[188];
        //public static byte VCALL_CKOVERFLOW = OPCODELIST[190];
        //public static byte VCALL_RANGECHK = OPCODELIST[192];
        //public static byte VCALL_INITOBJ = OPCODELIST[194];
        //public static byte VCALL_LDFLD = OPCODELIST[196];
        //public static byte VCALL_LDFTN = OPCODELIST[198];
        //public static byte VCALL_TOKEN = OPCODELIST[200];
        //public static byte VCALL_THROW = OPCODELIST[202];
        //public static byte VCALL_SIZEOF = OPCODELIST[204];
        //public static byte VCALL_STFLD = OPCODELIST[206];
        //public static byte VCALL_BOX = OPCODELIST[208];
        //public static byte VCALL_UNBOX = OPCODELIST[210];
        //public static byte VCALL_LOCALLOC = OPCODELIST[212];

        //public static byte ECALL_CALL = OPCODELIST[214];
        //public static byte ECALL_CALLVIRT = OPCODELIST[216];
        //public static byte ECALL_NEWOBJ = OPCODELIST[218];
        //public static byte ECALL_CALLVIRT_CONSTRAINED = OPCODELIST[220];

        //public static byte HELPER_INIT = OPCODELIST[222];

        //public static byte FLAG_INSTANCE = OPCODELIST[224];

        //public static byte EH_CATCH = OPCODELIST[226];
        //public static byte EH_FILTER = OPCODELIST[228];
        //public static byte EH_FAULT = OPCODELIST[230];
        //public static byte EH_FINALLY = OPCODELIST[232];
        #endregion
    }
}