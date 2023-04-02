using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using dnlib.DotNet;

namespace xVM.Helper.Core.Helpers
{
    public static class FullNameToMethod_Class
    {
        public static IList<MethodDef> FullNamesToMethods(ModuleDef module, IList<string> Methods_FullName)
        {
            var MDLIST = new HashSet<MethodDef>();
            var Custom_FullNames = new HashSet<string>(Methods_FullName);
            var Custom_FullNames_SB = new StringBuilder();
            var Custom_stream = new MemoryStream();
            var All_MDS_FullName_MS = new MemoryStream();

            #region Custom Methods FullNames Write To "Custom_FullNames_SB"
            //////////////////////////////////////////////////////////////////////////////
            foreach (var Custom_FullNamesToCustom_FullNames_SB in Custom_FullNames)
                Custom_FullNames_SB.AppendLine(Custom_FullNamesToCustom_FullNames_SB);
            //////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Custom Methods FullNames Write To "Custom_stream"
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var writer_NULL = new StreamWriter(Custom_stream);
            for (var i = 0; i < Custom_FullNames.ToList().Count; i++)
            {
                writer_NULL.WriteLine(Custom_FullNames.ToList()[i]);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region All Methods FullNames Write To "All_MDS_FullName_MS"
            /////////////////////////////////////////////////////////////////////////////////
            var All_MDS_FullName_SW = new StreamWriter(All_MDS_FullName_MS);
            for (var x = 0; x < module.GetTypes().Count(); x++)
                for (var i = 0; i < module.GetTypes().ToArray()[x].Methods.Count; i++)
                All_MDS_FullName_SW.WriteLine(module.GetTypes().ToArray()[x].Methods[i]);
            /////////////////////////////////////////////////////////////////////////////////
            #endregion

            foreach (var Def in module.GetTypes())
            {
                foreach (var All_MD in Def.Methods)
                {
                    var All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName.Add(All_MD.FullName);

                    var All_MDS_FullName_SB = new StringBuilder();
                    All_MDS_FullName_SB.AppendLine(All_MD.FullName);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region MethodA
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName_SB.ToString().Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodB
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        if (Custom_FullNames.Contains(All_MD_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodC
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName.Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodD
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (Custom_FullName == All_MD.FullName)
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodE
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        foreach (var Custom_FullName in Custom_FullNames)
                            if (All_MD_FullName == Custom_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodF
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        foreach (var All_MD_FullName in All_MDS_FullName)
                            if (Custom_FullName == All_MD_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodG
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames_SB.ToString().Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodH
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames.Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    //#region MethodI
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NuLL = Encoding.Default.GetString(Custom_stream.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NuLL.Length; i++)
                    //    if (All_MDS_FullName_MS.ToArray().Contains(Encoding.Default.GetBytes(array_NuLL[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    //#region MethodJ
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NUll = Encoding.Default.GetString(All_MDS_FullName_MS.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NUll.Length; i++)
                    //    if (Custom_stream.ToArray().Contains(Encoding.Default.GetBytes(array_NUll[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    All_MDS_FullName.Clear();
                    All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName_SB = new StringBuilder();
                    Custom_stream = new MemoryStream();
                }
            }
            return MDLIST.Distinct().ToList();
        }

        public static IList<MethodDef> FullNamesToMethods(ModuleDef module, HashSet<string> Methods_FullName)
        {
            var MDLIST = new HashSet<MethodDef>();
            var Custom_FullNames = Methods_FullName;
            var Custom_FullNames_SB = new StringBuilder();
            var Custom_stream = new MemoryStream();
            var All_MDS_FullName_MS = new MemoryStream();

            #region Custom Methods FullNames Write To "Custom_FullNames_SB"
            //////////////////////////////////////////////////////////////////////////////
            foreach (var Custom_FullNamesToCustom_FullNames_SB in Custom_FullNames)
                Custom_FullNames_SB.AppendLine(Custom_FullNamesToCustom_FullNames_SB);
            //////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Custom Methods FullNames Write To "Custom_stream"
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var writer_NULL = new StreamWriter(Custom_stream);
            for (var i = 0; i < Custom_FullNames.ToList().Count; i++)
            {
                writer_NULL.WriteLine(Custom_FullNames.ToList()[i]);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region All Methods FullNames Write To "All_MDS_FullName_MS"
            /////////////////////////////////////////////////////////////////////////////////
            var All_MDS_FullName_SW = new StreamWriter(All_MDS_FullName_MS);
            for (var x = 0; x < module.GetTypes().Count(); x++)
                for (var i = 0; i < module.GetTypes().ToArray()[x].Methods.Count; i++)
                    All_MDS_FullName_SW.WriteLine(module.GetTypes().ToArray()[x].Methods[i]);
            /////////////////////////////////////////////////////////////////////////////////
            #endregion

            foreach (var Def in module.GetTypes())
            {
                foreach (var All_MD in Def.Methods)
                {
                    var All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName.Add(All_MD.FullName);

                    var All_MDS_FullName_SB = new StringBuilder();
                    All_MDS_FullName_SB.AppendLine(All_MD.FullName);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region MethodA
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName_SB.ToString().Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodB
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        if (Custom_FullNames.Contains(All_MD_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodC
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName.Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodD
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (Custom_FullName == All_MD.FullName)
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodE
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        foreach (var Custom_FullName in Custom_FullNames)
                            if (All_MD_FullName == Custom_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodF
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        foreach (var All_MD_FullName in All_MDS_FullName)
                            if (Custom_FullName == All_MD_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodG
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames_SB.ToString().Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodH
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames.Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    //#region MethodI
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NuLL = Encoding.Default.GetString(Custom_stream.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NuLL.Length; i++)
                    //    if (All_MDS_FullName_MS.ToArray().Contains(Encoding.Default.GetBytes(array_NuLL[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    //#region MethodJ
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NUll = Encoding.Default.GetString(All_MDS_FullName_MS.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NUll.Length; i++)
                    //    if (Custom_stream.ToArray().Contains(Encoding.Default.GetBytes(array_NUll[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    All_MDS_FullName.Clear();
                    All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName_SB = new StringBuilder();
                    Custom_stream = new MemoryStream();
                }
            }
            return MDLIST.Distinct().ToList();
        }
        
        public static IList<MethodDef> FullNamesToMethods(ModuleDef module, List<string> Methods_FullName)
        {
            var MDLIST = new HashSet<MethodDef>();
            var Custom_FullNames = new HashSet<string>(Methods_FullName);
            var Custom_FullNames_SB = new StringBuilder();
            var Custom_stream = new MemoryStream();
            var All_MDS_FullName_MS = new MemoryStream();

            #region Custom Methods FullNames Write To "Custom_FullNames_SB"
            //////////////////////////////////////////////////////////////////////////////
            foreach (var Custom_FullNamesToCustom_FullNames_SB in Custom_FullNames)
                Custom_FullNames_SB.AppendLine(Custom_FullNamesToCustom_FullNames_SB);
            //////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Custom Methods FullNames Write To "Custom_stream"
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var writer_NULL = new StreamWriter(Custom_stream);
            for (var i = 0; i < Custom_FullNames.ToList().Count; i++)
            {
                writer_NULL.WriteLine(Custom_FullNames.ToList()[i]);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region All Methods FullNames Write To "All_MDS_FullName_MS"
            /////////////////////////////////////////////////////////////////////////////////
            var All_MDS_FullName_SW = new StreamWriter(All_MDS_FullName_MS);
            for (var x = 0; x < module.GetTypes().Count(); x++)
                for (var i = 0; i < module.GetTypes().ToArray()[x].Methods.Count; i++)
                    All_MDS_FullName_SW.WriteLine(module.GetTypes().ToArray()[x].Methods[i]);
            /////////////////////////////////////////////////////////////////////////////////
            #endregion

            foreach (var Def in module.GetTypes())
            {
                foreach (var All_MD in Def.Methods)
                {
                    var All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName.Add(All_MD.FullName);

                    var All_MDS_FullName_SB = new StringBuilder();
                    All_MDS_FullName_SB.AppendLine(All_MD.FullName);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region MethodA
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName_SB.ToString().Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodB
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        if (Custom_FullNames.Contains(All_MD_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodC
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName.Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodD
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (Custom_FullName == All_MD.FullName)
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodE
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        foreach (var Custom_FullName in Custom_FullNames)
                            if (All_MD_FullName == Custom_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodF
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        foreach (var All_MD_FullName in All_MDS_FullName)
                            if (Custom_FullName == All_MD_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodG
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames_SB.ToString().Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodH
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames.Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    //#region MethodI
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NuLL = Encoding.Default.GetString(Custom_stream.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NuLL.Length; i++)
                    //    if (All_MDS_FullName_MS.ToArray().Contains(Encoding.Default.GetBytes(array_NuLL[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    //#region MethodJ
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NUll = Encoding.Default.GetString(All_MDS_FullName_MS.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NUll.Length; i++)
                    //    if (Custom_stream.ToArray().Contains(Encoding.Default.GetBytes(array_NUll[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    All_MDS_FullName.Clear();
                    All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName_SB = new StringBuilder();
                    Custom_stream = new MemoryStream();
                }
            }
            return MDLIST.Distinct().ToList();
        }

        public static MethodDef FullNameToMethod(ModuleDef module, string Method_FullName)
        {
            var MDLIST = new HashSet<MethodDef>();
            var Custom_FullNames = new HashSet<string>();
            var Custom_FullNames_SB = new StringBuilder();
            var Custom_stream = new MemoryStream();
            var All_MDS_FullName_MS = new MemoryStream();

            Custom_FullNames.Add(Method_FullName);

            #region Custom Methods FullNames Write To "Custom_FullNames_SB"
            //////////////////////////////////////////////////////////////////////////////
            foreach (var Custom_FullNamesToCustom_FullNames_SB in Custom_FullNames)
                Custom_FullNames_SB.AppendLine(Custom_FullNamesToCustom_FullNames_SB);
            //////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Custom Methods FullNames Write To "Custom_stream"
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var writer_NULL = new StreamWriter(Custom_stream);
            for (var i = 0; i < Custom_FullNames.ToList().Count; i++)
            {
                writer_NULL.WriteLine(Custom_FullNames.ToList()[i]);
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region All Methods FullNames Write To "All_MDS_FullName_MS"
            /////////////////////////////////////////////////////////////////////////////////
            var All_MDS_FullName_SW = new StreamWriter(All_MDS_FullName_MS);
            for (var x = 0; x < module.GetTypes().Count(); x++)
                for (var i = 0; i < module.GetTypes().ToArray()[x].Methods.Count; i++)
                    All_MDS_FullName_SW.WriteLine(module.GetTypes().ToArray()[x].Methods[i]);
            /////////////////////////////////////////////////////////////////////////////////
            #endregion

            foreach (var Def in module.GetTypes())
            {
                foreach (var All_MD in Def.Methods)
                {
                    var All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName.Add(All_MD.FullName);

                    var All_MDS_FullName_SB = new StringBuilder();
                    All_MDS_FullName_SB.AppendLine(All_MD.FullName);
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region MethodA
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName_SB.ToString().Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodB
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        if (Custom_FullNames.Contains(All_MD_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodC
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (All_MDS_FullName.Contains(Custom_FullName))
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodD
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        if (Custom_FullName == All_MD.FullName)
                        {
                            MDLIST.Remove(All_MD);
                            MDLIST.Add(All_MD);
                        }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodE
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var All_MD_FullName in All_MDS_FullName)
                        foreach (var Custom_FullName in Custom_FullNames)
                            if (All_MD_FullName == Custom_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodF
                    //////////////////////////////////////////////////////////////////////////
                    foreach (var Custom_FullName in Custom_FullNames)
                        foreach (var All_MD_FullName in All_MDS_FullName)
                            if (Custom_FullName == All_MD_FullName)
                            {
                                MDLIST.Remove(All_MD);
                                MDLIST.Add(All_MD);
                            }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodG
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames_SB.ToString().Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region MethodH
                    //////////////////////////////////////////////////////////////////////////
                    if (Custom_FullNames.Contains(All_MD.FullName))
                    {
                        MDLIST.Remove(All_MD);
                        MDLIST.Add(All_MD);
                    }
                    //////////////////////////////////////////////////////////////////////////
                    #endregion

                    //#region MethodI
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NuLL = Encoding.Default.GetString(Custom_stream.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NuLL.Length; i++)
                    //    if (All_MDS_FullName_MS.ToArray().Contains(Encoding.Default.GetBytes(array_NuLL[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    //#region MethodJ
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //var array_NUll = Encoding.Default.GetString(All_MDS_FullName_MS.ToArray()).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    //for (var i = 0; i < array_NUll.Length; i++)
                    //    if (Custom_stream.ToArray().Contains(Encoding.Default.GetBytes(array_NUll[i])))
                    //    {
                    //        MDLIST.Remove(All_MD);
                    //        MDLIST.Add(All_MD);
                    //    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //#endregion

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    All_MDS_FullName.Clear();
                    All_MDS_FullName = new HashSet<string>();
                    All_MDS_FullName_SB = new StringBuilder();
                    Custom_stream = new MemoryStream();
                }
            }
            return MDLIST.Distinct().ToList()[0];
        }
    }
}
