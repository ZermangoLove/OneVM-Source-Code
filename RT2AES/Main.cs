using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RT2AES
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private static bool IsDotNetAssembly(string assemblyPath)
        {
            bool result;
            try
            {
                System.Reflection.AssemblyName.GetAssemblyName(assemblyPath);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private void _DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void browseRT_Click(object sender, EventArgs e)
        {
            OpenFileDialog AsmOFD = new OpenFileDialog();
            AsmOFD.FileName = string.Empty;
            AsmOFD.Title = "Choose RT To Encrypt";
            AsmOFD.Filter = "RT File(*.dll)|*.dll";
            AsmOFD.CheckFileExists = true;

            if (AsmOFD.ShowDialog() == DialogResult.OK)
            {
                string fileLocation = AsmOFD.FileName;
                if (fileLocation.Length > 0 && IsDotNetAssembly(fileLocation))
                {
                    rtBox.Text = fileLocation;
                }
                else
                {
                    MessageBox.Show("Your file is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rtBox_DragDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                string[] array = data as string[];
                if (array.Length == 1)
                {
                    string fileLocation = array[0];
                    if (IsDotNetAssembly(fileLocation))
                    {
                        rtBox.Text = fileLocation;
                    }
                    else
                    {
                        MessageBox.Show("Your file is not .NET based!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private byte[] GetByteArray()
        {
            Random rnd = new Random();
            byte[] b = new byte[1 * 1024]; // convert kb to byte
            rnd.NextBytes(b);
            return b;
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 115, 21, 58, 64, 101, 144, 255, 15 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private void encRT_Click(object sender, EventArgs e)
        {
            var pass = GetByteArray();
            var enc = AES_Encrypt(File.ReadAllBytes(rtBox.Text), pass);

            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(typeof(Main).Assembly.Location), "c60f5823495b4242"), enc);
            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(typeof(Main).Assembly.Location), "f52b6acb1e894178"), pass);

            MessageBox.Show("Encrypted!");
        }
    }
}
