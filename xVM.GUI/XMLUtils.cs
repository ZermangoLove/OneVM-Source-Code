using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using xVM.Helper.Core.Helpers.System.Xml.Linq;

namespace xVM.GUI
{
    internal class XMLUtils
    {
        public static string Open_Project_Location = string.Empty;

        public static string Module_Location = string.Empty;

        public static IList<string> Virt_Methods_Name = new List<string>();
        public static IList<string> Mutate_Methods_Name = new List<string>();
        public static IList<string> Virt_Methods_FullName = new List<string>();
        public static IList<string> Mutate_Methods_FullName = new List<string>();
        public static IList<string> MergeNET_Assembly_List = new List<string>();

        public static string SNKFile_Location = string.Empty;
        public static string SNK_Password = string.Empty;

        public static bool AntiDebug = false;
        public static bool AntiDump = false;
        public static bool ProcessMonitor = false;
        public static bool Renamer = false;
        public static bool StringVirtualize = false;
        public static bool StringEncrypt = false;
        public static bool ResourceProt = false;
        public static bool Junk = false;
        public static bool RandomOutline = false;
        public static bool Arithmetic = false;
        public static bool IntConfusion = false;
        public static bool IntEncoding = false;
        public static bool ImportProtection = false;
        public static bool LocalToField = false;

        public static int JunkNum = 100;
        public static int RandomOutlineLength = 15;

        public static string RuntimeName = "xVM.Runtime";
        public static string WriteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string RuntimeMode = "Merge";
        public static string RandomOutlineMode = "Ascii";

        public static bool Runtime_VMP_Mode { get; set; } = false;
        public static bool Runtime_VMP_Merge_Mode { get; set; } = false;
        public static bool Runtime_Normal_Mode { get; set; } = false;
        public static bool Runtime_Normal_Merge_Mode { get; set; } = true;

        public static bool RandomOutline_Ascii_Mode { get; set; } = true;
        public static bool RandomOutline_Numbers_Mode { get; set; } = false;
        public static bool RandomOutline_Symbols_Mode { get; set; } = false;
        public static bool RandomOutline_Hex_Mode { get; set; } = false;

        public static void ResetXMLProject()
        {
            Module_Location = string.Empty;

            Virt_Methods_FullName = new List<string>();
            Mutate_Methods_FullName = new List<string>();
            MergeNET_Assembly_List = new List<string>();

            SNKFile_Location = string.Empty;
            SNK_Password = string.Empty;

            AntiDebug = false;
            AntiDump = false;
            ProcessMonitor = false;
            Renamer = false;
            StringVirtualize = false;
            StringEncrypt = false;
            ResourceProt = false;
            Junk = false;
            RandomOutline = false;
            Arithmetic = false;
            IntConfusion = false;
            IntEncoding = false;
            ImportProtection = false;
            LocalToField = false;

            JunkNum = 100;
            RandomOutlineLength = 15;

            RuntimeName = "xVM.Runtime";
            WriteDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            RuntimeMode = "Merge";
            RandomOutlineMode = "Ascii";
        }

        public static void ReadProject(MemoryStream xmlFile)
        {
            if (xmlFile != null)
            {
                var xVM_Config = XDocument.Load(new StreamReader(xmlFile)).Element("xVM");

                #region Load Module Location
                ///////////////////////////////////////////////////////////////
                if (xVM_Config.Element("Module").IsEmpty == false)
                {
                    Module_Location = xVM_Config.Element("Module").Value;
                }
                ///////////////////////////////////////////////////////////////
                #endregion

                #region Load SNK Location And Password
                //////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("SNKFile").IsEmpty == false)
                {
                    SNKFile_Location = xVM_Config.Element("SNKFile").Value;
                }

                if (xVM_Config.Element("SNKPassword").IsEmpty == false)
                {
                    SNK_Password = xVM_Config.Element("SNKPassword").Value;
                }
                //////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Protection Settings
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("AntiDebug").IsEmpty == false)
                    AntiDebug = Convert.ToBoolean(xVM_Config.Element("AntiDebug").Value);

                if (xVM_Config.Element("AntiDump").IsEmpty == false)
                    AntiDump = Convert.ToBoolean(xVM_Config.Element("AntiDump").Value);

                if (xVM_Config.Element("ProcessMonitor").IsEmpty == false)
                    ProcessMonitor = Convert.ToBoolean(xVM_Config.Element("ProcessMonitor").Value);

                if (xVM_Config.Element("Renamer").IsEmpty == false)
                    Renamer = Convert.ToBoolean(xVM_Config.Element("Renamer").Value);

                if (xVM_Config.Element("VirtualizeStrings").IsEmpty == false)
                    StringVirtualize = Convert.ToBoolean(xVM_Config.Element("VirtualizeStrings").Value);

                if (xVM_Config.Element("EncryptStrings").IsEmpty == false)
                    StringEncrypt = Convert.ToBoolean(xVM_Config.Element("EncryptStrings").Value);

                if (xVM_Config.Element("ResourceProtection").IsEmpty == false)
                    ResourceProt = Convert.ToBoolean(xVM_Config.Element("ResourceProtection").Value);

                if (xVM_Config.Element("Junk").IsEmpty == false)
                    Junk = Convert.ToBoolean(xVM_Config.Element("Junk").Value);

                if (xVM_Config.Element("RandomOutline").IsEmpty == false)
                    RandomOutline = Convert.ToBoolean(xVM_Config.Element("RandomOutline").Value);

                if (xVM_Config.Element("Arithmetic").IsEmpty == false)
                    Arithmetic = Convert.ToBoolean(xVM_Config.Element("Arithmetic").Value);

                if (xVM_Config.Element("IntConfusion").IsEmpty == false)
                    IntConfusion = Convert.ToBoolean(xVM_Config.Element("IntConfusion").Value);

                if (xVM_Config.Element("IntEncoding").IsEmpty == false)
                    IntEncoding = Convert.ToBoolean(xVM_Config.Element("IntEncoding").Value);

                if (xVM_Config.Element("ImportProtection").IsEmpty == false)
                    ImportProtection = Convert.ToBoolean(xVM_Config.Element("ImportProtection").Value);

                if (xVM_Config.Element("LocalToField").IsEmpty == false)
                    LocalToField = Convert.ToBoolean(xVM_Config.Element("LocalToField").Value);

                ////////////

                if (xVM_Config.Element("JunkNum").IsEmpty == false)
                    JunkNum = Convert.ToInt32(xVM_Config.Element("JunkNum").Value);

                if (xVM_Config.Element("RandomOutlineLength").IsEmpty == false)
                    RandomOutlineLength = Convert.ToInt32(xVM_Config.Element("RandomOutlineLength").Value);
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Runtime Dll Name
                //////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("RuntimeName").IsEmpty == false)
                {
                    RuntimeName = xVM_Config.Element("RuntimeName").Value;
                }
                //////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Runtime Out Location
                //////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("WriteDirectory").IsEmpty == false)
                {
                    WriteDirectory = xVM_Config.Element("WriteDirectory").Value;
                }
                //////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Runtime Protect Mode
                //////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("RuntimeMode").IsEmpty == false)
                {
                    RuntimeMode = xVM_Config.Element("RuntimeMode").Value;

                    if (RuntimeMode == "VMP")
                    {
                        Runtime_VMP_Mode = true;
                        Runtime_VMP_Merge_Mode = false;
                        Runtime_Normal_Mode = false;
                        Runtime_Normal_Merge_Mode = false;
                    }
                    else if (RuntimeMode == "VMP Merge")
                    {
                        Runtime_VMP_Mode = false;
                        Runtime_VMP_Merge_Mode = true;
                        Runtime_Normal_Mode = false;
                        Runtime_Normal_Merge_Mode = false;
                    }
                    else if (RuntimeMode == "Normal")
                    {
                        Runtime_VMP_Mode = false;
                        Runtime_VMP_Merge_Mode = false;
                        Runtime_Normal_Mode = true;
                        Runtime_Normal_Merge_Mode = false;
                    }
                    else if (RuntimeMode == "Normal Merge")
                    {
                        Runtime_VMP_Mode = false;
                        Runtime_VMP_Merge_Mode = false;
                        Runtime_Normal_Mode = false;
                        Runtime_Normal_Merge_Mode = true;
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read RandomOutline Mode
                //////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("RandomOutlineMode").IsEmpty == false)
                {
                    RandomOutlineMode = xVM_Config.Element("RandomOutlineMode").Value;

                    if (RandomOutlineMode == "Ascii")
                    {
                        RandomOutline_Ascii_Mode = true;
                        RandomOutline_Numbers_Mode = false;
                        RandomOutline_Symbols_Mode = false;
                        RandomOutline_Hex_Mode = false;
                    }
                    else if (RandomOutlineMode == "Numbers")
                    {
                        RandomOutline_Ascii_Mode = false;
                        RandomOutline_Numbers_Mode = true;
                        RandomOutline_Symbols_Mode = false;
                        RandomOutline_Hex_Mode = false;
                    }
                    else if (RandomOutlineMode == "Symbols")
                    {
                        RandomOutline_Ascii_Mode = false;
                        RandomOutline_Numbers_Mode = false;
                        RandomOutline_Symbols_Mode = true;
                        RandomOutline_Hex_Mode = false;
                    }
                    else if (RandomOutlineMode == "Hex")
                    {
                        RandomOutline_Ascii_Mode = false;
                        RandomOutline_Numbers_Mode = false;
                        RandomOutline_Symbols_Mode = false;
                        RandomOutline_Hex_Mode = true;
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Mutate Methods
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("Mutate").IsEmpty == false)
                    foreach (XElement method in xVM_Config.Element("Mutate").Element("Methods").Elements("Method"))
                    {
                        Mutate_Methods_FullName.Add(method.Value);
                    }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Virtualize Methods
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("Virtualize").IsEmpty == false)
                    foreach (XElement method in xVM_Config.Element("Virtualize").Element("Methods").Elements("Method"))
                    {
                        Virt_Methods_FullName.Add(method.Value);
                    }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Read Merge .NET Assemblies
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (xVM_Config.Element("MergeNET").IsEmpty == false)
                    foreach (XElement method in xVM_Config.Element("MergeNET").Element("Locations").Elements("Location"))
                    {
                        MergeNET_Assembly_List.Add(method.Value);
                    }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
        }

        public static bool SaveProject(MemoryStream oneVMConfig)
        {
            using (XmlWriter writer = XmlWriter.Create(oneVMConfig, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                CloseOutput = true,
                OmitXmlDeclaration = true
            }))
            {
                writer.WriteStartElement("xVM");

                writer.WriteElementString("Module", Module_Location);

                writer.WriteElementString("SNKFile", SNKFile_Location);
                writer.WriteElementString("SNKPassword", SNK_Password);

                writer.WriteElementString("AntiDebug", AntiDebug.ToString());
                writer.WriteElementString("AntiDump", AntiDump.ToString());
                writer.WriteElementString("ProcessMonitor", ProcessMonitor.ToString());
                writer.WriteElementString("Renamer", Renamer.ToString());
                writer.WriteElementString("VirtualizeStrings", StringVirtualize.ToString());
                writer.WriteElementString("EncryptStrings", StringEncrypt.ToString());
                writer.WriteElementString("ResourceProtection", ResourceProt.ToString());
                writer.WriteElementString("Junk", Junk.ToString());
                writer.WriteElementString("RandomOutline", RandomOutline.ToString());
                writer.WriteElementString("Arithmetic", Arithmetic.ToString());
                writer.WriteElementString("IntConfusion", IntConfusion.ToString());
                writer.WriteElementString("IntEncoding", IntEncoding.ToString());
                writer.WriteElementString("ImportProtection", ImportProtection.ToString());
                writer.WriteElementString("LocalToField", LocalToField.ToString());

                writer.WriteElementString("JunkNum", JunkNum.ToString());
                writer.WriteElementString("RandomOutlineLength", RandomOutlineLength.ToString());

                writer.WriteElementString("RuntimeName", RuntimeName);
                writer.WriteElementString("WriteDirectory", WriteDirectory);
                writer.WriteElementString("RuntimeMode", RuntimeMode);
                writer.WriteElementString("RandomOutlineMode", RandomOutlineMode);

                writer.WriteStartElement("Virtualize");
                writer.WriteStartElement("Methods");
                if (Virt_Methods_FullName != null)
                    foreach (var md in Virt_Methods_FullName.Distinct())
                        writer.WriteElementString("Method", md);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("Mutate");
                writer.WriteStartElement("Methods");
                if (Mutate_Methods_FullName != null)
                    foreach (var md in Mutate_Methods_FullName.Distinct())
                        writer.WriteElementString("Method", md);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("MergeNET");
                writer.WriteStartElement("Locations");
                if (MergeNET_Assembly_List != null)
                    foreach (var md in MergeNET_Assembly_List.Distinct())
                        writer.WriteElementString("Location", md);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
            return true;
        }
    }
}
