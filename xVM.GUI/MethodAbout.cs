using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xVM.GUI
{
    public partial class MethodAbout : Form
    {
        ModuleDef Module;
        string Selected_Method_FullName;

        public MethodAbout(ModuleDef module, string sel_flmd_name)
        {
            Module = module;
            Selected_Method_FullName = sel_flmd_name;

            InitializeComponent();
        }

        private void MethodAbout_Load(object sender, EventArgs e)
        {
            foreach (var type in Module.GetTypes())
                foreach (var method in type.Methods)
                {
                    if (method.FullName == Selected_Method_FullName)
                    {
                        var type_namespace = new ListViewItem(string.Format("Method Class Namespace: {0}", type.Namespace), 0);
                        type_namespace.ForeColor = Color.LightYellow;
                        MethodAbout_listView.Items.Add(type_namespace);

                        var type_class = new ListViewItem(string.Format("Method Class Name: {0}", type.Name), 1);
                        type_class.ForeColor = Color.PaleGreen;
                        MethodAbout_listView.Items.Add(type_class);

                        #region Method Name
                        /////////////////////////////////////////////////////////////////////////////////////////////////
                        var type_method = new ListViewItem(string.Format("Method Name: {0}", method.Name));

                        if (method.IsPublic && method.IsConstructor)
                        {
                            type_method.ForeColor = Color.FromArgb(0x4EC9B0);
                            type_method.ImageIndex = 2;
                        }
                        else if (method.IsPrivate && method.IsConstructor)
                        {
                            type_method.ForeColor = Color.FromArgb(0x45B29C);
                            type_method.ImageIndex = 3;
                        }
                        else if (method.IsAssembly && method.IsConstructor)
                        {
                            type_method.ForeColor = Color.FromArgb(0x4EC9B0);
                            type_method.ImageIndex = 4;
                        }
                        else if (method.IsFamily && method.IsConstructor)
                        {
                            type_method.ForeColor = Color.FromArgb(0x4EC9B0);
                            type_method.ImageIndex = 5;
                        }
                        else if (method.IsFamilyOrAssembly && method.IsConstructor)
                        {
                            type_method.ForeColor = Color.FromArgb(0x4EC9B0);
                            type_method.ImageIndex = 6;
                        }
                        else if (method.IsPublic)
                        {
                            type_method.ForeColor = Color.FromArgb(0xFF8000);
                            type_method.ImageIndex = 2;
                        }
                        else if (method.IsPrivate)
                        {
                            type_method.ForeColor = Color.FromArgb(0xCC6600);
                            type_method.ImageIndex = 3;
                        }
                        else if (method.IsAssembly)
                        {
                            type_method.ForeColor = Color.FromArgb(0xFF8000);
                            type_method.ImageIndex = 4;
                        }
                        else if (method.IsFamily)
                        {
                            type_method.ForeColor = Color.FromArgb(0xFF8000);
                            type_method.ImageIndex = 5;
                        }
                        else if (method.IsFamilyOrAssembly)
                        {
                            type_method.ForeColor = Color.FromArgb(0xFF8000);
                            type_method.ImageIndex = 6;
                        }

                        MethodAbout_listView.Items.Add(type_method);
                        /////////////////////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        var method_fullname = new ListViewItem(string.Format("Method Full Name: {0}", method.FullName), 7);
                        method_fullname.ForeColor = Color.LavenderBlush;
                        MethodAbout_listView.Items.Add(method_fullname);

                        var method_return_type = new ListViewItem(string.Format("Return Type: {0}", method.ReturnType), 10);
                        method_return_type.ForeColor = Color.FromArgb(0xF2E5C6);
                        MethodAbout_listView.Items.Add(method_return_type);

                        var method_params_count = new ListViewItem(string.Format("Method Parameters Count: {0}", method.Parameters.Count), 8);
                        method_params_count.ForeColor = Color.AliceBlue;
                        MethodAbout_listView.Items.Add(method_params_count);

                        var method_mdtoken = new ListViewItem(string.Format("Method Metadata Token: 0x{0:X4}", method.MDToken), 9);
                        method_mdtoken.ForeColor = Color.FromArgb(0xDAD9E3);
                        MethodAbout_listView.Items.Add(method_mdtoken);

             
                        foreach (var instr in method.Body.Instructions)
                        {
                            ILBox.AppendText($"IL_{instr.Offset:X4}: ", Color.FromArgb(0x806F4D));
                            ILBox.AppendText(instr.OpCode.Name, Color.FromArgb(0xAD5C85));
                            InstructionPrinterCustom.AddOperandString(ILBox, instr, " ");
                            ILBox.AppendText(Environment.NewLine);
                        }
                    }
                }
        }

        private void MethodAbout_listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Selected = false;
        }

        private void MinimizeFormButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
