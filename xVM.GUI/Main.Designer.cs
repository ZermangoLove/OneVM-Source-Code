
namespace xVM.GUI
{
    partial class Main
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.TeenForm = new Teen.ThirteenForm();
            this.SelectAllFun_Label = new System.Windows.Forms.Label();
            this.SelectAllFun_ComboBox = new Teen.ThirteenComboBox();
            this.ShowHiddenTypesAndMethods_CheckBox = new System.Windows.Forms.CheckBox();
            this.MinimizeFormButton = new Teen.ThirteenButton();
            this.ExitButton = new Teen.ThirteenButton();
            this.ResetProj_Button = new Teen.ThirteenButton();
            this.OpenProj_Button = new Teen.ThirteenButton();
            this.SaveProj_Button = new Teen.ThirteenButton();
            this.ProtectButton = new Teen.ThirteenButton();
            this.NullPanel = new System.Windows.Forms.Panel();
            this.TControl = new Teen.ThirteenTabControl();
            this.MainPage = new System.Windows.Forms.TabPage();
            this.NullGroupBox1 = new System.Windows.Forms.GroupBox();
            this.NulLabel = new System.Windows.Forms.Label();
            this.SNK_PasswordBox = new Teen.ThirteenTextBox();
            this.SNKBox = new Teen.ThirteenTextBox();
            this.BrowseSNKButton = new Teen.ThirteenButton();
            this.NulLabel0 = new System.Windows.Forms.Label();
            this.ActiveSNK_CheckBox = new System.Windows.Forms.CheckBox();
            this.NullGroupBox0 = new System.Windows.Forms.GroupBox();
            this.OutDestination = new Teen.ThirteenTextBox();
            this.NullGroupBox = new System.Windows.Forms.GroupBox();
            this.BrowseAsmButton = new Teen.ThirteenButton();
            this.AsmTextBox = new Teen.ThirteenTextBox();
            this.ProtectionPage = new System.Windows.Forms.TabPage();
            this.NullGroupBox2 = new System.Windows.Forms.GroupBox();
            this.NullGroupBox8 = new System.Windows.Forms.GroupBox();
            this.ProcessMonitor_CheckBox = new System.Windows.Forms.CheckBox();
            this.antiDump_CheckBox = new System.Windows.Forms.CheckBox();
            this.antiDebug_CheckBox = new System.Windows.Forms.CheckBox();
            this.NullGroupBox9 = new System.Windows.Forms.GroupBox();
            this.IntEncoding_CheckBox = new System.Windows.Forms.CheckBox();
            this.IntConfusion_CheckBox = new System.Windows.Forms.CheckBox();
            this.Arithmetic_CheckBox = new System.Windows.Forms.CheckBox();
            this.EncryptAllStrings_CheckBox = new System.Windows.Forms.CheckBox();
            this.renamer_CheckBox = new System.Windows.Forms.CheckBox();
            this.VirtualizeAllStrings_CheckBox = new System.Windows.Forms.CheckBox();
            this.NullGroupBox10 = new System.Windows.Forms.GroupBox();
            this.RandomOutlineLength = new System.Windows.Forms.NumericUpDown();
            this.JunkNumber = new System.Windows.Forms.NumericUpDown();
            this.RandomOutline_ComboBox = new Teen.ThirteenComboBox();
            this.RandomOutline_CheckBox = new System.Windows.Forms.CheckBox();
            this.Junk_CheckBox = new System.Windows.Forms.CheckBox();
            this.resourceProt_CheckBox = new System.Windows.Forms.CheckBox();
            this.FunctionsPage = new System.Windows.Forms.TabPage();
            this.MethodTree = new System.Windows.Forms.TreeView();
            this.MethodTree_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mutateMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.virtualizeMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UnSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.methodAboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IMGList = new System.Windows.Forms.ImageList(this.components);
            this.MergeNETPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MergeNET_AddLibrary_Button = new Teen.ThirteenButton();
            this.MergeNET_SelecetAndRemove_Button = new Teen.ThirteenButton();
            this.NETAssembly_ListBox = new System.Windows.Forms.ListBox();
            this.RuntimePage = new System.Windows.Forms.TabPage();
            this.NullGroupBox4 = new System.Windows.Forms.GroupBox();
            this.NullGroupBox6 = new System.Windows.Forms.GroupBox();
            this.ModeComboBox = new Teen.ThirteenComboBox();
            this.NullGroupBox5 = new System.Windows.Forms.GroupBox();
            this.RandomRuntimeName_Button = new Teen.ThirteenButton();
            this.RuntimeNameBox = new Teen.ThirteenTextBox();
            this.LogPage = new System.Windows.Forms.TabPage();
            this.Log_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.ToolsPage = new System.Windows.Forms.TabPage();
            this.NullGroupBox7 = new System.Windows.Forms.GroupBox();
            this.HashUpdaterToolButton = new Teen.ThirteenButton();
            this.Junk_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.NopAttack_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.JunkNumber_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RandomOutlineLength_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ProcessMonitor_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ImportProtection_CheckBox = new System.Windows.Forms.CheckBox();
            this.LocalToField_CheckBox = new System.Windows.Forms.CheckBox();
            this.TeenForm.SuspendLayout();
            this.TControl.SuspendLayout();
            this.MainPage.SuspendLayout();
            this.NullGroupBox1.SuspendLayout();
            this.NullGroupBox0.SuspendLayout();
            this.NullGroupBox.SuspendLayout();
            this.ProtectionPage.SuspendLayout();
            this.NullGroupBox2.SuspendLayout();
            this.NullGroupBox8.SuspendLayout();
            this.NullGroupBox9.SuspendLayout();
            this.NullGroupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RandomOutlineLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JunkNumber)).BeginInit();
            this.FunctionsPage.SuspendLayout();
            this.MethodTree_ContextMenu.SuspendLayout();
            this.MergeNETPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.RuntimePage.SuspendLayout();
            this.NullGroupBox4.SuspendLayout();
            this.NullGroupBox6.SuspendLayout();
            this.NullGroupBox5.SuspendLayout();
            this.LogPage.SuspendLayout();
            this.ToolsPage.SuspendLayout();
            this.NullGroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // TeenForm
            // 
            this.TeenForm.AccentColor = System.Drawing.Color.Gray;
            this.TeenForm.AllowDrop = true;
            this.TeenForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.TeenForm.ColorScheme = Teen.ThirteenForm.ColorSchemes.Dark;
            this.TeenForm.Controls.Add(this.SelectAllFun_Label);
            this.TeenForm.Controls.Add(this.SelectAllFun_ComboBox);
            this.TeenForm.Controls.Add(this.ShowHiddenTypesAndMethods_CheckBox);
            this.TeenForm.Controls.Add(this.MinimizeFormButton);
            this.TeenForm.Controls.Add(this.ExitButton);
            this.TeenForm.Controls.Add(this.ResetProj_Button);
            this.TeenForm.Controls.Add(this.OpenProj_Button);
            this.TeenForm.Controls.Add(this.SaveProj_Button);
            this.TeenForm.Controls.Add(this.ProtectButton);
            this.TeenForm.Controls.Add(this.NullPanel);
            this.TeenForm.Controls.Add(this.TControl);
            this.TeenForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TeenForm.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.TeenForm.ForeColor = System.Drawing.Color.White;
            this.TeenForm.Location = new System.Drawing.Point(0, 0);
            this.TeenForm.Margin = new System.Windows.Forms.Padding(4);
            this.TeenForm.Name = "TeenForm";
            this.TeenForm.Size = new System.Drawing.Size(1155, 598);
            this.TeenForm.TabIndex = 0;
            this.TeenForm.TabStop = false;
            this.TeenForm.Text = "xVM Protection - v1.1.3.1254";
            // 
            // SelectAllFun_Label
            // 
            this.SelectAllFun_Label.AutoSize = true;
            this.SelectAllFun_Label.Location = new System.Drawing.Point(439, 562);
            this.SelectAllFun_Label.Name = "SelectAllFun_Label";
            this.SelectAllFun_Label.Size = new System.Drawing.Size(82, 23);
            this.SelectAllFun_Label.TabIndex = 18;
            this.SelectAllFun_Label.Text = "Select All:";
            // 
            // SelectAllFun_ComboBox
            // 
            this.SelectAllFun_ComboBox.AccentColor = System.Drawing.Color.Gray;
            this.SelectAllFun_ComboBox.AllowDrop = true;
            this.SelectAllFun_ComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.SelectAllFun_ComboBox.ColorScheme = Teen.ThirteenComboBox.ColorSchemes.Dark;
            this.SelectAllFun_ComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.SelectAllFun_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectAllFun_ComboBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.SelectAllFun_ComboBox.ForeColor = System.Drawing.Color.White;
            this.SelectAllFun_ComboBox.FormattingEnabled = true;
            this.SelectAllFun_ComboBox.Items.AddRange(new object[] {
            "None",
            "Mutate",
            "Virtualize",
            "Ultra"});
            this.SelectAllFun_ComboBox.Location = new System.Drawing.Point(528, 559);
            this.SelectAllFun_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.SelectAllFun_ComboBox.Name = "SelectAllFun_ComboBox";
            this.SelectAllFun_ComboBox.Size = new System.Drawing.Size(129, 30);
            this.SelectAllFun_ComboBox.TabIndex = 17;
            this.SelectAllFun_ComboBox.TabStop = false;
            this.SelectAllFun_ComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectAllFun_ComboBox_SelectedIndexChanged);
            // 
            // ShowHiddenTypesAndMethods_CheckBox
            // 
            this.ShowHiddenTypesAndMethods_CheckBox.AllowDrop = true;
            this.ShowHiddenTypesAndMethods_CheckBox.AutoSize = true;
            this.ShowHiddenTypesAndMethods_CheckBox.Location = new System.Drawing.Point(699, 560);
            this.ShowHiddenTypesAndMethods_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.ShowHiddenTypesAndMethods_CheckBox.Name = "ShowHiddenTypesAndMethods_CheckBox";
            this.ShowHiddenTypesAndMethods_CheckBox.Size = new System.Drawing.Size(283, 27);
            this.ShowHiddenTypesAndMethods_CheckBox.TabIndex = 16;
            this.ShowHiddenTypesAndMethods_CheckBox.TabStop = false;
            this.ShowHiddenTypesAndMethods_CheckBox.Text = "Show Hidden Types And Methods";
            this.ShowHiddenTypesAndMethods_CheckBox.UseVisualStyleBackColor = true;
            this.ShowHiddenTypesAndMethods_CheckBox.CheckedChanged += new System.EventHandler(this.ShowHiddenTypesAndMethods_CheckBox_CheckedChanged);
            // 
            // MinimizeFormButton
            // 
            this.MinimizeFormButton.AccentColor = System.Drawing.Color.Gray;
            this.MinimizeFormButton.AllowDrop = true;
            this.MinimizeFormButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinimizeFormButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.MinimizeFormButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.MinimizeFormButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MinimizeFormButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.MinimizeFormButton.ForeColor = System.Drawing.Color.White;
            this.MinimizeFormButton.Location = new System.Drawing.Point(1044, 4);
            this.MinimizeFormButton.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeFormButton.Name = "MinimizeFormButton";
            this.MinimizeFormButton.Size = new System.Drawing.Size(41, 28);
            this.MinimizeFormButton.TabIndex = 15;
            this.MinimizeFormButton.Text = "__";
            this.MinimizeFormButton.UseVisualStyleBackColor = false;
            this.MinimizeFormButton.Click += new System.EventHandler(this.MinimizeFormButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.AccentColor = System.Drawing.Color.Gray;
            this.ExitButton.AllowDrop = true;
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ExitButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExitButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(1088, 4);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(63, 28);
            this.ExitButton.TabIndex = 14;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // ResetProj_Button
            // 
            this.ResetProj_Button.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ResetProj_Button.AllowDrop = true;
            this.ResetProj_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResetProj_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ResetProj_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.ResetProj_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ResetProj_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.ResetProj_Button.ForeColor = System.Drawing.Color.White;
            this.ResetProj_Button.Location = new System.Drawing.Point(16, 558);
            this.ResetProj_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ResetProj_Button.Name = "ResetProj_Button";
            this.ResetProj_Button.Size = new System.Drawing.Size(119, 28);
            this.ResetProj_Button.TabIndex = 13;
            this.ResetProj_Button.TabStop = false;
            this.ResetProj_Button.Text = "Reset";
            this.ResetProj_Button.UseVisualStyleBackColor = false;
            this.ResetProj_Button.Click += new System.EventHandler(this.ResetProj_Button_Click);
            // 
            // OpenProj_Button
            // 
            this.OpenProj_Button.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.OpenProj_Button.AllowDrop = true;
            this.OpenProj_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OpenProj_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.OpenProj_Button.CausesValidation = false;
            this.OpenProj_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.OpenProj_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OpenProj_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.OpenProj_Button.ForeColor = System.Drawing.Color.White;
            this.OpenProj_Button.Location = new System.Drawing.Point(143, 558);
            this.OpenProj_Button.Margin = new System.Windows.Forms.Padding(4);
            this.OpenProj_Button.Name = "OpenProj_Button";
            this.OpenProj_Button.Size = new System.Drawing.Size(119, 28);
            this.OpenProj_Button.TabIndex = 12;
            this.OpenProj_Button.TabStop = false;
            this.OpenProj_Button.Text = "Open Project";
            this.OpenProj_Button.UseVisualStyleBackColor = false;
            this.OpenProj_Button.Click += new System.EventHandler(this.OpenProj_Button_Click);
            // 
            // SaveProj_Button
            // 
            this.SaveProj_Button.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.SaveProj_Button.AllowDrop = true;
            this.SaveProj_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveProj_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.SaveProj_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.SaveProj_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProj_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.SaveProj_Button.ForeColor = System.Drawing.Color.White;
            this.SaveProj_Button.Location = new System.Drawing.Point(269, 558);
            this.SaveProj_Button.Margin = new System.Windows.Forms.Padding(4);
            this.SaveProj_Button.Name = "SaveProj_Button";
            this.SaveProj_Button.Size = new System.Drawing.Size(119, 28);
            this.SaveProj_Button.TabIndex = 11;
            this.SaveProj_Button.TabStop = false;
            this.SaveProj_Button.Text = "Save Project";
            this.SaveProj_Button.UseVisualStyleBackColor = false;
            this.SaveProj_Button.Click += new System.EventHandler(this.SaveProj_Button_Click);
            // 
            // ProtectButton
            // 
            this.ProtectButton.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ProtectButton.AllowDrop = true;
            this.ProtectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ProtectButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.ProtectButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ProtectButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.ProtectButton.ForeColor = System.Drawing.Color.White;
            this.ProtectButton.Location = new System.Drawing.Point(1039, 558);
            this.ProtectButton.Margin = new System.Windows.Forms.Padding(4);
            this.ProtectButton.Name = "ProtectButton";
            this.ProtectButton.Size = new System.Drawing.Size(100, 28);
            this.ProtectButton.TabIndex = 7;
            this.ProtectButton.TabStop = false;
            this.ProtectButton.Text = "Protect";
            this.ProtectButton.UseVisualStyleBackColor = false;
            this.ProtectButton.Click += new System.EventHandler(this.ProtectButton_Click);
            // 
            // NullPanel
            // 
            this.NullPanel.AllowDrop = true;
            this.NullPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NullPanel.BackColor = System.Drawing.Color.Gray;
            this.NullPanel.Location = new System.Drawing.Point(-1, 545);
            this.NullPanel.Margin = new System.Windows.Forms.Padding(4);
            this.NullPanel.Name = "NullPanel";
            this.NullPanel.Size = new System.Drawing.Size(1469, 1);
            this.NullPanel.TabIndex = 1;
            // 
            // TControl
            // 
            this.TControl.AccentColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.TControl.AllowDrop = true;
            this.TControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TControl.ColorScheme = Teen.ThirteenTabControl.ColorSchemes.Dark;
            this.TControl.Controls.Add(this.MainPage);
            this.TControl.Controls.Add(this.ProtectionPage);
            this.TControl.Controls.Add(this.FunctionsPage);
            this.TControl.Controls.Add(this.MergeNETPage);
            this.TControl.Controls.Add(this.RuntimePage);
            this.TControl.Controls.Add(this.LogPage);
            this.TControl.Controls.Add(this.ToolsPage);
            this.TControl.ForeColor = System.Drawing.Color.White;
            this.TControl.Location = new System.Drawing.Point(16, 55);
            this.TControl.Margin = new System.Windows.Forms.Padding(4);
            this.TControl.Name = "TControl";
            this.TControl.SelectedIndex = 0;
            this.TControl.Size = new System.Drawing.Size(1123, 478);
            this.TControl.TabIndex = 0;
            this.TControl.TabStop = false;
            this.TControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.TControl_Selected);
            // 
            // MainPage
            // 
            this.MainPage.AllowDrop = true;
            this.MainPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.MainPage.Controls.Add(this.NullGroupBox1);
            this.MainPage.Controls.Add(this.NullGroupBox0);
            this.MainPage.Controls.Add(this.NullGroupBox);
            this.MainPage.Location = new System.Drawing.Point(4, 28);
            this.MainPage.Margin = new System.Windows.Forms.Padding(4);
            this.MainPage.Name = "MainPage";
            this.MainPage.Padding = new System.Windows.Forms.Padding(4);
            this.MainPage.Size = new System.Drawing.Size(1115, 446);
            this.MainPage.TabIndex = 0;
            this.MainPage.Text = "Main";
            // 
            // NullGroupBox1
            // 
            this.NullGroupBox1.AllowDrop = true;
            this.NullGroupBox1.Controls.Add(this.NulLabel);
            this.NullGroupBox1.Controls.Add(this.SNK_PasswordBox);
            this.NullGroupBox1.Controls.Add(this.SNKBox);
            this.NullGroupBox1.Controls.Add(this.BrowseSNKButton);
            this.NullGroupBox1.Controls.Add(this.NulLabel0);
            this.NullGroupBox1.Controls.Add(this.ActiveSNK_CheckBox);
            this.NullGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NullGroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox1.Location = new System.Drawing.Point(4, 199);
            this.NullGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox1.Name = "NullGroupBox1";
            this.NullGroupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox1.Size = new System.Drawing.Size(1107, 243);
            this.NullGroupBox1.TabIndex = 2;
            this.NullGroupBox1.TabStop = false;
            this.NullGroupBox1.Text = "Signing";
            // 
            // NulLabel
            // 
            this.NulLabel.AllowDrop = true;
            this.NulLabel.AutoSize = true;
            this.NulLabel.Location = new System.Drawing.Point(23, 44);
            this.NulLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NulLabel.Name = "NulLabel";
            this.NulLabel.Size = new System.Drawing.Size(187, 23);
            this.NulLabel.TabIndex = 8;
            this.NulLabel.Text = "Strong Name Key (SNK):";
            // 
            // SNK_PasswordBox
            // 
            this.SNK_PasswordBox.AllowDrop = true;
            this.SNK_PasswordBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SNK_PasswordBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SNK_PasswordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SNK_PasswordBox.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Dark;
            this.SNK_PasswordBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.SNK_PasswordBox.ForeColor = System.Drawing.Color.White;
            this.SNK_PasswordBox.Location = new System.Drawing.Point(27, 154);
            this.SNK_PasswordBox.Margin = new System.Windows.Forms.Padding(4);
            this.SNK_PasswordBox.Name = "SNK_PasswordBox";
            this.SNK_PasswordBox.Size = new System.Drawing.Size(1050, 29);
            this.SNK_PasswordBox.TabIndex = 7;
            this.SNK_PasswordBox.TabStop = false;
            // 
            // SNKBox
            // 
            this.SNKBox.AllowDrop = true;
            this.SNKBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SNKBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.SNKBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SNKBox.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Dark;
            this.SNKBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.SNKBox.ForeColor = System.Drawing.Color.White;
            this.SNKBox.Location = new System.Drawing.Point(27, 69);
            this.SNKBox.Margin = new System.Windows.Forms.Padding(4);
            this.SNKBox.Name = "SNKBox";
            this.SNKBox.Size = new System.Drawing.Size(932, 29);
            this.SNKBox.TabIndex = 6;
            this.SNKBox.TabStop = false;
            this.SNKBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.SNK_DragDrop);
            this.SNKBox.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // BrowseSNKButton
            // 
            this.BrowseSNKButton.AccentColor = System.Drawing.Color.Gray;
            this.BrowseSNKButton.AllowDrop = true;
            this.BrowseSNKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseSNKButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BrowseSNKButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.BrowseSNKButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BrowseSNKButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.BrowseSNKButton.ForeColor = System.Drawing.Color.White;
            this.BrowseSNKButton.Location = new System.Drawing.Point(967, 69);
            this.BrowseSNKButton.Margin = new System.Windows.Forms.Padding(4);
            this.BrowseSNKButton.Name = "BrowseSNKButton";
            this.BrowseSNKButton.Size = new System.Drawing.Size(111, 31);
            this.BrowseSNKButton.TabIndex = 5;
            this.BrowseSNKButton.TabStop = false;
            this.BrowseSNKButton.Text = "Browse...";
            this.BrowseSNKButton.UseVisualStyleBackColor = false;
            this.BrowseSNKButton.Click += new System.EventHandler(this.BrowseSNKButton_Click);
            // 
            // NulLabel0
            // 
            this.NulLabel0.AllowDrop = true;
            this.NulLabel0.AutoSize = true;
            this.NulLabel0.Location = new System.Drawing.Point(23, 123);
            this.NulLabel0.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NulLabel0.Name = "NulLabel0";
            this.NulLabel0.Size = new System.Drawing.Size(114, 23);
            this.NulLabel0.TabIndex = 4;
            this.NulLabel0.Text = "Key Password:";
            // 
            // ActiveSNK_CheckBox
            // 
            this.ActiveSNK_CheckBox.AllowDrop = true;
            this.ActiveSNK_CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveSNK_CheckBox.AutoSize = true;
            this.ActiveSNK_CheckBox.Location = new System.Drawing.Point(931, 208);
            this.ActiveSNK_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.ActiveSNK_CheckBox.Name = "ActiveSNK_CheckBox";
            this.ActiveSNK_CheckBox.Size = new System.Drawing.Size(171, 27);
            this.ActiveSNK_CheckBox.TabIndex = 0;
            this.ActiveSNK_CheckBox.TabStop = false;
            this.ActiveSNK_CheckBox.Text = "Sign The Assembly";
            this.ActiveSNK_CheckBox.UseVisualStyleBackColor = true;
            this.ActiveSNK_CheckBox.CheckedChanged += new System.EventHandler(this.ActiveSNK_CheckBox_CheckedChanged);
            // 
            // NullGroupBox0
            // 
            this.NullGroupBox0.AllowDrop = true;
            this.NullGroupBox0.Controls.Add(this.OutDestination);
            this.NullGroupBox0.Dock = System.Windows.Forms.DockStyle.Top;
            this.NullGroupBox0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox0.Location = new System.Drawing.Point(4, 101);
            this.NullGroupBox0.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox0.Name = "NullGroupBox0";
            this.NullGroupBox0.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox0.Size = new System.Drawing.Size(1107, 98);
            this.NullGroupBox0.TabIndex = 1;
            this.NullGroupBox0.TabStop = false;
            this.NullGroupBox0.Text = "Destination";
            // 
            // OutDestination
            // 
            this.OutDestination.AllowDrop = true;
            this.OutDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutDestination.BackColor = System.Drawing.Color.White;
            this.OutDestination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutDestination.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Light;
            this.OutDestination.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.OutDestination.ForeColor = System.Drawing.Color.Black;
            this.OutDestination.Location = new System.Drawing.Point(27, 41);
            this.OutDestination.Margin = new System.Windows.Forms.Padding(4);
            this.OutDestination.Name = "OutDestination";
            this.OutDestination.ReadOnly = true;
            this.OutDestination.Size = new System.Drawing.Size(1050, 29);
            this.OutDestination.TabIndex = 1;
            this.OutDestination.TabStop = false;
            // 
            // NullGroupBox
            // 
            this.NullGroupBox.AllowDrop = true;
            this.NullGroupBox.Controls.Add(this.BrowseAsmButton);
            this.NullGroupBox.Controls.Add(this.AsmTextBox);
            this.NullGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.NullGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox.Location = new System.Drawing.Point(4, 4);
            this.NullGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox.Name = "NullGroupBox";
            this.NullGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox.Size = new System.Drawing.Size(1107, 97);
            this.NullGroupBox.TabIndex = 0;
            this.NullGroupBox.TabStop = false;
            this.NullGroupBox.Text = "Assembly";
            // 
            // BrowseAsmButton
            // 
            this.BrowseAsmButton.AccentColor = System.Drawing.Color.Gray;
            this.BrowseAsmButton.AllowDrop = true;
            this.BrowseAsmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseAsmButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BrowseAsmButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.BrowseAsmButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BrowseAsmButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.BrowseAsmButton.ForeColor = System.Drawing.Color.White;
            this.BrowseAsmButton.Location = new System.Drawing.Point(967, 41);
            this.BrowseAsmButton.Margin = new System.Windows.Forms.Padding(4);
            this.BrowseAsmButton.Name = "BrowseAsmButton";
            this.BrowseAsmButton.Size = new System.Drawing.Size(111, 31);
            this.BrowseAsmButton.TabIndex = 1;
            this.BrowseAsmButton.TabStop = false;
            this.BrowseAsmButton.Text = "Browse...";
            this.BrowseAsmButton.UseVisualStyleBackColor = false;
            this.BrowseAsmButton.Click += new System.EventHandler(this.BrowseAsmButton_Click);
            // 
            // AsmTextBox
            // 
            this.AsmTextBox.AllowDrop = true;
            this.AsmTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AsmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.AsmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AsmTextBox.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Dark;
            this.AsmTextBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.AsmTextBox.ForeColor = System.Drawing.Color.White;
            this.AsmTextBox.Location = new System.Drawing.Point(27, 41);
            this.AsmTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.AsmTextBox.Name = "AsmTextBox";
            this.AsmTextBox.Size = new System.Drawing.Size(932, 29);
            this.AsmTextBox.TabIndex = 0;
            this.AsmTextBox.TabStop = false;
            this.AsmTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.Asm_DragDrop);
            this.AsmTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // ProtectionPage
            // 
            this.ProtectionPage.AllowDrop = true;
            this.ProtectionPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ProtectionPage.Controls.Add(this.NullGroupBox2);
            this.ProtectionPage.Location = new System.Drawing.Point(4, 28);
            this.ProtectionPage.Margin = new System.Windows.Forms.Padding(4);
            this.ProtectionPage.Name = "ProtectionPage";
            this.ProtectionPage.Padding = new System.Windows.Forms.Padding(4);
            this.ProtectionPage.Size = new System.Drawing.Size(1115, 446);
            this.ProtectionPage.TabIndex = 1;
            this.ProtectionPage.Text = "Protection";
            // 
            // NullGroupBox2
            // 
            this.NullGroupBox2.AllowDrop = true;
            this.NullGroupBox2.Controls.Add(this.NullGroupBox8);
            this.NullGroupBox2.Controls.Add(this.NullGroupBox9);
            this.NullGroupBox2.Controls.Add(this.NullGroupBox10);
            this.NullGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NullGroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox2.Location = new System.Drawing.Point(4, 4);
            this.NullGroupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox2.Name = "NullGroupBox2";
            this.NullGroupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox2.Size = new System.Drawing.Size(1107, 438);
            this.NullGroupBox2.TabIndex = 1;
            this.NullGroupBox2.TabStop = false;
            this.NullGroupBox2.Text = "Options";
            // 
            // NullGroupBox8
            // 
            this.NullGroupBox8.AllowDrop = true;
            this.NullGroupBox8.Controls.Add(this.ProcessMonitor_CheckBox);
            this.NullGroupBox8.Controls.Add(this.antiDump_CheckBox);
            this.NullGroupBox8.Controls.Add(this.antiDebug_CheckBox);
            this.NullGroupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NullGroupBox8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox8.Location = new System.Drawing.Point(4, 26);
            this.NullGroupBox8.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox8.Name = "NullGroupBox8";
            this.NullGroupBox8.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox8.Size = new System.Drawing.Size(369, 408);
            this.NullGroupBox8.TabIndex = 6;
            this.NullGroupBox8.TabStop = false;
            this.NullGroupBox8.Text = "Antis";
            // 
            // ProcessMonitor_CheckBox
            // 
            this.ProcessMonitor_CheckBox.AllowDrop = true;
            this.ProcessMonitor_CheckBox.AutoSize = true;
            this.ProcessMonitor_CheckBox.Location = new System.Drawing.Point(16, 104);
            this.ProcessMonitor_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.ProcessMonitor_CheckBox.Name = "ProcessMonitor_CheckBox";
            this.ProcessMonitor_CheckBox.Size = new System.Drawing.Size(152, 27);
            this.ProcessMonitor_CheckBox.TabIndex = 5;
            this.ProcessMonitor_CheckBox.TabStop = false;
            this.ProcessMonitor_CheckBox.Text = "Process Monitor";
            this.ProcessMonitor_ToolTip.SetToolTip(this.ProcessMonitor_CheckBox, "Using this mode will increase CPU consumption.");
            this.ProcessMonitor_CheckBox.UseVisualStyleBackColor = true;
            // 
            // antiDump_CheckBox
            // 
            this.antiDump_CheckBox.AllowDrop = true;
            this.antiDump_CheckBox.AutoSize = true;
            this.antiDump_CheckBox.Location = new System.Drawing.Point(16, 69);
            this.antiDump_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.antiDump_CheckBox.Name = "antiDump_CheckBox";
            this.antiDump_CheckBox.Size = new System.Drawing.Size(111, 27);
            this.antiDump_CheckBox.TabIndex = 4;
            this.antiDump_CheckBox.TabStop = false;
            this.antiDump_CheckBox.Text = "Anti Dump";
            this.antiDump_CheckBox.UseVisualStyleBackColor = true;
            // 
            // antiDebug_CheckBox
            // 
            this.antiDebug_CheckBox.AllowDrop = true;
            this.antiDebug_CheckBox.AutoSize = true;
            this.antiDebug_CheckBox.Location = new System.Drawing.Point(16, 36);
            this.antiDebug_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.antiDebug_CheckBox.Name = "antiDebug_CheckBox";
            this.antiDebug_CheckBox.Size = new System.Drawing.Size(116, 27);
            this.antiDebug_CheckBox.TabIndex = 3;
            this.antiDebug_CheckBox.TabStop = false;
            this.antiDebug_CheckBox.Text = "Anti Debug";
            this.antiDebug_CheckBox.UseVisualStyleBackColor = true;
            // 
            // NullGroupBox9
            // 
            this.NullGroupBox9.AllowDrop = true;
            this.NullGroupBox9.Controls.Add(this.IntEncoding_CheckBox);
            this.NullGroupBox9.Controls.Add(this.IntConfusion_CheckBox);
            this.NullGroupBox9.Controls.Add(this.Arithmetic_CheckBox);
            this.NullGroupBox9.Controls.Add(this.EncryptAllStrings_CheckBox);
            this.NullGroupBox9.Controls.Add(this.renamer_CheckBox);
            this.NullGroupBox9.Controls.Add(this.VirtualizeAllStrings_CheckBox);
            this.NullGroupBox9.Dock = System.Windows.Forms.DockStyle.Right;
            this.NullGroupBox9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox9.Location = new System.Drawing.Point(373, 26);
            this.NullGroupBox9.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox9.Name = "NullGroupBox9";
            this.NullGroupBox9.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox9.Size = new System.Drawing.Size(365, 408);
            this.NullGroupBox9.TabIndex = 5;
            this.NullGroupBox9.TabStop = false;
            this.NullGroupBox9.Text = "Code Manglers";
            // 
            // IntEncoding_CheckBox
            // 
            this.IntEncoding_CheckBox.AllowDrop = true;
            this.IntEncoding_CheckBox.AutoSize = true;
            this.IntEncoding_CheckBox.Location = new System.Drawing.Point(16, 209);
            this.IntEncoding_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.IntEncoding_CheckBox.Name = "IntEncoding_CheckBox";
            this.IntEncoding_CheckBox.Size = new System.Drawing.Size(124, 27);
            this.IntEncoding_CheckBox.TabIndex = 6;
            this.IntEncoding_CheckBox.TabStop = false;
            this.IntEncoding_CheckBox.Text = "Int Encoding";
            this.IntEncoding_CheckBox.UseVisualStyleBackColor = true;
            // 
            // IntConfusion_CheckBox
            // 
            this.IntConfusion_CheckBox.AllowDrop = true;
            this.IntConfusion_CheckBox.AutoSize = true;
            this.IntConfusion_CheckBox.Location = new System.Drawing.Point(16, 174);
            this.IntConfusion_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.IntConfusion_CheckBox.Name = "IntConfusion_CheckBox";
            this.IntConfusion_CheckBox.Size = new System.Drawing.Size(129, 27);
            this.IntConfusion_CheckBox.TabIndex = 5;
            this.IntConfusion_CheckBox.TabStop = false;
            this.IntConfusion_CheckBox.Text = "Int Confusion";
            this.IntConfusion_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Arithmetic_CheckBox
            // 
            this.Arithmetic_CheckBox.AllowDrop = true;
            this.Arithmetic_CheckBox.AutoSize = true;
            this.Arithmetic_CheckBox.Location = new System.Drawing.Point(16, 139);
            this.Arithmetic_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Arithmetic_CheckBox.Name = "Arithmetic_CheckBox";
            this.Arithmetic_CheckBox.Size = new System.Drawing.Size(107, 27);
            this.Arithmetic_CheckBox.TabIndex = 4;
            this.Arithmetic_CheckBox.TabStop = false;
            this.Arithmetic_CheckBox.Text = "Arithmetic";
            this.Arithmetic_CheckBox.UseVisualStyleBackColor = true;
            // 
            // EncryptAllStrings_CheckBox
            // 
            this.EncryptAllStrings_CheckBox.AllowDrop = true;
            this.EncryptAllStrings_CheckBox.AutoSize = true;
            this.EncryptAllStrings_CheckBox.Location = new System.Drawing.Point(16, 104);
            this.EncryptAllStrings_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.EncryptAllStrings_CheckBox.Name = "EncryptAllStrings_CheckBox";
            this.EncryptAllStrings_CheckBox.Size = new System.Drawing.Size(166, 27);
            this.EncryptAllStrings_CheckBox.TabIndex = 3;
            this.EncryptAllStrings_CheckBox.TabStop = false;
            this.EncryptAllStrings_CheckBox.Text = "Encrypt All Strings";
            this.EncryptAllStrings_CheckBox.UseVisualStyleBackColor = true;
            // 
            // renamer_CheckBox
            // 
            this.renamer_CheckBox.AllowDrop = true;
            this.renamer_CheckBox.AutoSize = true;
            this.renamer_CheckBox.Location = new System.Drawing.Point(16, 36);
            this.renamer_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.renamer_CheckBox.Name = "renamer_CheckBox";
            this.renamer_CheckBox.Size = new System.Drawing.Size(236, 27);
            this.renamer_CheckBox.TabIndex = 2;
            this.renamer_CheckBox.TabStop = false;
            this.renamer_CheckBox.Text = "Renamer (Just EXEC) [BETA]";
            this.renamer_CheckBox.UseVisualStyleBackColor = true;
            // 
            // VirtualizeAllStrings_CheckBox
            // 
            this.VirtualizeAllStrings_CheckBox.AllowDrop = true;
            this.VirtualizeAllStrings_CheckBox.AutoSize = true;
            this.VirtualizeAllStrings_CheckBox.Location = new System.Drawing.Point(16, 69);
            this.VirtualizeAllStrings_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.VirtualizeAllStrings_CheckBox.Name = "VirtualizeAllStrings_CheckBox";
            this.VirtualizeAllStrings_CheckBox.Size = new System.Drawing.Size(178, 27);
            this.VirtualizeAllStrings_CheckBox.TabIndex = 1;
            this.VirtualizeAllStrings_CheckBox.TabStop = false;
            this.VirtualizeAllStrings_CheckBox.Text = "Virtualize All Strings";
            this.VirtualizeAllStrings_CheckBox.UseVisualStyleBackColor = true;
            // 
            // NullGroupBox10
            // 
            this.NullGroupBox10.AllowDrop = true;
            this.NullGroupBox10.Controls.Add(this.LocalToField_CheckBox);
            this.NullGroupBox10.Controls.Add(this.ImportProtection_CheckBox);
            this.NullGroupBox10.Controls.Add(this.RandomOutlineLength);
            this.NullGroupBox10.Controls.Add(this.JunkNumber);
            this.NullGroupBox10.Controls.Add(this.RandomOutline_ComboBox);
            this.NullGroupBox10.Controls.Add(this.RandomOutline_CheckBox);
            this.NullGroupBox10.Controls.Add(this.Junk_CheckBox);
            this.NullGroupBox10.Controls.Add(this.resourceProt_CheckBox);
            this.NullGroupBox10.Dock = System.Windows.Forms.DockStyle.Right;
            this.NullGroupBox10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox10.Location = new System.Drawing.Point(738, 26);
            this.NullGroupBox10.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox10.Name = "NullGroupBox10";
            this.NullGroupBox10.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox10.Size = new System.Drawing.Size(365, 408);
            this.NullGroupBox10.TabIndex = 4;
            this.NullGroupBox10.TabStop = false;
            this.NullGroupBox10.Text = "Misc";
            // 
            // RandomOutlineLength
            // 
            this.RandomOutlineLength.Location = new System.Drawing.Point(294, 104);
            this.RandomOutlineLength.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.RandomOutlineLength.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.RandomOutlineLength.Name = "RandomOutlineLength";
            this.RandomOutlineLength.Size = new System.Drawing.Size(64, 29);
            this.RandomOutlineLength.TabIndex = 20;
            this.RandomOutlineLength_ToolTip.SetToolTip(this.RandomOutlineLength, "Random Outline Length\r\n- Min: 15\r\n- Max: 150");
            this.RandomOutlineLength.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // JunkNumber
            // 
            this.JunkNumber.Location = new System.Drawing.Point(87, 70);
            this.JunkNumber.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.JunkNumber.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.JunkNumber.Name = "JunkNumber";
            this.JunkNumber.Size = new System.Drawing.Size(109, 29);
            this.JunkNumber.TabIndex = 18;
            this.JunkNumber_ToolTip.SetToolTip(this.JunkNumber, "Junk Number\r\n- Min: 100\r\n- Max: 1000000");
            this.JunkNumber.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // RandomOutline_ComboBox
            // 
            this.RandomOutline_ComboBox.AccentColor = System.Drawing.Color.Gray;
            this.RandomOutline_ComboBox.AllowDrop = true;
            this.RandomOutline_ComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.RandomOutline_ComboBox.ColorScheme = Teen.ThirteenComboBox.ColorSchemes.Dark;
            this.RandomOutline_ComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.RandomOutline_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RandomOutline_ComboBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.RandomOutline_ComboBox.ForeColor = System.Drawing.Color.White;
            this.RandomOutline_ComboBox.FormattingEnabled = true;
            this.RandomOutline_ComboBox.Items.AddRange(new object[] {
            "Ascii",
            "Numbers",
            "Symbols",
            "Hex"});
            this.RandomOutline_ComboBox.Location = new System.Drawing.Point(176, 103);
            this.RandomOutline_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.RandomOutline_ComboBox.Name = "RandomOutline_ComboBox";
            this.RandomOutline_ComboBox.Size = new System.Drawing.Size(111, 30);
            this.RandomOutline_ComboBox.TabIndex = 17;
            this.RandomOutline_ComboBox.TabStop = false;
            // 
            // RandomOutline_CheckBox
            // 
            this.RandomOutline_CheckBox.AllowDrop = true;
            this.RandomOutline_CheckBox.AutoSize = true;
            this.RandomOutline_CheckBox.Location = new System.Drawing.Point(16, 104);
            this.RandomOutline_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.RandomOutline_CheckBox.Name = "RandomOutline_CheckBox";
            this.RandomOutline_CheckBox.Size = new System.Drawing.Size(152, 27);
            this.RandomOutline_CheckBox.TabIndex = 6;
            this.RandomOutline_CheckBox.TabStop = false;
            this.RandomOutline_CheckBox.Text = "Random Outline";
            this.RandomOutline_CheckBox.UseVisualStyleBackColor = true;
            this.RandomOutline_CheckBox.CheckedChanged += new System.EventHandler(this.RandomOutline_CheckBox_CheckedChanged);
            // 
            // Junk_CheckBox
            // 
            this.Junk_CheckBox.AllowDrop = true;
            this.Junk_CheckBox.AutoSize = true;
            this.Junk_CheckBox.Location = new System.Drawing.Point(16, 69);
            this.Junk_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.Junk_CheckBox.Name = "Junk_CheckBox";
            this.Junk_CheckBox.Size = new System.Drawing.Size(64, 27);
            this.Junk_CheckBox.TabIndex = 4;
            this.Junk_CheckBox.TabStop = false;
            this.Junk_CheckBox.Text = "Junk";
            this.Junk_ToolTip.SetToolTip(this.Junk_CheckBox, "If you choose this mode, your file will increase in file size.");
            this.Junk_CheckBox.UseVisualStyleBackColor = true;
            // 
            // resourceProt_CheckBox
            // 
            this.resourceProt_CheckBox.AllowDrop = true;
            this.resourceProt_CheckBox.AutoSize = true;
            this.resourceProt_CheckBox.Location = new System.Drawing.Point(16, 36);
            this.resourceProt_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.resourceProt_CheckBox.Name = "resourceProt_CheckBox";
            this.resourceProt_CheckBox.Size = new System.Drawing.Size(180, 27);
            this.resourceProt_CheckBox.TabIndex = 3;
            this.resourceProt_CheckBox.TabStop = false;
            this.resourceProt_CheckBox.Text = "Resource Protection";
            this.resourceProt_CheckBox.UseVisualStyleBackColor = true;
            // 
            // FunctionsPage
            // 
            this.FunctionsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.FunctionsPage.Controls.Add(this.MethodTree);
            this.FunctionsPage.Location = new System.Drawing.Point(4, 28);
            this.FunctionsPage.Margin = new System.Windows.Forms.Padding(4);
            this.FunctionsPage.Name = "FunctionsPage";
            this.FunctionsPage.Padding = new System.Windows.Forms.Padding(4);
            this.FunctionsPage.Size = new System.Drawing.Size(1115, 446);
            this.FunctionsPage.TabIndex = 6;
            this.FunctionsPage.Text = "Functions";
            // 
            // MethodTree
            // 
            this.MethodTree.AllowDrop = true;
            this.MethodTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.MethodTree.ContextMenuStrip = this.MethodTree_ContextMenu;
            this.MethodTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MethodTree.ForeColor = System.Drawing.Color.White;
            this.MethodTree.ImageIndex = 0;
            this.MethodTree.ImageList = this.IMGList;
            this.MethodTree.Location = new System.Drawing.Point(4, 4);
            this.MethodTree.Margin = new System.Windows.Forms.Padding(4);
            this.MethodTree.Name = "MethodTree";
            this.MethodTree.SelectedImageIndex = 0;
            this.MethodTree.ShowPlusMinus = false;
            this.MethodTree.ShowRootLines = false;
            this.MethodTree.Size = new System.Drawing.Size(1107, 438);
            this.MethodTree.TabIndex = 0;
            this.MethodTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.MethodTree_BeforeCollapse);
            this.MethodTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.MethodTree_BeforeExpand);
            this.MethodTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.MethodTree_NodeMouseClick);
            this.MethodTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MethodTree_MouseClick);
            // 
            // MethodTree_ContextMenu
            // 
            this.MethodTree_ContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MethodTree_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mutateMethodToolStripMenuItem,
            this.virtualizeMethodToolStripMenuItem,
            this.ultraToolStripMenuItem,
            this.UnSelectToolStripMenuItem,
            this.methodAboutToolStripMenuItem});
            this.MethodTree_ContextMenu.Name = "MethodTree_ContextMenu";
            this.MethodTree_ContextMenu.Size = new System.Drawing.Size(201, 134);
            this.MethodTree_ContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MethodTree_ContextMenu_Opening);
            // 
            // mutateMethodToolStripMenuItem
            // 
            this.mutateMethodToolStripMenuItem.Image = global::xVM.GUI.Properties.Resources.text_x_cobol;
            this.mutateMethodToolStripMenuItem.Name = "mutateMethodToolStripMenuItem";
            this.mutateMethodToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.mutateMethodToolStripMenuItem.Text = "Mutate Method";
            this.mutateMethodToolStripMenuItem.Click += new System.EventHandler(this.mutateMethodToolStripMenuItem_Click);
            // 
            // virtualizeMethodToolStripMenuItem
            // 
            this.virtualizeMethodToolStripMenuItem.Image = global::xVM.GUI.Properties.Resources.application_x_pkcs7_certificates;
            this.virtualizeMethodToolStripMenuItem.Name = "virtualizeMethodToolStripMenuItem";
            this.virtualizeMethodToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.virtualizeMethodToolStripMenuItem.Text = "Virtualize Method";
            this.virtualizeMethodToolStripMenuItem.Click += new System.EventHandler(this.virtualizeMethodToolStripMenuItem_Click);
            // 
            // ultraToolStripMenuItem
            // 
            this.ultraToolStripMenuItem.Image = global::xVM.GUI.Properties.Resources.text_x_ruby;
            this.ultraToolStripMenuItem.Name = "ultraToolStripMenuItem";
            this.ultraToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.ultraToolStripMenuItem.Text = "Ultra";
            this.ultraToolStripMenuItem.Click += new System.EventHandler(this.ultraToolStripMenuItem_Click);
            // 
            // UnSelectToolStripMenuItem
            // 
            this.UnSelectToolStripMenuItem.Image = global::xVM.GUI.Properties.Resources.text_x_csharp;
            this.UnSelectToolStripMenuItem.Name = "UnSelectToolStripMenuItem";
            this.UnSelectToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.UnSelectToolStripMenuItem.Text = "UnSelect";
            this.UnSelectToolStripMenuItem.Click += new System.EventHandler(this.UnSelectToolStripMenuItem_Click);
            // 
            // methodAboutToolStripMenuItem
            // 
            this.methodAboutToolStripMenuItem.Image = global::xVM.GUI.Properties.Resources.IconInfo;
            this.methodAboutToolStripMenuItem.Name = "methodAboutToolStripMenuItem";
            this.methodAboutToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.methodAboutToolStripMenuItem.Text = "Method About";
            this.methodAboutToolStripMenuItem.Click += new System.EventHandler(this.methodAboutToolStripMenuItem_Click);
            // 
            // IMGList
            // 
            this.IMGList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IMGList.ImageStream")));
            this.IMGList.TransparentColor = System.Drawing.Color.Transparent;
            this.IMGList.Images.SetKeyName(0, "folder.png");
            this.IMGList.Images.SetKeyName(1, "folder-open.png");
            this.IMGList.Images.SetKeyName(2, "public_method.png");
            this.IMGList.Images.SetKeyName(3, "private_method.png");
            this.IMGList.Images.SetKeyName(4, "internal_method.png");
            this.IMGList.Images.SetKeyName(5, "protected_method.png");
            this.IMGList.Images.SetKeyName(6, "protected_internal_method.png");
            this.IMGList.Images.SetKeyName(7, "mutation_img.png");
            this.IMGList.Images.SetKeyName(8, "virt_img.png");
            this.IMGList.Images.SetKeyName(9, "ultra_img.png");
            this.IMGList.Images.SetKeyName(10, "IconInfo.ico");
            // 
            // MergeNETPage
            // 
            this.MergeNETPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.MergeNETPage.Controls.Add(this.groupBox1);
            this.MergeNETPage.Location = new System.Drawing.Point(4, 28);
            this.MergeNETPage.Margin = new System.Windows.Forms.Padding(4);
            this.MergeNETPage.Name = "MergeNETPage";
            this.MergeNETPage.Size = new System.Drawing.Size(1115, 446);
            this.MergeNETPage.TabIndex = 7;
            this.MergeNETPage.Text = "Merge .NET Assembly (BETA)";
            // 
            // groupBox1
            // 
            this.groupBox1.AllowDrop = true;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.NETAssembly_ListBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1115, 446);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Merge";
            // 
            // groupBox2
            // 
            this.groupBox2.AllowDrop = true;
            this.groupBox2.Controls.Add(this.MergeNET_AddLibrary_Button);
            this.groupBox2.Controls.Add(this.MergeNET_SelecetAndRemove_Button);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox2.Location = new System.Drawing.Point(849, 16);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(255, 127);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // MergeNET_AddLibrary_Button
            // 
            this.MergeNET_AddLibrary_Button.AccentColor = System.Drawing.Color.Gray;
            this.MergeNET_AddLibrary_Button.AllowDrop = true;
            this.MergeNET_AddLibrary_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeNET_AddLibrary_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.MergeNET_AddLibrary_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.MergeNET_AddLibrary_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MergeNET_AddLibrary_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.MergeNET_AddLibrary_Button.ForeColor = System.Drawing.Color.White;
            this.MergeNET_AddLibrary_Button.Location = new System.Drawing.Point(19, 30);
            this.MergeNET_AddLibrary_Button.Margin = new System.Windows.Forms.Padding(4);
            this.MergeNET_AddLibrary_Button.Name = "MergeNET_AddLibrary_Button";
            this.MergeNET_AddLibrary_Button.Size = new System.Drawing.Size(216, 36);
            this.MergeNET_AddLibrary_Button.TabIndex = 2;
            this.MergeNET_AddLibrary_Button.TabStop = false;
            this.MergeNET_AddLibrary_Button.Text = "Add";
            this.MergeNET_AddLibrary_Button.UseVisualStyleBackColor = false;
            this.MergeNET_AddLibrary_Button.Click += new System.EventHandler(this.MergeNET_AddLibrary_Button_Click);
            // 
            // MergeNET_SelecetAndRemove_Button
            // 
            this.MergeNET_SelecetAndRemove_Button.AccentColor = System.Drawing.Color.Gray;
            this.MergeNET_SelecetAndRemove_Button.AllowDrop = true;
            this.MergeNET_SelecetAndRemove_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeNET_SelecetAndRemove_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.MergeNET_SelecetAndRemove_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.MergeNET_SelecetAndRemove_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MergeNET_SelecetAndRemove_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.MergeNET_SelecetAndRemove_Button.ForeColor = System.Drawing.Color.White;
            this.MergeNET_SelecetAndRemove_Button.Location = new System.Drawing.Point(19, 73);
            this.MergeNET_SelecetAndRemove_Button.Margin = new System.Windows.Forms.Padding(4);
            this.MergeNET_SelecetAndRemove_Button.Name = "MergeNET_SelecetAndRemove_Button";
            this.MergeNET_SelecetAndRemove_Button.Size = new System.Drawing.Size(216, 36);
            this.MergeNET_SelecetAndRemove_Button.TabIndex = 3;
            this.MergeNET_SelecetAndRemove_Button.TabStop = false;
            this.MergeNET_SelecetAndRemove_Button.Text = "Select and Remove";
            this.MergeNET_SelecetAndRemove_Button.UseVisualStyleBackColor = false;
            this.MergeNET_SelecetAndRemove_Button.Click += new System.EventHandler(this.MergeNET_SelecetAndRemove_Button_Click);
            // 
            // NETAssembly_ListBox
            // 
            this.NETAssembly_ListBox.AllowDrop = true;
            this.NETAssembly_ListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.NETAssembly_ListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.NETAssembly_ListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NETAssembly_ListBox.FormattingEnabled = true;
            this.NETAssembly_ListBox.ItemHeight = 21;
            this.NETAssembly_ListBox.Location = new System.Drawing.Point(4, 26);
            this.NETAssembly_ListBox.Margin = new System.Windows.Forms.Padding(4);
            this.NETAssembly_ListBox.Name = "NETAssembly_ListBox";
            this.NETAssembly_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.NETAssembly_ListBox.Size = new System.Drawing.Size(836, 416);
            this.NETAssembly_ListBox.TabIndex = 0;
            this.NETAssembly_ListBox.TabStop = false;
            this.NETAssembly_ListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.NETAssembly_ListBox_DragDrop);
            this.NETAssembly_ListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            this.NETAssembly_ListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NETAssembly_ListBox_KeyDown);
            // 
            // RuntimePage
            // 
            this.RuntimePage.AllowDrop = true;
            this.RuntimePage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.RuntimePage.Controls.Add(this.NullGroupBox4);
            this.RuntimePage.Location = new System.Drawing.Point(4, 28);
            this.RuntimePage.Margin = new System.Windows.Forms.Padding(4);
            this.RuntimePage.Name = "RuntimePage";
            this.RuntimePage.Padding = new System.Windows.Forms.Padding(4);
            this.RuntimePage.Size = new System.Drawing.Size(1115, 446);
            this.RuntimePage.TabIndex = 3;
            this.RuntimePage.Text = "Runtime Settings";
            // 
            // NullGroupBox4
            // 
            this.NullGroupBox4.AllowDrop = true;
            this.NullGroupBox4.Controls.Add(this.NullGroupBox6);
            this.NullGroupBox4.Controls.Add(this.NullGroupBox5);
            this.NullGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NullGroupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox4.Location = new System.Drawing.Point(4, 4);
            this.NullGroupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox4.Name = "NullGroupBox4";
            this.NullGroupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox4.Size = new System.Drawing.Size(1107, 438);
            this.NullGroupBox4.TabIndex = 2;
            this.NullGroupBox4.TabStop = false;
            this.NullGroupBox4.Text = "Runtime Options";
            // 
            // NullGroupBox6
            // 
            this.NullGroupBox6.AllowDrop = true;
            this.NullGroupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NullGroupBox6.Controls.Add(this.ModeComboBox);
            this.NullGroupBox6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox6.Location = new System.Drawing.Point(791, 0);
            this.NullGroupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox6.Name = "NullGroupBox6";
            this.NullGroupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox6.Size = new System.Drawing.Size(316, 69);
            this.NullGroupBox6.TabIndex = 4;
            this.NullGroupBox6.TabStop = false;
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.AccentColor = System.Drawing.Color.Gray;
            this.ModeComboBox.AllowDrop = true;
            this.ModeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ModeComboBox.ColorScheme = Teen.ThirteenComboBox.ColorSchemes.Dark;
            this.ModeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeComboBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.ModeComboBox.ForeColor = System.Drawing.Color.White;
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "Normal Runtime Protect",
            "Normal Runtime Protect + Merge"});
            this.ModeComboBox.Location = new System.Drawing.Point(8, 25);
            this.ModeComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(299, 30);
            this.ModeComboBox.TabIndex = 5;
            this.ModeComboBox.TabStop = false;
            this.ModeComboBox.SelectedIndexChanged += new System.EventHandler(this.ModeComboBox_SelectedIndexChanged);
            // 
            // NullGroupBox5
            // 
            this.NullGroupBox5.AllowDrop = true;
            this.NullGroupBox5.Controls.Add(this.RandomRuntimeName_Button);
            this.NullGroupBox5.Controls.Add(this.RuntimeNameBox);
            this.NullGroupBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox5.Location = new System.Drawing.Point(24, 31);
            this.NullGroupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox5.Name = "NullGroupBox5";
            this.NullGroupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox5.Size = new System.Drawing.Size(383, 114);
            this.NullGroupBox5.TabIndex = 3;
            this.NullGroupBox5.TabStop = false;
            this.NullGroupBox5.Text = "Runtime Name";
            // 
            // RandomRuntimeName_Button
            // 
            this.RandomRuntimeName_Button.AccentColor = System.Drawing.Color.Gray;
            this.RandomRuntimeName_Button.AllowDrop = true;
            this.RandomRuntimeName_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.RandomRuntimeName_Button.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.RandomRuntimeName_Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RandomRuntimeName_Button.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.RandomRuntimeName_Button.ForeColor = System.Drawing.Color.White;
            this.RandomRuntimeName_Button.Location = new System.Drawing.Point(223, 68);
            this.RandomRuntimeName_Button.Margin = new System.Windows.Forms.Padding(4);
            this.RandomRuntimeName_Button.Name = "RandomRuntimeName_Button";
            this.RandomRuntimeName_Button.Size = new System.Drawing.Size(139, 28);
            this.RandomRuntimeName_Button.TabIndex = 8;
            this.RandomRuntimeName_Button.TabStop = false;
            this.RandomRuntimeName_Button.Text = "Random Name";
            this.RandomRuntimeName_Button.UseVisualStyleBackColor = false;
            this.RandomRuntimeName_Button.Click += new System.EventHandler(this.RandomRuntimeName_Button_Click);
            // 
            // RuntimeNameBox
            // 
            this.RuntimeNameBox.AllowDrop = true;
            this.RuntimeNameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.RuntimeNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RuntimeNameBox.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Dark;
            this.RuntimeNameBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.RuntimeNameBox.ForeColor = System.Drawing.Color.White;
            this.RuntimeNameBox.Location = new System.Drawing.Point(20, 30);
            this.RuntimeNameBox.Margin = new System.Windows.Forms.Padding(4);
            this.RuntimeNameBox.Name = "RuntimeNameBox";
            this.RuntimeNameBox.Size = new System.Drawing.Size(341, 29);
            this.RuntimeNameBox.TabIndex = 4;
            this.RuntimeNameBox.TabStop = false;
            // 
            // LogPage
            // 
            this.LogPage.AllowDrop = true;
            this.LogPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.LogPage.Controls.Add(this.Log_RichTextBox);
            this.LogPage.Location = new System.Drawing.Point(4, 28);
            this.LogPage.Margin = new System.Windows.Forms.Padding(4);
            this.LogPage.Name = "LogPage";
            this.LogPage.Padding = new System.Windows.Forms.Padding(4);
            this.LogPage.Size = new System.Drawing.Size(1115, 446);
            this.LogPage.TabIndex = 4;
            this.LogPage.Text = "Log";
            // 
            // Log_RichTextBox
            // 
            this.Log_RichTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Log_RichTextBox.DetectUrls = false;
            this.Log_RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Log_RichTextBox.ForeColor = System.Drawing.Color.White;
            this.Log_RichTextBox.Location = new System.Drawing.Point(4, 4);
            this.Log_RichTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.Log_RichTextBox.Name = "Log_RichTextBox";
            this.Log_RichTextBox.ReadOnly = true;
            this.Log_RichTextBox.Size = new System.Drawing.Size(1107, 438);
            this.Log_RichTextBox.TabIndex = 0;
            this.Log_RichTextBox.Text = "";
            this.Log_RichTextBox.WordWrap = false;
            // 
            // ToolsPage
            // 
            this.ToolsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ToolsPage.Controls.Add(this.NullGroupBox7);
            this.ToolsPage.Location = new System.Drawing.Point(4, 28);
            this.ToolsPage.Margin = new System.Windows.Forms.Padding(4);
            this.ToolsPage.Name = "ToolsPage";
            this.ToolsPage.Padding = new System.Windows.Forms.Padding(4);
            this.ToolsPage.Size = new System.Drawing.Size(1115, 446);
            this.ToolsPage.TabIndex = 5;
            this.ToolsPage.Text = "xVM Tools";
            // 
            // NullGroupBox7
            // 
            this.NullGroupBox7.AllowDrop = true;
            this.NullGroupBox7.Controls.Add(this.HashUpdaterToolButton);
            this.NullGroupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NullGroupBox7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox7.Location = new System.Drawing.Point(4, 4);
            this.NullGroupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.NullGroupBox7.Name = "NullGroupBox7";
            this.NullGroupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.NullGroupBox7.Size = new System.Drawing.Size(1107, 438);
            this.NullGroupBox7.TabIndex = 11;
            this.NullGroupBox7.TabStop = false;
            this.NullGroupBox7.Text = "Tools";
            // 
            // HashUpdaterToolButton
            // 
            this.HashUpdaterToolButton.AccentColor = System.Drawing.Color.Gray;
            this.HashUpdaterToolButton.AllowDrop = true;
            this.HashUpdaterToolButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.HashUpdaterToolButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.HashUpdaterToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.HashUpdaterToolButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.HashUpdaterToolButton.ForeColor = System.Drawing.Color.White;
            this.HashUpdaterToolButton.Location = new System.Drawing.Point(21, 33);
            this.HashUpdaterToolButton.Margin = new System.Windows.Forms.Padding(4);
            this.HashUpdaterToolButton.Name = "HashUpdaterToolButton";
            this.HashUpdaterToolButton.Size = new System.Drawing.Size(172, 28);
            this.HashUpdaterToolButton.TabIndex = 10;
            this.HashUpdaterToolButton.TabStop = false;
            this.HashUpdaterToolButton.Text = "Hash Updater Tool";
            this.HashUpdaterToolButton.UseVisualStyleBackColor = false;
            this.HashUpdaterToolButton.Click += new System.EventHandler(this.HashUpdaterToolButton_Click);
            // 
            // Junk_ToolTip
            // 
            this.Junk_ToolTip.AutoPopDelay = 10000;
            this.Junk_ToolTip.InitialDelay = 100;
            this.Junk_ToolTip.IsBalloon = true;
            this.Junk_ToolTip.ReshowDelay = 100;
            this.Junk_ToolTip.ShowAlways = true;
            this.Junk_ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.Junk_ToolTip.ToolTipTitle = "Warning";
            // 
            // NopAttack_ToolTip
            // 
            this.NopAttack_ToolTip.AutoPopDelay = 10000;
            this.NopAttack_ToolTip.InitialDelay = 100;
            this.NopAttack_ToolTip.IsBalloon = true;
            this.NopAttack_ToolTip.ReshowDelay = 100;
            this.NopAttack_ToolTip.ShowAlways = true;
            this.NopAttack_ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.NopAttack_ToolTip.ToolTipTitle = "Warning";
            // 
            // JunkNumber_ToolTip
            // 
            this.JunkNumber_ToolTip.AutoPopDelay = 10000;
            this.JunkNumber_ToolTip.InitialDelay = 100;
            this.JunkNumber_ToolTip.IsBalloon = true;
            this.JunkNumber_ToolTip.ReshowDelay = 100;
            this.JunkNumber_ToolTip.ShowAlways = true;
            this.JunkNumber_ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.JunkNumber_ToolTip.ToolTipTitle = "Infomation";
            // 
            // RandomOutlineLength_ToolTip
            // 
            this.RandomOutlineLength_ToolTip.AutoPopDelay = 10000;
            this.RandomOutlineLength_ToolTip.InitialDelay = 100;
            this.RandomOutlineLength_ToolTip.IsBalloon = true;
            this.RandomOutlineLength_ToolTip.ReshowDelay = 100;
            this.RandomOutlineLength_ToolTip.ShowAlways = true;
            this.RandomOutlineLength_ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.RandomOutlineLength_ToolTip.ToolTipTitle = "Infomation";
            // 
            // ProcessMonitor_ToolTip
            // 
            this.ProcessMonitor_ToolTip.AutoPopDelay = 10000;
            this.ProcessMonitor_ToolTip.InitialDelay = 100;
            this.ProcessMonitor_ToolTip.IsBalloon = true;
            this.ProcessMonitor_ToolTip.ReshowDelay = 100;
            this.ProcessMonitor_ToolTip.ShowAlways = true;
            this.ProcessMonitor_ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.ProcessMonitor_ToolTip.ToolTipTitle = "Warning";
            // 
            // ImportProtection_CheckBox
            // 
            this.ImportProtection_CheckBox.AllowDrop = true;
            this.ImportProtection_CheckBox.AutoSize = true;
            this.ImportProtection_CheckBox.Location = new System.Drawing.Point(16, 139);
            this.ImportProtection_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.ImportProtection_CheckBox.Name = "ImportProtection_CheckBox";
            this.ImportProtection_CheckBox.Size = new System.Drawing.Size(161, 27);
            this.ImportProtection_CheckBox.TabIndex = 7;
            this.ImportProtection_CheckBox.TabStop = false;
            this.ImportProtection_CheckBox.Text = "Import Protection";
            this.ImportProtection_CheckBox.UseVisualStyleBackColor = true;
            // 
            // LocalToField_CheckBox
            // 
            this.LocalToField_CheckBox.AllowDrop = true;
            this.LocalToField_CheckBox.AutoSize = true;
            this.LocalToField_CheckBox.Location = new System.Drawing.Point(16, 174);
            this.LocalToField_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.LocalToField_CheckBox.Name = "LocalToField_CheckBox";
            this.LocalToField_CheckBox.Size = new System.Drawing.Size(133, 27);
            this.LocalToField_CheckBox.TabIndex = 21;
            this.LocalToField_CheckBox.TabStop = false;
            this.LocalToField_CheckBox.Text = "Local To Field";
            this.LocalToField_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(1155, 598);
            this.ControlBox = false;
            this.Controls.Add(this.TeenForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xVM.GUI";
            this.Load += new System.EventHandler(this._Load);
            this.TeenForm.ResumeLayout(false);
            this.TeenForm.PerformLayout();
            this.TControl.ResumeLayout(false);
            this.MainPage.ResumeLayout(false);
            this.NullGroupBox1.ResumeLayout(false);
            this.NullGroupBox1.PerformLayout();
            this.NullGroupBox0.ResumeLayout(false);
            this.NullGroupBox0.PerformLayout();
            this.NullGroupBox.ResumeLayout(false);
            this.NullGroupBox.PerformLayout();
            this.ProtectionPage.ResumeLayout(false);
            this.NullGroupBox2.ResumeLayout(false);
            this.NullGroupBox8.ResumeLayout(false);
            this.NullGroupBox8.PerformLayout();
            this.NullGroupBox9.ResumeLayout(false);
            this.NullGroupBox9.PerformLayout();
            this.NullGroupBox10.ResumeLayout(false);
            this.NullGroupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RandomOutlineLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JunkNumber)).EndInit();
            this.FunctionsPage.ResumeLayout(false);
            this.MethodTree_ContextMenu.ResumeLayout(false);
            this.MergeNETPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.RuntimePage.ResumeLayout(false);
            this.NullGroupBox4.ResumeLayout(false);
            this.NullGroupBox6.ResumeLayout(false);
            this.NullGroupBox5.ResumeLayout(false);
            this.NullGroupBox5.PerformLayout();
            this.LogPage.ResumeLayout(false);
            this.ToolsPage.ResumeLayout(false);
            this.NullGroupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Teen.ThirteenForm TeenForm;
        private Teen.ThirteenTabControl TControl;
        private System.Windows.Forms.TabPage MainPage;
        private System.Windows.Forms.TabPage ProtectionPage;
        private System.Windows.Forms.Panel NullPanel;
        private System.Windows.Forms.TabPage RuntimePage;
        private System.Windows.Forms.GroupBox NullGroupBox;
        private System.Windows.Forms.GroupBox NullGroupBox0;
        private System.Windows.Forms.GroupBox NullGroupBox1;
        private System.Windows.Forms.CheckBox ActiveSNK_CheckBox;
        private Teen.ThirteenTextBox AsmTextBox;
        private Teen.ThirteenTextBox OutDestination;
        private Teen.ThirteenButton BrowseAsmButton;
        private System.Windows.Forms.Label NulLabel0;
        private Teen.ThirteenButton BrowseSNKButton;
        private System.Windows.Forms.Label NulLabel;
        private Teen.ThirteenTextBox SNK_PasswordBox;
        private Teen.ThirteenTextBox SNKBox;
        private Teen.ThirteenButton ProtectButton;
        private System.Windows.Forms.GroupBox NullGroupBox2;
        private System.Windows.Forms.GroupBox NullGroupBox4;
        private System.Windows.Forms.GroupBox NullGroupBox5;
        private Teen.ThirteenTextBox RuntimeNameBox;
        private Teen.ThirteenButton RandomRuntimeName_Button;
        private System.Windows.Forms.GroupBox NullGroupBox6;
        private Teen.ThirteenButton HashUpdaterToolButton;
        private System.Windows.Forms.CheckBox VirtualizeAllStrings_CheckBox;
        private Teen.ThirteenComboBox ModeComboBox;
        private Teen.ThirteenButton OpenProj_Button;
        private Teen.ThirteenButton SaveProj_Button;
        private System.Windows.Forms.TabPage LogPage;
        private System.Windows.Forms.RichTextBox Log_RichTextBox;
        private System.Windows.Forms.TabPage ToolsPage;
        private System.Windows.Forms.GroupBox NullGroupBox7;
        private Teen.ThirteenButton ResetProj_Button;
        private System.Windows.Forms.TabPage FunctionsPage;
        private System.Windows.Forms.TreeView MethodTree;
        private System.Windows.Forms.ImageList IMGList;
        private System.Windows.Forms.ContextMenuStrip MethodTree_ContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mutateMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem virtualizeMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ultraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UnSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem methodAboutToolStripMenuItem;
        private Teen.ThirteenButton MinimizeFormButton;
        private Teen.ThirteenButton ExitButton;
        private System.Windows.Forms.CheckBox renamer_CheckBox;
        private System.Windows.Forms.GroupBox NullGroupBox8;
        private System.Windows.Forms.GroupBox NullGroupBox9;
        private System.Windows.Forms.GroupBox NullGroupBox10;
        private System.Windows.Forms.CheckBox antiDebug_CheckBox;
        private System.Windows.Forms.CheckBox antiDump_CheckBox;
        private System.Windows.Forms.CheckBox resourceProt_CheckBox;
        private System.Windows.Forms.TabPage MergeNETPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox NETAssembly_ListBox;
        private Teen.ThirteenButton MergeNET_SelecetAndRemove_Button;
        private Teen.ThirteenButton MergeNET_AddLibrary_Button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ShowHiddenTypesAndMethods_CheckBox;
        private System.Windows.Forms.CheckBox EncryptAllStrings_CheckBox;
        private Teen.ThirteenComboBox RandomOutline_ComboBox;
        private System.Windows.Forms.CheckBox RandomOutline_CheckBox;
        private System.Windows.Forms.CheckBox Junk_CheckBox;
        private System.Windows.Forms.ToolTip NopAttack_ToolTip;
        private System.Windows.Forms.ToolTip Junk_ToolTip;
        private System.Windows.Forms.NumericUpDown RandomOutlineLength;
        private System.Windows.Forms.NumericUpDown JunkNumber;
        private System.Windows.Forms.ToolTip RandomOutlineLength_ToolTip;
        private System.Windows.Forms.ToolTip JunkNumber_ToolTip;
        private System.Windows.Forms.CheckBox Arithmetic_CheckBox;
        private System.Windows.Forms.CheckBox IntConfusion_CheckBox;
        private System.Windows.Forms.CheckBox ProcessMonitor_CheckBox;
        private System.Windows.Forms.ToolTip ProcessMonitor_ToolTip;
        private System.Windows.Forms.Label SelectAllFun_Label;
        private Teen.ThirteenComboBox SelectAllFun_ComboBox;
        private System.Windows.Forms.CheckBox IntEncoding_CheckBox;
        private System.Windows.Forms.CheckBox LocalToField_CheckBox;
        private System.Windows.Forms.CheckBox ImportProtection_CheckBox;
    }
}

