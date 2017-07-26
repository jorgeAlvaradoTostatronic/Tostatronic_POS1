namespace Maxima_Distribuidores_VS
{
    partial class uscCredito
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uscCredito));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.txtApellidoPaterno = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblRfc = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtRfc = new System.Windows.Forms.TextBox();
            this.dgvCredito = new System.Windows.Forms.DataGridView();
            this.folio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.impuesto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.abonado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.restante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCredito)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(350, 446);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(64, 13);
            this.lblTotal.TabIndex = 124;
            this.lblTotal.Text = "Total deuda";
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscarCliente.Image")));
            this.btnBuscarCliente.Location = new System.Drawing.Point(505, 12);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(24, 24);
            this.btnBuscarCliente.TabIndex = 118;
            this.btnBuscarCliente.UseVisualStyleBackColor = true;
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // txtApellidoPaterno
            // 
            this.txtApellidoPaterno.Enabled = false;
            this.txtApellidoPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtApellidoPaterno.Location = new System.Drawing.Point(374, 47);
            this.txtApellidoPaterno.Name = "txtApellidoPaterno";
            this.txtApellidoPaterno.Size = new System.Drawing.Size(155, 23);
            this.txtApellidoPaterno.TabIndex = 117;
            // 
            // txtNombre
            // 
            this.txtNombre.Enabled = false;
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNombre.Location = new System.Drawing.Point(213, 47);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(155, 23);
            this.txtNombre.TabIndex = 116;
            // 
            // lblRfc
            // 
            this.lblRfc.AutoSize = true;
            this.lblRfc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblRfc.Location = new System.Drawing.Point(140, 19);
            this.lblRfc.Name = "lblRfc";
            this.lblRfc.Size = new System.Drawing.Size(44, 17);
            this.lblRfc.TabIndex = 115;
            this.lblRfc.Text = "RFC:*";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblNombre.Location = new System.Drawing.Point(140, 53);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(71, 17);
            this.lblNombre.TabIndex = 114;
            this.lblNombre.Text = "Nombre: *";
            // 
            // txtRfc
            // 
            this.txtRfc.Enabled = false;
            this.txtRfc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtRfc.Location = new System.Drawing.Point(213, 13);
            this.txtRfc.Name = "txtRfc";
            this.txtRfc.Size = new System.Drawing.Size(286, 23);
            this.txtRfc.TabIndex = 113;
            // 
            // dgvCredito
            // 
            this.dgvCredito.AllowUserToAddRows = false;
            this.dgvCredito.AllowUserToDeleteRows = false;
            this.dgvCredito.AllowUserToResizeColumns = false;
            this.dgvCredito.AllowUserToResizeRows = false;
            this.dgvCredito.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvCredito.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCredito.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCredito.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.folio,
            this.fecha,
            this.subtotal,
            this.impuesto,
            this.total,
            this.abonado,
            this.restante});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.GreenYellow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCredito.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCredito.Enabled = false;
            this.dgvCredito.Location = new System.Drawing.Point(0, 92);
            this.dgvCredito.MultiSelect = false;
            this.dgvCredito.Name = "dgvCredito";
            this.dgvCredito.RowHeadersVisible = false;
            this.dgvCredito.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCredito.Size = new System.Drawing.Size(881, 339);
            this.dgvCredito.TabIndex = 110;
            // 
            // folio
            // 
            this.folio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.folio.FillWeight = 50F;
            this.folio.HeaderText = "Folio";
            this.folio.Name = "folio";
            this.folio.ReadOnly = true;
            // 
            // fecha
            // 
            this.fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fecha.HeaderText = "Fecha";
            this.fecha.Name = "fecha";
            this.fecha.ReadOnly = true;
            // 
            // subtotal
            // 
            this.subtotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.subtotal.HeaderText = "Subtotal";
            this.subtotal.Name = "subtotal";
            this.subtotal.ReadOnly = true;
            this.subtotal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // impuesto
            // 
            this.impuesto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.impuesto.HeaderText = "Impuesto";
            this.impuesto.Name = "impuesto";
            this.impuesto.ReadOnly = true;
            // 
            // total
            // 
            this.total.HeaderText = "Total Venta";
            this.total.Name = "total";
            // 
            // abonado
            // 
            this.abonado.HeaderText = "Abonado";
            this.abonado.Name = "abonado";
            this.abonado.ReadOnly = true;
            // 
            // restante
            // 
            this.restante.HeaderText = "Restante";
            this.restante.Name = "restante";
            this.restante.ReadOnly = true;
            // 
            // txtTotal
            // 
            this.txtTotal.Enabled = false;
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.ForeColor = System.Drawing.Color.Blue;
            this.txtTotal.Location = new System.Drawing.Point(3, 462);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(757, 35);
            this.txtTotal.TabIndex = 109;
            this.txtTotal.Text = "$0.00";
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(21, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(56, 18);
            this.lblTitulo.TabIndex = 108;
            this.lblTitulo.Text = "Credito";
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimir.Image")));
            this.btnImprimir.Location = new System.Drawing.Point(766, 462);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(115, 31);
            this.btnImprimir.TabIndex = 107;
            this.btnImprimir.UseVisualStyleBackColor = true;
            // 
            // uscCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnBuscarCliente);
            this.Controls.Add(this.txtApellidoPaterno);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblRfc);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtRfc);
            this.Controls.Add(this.dgvCredito);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnImprimir);
            this.Name = "uscCredito";
            this.Size = new System.Drawing.Size(884, 504);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCredito)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.TextBox txtApellidoPaterno;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblRfc;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtRfc;
        private System.Windows.Forms.DataGridView dgvCredito;
        private System.Windows.Forms.DataGridViewTextBoxColumn folio;
        private System.Windows.Forms.DataGridViewTextBoxColumn fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn impuesto;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn abonado;
        private System.Windows.Forms.DataGridViewTextBoxColumn restante;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnImprimir;
    }
}
