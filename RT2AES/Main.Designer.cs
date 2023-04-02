
namespace RT2AES
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
            this.rtBox = new System.Windows.Forms.TextBox();
            this.nullA = new System.Windows.Forms.Label();
            this.encRT = new System.Windows.Forms.Button();
            this.browseRT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtBox
            // 
            this.rtBox.AllowDrop = true;
            this.rtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtBox.Location = new System.Drawing.Point(20, 32);
            this.rtBox.Margin = new System.Windows.Forms.Padding(4);
            this.rtBox.Name = "rtBox";
            this.rtBox.Size = new System.Drawing.Size(833, 26);
            this.rtBox.TabIndex = 0;
            this.rtBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.rtBox_DragDrop);
            this.rtBox.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // nullA
            // 
            this.nullA.AllowDrop = true;
            this.nullA.AutoSize = true;
            this.nullA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nullA.Location = new System.Drawing.Point(16, 11);
            this.nullA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nullA.Name = "nullA";
            this.nullA.Size = new System.Drawing.Size(119, 20);
            this.nullA.TabIndex = 1;
            this.nullA.Text = "xVM Runtime :";
            this.nullA.DragDrop += new System.Windows.Forms.DragEventHandler(this.rtBox_DragDrop);
            this.nullA.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // encRT
            // 
            this.encRT.AllowDrop = true;
            this.encRT.Location = new System.Drawing.Point(735, 102);
            this.encRT.Margin = new System.Windows.Forms.Padding(4);
            this.encRT.Name = "encRT";
            this.encRT.Size = new System.Drawing.Size(120, 28);
            this.encRT.TabIndex = 2;
            this.encRT.Text = "Encrypt";
            this.encRT.UseVisualStyleBackColor = true;
            this.encRT.Click += new System.EventHandler(this.encRT_Click);
            this.encRT.DragDrop += new System.Windows.Forms.DragEventHandler(this.rtBox_DragDrop);
            this.encRT.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            // 
            // browseRT
            // 
            this.browseRT.AllowDrop = true;
            this.browseRT.Location = new System.Drawing.Point(735, 66);
            this.browseRT.Margin = new System.Windows.Forms.Padding(4);
            this.browseRT.Name = "browseRT";
            this.browseRT.Size = new System.Drawing.Size(120, 28);
            this.browseRT.TabIndex = 3;
            this.browseRT.Text = "Browse...";
            this.browseRT.UseVisualStyleBackColor = true;
            this.browseRT.Click += new System.EventHandler(this.browseRT_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 134);
            this.Controls.Add(this.browseRT);
            this.Controls.Add(this.encRT);
            this.Controls.Add(this.nullA);
            this.Controls.Add(this.rtBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Runtime To AES";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.rtBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this._DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox rtBox;
        private System.Windows.Forms.Label nullA;
        private System.Windows.Forms.Button encRT;
        private System.Windows.Forms.Button browseRT;
    }
}

