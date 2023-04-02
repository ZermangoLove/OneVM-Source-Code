
namespace xVM.GUI
{
    partial class HashUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HashUpdater));
            this.TeenForm = new Teen.ThirteenForm();
            this.NullGroupBox = new System.Windows.Forms.GroupBox();
            this.UpdateAsmHashButton = new Teen.ThirteenButton();
            this.BrowseAsmButton = new Teen.ThirteenButton();
            this.AsmTextBox = new Teen.ThirteenTextBox();
            this.MinimizeFormButton = new Teen.ThirteenButton();
            this.ExitButton = new Teen.ThirteenButton();
            this.TeenForm.SuspendLayout();
            this.NullGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TeenForm
            // 
            this.TeenForm.AccentColor = System.Drawing.Color.Gray;
            this.TeenForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.TeenForm.ColorScheme = Teen.ThirteenForm.ColorSchemes.Dark;
            this.TeenForm.Controls.Add(this.NullGroupBox);
            this.TeenForm.Controls.Add(this.MinimizeFormButton);
            this.TeenForm.Controls.Add(this.ExitButton);
            this.TeenForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TeenForm.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.TeenForm.ForeColor = System.Drawing.Color.White;
            this.TeenForm.Location = new System.Drawing.Point(0, 0);
            this.TeenForm.Name = "TeenForm";
            this.TeenForm.Size = new System.Drawing.Size(704, 149);
            this.TeenForm.TabIndex = 0;
            this.TeenForm.Text = "Hash Updater";
            // 
            // NullGroupBox
            // 
            this.NullGroupBox.AllowDrop = true;
            this.NullGroupBox.Controls.Add(this.UpdateAsmHashButton);
            this.NullGroupBox.Controls.Add(this.BrowseAsmButton);
            this.NullGroupBox.Controls.Add(this.AsmTextBox);
            this.NullGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.NullGroupBox.Location = new System.Drawing.Point(12, 45);
            this.NullGroupBox.Name = "NullGroupBox";
            this.NullGroupBox.Size = new System.Drawing.Size(680, 93);
            this.NullGroupBox.TabIndex = 12;
            this.NullGroupBox.TabStop = false;
            this.NullGroupBox.Text = "Assembly";
            // 
            // UpdateAsmHashButton
            // 
            this.UpdateAsmHashButton.AccentColor = System.Drawing.Color.Gray;
            this.UpdateAsmHashButton.AllowDrop = true;
            this.UpdateAsmHashButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.UpdateAsmHashButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.UpdateAsmHashButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UpdateAsmHashButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.UpdateAsmHashButton.ForeColor = System.Drawing.Color.White;
            this.UpdateAsmHashButton.Location = new System.Drawing.Point(527, 58);
            this.UpdateAsmHashButton.Name = "UpdateAsmHashButton";
            this.UpdateAsmHashButton.Size = new System.Drawing.Size(134, 25);
            this.UpdateAsmHashButton.TabIndex = 2;
            this.UpdateAsmHashButton.Text = "Update Hash";
            this.UpdateAsmHashButton.UseVisualStyleBackColor = false;
            this.UpdateAsmHashButton.Click += new System.EventHandler(this.UpdateAsmHashButton_Click);
            // 
            // BrowseAsmButton
            // 
            this.BrowseAsmButton.AccentColor = System.Drawing.Color.Gray;
            this.BrowseAsmButton.AllowDrop = true;
            this.BrowseAsmButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BrowseAsmButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.BrowseAsmButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BrowseAsmButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.BrowseAsmButton.ForeColor = System.Drawing.Color.White;
            this.BrowseAsmButton.Location = new System.Drawing.Point(582, 29);
            this.BrowseAsmButton.Name = "BrowseAsmButton";
            this.BrowseAsmButton.Size = new System.Drawing.Size(79, 25);
            this.BrowseAsmButton.TabIndex = 1;
            this.BrowseAsmButton.Text = "Browse...";
            this.BrowseAsmButton.UseVisualStyleBackColor = false;
            this.BrowseAsmButton.Click += new System.EventHandler(this.BrowseAsmButton_Click);
            // 
            // AsmTextBox
            // 
            this.AsmTextBox.AllowDrop = true;
            this.AsmTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.AsmTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AsmTextBox.ColorScheme = Teen.ThirteenTextBox.ColorSchemes.Dark;
            this.AsmTextBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.AsmTextBox.ForeColor = System.Drawing.Color.White;
            this.AsmTextBox.Location = new System.Drawing.Point(20, 29);
            this.AsmTextBox.Name = "AsmTextBox";
            this.AsmTextBox.Size = new System.Drawing.Size(556, 25);
            this.AsmTextBox.TabIndex = 0;
            this.AsmTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.Asm_DragDrop);
            this.AsmTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // MinimizeFormButton
            // 
            this.MinimizeFormButton.AccentColor = System.Drawing.Color.Gray;
            this.MinimizeFormButton.AllowDrop = true;
            this.MinimizeFormButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.MinimizeFormButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.MinimizeFormButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MinimizeFormButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.MinimizeFormButton.ForeColor = System.Drawing.Color.White;
            this.MinimizeFormButton.Location = new System.Drawing.Point(621, 4);
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
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ExitButton.ColorScheme = Teen.ThirteenButton.ColorSchemes.Dark;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ExitButton.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F);
            this.ExitButton.ForeColor = System.Drawing.Color.White;
            this.ExitButton.Location = new System.Drawing.Point(654, 4);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(47, 23);
            this.ExitButton.TabIndex = 10;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // HashUpdater
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 149);
            this.ControlBox = false;
            this.Controls.Add(this.TeenForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HashUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HashUpdater";
            this.TeenForm.ResumeLayout(false);
            this.NullGroupBox.ResumeLayout(false);
            this.NullGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Teen.ThirteenForm TeenForm;
        private Teen.ThirteenButton MinimizeFormButton;
        private Teen.ThirteenButton ExitButton;
        private System.Windows.Forms.GroupBox NullGroupBox;
        private Teen.ThirteenButton BrowseAsmButton;
        private Teen.ThirteenTextBox AsmTextBox;
        private Teen.ThirteenButton UpdateAsmHashButton;
    }
}