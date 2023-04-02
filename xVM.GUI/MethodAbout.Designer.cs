
namespace xVM.GUI
{
    partial class MethodAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MethodAbout));
            this.TeenForm = new Teen.ThirteenForm();
            this.MinimizeFormButton = new Teen.ThirteenButton();
            this.ExitButton = new Teen.ThirteenButton();
            this.NullGroupBox0 = new System.Windows.Forms.GroupBox();
            this.ILBox = new System.Windows.Forms.RichTextBox();
            this.MethodAbout_listView = new System.Windows.Forms.ListView();
            this.IMGList = new System.Windows.Forms.ImageList(this.components);
            this.TeenForm.SuspendLayout();
            this.NullGroupBox0.SuspendLayout();
            this.SuspendLayout();
            // 
            // TeenForm
            // 
            this.TeenForm.AccentColor = System.Drawing.Color.Gray;
            this.TeenForm.AllowDrop = true;
            this.TeenForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.TeenForm.ColorScheme = Teen.ThirteenForm.ColorSchemes.Dark;
            this.TeenForm.Controls.Add(this.MinimizeFormButton);
            this.TeenForm.Controls.Add(this.ExitButton);
            this.TeenForm.Controls.Add(this.NullGroupBox0);
            this.TeenForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TeenForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.TeenForm.ForeColor = System.Drawing.Color.White;
            this.TeenForm.Location = new System.Drawing.Point(0, 0);
            this.TeenForm.Name = "TeenForm";
            this.TeenForm.Size = new System.Drawing.Size(800, 528);
            this.TeenForm.TabIndex = 1;
            this.TeenForm.TabStop = false;
            this.TeenForm.Text = "Method About";
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
            this.MinimizeFormButton.Location = new System.Drawing.Point(717, 3);
            this.MinimizeFormButton.Name = "MinimizeFormButton";
            this.MinimizeFormButton.Size = new System.Drawing.Size(31, 23);
            this.MinimizeFormButton.TabIndex = 11;
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
            this.ExitButton.Location = new System.Drawing.Point(750, 3);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(47, 23);
            this.ExitButton.TabIndex = 10;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NullGroupBox0
            // 
            this.NullGroupBox0.Controls.Add(this.ILBox);
            this.NullGroupBox0.Controls.Add(this.MethodAbout_listView);
            this.NullGroupBox0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox0.Location = new System.Drawing.Point(12, 35);
            this.NullGroupBox0.Name = "NullGroupBox0";
            this.NullGroupBox0.Size = new System.Drawing.Size(776, 481);
            this.NullGroupBox0.TabIndex = 0;
            this.NullGroupBox0.TabStop = false;
            this.NullGroupBox0.Text = "Info";
            // 
            // ILBox
            // 
            this.ILBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ILBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ILBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ILBox.ForeColor = System.Drawing.Color.White;
            this.ILBox.Location = new System.Drawing.Point(3, 225);
            this.ILBox.Name = "ILBox";
            this.ILBox.ReadOnly = true;
            this.ILBox.Size = new System.Drawing.Size(770, 253);
            this.ILBox.TabIndex = 1;
            this.ILBox.TabStop = false;
            this.ILBox.Text = "";
            this.ILBox.WordWrap = false;
            // 
            // MethodAbout_listView
            // 
            this.MethodAbout_listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.MethodAbout_listView.AllowDrop = true;
            this.MethodAbout_listView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.MethodAbout_listView.Dock = System.Windows.Forms.DockStyle.Top;
            this.MethodAbout_listView.ForeColor = System.Drawing.Color.White;
            this.MethodAbout_listView.HideSelection = false;
            this.MethodAbout_listView.LargeImageList = this.IMGList;
            this.MethodAbout_listView.Location = new System.Drawing.Point(3, 18);
            this.MethodAbout_listView.MultiSelect = false;
            this.MethodAbout_listView.Name = "MethodAbout_listView";
            this.MethodAbout_listView.Size = new System.Drawing.Size(770, 207);
            this.MethodAbout_listView.SmallImageList = this.IMGList;
            this.MethodAbout_listView.TabIndex = 0;
            this.MethodAbout_listView.UseCompatibleStateImageBehavior = false;
            this.MethodAbout_listView.View = System.Windows.Forms.View.SmallIcon;
            this.MethodAbout_listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.MethodAbout_listView_ItemSelectionChanged);
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
            this.IMGList.Images.SetKeyName(7, "method_fullname.png");
            this.IMGList.Images.SetKeyName(8, "params.png");
            this.IMGList.Images.SetKeyName(9, "mdtoken.png");
            this.IMGList.Images.SetKeyName(10, "type.png");
            // 
            // MethodAbout
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(800, 528);
            this.ControlBox = false;
            this.Controls.Add(this.TeenForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MethodAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MethodAbout";
            this.Load += new System.EventHandler(this.MethodAbout_Load);
            this.TeenForm.ResumeLayout(false);
            this.NullGroupBox0.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Teen.ThirteenForm TeenForm;
        private System.Windows.Forms.GroupBox NullGroupBox0;
        private Teen.ThirteenButton MinimizeFormButton;
        private Teen.ThirteenButton ExitButton;
        private System.Windows.Forms.ListView MethodAbout_listView;
        private System.Windows.Forms.RichTextBox ILBox;
        private System.Windows.Forms.ImageList IMGList;
    }
}