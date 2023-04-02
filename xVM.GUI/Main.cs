using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

using xVM.Helper.Internal;
using xVM.Helper.Core.Helpers;
using xVM.Helper.Core.Protections;
using xVM.Helper.Core.Protections.Mutation;
using xVM.Helper.Core.Protections.SugarControlFlow;
using Arithmetic_Obfuscation.Arithmetic;
using System.Text.RegularExpressions;

namespace xVM.GUI
{
    public partial class Main : Form
    {
        public static ModuleDefMD moduleDefMD;

        private MemoryStream Proj;
        private List<string> tempMethodsList = new List<string>();
        private readonly string OUTDirName = "xVM_Protected";
        //private HashUpdater HashUpdaterTool = new HashUpdater();
        public Process Process { get; set; }

        public object lb_item = null;

        public Main()
        {
            InitializeComponent();
        }

        #region Asm File Load And Search Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BrowseAsmButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog AsmOFD = new OpenFileDialog();
            AsmOFD.FileName = string.Empty;
            AsmOFD.Title = "Choose Assembly To Protect";
            AsmOFD.Filter = "Assembly File(*.exe, *.dll)|*.exe;*.dll|All files(*.*)|*.*";
            AsmOFD.CheckFileExists = true;

            if (AsmOFD.ShowDialog() == DialogResult.OK)
            {
                string fileLocation = AsmOFD.FileName;
                if (fileLocation.Length > 0 && Utils.IsDotNetAssembly(fileLocation))
                {
                    moduleDefMD = null;
                    moduleDefMD = ModuleDefMD.Load(fileLocation);

                    ResetProject();

                    MethodTree.Enabled = true;

                    XMLUtils.Module_Location = fileLocation;
                    AsmTextBox.Text = XMLUtils.Module_Location;
                    FileInfo fileInfo = new FileInfo(XMLUtils.Module_Location);
                    OutDestination.Text = fileInfo.DirectoryName + "\\" + OUTDirName + "\\" + fileInfo.Name;
                    AddMethodsToCheckListBox();
                }
                else
                {
                    MessageBox.Show("Your file is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Asm_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                string[] array = data as string[];
                if (array.Length == 1)
                {
                    string fileLocation = array[0];
                    if (Utils.IsDotNetAssembly(fileLocation))
                    {
                        moduleDefMD = null;
                        moduleDefMD = ModuleDefMD.Load(fileLocation);

                        ResetProject();

                        MethodTree.Enabled = true;

                        XMLUtils.Module_Location = fileLocation;
                        AsmTextBox.Text = XMLUtils.Module_Location;
                        FileInfo fileInfo = new FileInfo(XMLUtils.Module_Location);
                        OutDestination.Text = fileInfo.DirectoryName + "\\" + OUTDirName + "\\" + fileInfo.Name;
                        AddMethodsToCheckListBox();
                    }
                    else
                    {
                        MessageBox.Show("Your file is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region SNK File Load
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ActiveSNK_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveSNK_CheckBox.Checked == true)
            {
                SNKBox.Enabled = true;
                SNK_PasswordBox.Enabled = true;
                BrowseSNKButton.Enabled = true;
            }
            else
            {
                SNKBox.Enabled = false;
                SNK_PasswordBox.Enabled = false;
                BrowseSNKButton.Enabled = false;
            }
        }

        private void BrowseSNKButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog SNK_OFD = new OpenFileDialog();
            SNK_OFD.FileName = string.Empty;
            SNK_OFD.Title = "Choose Strong Name Key (SNK)";
            SNK_OFD.Filter = "Strong Name Key File(*.pfx, *.snk)|*.pfx;*.snk";
            SNK_OFD.CheckFileExists = true;

            if (SNK_OFD.ShowDialog() == DialogResult.OK)
            {
                if (SNK_OFD.FileName.Length > 0)
                {
                    XMLUtils.SNKFile_Location = SNK_OFD.FileName;
                    SNKBox.Text = XMLUtils.SNKFile_Location;
                }
            }
        }

        private void SNK_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                string[] array = data as string[];
                if (array.Length == 1)
                {
                    if (Path.GetExtension(array[0]).ToLower() == ".snk" || Path.GetExtension(array[0]).ToLower() == ".pfx")
                    {
                        XMLUtils.SNKFile_Location = array[0];
                        SNKBox.Text = XMLUtils.SNKFile_Location;
                    }
                    else
                        MessageBox.Show("Just \".snk and .pfx\" File!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region MethodTree Configure
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Type Icon
        private void MethodTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 0;
            e.Node.SelectedImageIndex = 0;
        }

        // Opened Type Icon
        private void MethodTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
        }

        // Fix Right Click
        private void MethodTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            MethodTree.SelectedNode = e.Node;
        }

        // Fix Right Click 2
        private void MethodTree_MouseClick(object sender, MouseEventArgs e)
        {
            MethodTree.SelectedNode = MethodTree.GetNodeAt(e.X, e.Y);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region MethodTree Context Settings
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string MethodNameToMethodFullName(ModuleDefMD moduleDef, string methodName)
        {
            string result = "";
            foreach (var typeDef in moduleDef.GetTypes())
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    Regex regex = new Regex(@"\(([^)]*)\)");
                    if (regex.Match(methodName).Groups[1].Value == string.Format("0x{0:X4}", methodDef.MDToken))
                    {
                        result = methodDef.FullName;
                    }    
                }    
            }
            return result;
        }

        public static string MethodFullNameToMethodName(ModuleDefMD moduleDef, string methodFullName)
        {
            string result = "";
            foreach (var typeDef in moduleDef.GetTypes())
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    if (methodDef.FullName == methodFullName)
                    {
                        result = methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")";
                    }    
                }    
            }
            return result;
        }

        public static IList<string> MethodFullNameToMethodName(ModuleDefMD moduleDef, IList<string> methodFullName)
        {
            IList<string> result = new List<string>();
            foreach (var typeDef in moduleDef.GetTypes())
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    foreach (var control in methodFullName)
                    {
                        if (control == methodDef.FullName)
                        {
                            result.Add(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");
                        }    
                    }    
                }    
            }
            return result;
        }

        private void mutateMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Cancel All Context Mode
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (MethodTree.SelectedNode.ImageIndex == 7 || MethodTree.SelectedNode.ImageIndex == 9)  // Detect Mutate Selected Method
                try
                {
                    if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        foreach (var mutate in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                        foreach (var mutate in XMLUtils.Mutate_Methods_Name)
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                    }
                }
                catch { }

            if (MethodTree.SelectedNode.ImageIndex == 8 || MethodTree.SelectedNode.ImageIndex == 9) // Detect Virt Selected Method
                try
                {
                    foreach (var virt in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                    foreach (var virt in XMLUtils.Virt_Methods_Name)
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                }
                catch { }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Virt Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Virt_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("Mutate Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Virt_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Virt_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("Mutate Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
            {
                XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                XMLUtils.Mutate_Methods_Name.Add(MethodTree.SelectedNode.Text);
            }

            // Control Method After Change Icon
            foreach (var mutate_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                if (mutate_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 7;
                    MethodTree.SelectedNode.SelectedImageIndex = 7;
                }
            foreach (var mutate_md in XMLUtils.Mutate_Methods_Name)
                if (mutate_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 7;
                    MethodTree.SelectedNode.SelectedImageIndex = 7;
                }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void virtualizeMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Cancel All Context Mode
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (MethodTree.SelectedNode.ImageIndex == 7 || MethodTree.SelectedNode.ImageIndex == 9)  // Detect Mutate Selected Method
                try
                {
                    if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        foreach (var mutate in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                        foreach (var mutate in XMLUtils.Mutate_Methods_Name)
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                    }
                }
                catch { }

            if (MethodTree.SelectedNode.ImageIndex == 8 || MethodTree.SelectedNode.ImageIndex == 9) // Detect Virt Selected Method
                try
                {
                    foreach (var virt in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                    foreach (var virt in XMLUtils.Virt_Methods_Name)
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                }
                catch { }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Mutate Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Mutate_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("Virtualize Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Mutate_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Mutate_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("Virtualize Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
            {
                XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                XMLUtils.Virt_Methods_Name.Add(MethodTree.SelectedNode.Text);
            }

            // Control Method After Change Icon
            foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 8;
                    MethodTree.SelectedNode.SelectedImageIndex = 8;
                }
            foreach (var virt_md in XMLUtils.Virt_Methods_Name)
                if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 8;
                    MethodTree.SelectedNode.SelectedImageIndex = 8;
                }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void ultraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Cancel All Context Mode
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (MethodTree.SelectedNode.ImageIndex == 7 || MethodTree.SelectedNode.ImageIndex == 9)  // Detect Mutate Selected Method
                try
                {
                    if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        foreach (var mutate in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                        foreach (var mutate in XMLUtils.Mutate_Methods_Name)
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                    }
                }
                catch { }

            if (MethodTree.SelectedNode.ImageIndex == 8 || MethodTree.SelectedNode.ImageIndex == 9) // Detect Virt Selected Method
                try
                {
                    foreach (var virt in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                    foreach (var virt in XMLUtils.Virt_Methods_Name)
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                }
                catch { }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Mutate Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Mutate_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Mutate_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Mutate_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Virt Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Virt_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Virt_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Virt_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Mutate_Methods_FullName Add
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
            {
                XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                XMLUtils.Mutate_Methods_Name.Add(MethodTree.SelectedNode.Text);
            }

            // Control Method After Change Icon
            foreach (var mutate_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                if (mutate_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 7;
                    MethodTree.SelectedNode.SelectedImageIndex = 7;
                }
            foreach (var mutate_md in XMLUtils.Mutate_Methods_Name)
                if (mutate_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 7;
                    MethodTree.SelectedNode.SelectedImageIndex = 7;
                }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Virt_Methods_FullName Add
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
            {
                XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                XMLUtils.Virt_Methods_Name.Add(MethodTree.SelectedNode.Text);
            }

            // Control Method After Change Icon
            foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 8;
                    MethodTree.SelectedNode.SelectedImageIndex = 8;
                }
            foreach (var virt_md in XMLUtils.Virt_Methods_Name)
                if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                {
                    MethodTree.SelectedNode.ImageIndex = 8;
                    MethodTree.SelectedNode.SelectedImageIndex = 8;
                }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            MethodTree.SelectedNode.ImageIndex = 9;
            MethodTree.SelectedNode.SelectedImageIndex = 9;
        }

        private void UnSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Cancel All Context Mode
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (MethodTree.SelectedNode.ImageIndex == 7 || MethodTree.SelectedNode.ImageIndex == 9)  // Detect Mutate Selected Method
                try
                {
                    if (Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        foreach (var mutate in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                        foreach (var mutate in XMLUtils.Mutate_Methods_Name)
                            if (mutate == MethodTree.SelectedNode.Text)
                                XMLUtils.Mutate_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                    }
                }
                catch { }

            if (MethodTree.SelectedNode.ImageIndex == 8 || MethodTree.SelectedNode.ImageIndex == 9) // Detect Virt Selected Method
                try
                {
                    foreach (var virt in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_FullName.Remove(MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text));
                    foreach (var virt in XMLUtils.Virt_Methods_Name)
                        if (virt == MethodTree.SelectedNode.Text)
                            XMLUtils.Virt_Methods_Name.Remove(MethodTree.SelectedNode.Text);
                }
                catch { }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Virt Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Virt_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Virt_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Virt_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Virt_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            #region Control Mutate Method
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (XMLUtils.Mutate_Methods_FullName.Count != 0)
                foreach (var virt_md in MethodFullNameToMethodName(moduleDefMD, XMLUtils.Mutate_Methods_FullName))
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            if (XMLUtils.Mutate_Methods_Name.Count != 0)
                foreach (var virt_md in XMLUtils.Mutate_Methods_Name)
                    if (virt_md.Contains(MethodTree.SelectedNode.Text) && Convert.ToInt32(MethodTree.SelectedNode.Tag) == 2)
                    {
                        MessageBox.Show("UnSelect Context Failed!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            if (ShowHiddenTypesAndMethods_CheckBox.Checked == true)
            {
                var module0 = ModuleDefMD.Load(XMLUtils.Module_Location);
                foreach (TypeDef type in module0.GetTypes())
                {
                    foreach (MethodDef methodDef in type.Methods)
                        if (methodDef != module0.GlobalType.FindOrCreateStaticConstructor())
                            if (Utils.OVMAnalyzer(methodDef) && methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")" == MethodTree.SelectedNode.Text)
                            {
                                if (methodDef.IsPublic && methodDef.IsConstructor)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 2;
                                    MethodTree.SelectedNode.SelectedImageIndex = 2;
                                }
                                else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 3;
                                    MethodTree.SelectedNode.SelectedImageIndex = 3;
                                }
                                else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 4;
                                    MethodTree.SelectedNode.SelectedImageIndex = 4;
                                }
                                else if (methodDef.IsFamily && methodDef.IsConstructor)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 5;
                                    MethodTree.SelectedNode.SelectedImageIndex = 5;
                                }
                                else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 6;
                                    MethodTree.SelectedNode.SelectedImageIndex = 6;
                                }
                                else if (methodDef.IsPublic)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 2;
                                    MethodTree.SelectedNode.SelectedImageIndex = 2;
                                }
                                else if (methodDef.IsPrivate)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 3;
                                    MethodTree.SelectedNode.SelectedImageIndex = 3;
                                }
                                else if (methodDef.IsAssembly)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 4;
                                    MethodTree.SelectedNode.SelectedImageIndex = 4;
                                }
                                else if (methodDef.IsFamily)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 5;
                                    MethodTree.SelectedNode.SelectedImageIndex = 5;
                                }
                                else if (methodDef.IsFamilyOrAssembly)
                                {
                                    MethodTree.SelectedNode.ImageIndex = 6;
                                    MethodTree.SelectedNode.SelectedImageIndex = 6;
                                }
                            }
                }
                return;
            }    

            var module = ModuleDefMD.Load(XMLUtils.Module_Location);
            foreach (TypeDef type in module.Types)
            {
                foreach (MethodDef methodDef in type.Methods)
                    if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                        if (Utils.OVMAnalyzer(methodDef) && methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")" == MethodTree.SelectedNode.Text)
                        {
                            if (methodDef.IsPublic && methodDef.IsConstructor)
                            {
                                MethodTree.SelectedNode.ImageIndex = 2;
                                MethodTree.SelectedNode.SelectedImageIndex = 2;
                            }
                            else if (methodDef.IsPrivate && methodDef.IsConstructor)
                            {
                                MethodTree.SelectedNode.ImageIndex = 3;
                                MethodTree.SelectedNode.SelectedImageIndex = 3;
                            }
                            else if (methodDef.IsAssembly && methodDef.IsConstructor)
                            {
                                MethodTree.SelectedNode.ImageIndex = 4;
                                MethodTree.SelectedNode.SelectedImageIndex = 4;
                            }
                            else if (methodDef.IsFamily && methodDef.IsConstructor)
                            {
                                MethodTree.SelectedNode.ImageIndex = 5;
                                MethodTree.SelectedNode.SelectedImageIndex = 5;
                            }
                            else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                            {
                                MethodTree.SelectedNode.ImageIndex = 6;
                                MethodTree.SelectedNode.SelectedImageIndex = 6;
                            }
                            else if (methodDef.IsPublic)
                            {
                                MethodTree.SelectedNode.ImageIndex = 2;
                                MethodTree.SelectedNode.SelectedImageIndex = 2;
                            }
                            else if (methodDef.IsPrivate)
                            {
                                MethodTree.SelectedNode.ImageIndex = 3;
                                MethodTree.SelectedNode.SelectedImageIndex = 3;
                            }
                            else if (methodDef.IsAssembly)
                            {
                                MethodTree.SelectedNode.ImageIndex = 4;
                                MethodTree.SelectedNode.SelectedImageIndex = 4;
                            }
                            else if (methodDef.IsFamily)
                            {
                                MethodTree.SelectedNode.ImageIndex = 5;
                                MethodTree.SelectedNode.SelectedImageIndex = 5;
                            }
                            else if (methodDef.IsFamilyOrAssembly)
                            {
                                MethodTree.SelectedNode.ImageIndex = 6;
                                MethodTree.SelectedNode.SelectedImageIndex = 6;
                            }
                        }
            }
        }

        private void methodAboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MethodAbout(ModuleDefMD.Load(XMLUtils.Module_Location), MethodNameToMethodFullName(moduleDefMD, MethodTree.SelectedNode.Text)).Show();
        }

        // Without Type Class Name Click
        private void MethodTree_ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // Şimdilik virt aktif
            //mutateMethodToolStripMenuItem.Enabled = false;
            //ultraToolStripMenuItem.Enabled = false;
            ////////////////////////////////////////////////////////////

            if (Convert.ToInt16(MethodTree.SelectedNode.Tag) == 0)
                MethodTree_ContextMenu.Enabled = false;
            else if (Convert.ToInt16(MethodTree.SelectedNode.Tag) == 1)
                MethodTree_ContextMenu.Enabled = false;
            else if (Convert.ToInt16(MethodTree.SelectedNode.Tag) == 2)
            {
                MethodTree_ContextMenu.Enabled = true;

                #region Method Mutate Olarak Seçilmişse "Mutate Method" Contextini Deaktif Et
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                if (MethodTree.SelectedNode.ImageIndex == 7 && MethodTree.SelectedNode.SelectedImageIndex == 7)
                    mutateMethodToolStripMenuItem.Enabled = false;
                else
                    mutateMethodToolStripMenuItem.Enabled = true;
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Method Virtualize Olarak Seçilmişse "Virtualize Method" Contextini Deaktif Et
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                if (MethodTree.SelectedNode.ImageIndex == 8 && MethodTree.SelectedNode.SelectedImageIndex == 8)
                    virtualizeMethodToolStripMenuItem.Enabled = false;
                else
                    virtualizeMethodToolStripMenuItem.Enabled = true;
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region Method Ultra Olarak Seçilmişse "Ultra" Contextini Deaktif Et
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                if (MethodTree.SelectedNode.ImageIndex == 9 && MethodTree.SelectedNode.SelectedImageIndex == 9)
                    ultraToolStripMenuItem.Enabled = false;
                else
                    ultraToolStripMenuItem.Enabled = true;
                ////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                #region UnSelect Contexti Methodda "Mutation, Virt, Ultra" Olarak İşaretlenmediyse Deaktif Et
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (MethodTree.SelectedNode.ImageIndex != 2 && MethodTree.SelectedNode.ImageIndex != 3 &&
                    MethodTree.SelectedNode.ImageIndex != 4 && MethodTree.SelectedNode.ImageIndex != 5 &&
                    MethodTree.SelectedNode.ImageIndex != 6)
                    UnSelectToolStripMenuItem.Enabled = true;
                else
                    UnSelectToolStripMenuItem.Enabled = false;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Merge .NET Assembly Page Settings
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void MergeNET_AddLibrary_Button_Click(object sender, EventArgs e)
        {
            Stream myStream;

            OpenFileDialog AsmDLL = new OpenFileDialog();
            AsmDLL.FileName = string.Empty;
            AsmDLL.Title = "Choose Assembly To Merge";
            AsmDLL.Filter = "Assembly File(s) (*.dll)|*.dll";
            AsmDLL.CheckFileExists = true;
            AsmDLL.Multiselect = true;

            if (AsmDLL.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileLocation in AsmDLL.FileNames)
                {
                    try
                    {
                        if ((myStream = AsmDLL.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                if (fileLocation.Length > 0 && Utils.IsDotNetAssembly(fileLocation))
                                {
                                    if (!NETAssembly_ListBox.Items.Contains(fileLocation))
                                        NETAssembly_ListBox.Items.Add(fileLocation);
                                    else
                                        MessageBox.Show("The library is already attached!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                    MessageBox.Show("Your library is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch { }
                }
            }

            // Stop focus
            NETAssembly_ListBox.Focus();
        }

        private void MergeNET_SelecetAndRemove_Button_Click(object sender, EventArgs e)
        {
            for (int x = NETAssembly_ListBox.SelectedIndices.Count - 1; x >= 0; x--)
            {
                int var = NETAssembly_ListBox.SelectedIndices[x];
                NETAssembly_ListBox.Items.RemoveAt(var);

            }

            // Stop focus
            NETAssembly_ListBox.Focus();
        }

        private void NETAssembly_ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                for (int x = NETAssembly_ListBox.Items.Count - 1; x >= 0; x--)
                {
                    NETAssembly_ListBox.SelectedItems.Add(NETAssembly_ListBox.Items[x]);
                }
            }
        }

        private void NETAssembly_ListBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] array = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var fileLocation in array)
            {
                if (Utils.IsDotNetAssembly(fileLocation))
                {
                    if (!NETAssembly_ListBox.Items.Contains(fileLocation))
                        NETAssembly_ListBox.Items.Add(fileLocation);
                    else
                        MessageBox.Show("The library is already attached!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Your library is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void _DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void _Load(object sender, EventArgs e)
        {
            SelectAllFun_Label.Visible = false;
            SelectAllFun_ComboBox.Visible = false;
            ShowHiddenTypesAndMethods_CheckBox.Visible = false;
            RandomOutline_ComboBox.Enabled = false;
            ResetProject();
        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModeComboBox.Text == "Normal Runtime Protect + Merge")
            {
                RuntimeNameBox.Enabled = false;
                RandomRuntimeName_Button.Enabled = false;
            }
            else
            {
                RuntimeNameBox.Enabled = true;
                RandomRuntimeName_Button.Enabled = true;
            }
        }

        private void AddMethodsToCheckListBox()
        {
            MethodTree.Nodes.Clear();
            tempMethodsList.Clear();

            var module = ModuleDefMD.Load(XMLUtils.Module_Location);
            var namespaces = new HashSet<string>();

            foreach (TypeDef type in module.GetTypes())
            {
                namespaces.Add(type.Namespace);
            }
            namespaces.Distinct();

            #region Empty so only print non-namespaces as classes
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            foreach (TypeDef type in module.Types)
            {
                if (type.Namespace == string.Empty && !type.Name.Contains("ImplementationDetails>") &&
                    !type.Name.Contains("<>f__AnonymousType"))
                {
                    var node = new TreeNode(type.Name, 0, 0);
                    node.Tag = 1;

                    foreach (MethodDef methodDef in type.Methods)
                        if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                            if (Utils.OVMAnalyzer(methodDef))
                            {
                                var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                if (methodDef.IsPublic && methodDef.IsConstructor)
                                {
                                    nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                    nod2.ImageIndex = 2;
                                    nod2.SelectedImageIndex = 2;
                                }
                                else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                {
                                    nod2.ForeColor = Color.FromArgb(0x45B29C);
                                    nod2.ImageIndex = 3;
                                    nod2.SelectedImageIndex = 3;
                                }
                                else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                {
                                    nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                    nod2.ImageIndex = 4;
                                    nod2.SelectedImageIndex = 4;
                                }
                                else if (methodDef.IsFamily && methodDef.IsConstructor)
                                {
                                    nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                    nod2.ImageIndex = 5;
                                    nod2.SelectedImageIndex = 5;
                                }
                                else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                {
                                    nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                    nod2.ImageIndex = 6;
                                    nod2.SelectedImageIndex = 6;
                                }
                                else if (methodDef.IsPublic)
                                {
                                    nod2.ForeColor = Color.FromArgb(0xFF8000);
                                    nod2.ImageIndex = 2;
                                    nod2.SelectedImageIndex = 2;
                                }
                                else if (methodDef.IsPrivate)
                                {
                                    nod2.ForeColor = Color.FromArgb(0xCC6600);
                                    nod2.ImageIndex = 3;
                                    nod2.SelectedImageIndex = 3;
                                }
                                else if (methodDef.IsAssembly)
                                {
                                    nod2.ForeColor = Color.FromArgb(0xFF8000);
                                    nod2.ImageIndex = 4;
                                    nod2.SelectedImageIndex = 4;
                                }
                                else if (methodDef.IsFamily)
                                {
                                    nod2.ForeColor = Color.FromArgb(0xFF8000);
                                    nod2.ImageIndex = 5;
                                    nod2.SelectedImageIndex = 5;
                                }
                                else if (methodDef.IsFamilyOrAssembly)
                                {
                                    nod2.ForeColor = Color.FromArgb(0xFF8000);
                                    nod2.ImageIndex = 6;
                                    nod2.SelectedImageIndex = 6;
                                }

                                nod2.Tag = 2;
                                node.Nodes.Add(nod2);

                                tempMethodsList.Add(methodDef.FullName);
                            }
                    MethodTree.Nodes.Add(node);
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            TreeNode node_Namespace = null;
            foreach (string nspace in namespaces)
            {
                if (nspace != string.Empty)
                {
                    node_Namespace = new TreeNode(nspace, 0, 0);
                    node_Namespace.Tag = 0;
                    MethodTree.Nodes.Add(node_Namespace);

                    foreach (TypeDef type in module.Types)
                    {
                        if (node_Namespace.Text == type.Namespace && type.Namespace != string.Empty && !type.IsValueType && !type.IsInterface)
                        {
                            string type_Name = type.Name.Contains("`") ? type.Name.Substring(0, type.Name.IndexOf('`')) : type.Name.Replace("`", string.Empty);
                            var node = new TreeNode(type_Name, 0, 0);
                            node.Tag = 1;

                            foreach (MethodDef methodDef in type.Methods)
                                if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                                    if (Utils.OVMAnalyzer(methodDef))
                                    {
                                        var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                        if (methodDef.IsPublic && methodDef.IsConstructor)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                            nod2.ImageIndex = 2;
                                            nod2.SelectedImageIndex = 2;
                                        }
                                        else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0x45B29C);
                                            nod2.ImageIndex = 3;
                                            nod2.SelectedImageIndex = 3;
                                        }
                                        else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                            nod2.ImageIndex = 4;
                                            nod2.SelectedImageIndex = 4;
                                        }
                                        else if (methodDef.IsFamily && methodDef.IsConstructor)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                            nod2.ImageIndex = 5;
                                            nod2.SelectedImageIndex = 5;
                                        }
                                        else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                            nod2.ImageIndex = 6;
                                            nod2.SelectedImageIndex = 6;
                                        }
                                        else if (methodDef.IsPublic)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0xFF8000);
                                            nod2.ImageIndex = 2;
                                            nod2.SelectedImageIndex = 2;
                                        }
                                        else if (methodDef.IsPrivate)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0xCC6600);
                                            nod2.ImageIndex = 3;
                                            nod2.SelectedImageIndex = 3;
                                        }
                                        else if (methodDef.IsAssembly)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0xFF8000);
                                            nod2.ImageIndex = 4;
                                            nod2.SelectedImageIndex = 4;
                                        }
                                        else if (methodDef.IsFamily)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0xFF8000);
                                            nod2.ImageIndex = 5;
                                            nod2.SelectedImageIndex = 5;
                                        }
                                        else if (methodDef.IsFamilyOrAssembly)
                                        {
                                            nod2.ForeColor = Color.FromArgb(0xFF8000);
                                            nod2.ImageIndex = 6;
                                            nod2.SelectedImageIndex = 6;
                                        }

                                        nod2.Tag = 2;
                                        node.Nodes.Add(nod2);

                                        tempMethodsList.Add(methodDef.FullName);
                                    }
                            node_Namespace.Nodes.Add(node);
                        }
                    }
                }
            }

            try
            {
                // Deleting Empty Classes
                TreeNode node = null;
                int nro = 0;
                while (nro < MethodTree.Nodes.Count)
                {
                    node = MethodTree.Nodes[nro];
                    if (node.Nodes.Count == 0)
                        MethodTree.Nodes.Remove(node);
                    else
                        nro++;
                }
                ///////////////////////////////////////////////////////
            }
            catch { }

            MethodTree.Sort();
        }

        private void ResetProject()
        {
            XMLUtils.ResetXMLProject();

            XMLUtils.Module_Location = string.Empty;

            XMLUtils.Mutate_Methods_Name = new List<string>();
            XMLUtils.Virt_Methods_Name = new List<string>();
            XMLUtils.Virt_Methods_FullName = new List<string>();
            XMLUtils.Mutate_Methods_FullName = new List<string>();

            ActiveSNK_CheckBox.Checked = false;

            antiDebug_CheckBox.Checked = false;
            antiDump_CheckBox.Checked = false;
            ProcessMonitor_CheckBox.Checked = false;
            renamer_CheckBox.Checked = false;
            VirtualizeAllStrings_CheckBox.Checked = false;
            EncryptAllStrings_CheckBox.Checked = false;
            resourceProt_CheckBox.Checked = false;
            Junk_CheckBox.Checked = false;
            RandomOutline_CheckBox.Checked = false;
            Arithmetic_CheckBox.Checked = false;
            IntConfusion_CheckBox.Checked = false;
            IntEncoding_CheckBox.Checked = false;
            ImportProtection_CheckBox.Checked = false;
            LocalToField_CheckBox.Checked = false;

            SNKBox.Enabled = false;
            SNK_PasswordBox.Enabled = false;
            BrowseSNKButton.Enabled = false;

            MethodTree.Enabled = false;

            AsmTextBox.Clear();
            OutDestination.Clear();
            SNKBox.Clear();
            SNK_PasswordBox.Clear();
            MethodTree.Nodes.Clear();
            tempMethodsList.Clear();
            Log_RichTextBox.Clear();
            NETAssembly_ListBox.Items.Clear();

            JunkNumber.Value = JunkNumber.Minimum; // 100
            RandomOutlineLength.Value = RandomOutlineLength.Minimum; // 15

            SelectAllFun_ComboBox.Text = SelectAllFun_ComboBox.Items[0].ToString();
            ModeComboBox.Text = ModeComboBox.Items[0].ToString();
            RandomOutline_ComboBox.Text = RandomOutline_ComboBox.Items[0].ToString();
            RuntimeNameBox.Text = "xVM.Runtime";
        }

        private void UpdateXMlConfig()
        {
            if (AsmTextBox.Text != string.Empty)
            {
                XMLUtils.Module_Location = AsmTextBox.Text;
                XMLUtils.WriteDirectory = Path.GetDirectoryName(OutDestination.Text);
            }

            if (ActiveSNK_CheckBox.Checked == true)
            {
                XMLUtils.SNKFile_Location = SNKBox.Text;
                XMLUtils.SNK_Password = SNK_PasswordBox.Text;

                if (ModeComboBox.Text == "Normal Runtime Protect")
                    XMLUtils.RuntimeMode = "Normal";
                else if (ModeComboBox.Text == "Normal Runtime Protect + Merge")
                    XMLUtils.RuntimeMode = "Merge";
            }
            else
            {
                if (ModeComboBox.Text == "Normal Runtime Protect")
                    XMLUtils.RuntimeMode = "Normal";
                else if (ModeComboBox.Text == "Normal Runtime Protect + Merge")
                    XMLUtils.RuntimeMode = "Merge";
            }

            if (RandomOutline_CheckBox.Checked == true)
            {
                if (RandomOutline_ComboBox.Text == "Ascii")
                    XMLUtils.RandomOutlineMode = "Ascii";
                else if (RandomOutline_ComboBox.Text == "Numbers")
                    XMLUtils.RandomOutlineMode = "Numbers";
                else if (RandomOutline_ComboBox.Text == "Symbols")
                    XMLUtils.RandomOutlineMode = "Symbols";
                else if (RandomOutline_ComboBox.Text == "Hex")
                    XMLUtils.RandomOutlineMode = "Hex";
            }

            if (NETAssembly_ListBox.Items.Count > 0)
            {
                foreach (string location in NETAssembly_ListBox.Items)
                {
                    XMLUtils.MergeNET_Assembly_List.Add(location);
                }
            }

            XMLUtils.RuntimeName = RuntimeNameBox.Text;

            XMLUtils.AntiDebug = antiDebug_CheckBox.Checked;
            XMLUtils.AntiDump = antiDump_CheckBox.Checked;
            XMLUtils.ProcessMonitor = ProcessMonitor_CheckBox.Checked;
            XMLUtils.Renamer = renamer_CheckBox.Checked;
            XMLUtils.StringVirtualize = VirtualizeAllStrings_CheckBox.Checked;
            XMLUtils.StringEncrypt = EncryptAllStrings_CheckBox.Checked;
            XMLUtils.ResourceProt = resourceProt_CheckBox.Checked;
            XMLUtils.Junk = Junk_CheckBox.Checked;
            XMLUtils.RandomOutline = RandomOutline_CheckBox.Checked;
            XMLUtils.Arithmetic = Arithmetic_CheckBox.Checked;
            XMLUtils.IntConfusion = IntConfusion_CheckBox.Checked;
            XMLUtils.IntEncoding = IntEncoding_CheckBox.Checked;
            XMLUtils.ImportProtection = ImportProtection_CheckBox.Checked;
            XMLUtils.LocalToField = LocalToField_CheckBox.Checked;

            XMLUtils.JunkNum = (int)JunkNumber.Value;
            XMLUtils.RandomOutlineLength = (int)RandomOutlineLength.Value;

            // virt all md (for testing)
            //XMLUtils.Virt_Methods_FullName = tempMethodsList;
            ///////////////////////////////////////////////////

            var stream = new MemoryStream();
            XMLUtils.SaveProject(stream);
            stream.Flush();
            var ProjBytes = stream.ToArray();
            stream.Close();

            Proj = new MemoryStream(ProjBytes);
        }

        private bool UpdatexVMConfig(string projLocation)
        {
            XMLUtils.ReadProject(new MemoryStream(File.ReadAllBytes(projLocation)));

            if (File.Exists(XMLUtils.Module_Location) && XMLUtils.Module_Location != string.Empty)
            {
                AsmTextBox.Text = XMLUtils.Module_Location;
                OutDestination.Text = OutDestination.Text = Path.GetDirectoryName(XMLUtils.Module_Location) + "\\" + OUTDirName + "\\" + Path.GetFileName(XMLUtils.Module_Location);
            }
            else
                return false;

            if (XMLUtils.SNKFile_Location == string.Empty)
                ActiveSNK_CheckBox.Checked = false;
            else
            {
                ActiveSNK_CheckBox.Checked = true;

                SNKBox.Text = XMLUtils.SNKFile_Location;
                SNK_PasswordBox.Text = XMLUtils.SNK_Password;
            }

            if (XMLUtils.MergeNET_Assembly_List.Count > 0)
            {
                foreach (string location in XMLUtils.MergeNET_Assembly_List)
                {
                    NETAssembly_ListBox.Items.Add(location);
                }
            }

            RuntimeNameBox.Text = XMLUtils.RuntimeName;

            if (XMLUtils.RuntimeMode == "Normal")
                ModeComboBox.Text = "Normal Runtime Protect";
            else if (XMLUtils.RuntimeMode == "Merge")
                ModeComboBox.Text = "Normal Runtime Protect + Merge";
            else
                ModeComboBox.Text = XMLUtils.RuntimeMode;

            if (XMLUtils.RandomOutlineMode == "Ascii")
                RandomOutline_ComboBox.Text = "Ascii";
            else if (XMLUtils.RandomOutlineMode == "Numbers")
                RandomOutline_ComboBox.Text = "Numbers";
            else if (XMLUtils.RandomOutlineMode == "Symbols")
                RandomOutline_ComboBox.Text = "Symbols";
            else if (XMLUtils.RandomOutlineMode == "Hex")
                RandomOutline_ComboBox.Text = "Hex";
            else
                RandomOutline_ComboBox.Text = XMLUtils.RandomOutlineMode;


            antiDebug_CheckBox.Checked = XMLUtils.AntiDebug;
            antiDump_CheckBox.Checked = XMLUtils.AntiDump;
            ProcessMonitor_CheckBox.Checked = XMLUtils.ProcessMonitor;
            renamer_CheckBox.Checked = XMLUtils.Renamer;
            VirtualizeAllStrings_CheckBox.Checked = XMLUtils.StringVirtualize;
            EncryptAllStrings_CheckBox.Checked = XMLUtils.StringEncrypt;
            resourceProt_CheckBox.Checked = XMLUtils.ResourceProt;
            Junk_CheckBox.Checked = XMLUtils.Junk;
            RandomOutline_CheckBox.Checked = XMLUtils.RandomOutline;
            Arithmetic_CheckBox.Checked = XMLUtils.Arithmetic;
            IntConfusion_CheckBox.Checked = XMLUtils.IntConfusion;
            IntEncoding_CheckBox.Checked = XMLUtils.IntEncoding;
            ImportProtection_CheckBox.Checked = XMLUtils.ImportProtection;
            LocalToField_CheckBox.Checked = XMLUtils.LocalToField;

            JunkNumber.Value = XMLUtils.JunkNum;
            RandomOutlineLength.Value = XMLUtils.RandomOutlineLength;

            moduleDefMD = null;
            moduleDefMD = ModuleDefMD.Load(XMLUtils.Module_Location);

            AddMethodsToCheckListBox();

            #region Read Virt Selected Method
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            foreach (var mutate in XMLUtils.Mutate_Methods_FullName)
            {
                XMLUtils.Mutate_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, mutate));
            }    
            foreach (var virt in XMLUtils.Virt_Methods_FullName)
            {
                XMLUtils.Virt_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, virt));
            }    
            
            foreach (TreeNode methodtree_node in MethodTree.Nodes)
                foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                    if (methodtree_nodes_node.Nodes.Count != 0)
                    {
                        foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                            if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text) &&
                                XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 9;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 9;
                            }
                            else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 8;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 8;
                            }
                            else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 7;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 7;
                            }
                    }
                    else
                    {
                        if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text) &&
                            XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 9;
                            methodtree_nodes_node.SelectedImageIndex = 9;
                        }
                        else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 8;
                            methodtree_nodes_node.SelectedImageIndex = 8;
                        }
                        else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 7;
                            methodtree_nodes_node.SelectedImageIndex = 7;
                        }
                    }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion

            return true;
        }

        private void ResetProj_Button_Click(object sender, EventArgs e)
        {
            XMLUtils.Open_Project_Location = string.Empty;

            XMLUtils.AntiDebug = false;
            XMLUtils.AntiDump = false;
            XMLUtils.ProcessMonitor = false;
            XMLUtils.Renamer = false;
            XMLUtils.StringVirtualize = false;
            XMLUtils.StringEncrypt = false;
            XMLUtils.ResourceProt = false;
            XMLUtils.Junk = false;
            XMLUtils.RandomOutline = false;
            XMLUtils.Arithmetic = false;
            XMLUtils.IntConfusion = false;
            XMLUtils.IntEncoding = false;
            XMLUtils.ImportProtection = false;
            XMLUtils.LocalToField = false;

            XMLUtils.JunkNum = 100;
            XMLUtils.RandomOutlineLength = 15;

            SelectAllFun_Label.Visible = false;
            SelectAllFun_ComboBox.Visible = false;

            ShowHiddenTypesAndMethods_CheckBox.Checked = false;
            ShowHiddenTypesAndMethods_CheckBox.Visible = false;

            ResetProject();
        }

        private void OpenProj_Button_Click(object sender, EventArgs e)
        {
            var OpenProj_Dialog = new OpenFileDialog();
            OpenProj_Dialog.FileName = string.Empty;
            OpenProj_Dialog.Title = "Open xVM Project File";
            OpenProj_Dialog.Filter = "xVM Project File(*.xml)|*.xml";
            OpenProj_Dialog.CheckFileExists = true;

            if (OpenProj_Dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = OpenProj_Dialog.FileName;
                if (fileName.Length > 0)
                {
                    ResetProject();
                    if (UpdatexVMConfig(fileName))
                    {
                        MethodTree.Enabled = true;

                        if (MethodTree == null)
                            AddMethodsToCheckListBox();

                        UpdateXMlConfig();

                        XMLUtils.Open_Project_Location = fileName;
                    }
                    else
                        MessageBox.Show("Assembly is Invalid!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveProj_Button_Click(object sender, EventArgs e)
        {
            if (AsmTextBox.Text != string.Empty && AsmTextBox.Text != "" && AsmTextBox.Text != " " && !AsmTextBox.Text.All(char.IsWhiteSpace) &&
                !string.IsNullOrWhiteSpace(AsmTextBox.Text))
            {
                UpdateXMlConfig();

                if (File.Exists(XMLUtils.Open_Project_Location))
                {
                    File.WriteAllBytes(XMLUtils.Open_Project_Location, Proj.ToArray());
                }
                else
                {
                    var SaveProj_Dialog = new SaveFileDialog();
                    SaveProj_Dialog.FileName = "xVM_Proj.xml";
                    SaveProj_Dialog.Title = "Save xVM Project";
                    SaveProj_Dialog.Filter = "xVM Project File(*.xml)|*.xml";

                    if (SaveProj_Dialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = SaveProj_Dialog.FileName;
                        if (fileName.Length > 0)
                        {
                            File.WriteAllBytes(fileName, Proj.ToArray());

                            if (File.Exists(fileName))
                            {
                                XMLUtils.Open_Project_Location = fileName;

                                MessageBox.Show("Successfully Saved!", "Info?", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                                MessageBox.Show("Failed.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Assembly is Invalid!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ProtectButton_Click(object sender, EventArgs e)
        {
            try
            {
                var collectionOfProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(AsmTextBox.Text));
                if (collectionOfProcess.Length >= 1)
                    if (collectionOfProcess[0].MainModule.FileName == AsmTextBox.Text)
                        collectionOfProcess[0].Kill();

                if (AsmTextBox.Text != string.Empty && AsmTextBox.Text != "" && AsmTextBox.Text != " " && !AsmTextBox.Text.All(char.IsWhiteSpace) &&
                    !string.IsNullOrWhiteSpace(AsmTextBox.Text))
                {
                    UpdateXMlConfig();

                    Log_RichTextBox.Clear();

                    MainPage.Enabled = false;
                    ProtectionPage.Enabled = false;
                    FunctionsPage.Enabled = false;
                    MergeNETPage.Enabled = false;
                    RuntimePage.Enabled = false;
                    ToolsPage.Enabled = false;
                    ProtectButton.Enabled = false;
                    ResetProj_Button.Enabled = false;
                    SaveProj_Button.Enabled = false;
                    OpenProj_Button.Enabled = false;
                    HashUpdaterToolButton.Enabled = false;
                    MergeNET_AddLibrary_Button.Enabled = false;
                    MergeNET_SelecetAndRemove_Button.Enabled = false;

                    if (TControl.SelectedTab != LogPage)
                        TControl.SelectedTab = LogPage;

                    #region First Log
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    #region Copyright
                    ////////////////////////////////////////////////
                    LOG("xVM .NET Virtualize v1.1.3.1254");
                    LOG("Copyright © xVM 2021-2022", true);
                    ////////////////////////////////////////////////
                    #endregion

                    #region Module File Info
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    LOG("Assembly: " + XMLUtils.Module_Location, true);
                    LOG("Destination Assembly: " + OutDestination.Text, true);
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region SNK Info
                    //////////////////////////////////////////////////////////////////////////////////
                    if (XMLUtils.SNKFile_Location == string.Empty)
                    {
                        LOG("Sign The Assembly: False", true);
                    }
                    else
                    {
                        LOG("SNK File Location: " + XMLUtils.SNKFile_Location, true);
                        LOG("SNK Password: " + XMLUtils.SNK_Password, true);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region RT Mode Info
                    //////////////////////////////////////////////////////////////////////////////////
                    if (ModeComboBox.Text == "Normal Runtime Protect")
                    {
                        LOG("Selected RT Protect Mode: " + ModeComboBox.Text, true);
                        LOG("Runtime Name: " + RuntimeNameBox.Text, true);
                    }
                    else if (ModeComboBox.Text == "Normal Runtime Protect + Merge")
                    {
                        LOG("Selected RT Protect Mode: Merge Runtime", true);
                    }
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Protection Options Info
                    //////////////////////////////////////////////////////////////////////////////////
                    if (antiDebug_CheckBox.Checked == true)
                        LOG("Anti Debug: True", true);
                    else
                        LOG("Anti Debug: False", true);

                    if (antiDump_CheckBox.Checked == true)
                        LOG("Anti Dump: True", true);
                    else
                        LOG("Anti Dump: False", true);

                    if (ProcessMonitor_CheckBox.Checked == true)
                        LOG("Process Monitor: True", true);
                    else
                        LOG("Process Monitor: False", true);

                    if (renamer_CheckBox.Checked == true)
                        LOG("Renamer: True", true);
                    else
                        LOG("Renamer: False", true);

                    if (VirtualizeAllStrings_CheckBox.Checked == true)
                        LOG("Virtualize All String: True", true);
                    else
                        LOG("Virtualize All String: False", true);

                    if (EncryptAllStrings_CheckBox.Checked == true)
                        LOG("Encrypt All String: True", true);
                    else
                        LOG("Encrypt All String: False", true);

                    if (Arithmetic_CheckBox.Checked == true)
                        LOG("Arithmetic: True", true);
                    else
                        LOG("Arithmetic: False", true);

                    if (IntConfusion_CheckBox.Checked == true)
                        LOG("Int Confusion: True", true);
                    else
                        LOG("Int Confusion: False", true);

                    if (IntEncoding_CheckBox.Checked == true)
                        LOG("Int Encoding: True", true);
                    else
                        LOG("Int Encoding: False", true);

                    if (ImportProtection_CheckBox.Checked == true)
                        LOG("Import Protection: True", true);
                    else
                        LOG("Import Protection: False", true);

                    if (LocalToField_CheckBox.Checked == true)
                        LOG("Local To Field: True", true);
                    else
                        LOG("Local To Field: False", true);

                    if (resourceProt_CheckBox.Checked == true)
                        LOG("Resource Protection: True", true);
                    else
                        LOG("Resource Protection: False", true);

                    if (Junk_CheckBox.Checked == true)
                        LOG("Junk: True", true);
                    else
                        LOG("Junk: False", true);

                    if (RandomOutline_CheckBox.Checked == true)
                        LOG("Random Outline: True", true);
                    else
                        LOG("Random Outline: False", true);
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Destination Info
                    //////////////////////////////////////////////////////////////////////////////////
                    LOG("Creating Destination Directory: " + XMLUtils.WriteDirectory, true);
                    //////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    #region Mutate Method Info
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (XMLUtils.Mutate_Methods_FullName.Count != 0)
                    {
                        LOG("-----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                        foreach (var virt_md in XMLUtils.Mutate_Methods_FullName)
                        {

                            LOG("[Mutate] " + virt_md);
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    if (XMLUtils.Virt_Methods_FullName.Count != 0)
                        LOG("-----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    else if (XMLUtils.Mutate_Methods_FullName.Count != 0)
                        LOG("-----------------------------------------------------------------------------------------------------------------------------------------------------------------", true);

                    #region Virt Method Info
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (XMLUtils.Virt_Methods_FullName.Count != 0)
                    {
                        foreach (var virt_md in XMLUtils.Virt_Methods_FullName)
                        {
                            LOG("[Virtualize] " + virt_md);
                        }
                        LOG("-----------------------------------------------------------------------------------------------------------------------------------------------------------------", true);
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #endregion


                    #region Check Out Directory
                    ////////////////////////////////////////////////////////////////////////////
                    if (!Directory.Exists(Path.GetDirectoryName(OutDestination.Text)))
                        Directory.CreateDirectory(Path.GetDirectoryName(OutDestination.Text));
                    ////////////////////////////////////////////////////////////////////////////
                    #endregion

                    var Threads = new List<Thread>();
                    Threads.Add(new Thread(() =>
                    {
                        var module = ModuleDefMD.Load(XMLUtils.Module_Location);
                        var Protection_Methods = new List<string>();
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        #region Random Outline
                        if (RandomOutline_CheckBox.Checked)
                        {
                            RandomOutline.thelength = (int)RandomOutlineLength.Value;
                            if (RandomOutline_ComboBox.Text == "Ascii")
                            {
                                RandomOutline.RandomOutlineMode = "Ascii";
                            }
                            else if (RandomOutline_ComboBox.Text == "Numbers")
                            {
                                RandomOutline.RandomOutlineMode = "Numbers";
                            }
                            else if (RandomOutline_ComboBox.Text == "Symbols")
                            {
                                RandomOutline.RandomOutlineMode = "Symbols";
                            }
                            else if (RandomOutline_ComboBox.Text == "Hex")
                            {
                                RandomOutline.RandomOutlineMode = "Hex";
                            }
                            new RandomOutline().Execute(module);
                        }
                        #endregion

                        #region Encrypt Strings
                        if (EncryptAllStrings_CheckBox.Checked)
                        {
                            InitializePhase.isEncryptAllStrings = true;
                            new EncryptStrings().Execute(module);
                        }
                        #endregion

                        #region Virtualize Strings
                        /////////////////////////////////////////////////////////////////////////////
                        if (VirtualizeAllStrings_CheckBox.Checked)
                        {
                            var inj = new HideCallString(module);
                            var list = inj.Inject_Runtime();

                            foreach (var injeted in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(injeted.FullName);
                                Protection_Methods.Add(injeted.FullName);
                            }

                            foreach (var def in module.Types)
                                foreach (var AllMD in def.Methods)
                                    inj.Execute(AllMD);
                        }
                        /////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Mutation
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (XMLUtils.Mutate_Methods_FullName != null && XMLUtils.Mutate_Methods_FullName != new List<string>())
                        {
                            foreach (var mutate_md in FullNameToMethod_Class.FullNamesToMethods(module, XMLUtils.Mutate_Methods_FullName))
                            {
                                MutationConfusion.Execute(mutate_md);
                                CEXControlFlow.Execute(mutate_md, 2);
                                NullControlFlow.Execute(mutate_md);
                                SugarControlFlow.Execute(mutate_md);
                                MutationConfusion.Execute(mutate_md);
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Process Monitor
                        /////////////////////////////////////////////////////////////////////////////
                        if (ProcessMonitor_CheckBox.Checked)
                        {
                            var list = ProcessMonitor.Execute(module);
                            foreach (var pm in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(pm.FullName);
                                Protection_Methods.Add(pm.FullName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Anti Debug
                        /////////////////////////////////////////////////////////////////
                        if (antiDebug_CheckBox.Checked == true)
                        {
                            var list = AntiDebug_Inject.Execute(module);
                            foreach (var dbg in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(dbg.FullName);
                                Protection_Methods.Add(dbg.FullName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////
                        #endregion

                        #region Anti Dump
                        /////////////////////////////////////////////////////////////////
                        if (antiDump_CheckBox.Checked == true)
                        {
                            var list = AntiDump_Inject.Execute(module);
                            foreach (var dmp in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(dmp.FullName);
                                Protection_Methods.Add(dmp.FullName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////
                        #endregion

                        #region Arithmetic
                        ////////////////////////////////////////////////////////////////////////////
                        if (Arithmetic_CheckBox.Checked)
                        {
                            new Arithmetic().Execute(module);
                        }
                        ////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Int Confusion
                        ///////////////////////////////////////////////////////////////////////
                        if (IntConfusion_CheckBox.Checked)
                        {
                            new IntConfusion().Execute(module);
                        }
                        //////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Int Encoding
                        ////////////////////////////////////////////////////////////////////
                        if (IntEncoding_CheckBox.Checked)
                        {
                            IntEncoding.Execute(module);
                        }
                        ////////////////////////////////////////////////////////////////////
                        #endregion

                        #region Merge .NET
                        /////////////////////////////////////////////////////////////////
                        if (NETAssembly_ListBox.Items.Count > 0)
                        {
                            var DLLS = new List<string>();
                            foreach (string location in NETAssembly_ListBox.Items)
                                DLLS.Add(location);

                            var list = MergeNET_Inject.Execute(module, DLLS);
                            foreach (var merg in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(merg.FullName);
                                Protection_Methods.Add(merg.FullName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////
                        #endregion

                        #region Resource Protection
                        /////////////////////////////////////////////////////////////////
                        if (resourceProt_CheckBox.Checked == true)
                        {
                            var list = ResourceProt_Inject.Execute(module);
                            foreach (var res in list)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(res.FullName);
                                Protection_Methods.Add(res.FullName);
                            }
                        }
                        /////////////////////////////////////////////////////////////////
                        #endregion

                        #region Local To Field
                        /////////////////////////////////////////////////////////////
                        if (LocalToField_CheckBox.Checked)
                        {
                            LocalToField.Execute(module);
                        }    
                        /////////////////////////////////////////////////////////////
                        #endregion

                        #region Import Protection
                        ///////////////////////////////////////////////////////
                        if (ImportProtection_CheckBox.Checked)
                        {
                            ImportProtection.Execute(module);
                        }    
                        ///////////////////////////////////////////////////////
                        #endregion

                        #region Update Virt Proj (For Injected Protections)
                        ///////////////////////////////////////
                        var stream = new MemoryStream();
                        XMLUtils.SaveProject(stream);
                        stream.Flush();
                        var ProjBytes = stream.ToArray();
                        stream.Close();
                        ///////////////////////////////////////
                        #endregion

                        #region Virtualization
                        //////////////////////////////////////////////////////////////
                        new VMTask().Exceute(module, new MemoryStream(ProjBytes));
                        //////////////////////////////////////////////////////////////
                        #endregion

                        #region Junk Add
                        if (Junk_CheckBox.Checked)
                        {
                            JunkAdd.Junks = (int)JunkNumber.Value;
                            new JunkAdd().Execute(module);
                        }
                        #endregion

                        #region Renamer
                        /////////////////////////////////////////////////////////////////////////
                        if (renamer_CheckBox.Checked)
                            new OtherRenamer().Process(module);
                        /////////////////////////////////////////////////////////////////////////
                        #endregion

                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        module.Write(OutDestination.Text, Helper.Core.Utils.ExecuteModuleWriterOptions);

                        #region Remove Injected Protection Methods from "Virt_Methods_FullName"
                        ////////////////////////////////////////////////////////////////////////////
                        foreach (var injmd in Protection_Methods)
                            if (XMLUtils.Virt_Methods_FullName.Contains(injmd))
                                XMLUtils.Virt_Methods_FullName.Remove(injmd);
                        ////////////////////////////////////////////////////////////////////////////
                        #endregion

                        MainPage.Enabled = true;
                        ProtectionPage.Enabled = true;
                        FunctionsPage.Enabled = true;
                        MergeNETPage.Enabled = true;
                        RuntimePage.Enabled = true;
                        ToolsPage.Enabled = true;
                        ProtectButton.Enabled = true;
                        ResetProj_Button.Enabled = true;
                        SaveProj_Button.Enabled = true;
                        OpenProj_Button.Enabled = true;
                        HashUpdaterToolButton.Enabled = true;
                        MergeNET_AddLibrary_Button.Enabled = true;
                        MergeNET_SelecetAndRemove_Button.Enabled = true;

                        #region End Log
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        LOG("Protection Done!", true);
                        LOG("Output Written To: " + OutDestination.Text, true);
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        module = null;
                    }));

                    foreach (var t in Threads)
                        t.Start();

                    foreach (var t in Threads)
                        t.IsBackground = true;
                }
                else
                    MessageBox.Show("Please select file!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception except)
            {
                #region End Log
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                LOG("Failed: " + except.ToString());
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                return;
            }
        }

        private void RandomRuntimeName_Button_Click(object sender, EventArgs e)
        {
            RuntimeNameBox.Text = new Utils().Random_VMProtect_HEX();
        }

        private void LOG(string text, bool newline = false)
        {
            Log_RichTextBox.AppendText(text);
            Log_RichTextBox.AppendText(Environment.NewLine);

            if (newline)
                Log_RichTextBox.AppendText(Environment.NewLine);
        }

        private void HashUpdaterToolButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    HashUpdaterTool.Show();
            //}
            //catch
            //{
            //    HashUpdaterTool = new HashUpdater();
            //    HashUpdaterTool.Show();
            //}
            MessageBox.Show("This feature is not available yet!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MinimizeFormButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void TControl_Selected(object sender, TabControlEventArgs e)
        {
            if (TControl.SelectedTab == FunctionsPage && AsmTextBox.Text != string.Empty)
            {
                SelectAllFun_Label.Visible = true;
                SelectAllFun_ComboBox.Visible = true;
                ShowHiddenTypesAndMethods_CheckBox.Visible = true;
                return;
            }
            else if (TControl.SelectedTab != FunctionsPage)
            {
                SelectAllFun_Label.Visible = false;
                SelectAllFun_ComboBox.Visible = false;
                ShowHiddenTypesAndMethods_CheckBox.Visible = false;
                return;
            }
        }

        private void ShowHiddenTypesAndMethods_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowHiddenTypesAndMethods_CheckBox.Checked == true)
            {
                if (AsmTextBox.Text == string.Empty)
                    return;
                SelectAllFun_ComboBox.Text = SelectAllFun_ComboBox.Items[0].ToString();
                ShowHiddenTypesAndMethods_CheckBox.Text = "Hide Hidden Types And Methods";
                MethodTree.Nodes.Clear();
                tempMethodsList.Clear();

                var module = ModuleDefMD.Load(XMLUtils.Module_Location);
                var namespaces = new HashSet<string>();

                foreach (TypeDef type in module.GetTypes())
                {
                    namespaces.Add(type.Namespace);
                }
                namespaces.Distinct();

                #region Empty so only print non-namespaces as classes
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (TypeDef type in module.GetTypes())
                {
                    if (type.Namespace == string.Empty && !type.Name.Contains("ImplementationDetails>") &&
                        !type.Name.Contains("<>f__AnonymousType"))
                    {
                        var node = new TreeNode(type.Name, 0, 0);
                        node.Tag = 1;

                        foreach (MethodDef methodDef in type.Methods)
                            if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                                if (Utils.OVMAnalyzer(methodDef))
                                {
                                    var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                    if (methodDef.IsPublic && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 2;
                                        nod2.SelectedImageIndex = 2;
                                    }
                                    else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x45B29C);
                                        nod2.ImageIndex = 3;
                                        nod2.SelectedImageIndex = 3;
                                    }
                                    else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 4;
                                        nod2.SelectedImageIndex = 4;
                                    }
                                    else if (methodDef.IsFamily && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 5;
                                        nod2.SelectedImageIndex = 5;
                                    }
                                    else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 6;
                                        nod2.SelectedImageIndex = 6;
                                    }
                                    else if (methodDef.IsPublic)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 2;
                                        nod2.SelectedImageIndex = 2;
                                    }
                                    else if (methodDef.IsPrivate)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xCC6600);
                                        nod2.ImageIndex = 3;
                                        nod2.SelectedImageIndex = 3;
                                    }
                                    else if (methodDef.IsAssembly)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 4;
                                        nod2.SelectedImageIndex = 4;
                                    }
                                    else if (methodDef.IsFamily)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 5;
                                        nod2.SelectedImageIndex = 5;
                                    }
                                    else if (methodDef.IsFamilyOrAssembly)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 6;
                                        nod2.SelectedImageIndex = 6;
                                    }

                                    nod2.Tag = 2;
                                    node.Nodes.Add(nod2);

                                    tempMethodsList.Add(methodDef.FullName);
                                }
                        MethodTree.Nodes.Add(node);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                TreeNode node_Namespace = null;
                foreach (string nspace in namespaces)
                {
                    if (nspace != string.Empty)
                    {
                        node_Namespace = new TreeNode(nspace, 0, 0);
                        node_Namespace.Tag = 0;
                        MethodTree.Nodes.Add(node_Namespace);

                        foreach (TypeDef type in module.GetTypes())
                        {
                            if (node_Namespace.Text == type.Namespace && type.Namespace != string.Empty && !type.IsValueType && !type.IsInterface)
                            {
                                string type_Name = type.Name.Contains("`") ? type.Name.Substring(0, type.Name.IndexOf('`')) : type.Name.Replace("`", string.Empty);
                                var node = new TreeNode(type_Name, 0, 0);
                                node.Tag = 1;

                                foreach (MethodDef methodDef in type.Methods)
                                    if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                                        if (Utils.OVMAnalyzer(methodDef))
                                        {
                                            var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                            if (methodDef.IsPublic && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 2;
                                                nod2.SelectedImageIndex = 2;
                                            }
                                            else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x45B29C);
                                                nod2.ImageIndex = 3;
                                                nod2.SelectedImageIndex = 3;
                                            }
                                            else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 4;
                                                nod2.SelectedImageIndex = 4;
                                            }
                                            else if (methodDef.IsFamily && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 5;
                                                nod2.SelectedImageIndex = 5;
                                            }
                                            else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 6;
                                                nod2.SelectedImageIndex = 6;
                                            }
                                            else if (methodDef.IsPublic)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 2;
                                                nod2.SelectedImageIndex = 2;
                                            }
                                            else if (methodDef.IsPrivate)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xCC6600);
                                                nod2.ImageIndex = 3;
                                                nod2.SelectedImageIndex = 3;
                                            }
                                            else if (methodDef.IsAssembly)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 4;
                                                nod2.SelectedImageIndex = 4;
                                            }
                                            else if (methodDef.IsFamily)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 5;
                                                nod2.SelectedImageIndex = 5;
                                            }
                                            else if (methodDef.IsFamilyOrAssembly)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 6;
                                                nod2.SelectedImageIndex = 6;
                                            }

                                            nod2.Tag = 2;
                                            node.Nodes.Add(nod2);

                                            tempMethodsList.Add(methodDef.FullName);
                                        }
                                node_Namespace.Nodes.Add(node);
                            }
                        }
                    }
                }

                try
                {
                    // Deleting Empty Classes
                    TreeNode node = null;
                    int nro = 0;
                    while (nro < MethodTree.Nodes.Count)
                    {
                        node = MethodTree.Nodes[nro];
                        if (node.Nodes.Count == 0)
                            MethodTree.Nodes.Remove(node);
                        else
                            nro++;
                    }
                    ///////////////////////////////////////////////////////
                }
                catch { }

                MethodTree.Sort();
                #region Read Virt Selected Method
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (var mutate in XMLUtils.Mutate_Methods_FullName)
                {
                    XMLUtils.Mutate_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, mutate));
                }
                foreach (var virt in XMLUtils.Virt_Methods_FullName)
                {
                    XMLUtils.Virt_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, virt));
                }

                foreach (TreeNode methodtree_node in MethodTree.Nodes)
                    foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                        if (methodtree_nodes_node.Nodes.Count != 0)
                        {
                            foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                                if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text) &&
                                    XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 9;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 9;
                                }
                                else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 8;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 8;
                                }
                                else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 7;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 7;
                                }
                        }
                        else
                        {
                            if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text) &&
                                XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 9;
                                methodtree_nodes_node.SelectedImageIndex = 9;
                            }
                            else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 8;
                                methodtree_nodes_node.SelectedImageIndex = 8;
                            }
                            else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 7;
                                methodtree_nodes_node.SelectedImageIndex = 7;
                            }
                        }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                return;
            }
            else if (ShowHiddenTypesAndMethods_CheckBox.Checked == false)
            {
                if (AsmTextBox.Text == string.Empty)
                    return;
                SelectAllFun_ComboBox.Text = SelectAllFun_ComboBox.Items[0].ToString();
                ShowHiddenTypesAndMethods_CheckBox.Text = "Show Hidden Types And Methods";
                MethodTree.Nodes.Clear();
                tempMethodsList.Clear();

                var module = ModuleDefMD.Load(XMLUtils.Module_Location);
                var namespaces = new HashSet<string>();

                foreach (TypeDef type in module.GetTypes())
                {
                    namespaces.Add(type.Namespace);
                }
                namespaces.Distinct();

                #region Empty so only print non-namespaces as classes
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (TypeDef type in module.Types)
                {
                    if (type.Namespace == string.Empty && !type.Name.Contains("ImplementationDetails>") &&
                        !type.Name.Contains("<>f__AnonymousType"))
                    {
                        var node = new TreeNode(type.Name, 0, 0);
                        node.Tag = 1;

                        foreach (MethodDef methodDef in type.Methods)
                            if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                                if (Utils.OVMAnalyzer(methodDef))
                                {
                                    var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                    if (methodDef.IsPublic && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 2;
                                        nod2.SelectedImageIndex = 2;
                                    }
                                    else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x45B29C);
                                        nod2.ImageIndex = 3;
                                        nod2.SelectedImageIndex = 3;
                                    }
                                    else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 4;
                                        nod2.SelectedImageIndex = 4;
                                    }
                                    else if (methodDef.IsFamily && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 5;
                                        nod2.SelectedImageIndex = 5;
                                    }
                                    else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                        nod2.ImageIndex = 6;
                                        nod2.SelectedImageIndex = 6;
                                    }
                                    else if (methodDef.IsPublic)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 2;
                                        nod2.SelectedImageIndex = 2;
                                    }
                                    else if (methodDef.IsPrivate)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xCC6600);
                                        nod2.ImageIndex = 3;
                                        nod2.SelectedImageIndex = 3;
                                    }
                                    else if (methodDef.IsAssembly)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 4;
                                        nod2.SelectedImageIndex = 4;
                                    }
                                    else if (methodDef.IsFamily)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 5;
                                        nod2.SelectedImageIndex = 5;
                                    }
                                    else if (methodDef.IsFamilyOrAssembly)
                                    {
                                        nod2.ForeColor = Color.FromArgb(0xFF8000);
                                        nod2.ImageIndex = 6;
                                        nod2.SelectedImageIndex = 6;
                                    }

                                    nod2.Tag = 2;
                                    node.Nodes.Add(nod2);

                                    tempMethodsList.Add(methodDef.FullName);
                                }
                        MethodTree.Nodes.Add(node);
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                TreeNode node_Namespace = null;
                foreach (string nspace in namespaces)
                {
                    if (nspace != string.Empty)
                    {
                        node_Namespace = new TreeNode(nspace, 0, 0);
                        node_Namespace.Tag = 0;
                        MethodTree.Nodes.Add(node_Namespace);

                        foreach (TypeDef type in module.Types)
                        {
                            if (node_Namespace.Text == type.Namespace && type.Namespace != string.Empty && !type.IsValueType && !type.IsInterface)
                            {
                                string type_Name = type.Name.Contains("`") ? type.Name.Substring(0, type.Name.IndexOf('`')) : type.Name.Replace("`", string.Empty);
                                var node = new TreeNode(type_Name, 0, 0);
                                node.Tag = 1;

                                foreach (MethodDef methodDef in type.Methods)
                                    if (methodDef != module.GlobalType.FindOrCreateStaticConstructor())
                                        if (Utils.OVMAnalyzer(methodDef))
                                        {
                                            var nod2 = new TreeNode(methodDef.Name + "  " + "(" + string.Format("0x{0:X4}", methodDef.MDToken) + ")");

                                            if (methodDef.IsPublic && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 2;
                                                nod2.SelectedImageIndex = 2;
                                            }
                                            else if (methodDef.IsPrivate && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x45B29C);
                                                nod2.ImageIndex = 3;
                                                nod2.SelectedImageIndex = 3;
                                            }
                                            else if (methodDef.IsAssembly && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 4;
                                                nod2.SelectedImageIndex = 4;
                                            }
                                            else if (methodDef.IsFamily && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 5;
                                                nod2.SelectedImageIndex = 5;
                                            }
                                            else if (methodDef.IsFamilyOrAssembly && methodDef.IsConstructor)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0x4EC9B0);
                                                nod2.ImageIndex = 6;
                                                nod2.SelectedImageIndex = 6;
                                            }
                                            else if (methodDef.IsPublic)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 2;
                                                nod2.SelectedImageIndex = 2;
                                            }
                                            else if (methodDef.IsPrivate)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xCC6600);
                                                nod2.ImageIndex = 3;
                                                nod2.SelectedImageIndex = 3;
                                            }
                                            else if (methodDef.IsAssembly)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 4;
                                                nod2.SelectedImageIndex = 4;
                                            }
                                            else if (methodDef.IsFamily)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 5;
                                                nod2.SelectedImageIndex = 5;
                                            }
                                            else if (methodDef.IsFamilyOrAssembly)
                                            {
                                                nod2.ForeColor = Color.FromArgb(0xFF8000);
                                                nod2.ImageIndex = 6;
                                                nod2.SelectedImageIndex = 6;
                                            }

                                            nod2.Tag = 2;
                                            node.Nodes.Add(nod2);

                                            tempMethodsList.Add(methodDef.FullName);
                                        }
                                node_Namespace.Nodes.Add(node);
                            }
                        }
                    }
                }

                try
                {
                    // Deleting Empty Classes
                    TreeNode node = null;
                    int nro = 0;
                    while (nro < MethodTree.Nodes.Count)
                    {
                        node = MethodTree.Nodes[nro];
                        if (node.Nodes.Count == 0)
                            MethodTree.Nodes.Remove(node);
                        else
                            nro++;
                    }
                    ///////////////////////////////////////////////////////
                }
                catch { }

                MethodTree.Sort();
                #region Read Virt Selected Method
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                foreach (var mutate in XMLUtils.Mutate_Methods_FullName)
                {
                    XMLUtils.Mutate_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, mutate));
                }
                foreach (var virt in XMLUtils.Virt_Methods_FullName)
                {
                    XMLUtils.Virt_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, virt));
                }

                foreach (TreeNode methodtree_node in MethodTree.Nodes)
                    foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                        if (methodtree_nodes_node.Nodes.Count != 0)
                        {
                            foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                                if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text) &&
                                    XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 9;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 9;
                                }
                                else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 8;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 8;
                                }
                                else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                                {
                                    methodtree_nodes_nodes_node.ImageIndex = 7;
                                    methodtree_nodes_nodes_node.SelectedImageIndex = 7;
                                }
                        }
                        else
                        {
                            if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text) &&
                                XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 9;
                                methodtree_nodes_node.SelectedImageIndex = 9;
                            }
                            else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 8;
                                methodtree_nodes_node.SelectedImageIndex = 8;
                            }
                            else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                            {
                                methodtree_nodes_node.ImageIndex = 7;
                                methodtree_nodes_node.SelectedImageIndex = 7;
                            }
                        }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                return;
            }
        }

        private void RandomOutline_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RandomOutline_CheckBox.Checked == true)
            {
                RandomOutline_ComboBox.Enabled = true;
                return;
            }
            if (RandomOutline_CheckBox.Checked == false)
            {
                RandomOutline_ComboBox.Enabled = false;
                return;
            }
        }

        private void SelectAllFun_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AsmTextBox.Text == string.Empty)
                return;
            #region None
            if (SelectAllFun_ComboBox.Text == "None") // None
            {
                XMLUtils.Mutate_Methods_Name.Clear();
                XMLUtils.Virt_Methods_Name.Clear();
                XMLUtils.Mutate_Methods_FullName.Clear();
                XMLUtils.Virt_Methods_FullName.Clear();
                ShowHiddenTypesAndMethods_CheckBox_CheckedChanged(sender, e);
            }
            #endregion

            #region Mutate
            else if (SelectAllFun_ComboBox.Text == "Mutate") // Mutate
            {
                XMLUtils.Mutate_Methods_Name.Clear();
                XMLUtils.Virt_Methods_Name.Clear();
                XMLUtils.Mutate_Methods_FullName.Clear();
                XMLUtils.Virt_Methods_FullName.Clear();
                foreach (TreeNode methodtree_node in MethodTree.Nodes)
                {
                    foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                    {
                        if (methodtree_nodes_node.Nodes.Count != 0)
                        {
                            foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                            {
                                XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_nodes_node.Text));
                            }
                        }
                        else
                        {
                            XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_node.Text));
                        }
                    }
                }
                goto ReadVirtSelectedMethod;
            }
            #endregion

            #region Virtualize
            else if (SelectAllFun_ComboBox.Text == "Virtualize") // Virtualize
            {
                XMLUtils.Mutate_Methods_Name.Clear();
                XMLUtils.Virt_Methods_Name.Clear();
                XMLUtils.Mutate_Methods_FullName.Clear();
                XMLUtils.Virt_Methods_FullName.Clear();
                foreach (TreeNode methodtree_node in MethodTree.Nodes)
                {
                    foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                    {
                        if (methodtree_nodes_node.Nodes.Count != 0)
                        {
                            foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                            {
                                XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_nodes_node.Text));
                            }
                        }
                        else
                        {
                            XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_node.Text));
                        }
                    }
                }
                goto ReadVirtSelectedMethod;
            }
            #endregion

            #region Ultra
            else if (SelectAllFun_ComboBox.Text == "Ultra") // Ultra
            {
                XMLUtils.Mutate_Methods_Name.Clear();
                XMLUtils.Virt_Methods_Name.Clear();
                XMLUtils.Mutate_Methods_FullName.Clear();
                XMLUtils.Virt_Methods_FullName.Clear();
                foreach (TreeNode methodtree_node in MethodTree.Nodes)
                {
                    foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                    {
                        if (methodtree_nodes_node.Nodes.Count != 0)
                        {
                            foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                            {
                                XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_nodes_node.Text));
                                XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_nodes_node.Text));
                            }
                        }
                        else
                        {
                            XMLUtils.Mutate_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_node.Text));
                            XMLUtils.Virt_Methods_FullName.Add(MethodNameToMethodFullName(moduleDefMD, methodtree_nodes_node.Text));
                        }
                    }
                }
                goto ReadVirtSelectedMethod;
            }
            #endregion

            ReadVirtSelectedMethod:
            #region Read Virt Selected Method
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            foreach (var mutate in XMLUtils.Mutate_Methods_FullName)
            {
                XMLUtils.Mutate_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, mutate));
            }
            foreach (var virt in XMLUtils.Virt_Methods_FullName)
            {
                XMLUtils.Virt_Methods_Name.Add(MethodFullNameToMethodName(moduleDefMD, virt));
            }

            foreach (TreeNode methodtree_node in MethodTree.Nodes)
                foreach (TreeNode methodtree_nodes_node in methodtree_node.Nodes)
                    if (methodtree_nodes_node.Nodes.Count != 0)
                    {
                        foreach (TreeNode methodtree_nodes_nodes_node in methodtree_nodes_node.Nodes)
                            if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text) &&
                                XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 9;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 9;
                            }
                            else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 8;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 8;
                            }
                            else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_nodes_node.Text))
                            {
                                methodtree_nodes_nodes_node.ImageIndex = 7;
                                methodtree_nodes_nodes_node.SelectedImageIndex = 7;
                            }
                    }
                    else
                    {
                        if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text) &&
                            XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 9;
                            methodtree_nodes_node.SelectedImageIndex = 9;
                        }
                        else if (XMLUtils.Virt_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 8;
                            methodtree_nodes_node.SelectedImageIndex = 8;
                        }
                        else if (XMLUtils.Mutate_Methods_Name.Contains(methodtree_nodes_node.Text))
                        {
                            methodtree_nodes_node.ImageIndex = 7;
                            methodtree_nodes_node.SelectedImageIndex = 7;
                        }
                    }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
        }
    }
}
