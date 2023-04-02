using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xVM.GUI
{
    public partial class HashUpdater : Form
    {
        public HashUpdater()
        {
            InitializeComponent();
        }

        #region Asm File Load And Search Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void BrowseAsmButton_Click(object sender, EventArgs e)
        {
            //OpenFileDialog AsmOFD = new OpenFileDialog();
            //AsmOFD.FileName = string.Empty;
            //AsmOFD.Title = "Choose Assembly To Protect";
            //AsmOFD.Filter = "Assembly File(*.exe, *.dll)|*.exe;*.dll|All files(*.*)|*.*";
            //AsmOFD.CheckFileExists = true;

            //if (AsmOFD.ShowDialog() == DialogResult.OK)
            //{
            //    string fileLocation = AsmOFD.FileName;
            //    if (fileLocation.Length > 0)
            //    {
            //        AsmTextBox.Text = fileLocation;
            //    }
            //}
        }

        private void Asm_DragDrop(object sender, DragEventArgs e)
        {
            //object data = e.Data.GetData(DataFormats.FileDrop);
            //if (data != null)
            //{
            //    string[] array = data as string[];
            //    if (array.Length == 1)
            //    {
            //        AsmTextBox.Text = array[0];
            //    }
            //}
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        private void _DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    e.Effect = DragDropEffects.Copy;
            //}
        }

        private void UpdateAsmHashButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var collectionOfProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(AsmTextBox.Text));
            //    if (collectionOfProcess.Length >= 1)
            //        if (collectionOfProcess[0].MainModule.FileName == AsmTextBox.Text)
            //            collectionOfProcess[0].Kill();

            //    if (AsmTextBox.Text != string.Empty && AsmTextBox.Text != "" && AsmTextBox.Text != " " && !AsmTextBox.Text.All(char.IsWhiteSpace) &&
            //        !string.IsNullOrWhiteSpace(AsmTextBox.Text))
            //    {
            //        var FileBytes = File.ReadAllBytes(AsmTextBox.Text);
            //        var newPath = Path.Combine(Path.GetDirectoryName(AsmTextBox.Text), Path.GetFileNameWithoutExtension(AsmTextBox.Text) + "_Updated" + Path.GetExtension(AsmTextBox.Text));
            //        File.WriteAllBytes(newPath, Helper.Core.Utils.Checker_HashUpdate(FileBytes));
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select file!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}
            //catch (Exception except)
            //{
            //    MessageBox.Show(except.ToString(), except.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //MessageBox.Show("Hash Updated!", "Info?", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
