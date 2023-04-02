using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using xVM.Helper.Core.RT;
using xVM.Helper.Core.Helpers.System.Xml.Linq;

namespace xVM.Helper.Core
{
    internal static class XMLUtils
    {
        public static IList<string> Methods_FullName { get; internal set; } = new List<string>();

        public static string SNKFile_Location { get; set; } = string.Empty;
        public static string SNK_Password { get; set; } = null;

        public static string RuntimeName { get; set; } = string.Empty;

        public static bool Runtime_Normal_Mode { get; set; } = false;
        public static bool Runtime_Normal_Merge_Mode { get; set; } = true;

        public static void Read(Virtualizer vr, MemoryStream xmlFile)
        {
            var thread = new Thread(() =>
            {
                if (xmlFile != null)
                {
                    var xVM_Config = XDocument.Load(new StreamReader(xmlFile)).Element("xVM");

                    #region Load SNK Location And Password
                    //////////////////////////////////////////////////////////////////////////////////
                    if (xVM_Config.Element("SNKFile").IsEmpty == false)
                        SNKFile_Location = xVM_Config.Element("SNKFile").Value;

                    if (xVM_Config.Element("SNKPassword").IsEmpty == false)
                        SNK_Password = xVM_Config.Element("SNKPassword").Value;
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Read Runtime Dll Name
                    //////////////////////////////////////////////////////////////////////////////////
                    if (xVM_Config.Element("RuntimeName").IsEmpty == false)
                        RuntimeName = xVM_Config.Element("RuntimeName").Value;
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Read Runtime Out Location
                    //////////////////////////////////////////////////////////////////////////////////
                    if (xVM_Config.Element("WriteDirectory").IsEmpty == false)
                    {
                        string location = xVM_Config.Element("WriteDirectory").Value;
                        SaveVMPRuntimeLib.WriteDirectory = location;
                        SaveNormalRuntimeLib.WriteDirectory = location;
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Read Runtime Protect Mode
                    //////////////////////////////////////////////////////////////////////////////////
                    if (xVM_Config.Element("RuntimeMode").IsEmpty == false)
                    {
                        string rt_mode = xVM_Config.Element("RuntimeMode").Value;

                        if (rt_mode == "Normal")
                        {
                            Runtime_Normal_Mode = true;
                            Runtime_Normal_Merge_Mode = false;
                        }
                        else if (rt_mode == "Merge")
                        {
                            Runtime_Normal_Mode = false;
                            Runtime_Normal_Merge_Mode = true;
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Read Virtualize Methods
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (xVM_Config.Element("Virtualize").IsEmpty == false)
                        foreach (XElement method in xVM_Config.Element("Virtualize").Element("Methods").Elements("Method"))
                            Methods_FullName.Add(method.Value);

                    #region Add Other Methods
                    ////////////////////////////////////////////////////
                    foreach (var custom_md in vr.Custom_Method_List)
                        Methods_FullName.Add(custom_md);
                    ////////////////////////////////////////////////////
                    #endregion

                    #region Delete if there are two of the same
                    ////////////////////////////////
                    Methods_FullName.Distinct();
                    ////////////////////////////////
                    #endregion

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    xmlFile.Flush();
                    xmlFile.Close();
                }
            });
            thread.Start();
            thread.Join();
        }
    }
}
