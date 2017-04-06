namespace Maxima_Distribuidores_VS
{
    partial class frmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.lblDerechos = new System.Windows.Forms.Label();
            this.pctBSplash = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pctBSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDerechos
            // 
            this.lblDerechos.AutoSize = true;
            this.lblDerechos.BackColor = System.Drawing.Color.Transparent;
            this.lblDerechos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblDerechos.ForeColor = System.Drawing.Color.White;
            this.lblDerechos.Location = new System.Drawing.Point(24, 244);
            this.lblDerechos.Name = "lblDerechos";
            this.lblDerechos.Size = new System.Drawing.Size(421, 17);
            this.lblDerechos.TabIndex = 2;
            this.lblDerechos.Text = "Todos los derechos reserveados ® Tostatronic - Software Desing";
            // 
            // pctBSplash
            // 
            this.pctBSplash.Image = ((System.Drawing.Image)(resources.GetObject("pctBSplash.Image")));
            this.pctBSplash.Location = new System.Drawing.Point(1, 1);
            this.pctBSplash.Name = "pctBSplash";
            this.pctBSplash.Size = new System.Drawing.Size(467, 266);
            this.pctBSplash.TabIndex = 3;
            this.pctBSplash.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(469, 270);
            this.Controls.Add(this.lblDerechos);
            this.Controls.Add(this.pctBSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSplash";
            this.Load += new System.EventHandler(this.frmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pctBSplash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDerechos;
        private System.Windows.Forms.PictureBox pctBSplash;

    }
}