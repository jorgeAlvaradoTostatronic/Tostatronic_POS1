namespace Maxima_Distribuidores_VS
{
    partial class uscCliente
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uscCliente));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnAccion = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gboTipoCliente = new System.Windows.Forms.GroupBox();
            this.rdbDistribuidor = new System.Windows.Forms.RadioButton();
            this.rdbPublico = new System.Windows.Forms.RadioButton();
            this.lblIndicacion = new System.Windows.Forms.Label();
            this.txtCelular = new System.Windows.Forms.TextBox();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.txtApellidoPaterno = new System.Windows.Forms.TextBox();
            this.txtApellidoMaterno = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblCelular = new System.Windows.Forms.Label();
            this.lblRfc = new System.Windows.Forms.Label();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.lblApellidoMaterno = new System.Windows.Forms.Label();
            this.lblApellidoPaterno = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtRfc = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.txtBusqueda = new System.Windows.Forms.TextBox();
            this.dgvClientes = new System.Windows.Forms.DataGridView();
            this.id_cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apellido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBusqueda = new System.Windows.Forms.Button();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtCP = new System.Windows.Forms.TextBox();
            this.txtColonia = new System.Windows.Forms.TextBox();
            this.lblCP = new System.Windows.Forms.Label();
            this.lblColonia = new System.Windows.Forms.Label();
            this.gboTipoCliente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(12, 32);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(127, 18);
            this.lblTitulo.TabIndex = 39;
            this.lblTitulo.Text = "NUEVO CLIENTE";
            // 
            // btnAccion
            // 
            this.btnAccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccion.Location = new System.Drawing.Point(17, 439);
            this.btnAccion.Name = "btnAccion";
            this.btnAccion.Size = new System.Drawing.Size(161, 31);
            this.btnAccion.TabIndex = 11;
            this.btnAccion.Text = "Guardar Cliente";
            this.btnAccion.UseVisualStyleBackColor = true;
            this.btnAccion.Click += new System.EventHandler(this.btnAccion_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = ((System.Drawing.Image)(resources.GetObject("btnCancelar.Image")));
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(709, 439);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(161, 31);
            this.btnCancelar.TabIndex = 41;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // gboTipoCliente
            // 
            this.gboTipoCliente.Controls.Add(this.rdbDistribuidor);
            this.gboTipoCliente.Controls.Add(this.rdbPublico);
            this.gboTipoCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gboTipoCliente.Location = new System.Drawing.Point(17, 352);
            this.gboTipoCliente.Name = "gboTipoCliente";
            this.gboTipoCliente.Size = new System.Drawing.Size(185, 82);
            this.gboTipoCliente.TabIndex = 9;
            this.gboTipoCliente.TabStop = false;
            this.gboTipoCliente.Text = "Tipo de cliente: *";
            // 
            // rdbDistribuidor
            // 
            this.rdbDistribuidor.AutoSize = true;
            this.rdbDistribuidor.Location = new System.Drawing.Point(24, 52);
            this.rdbDistribuidor.Name = "rdbDistribuidor";
            this.rdbDistribuidor.Size = new System.Drawing.Size(98, 21);
            this.rdbDistribuidor.TabIndex = 11;
            this.rdbDistribuidor.TabStop = true;
            this.rdbDistribuidor.Text = "Distribuidor";
            this.rdbDistribuidor.UseVisualStyleBackColor = true;
            // 
            // rdbPublico
            // 
            this.rdbPublico.AutoSize = true;
            this.rdbPublico.Location = new System.Drawing.Point(24, 25);
            this.rdbPublico.Name = "rdbPublico";
            this.rdbPublico.Size = new System.Drawing.Size(72, 21);
            this.rdbPublico.TabIndex = 10;
            this.rdbPublico.TabStop = true;
            this.rdbPublico.Text = "Publico";
            this.rdbPublico.UseVisualStyleBackColor = true;
            // 
            // lblIndicacion
            // 
            this.lblIndicacion.AutoSize = true;
            this.lblIndicacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblIndicacion.Location = new System.Drawing.Point(672, 16);
            this.lblIndicacion.Name = "lblIndicacion";
            this.lblIndicacion.Size = new System.Drawing.Size(169, 34);
            this.lblIndicacion.TabIndex = 96;
            this.lblIndicacion.Text = "Los campos marcados (*)\r\nson obligatorios";
            // 
            // txtCelular
            // 
            this.txtCelular.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtCelular.Location = new System.Drawing.Point(354, 317);
            this.txtCelular.Name = "txtCelular";
            this.txtCelular.ShortcutsEnabled = false;
            this.txtCelular.Size = new System.Drawing.Size(155, 23);
            this.txtCelular.TabIndex = 8;
            this.txtCelular.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCorreo_KeyPress);
            // 
            // txtTelefono
            // 
            this.txtTelefono.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtTelefono.Location = new System.Drawing.Point(119, 317);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.ShortcutsEnabled = false;
            this.txtTelefono.Size = new System.Drawing.Size(156, 23);
            this.txtTelefono.TabIndex = 7;
            this.txtTelefono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCorreo_KeyPress);
            // 
            // txtDireccion
            // 
            this.txtDireccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtDireccion.Location = new System.Drawing.Point(119, 242);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.ShortcutsEnabled = false;
            this.txtDireccion.Size = new System.Drawing.Size(390, 23);
            this.txtDireccion.TabIndex = 5;
            this.txtDireccion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCorreo_KeyPress);
            // 
            // txtApellidoPaterno
            // 
            this.txtApellidoPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtApellidoPaterno.Location = new System.Drawing.Point(119, 160);
            this.txtApellidoPaterno.Name = "txtApellidoPaterno";
            this.txtApellidoPaterno.ShortcutsEnabled = false;
            this.txtApellidoPaterno.Size = new System.Drawing.Size(156, 23);
            this.txtApellidoPaterno.TabIndex = 2;
            this.txtApellidoPaterno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // txtApellidoMaterno
            // 
            this.txtApellidoMaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtApellidoMaterno.Location = new System.Drawing.Point(354, 163);
            this.txtApellidoMaterno.Name = "txtApellidoMaterno";
            this.txtApellidoMaterno.ShortcutsEnabled = false;
            this.txtApellidoMaterno.Size = new System.Drawing.Size(156, 23);
            this.txtApellidoMaterno.TabIndex = 3;
            this.txtApellidoMaterno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNombre.Location = new System.Drawing.Point(119, 119);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ShortcutsEnabled = false;
            this.txtNombre.Size = new System.Drawing.Size(390, 23);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // lblCelular
            // 
            this.lblCelular.AutoSize = true;
            this.lblCelular.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblCelular.Location = new System.Drawing.Point(295, 326);
            this.lblCelular.Name = "lblCelular";
            this.lblCelular.Size = new System.Drawing.Size(56, 17);
            this.lblCelular.TabIndex = 89;
            this.lblCelular.Text = "Celular:";
            // 
            // lblRfc
            // 
            this.lblRfc.AutoSize = true;
            this.lblRfc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblRfc.Location = new System.Drawing.Point(14, 86);
            this.lblRfc.Name = "lblRfc";
            this.lblRfc.Size = new System.Drawing.Size(48, 17);
            this.lblRfc.TabIndex = 88;
            this.lblRfc.Text = "RFC: *";
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblTelefono.Location = new System.Drawing.Point(14, 323);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(68, 17);
            this.lblTelefono.TabIndex = 87;
            this.lblTelefono.Text = "Telefono:";
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblDireccion.Location = new System.Drawing.Point(14, 245);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(71, 17);
            this.lblDireccion.TabIndex = 86;
            this.lblDireccion.Text = "Dirección:";
            // 
            // lblApellidoMaterno
            // 
            this.lblApellidoMaterno.AutoSize = true;
            this.lblApellidoMaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblApellidoMaterno.Location = new System.Drawing.Point(277, 166);
            this.lblApellidoMaterno.Name = "lblApellidoMaterno";
            this.lblApellidoMaterno.Size = new System.Drawing.Size(81, 17);
            this.lblApellidoMaterno.TabIndex = 85;
            this.lblApellidoMaterno.Text = "Apellido M.:";
            // 
            // lblApellidoPaterno
            // 
            this.lblApellidoPaterno.AutoSize = true;
            this.lblApellidoPaterno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblApellidoPaterno.Location = new System.Drawing.Point(14, 163);
            this.lblApellidoPaterno.Name = "lblApellidoPaterno";
            this.lblApellidoPaterno.Size = new System.Drawing.Size(88, 17);
            this.lblApellidoPaterno.TabIndex = 84;
            this.lblApellidoPaterno.Text = "Apellido P.: *";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblNombre.Location = new System.Drawing.Point(14, 122);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(88, 17);
            this.lblNombre.TabIndex = 83;
            this.lblNombre.Text = "Nombre(s): *";
            // 
            // txtRfc
            // 
            this.txtRfc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtRfc.Location = new System.Drawing.Point(119, 83);
            this.txtRfc.Name = "txtRfc";
            this.txtRfc.ShortcutsEnabled = false;
            this.txtRfc.Size = new System.Drawing.Size(181, 23);
            this.txtRfc.TabIndex = 0;
            this.txtRfc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRfc_KeyPress);
            // 
            // lblCorreo
            // 
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblCorreo.Location = new System.Drawing.Point(14, 202);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Size = new System.Drawing.Size(59, 17);
            this.lblCorreo.TabIndex = 99;
            this.lblCorreo.Text = "Correo: ";
            // 
            // txtCorreo
            // 
            this.txtCorreo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtCorreo.Location = new System.Drawing.Point(119, 199);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.ShortcutsEnabled = false;
            this.txtCorreo.Size = new System.Drawing.Size(390, 23);
            this.txtCorreo.TabIndex = 4;
            this.txtCorreo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCorreo_KeyPress);
            // 
            // txtBusqueda
            // 
            this.txtBusqueda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBusqueda.Location = new System.Drawing.Point(528, 81);
            this.txtBusqueda.Name = "txtBusqueda";
            this.txtBusqueda.ShortcutsEnabled = false;
            this.txtBusqueda.Size = new System.Drawing.Size(313, 22);
            this.txtBusqueda.TabIndex = 13;
            this.txtBusqueda.TextChanged += new System.EventHandler(this.txtBusqueda_TextChanged);
            this.txtBusqueda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBusqueda_KeyDown);
            this.txtBusqueda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // dgvClientes
            // 
            this.dgvClientes.AllowUserToAddRows = false;
            this.dgvClientes.AllowUserToDeleteRows = false;
            this.dgvClientes.AllowUserToResizeColumns = false;
            this.dgvClientes.AllowUserToResizeRows = false;
            this.dgvClientes.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id_cliente,
            this.RFC,
            this.nombre,
            this.apellido});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.GreenYellow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClientes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvClientes.Location = new System.Drawing.Point(528, 119);
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes.Size = new System.Drawing.Size(342, 276);
            this.dgvClientes.TabIndex = 102;
            this.dgvClientes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClientes_CellClick);
            // 
            // id_cliente
            // 
            this.id_cliente.HeaderText = "ID";
            this.id_cliente.Name = "id_cliente";
            this.id_cliente.ReadOnly = true;
            this.id_cliente.Visible = false;
            // 
            // RFC
            // 
            this.RFC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RFC.FillWeight = 80F;
            this.RFC.HeaderText = "RFC";
            this.RFC.Name = "RFC";
            this.RFC.ReadOnly = true;
            // 
            // nombre
            // 
            this.nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nombre.FillWeight = 220F;
            this.nombre.HeaderText = "Nombre";
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            // 
            // apellido
            // 
            this.apellido.HeaderText = "Apellido Paterno";
            this.apellido.Name = "apellido";
            this.apellido.ReadOnly = true;
            // 
            // btnBusqueda
            // 
            this.btnBusqueda.Image = ((System.Drawing.Image)(resources.GetObject("btnBusqueda.Image")));
            this.btnBusqueda.Location = new System.Drawing.Point(847, 79);
            this.btnBusqueda.Name = "btnBusqueda";
            this.btnBusqueda.Size = new System.Drawing.Size(24, 24);
            this.btnBusqueda.TabIndex = 104;
            this.btnBusqueda.UseVisualStyleBackColor = true;
            this.btnBusqueda.Click += new System.EventHandler(this.btnBusqueda_Click);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblDescripcion.Location = new System.Drawing.Point(205, 352);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(132, 17);
            this.lblDescripcion.TabIndex = 105;
            this.lblDescripcion.Text = "Reseñas del cliente";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtDescripcion.Location = new System.Drawing.Point(208, 372);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ShortcutsEnabled = false;
            this.txtDescripcion.Size = new System.Drawing.Size(301, 62);
            this.txtDescripcion.TabIndex = 10;
            this.txtDescripcion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCorreo_KeyPress);
            // 
            // txtCP
            // 
            this.txtCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtCP.Location = new System.Drawing.Point(422, 278);
            this.txtCP.MaxLength = 5;
            this.txtCP.Name = "txtCP";
            this.txtCP.ShortcutsEnabled = false;
            this.txtCP.Size = new System.Drawing.Size(86, 23);
            this.txtCP.TabIndex = 7;
            // 
            // txtColonia
            // 
            this.txtColonia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtColonia.Location = new System.Drawing.Point(119, 278);
            this.txtColonia.MaxLength = 20;
            this.txtColonia.Name = "txtColonia";
            this.txtColonia.ShortcutsEnabled = false;
            this.txtColonia.Size = new System.Drawing.Size(261, 23);
            this.txtColonia.TabIndex = 6;
            // 
            // lblCP
            // 
            this.lblCP.AutoSize = true;
            this.lblCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblCP.Location = new System.Drawing.Point(386, 281);
            this.lblCP.Name = "lblCP";
            this.lblCP.Size = new System.Drawing.Size(30, 17);
            this.lblCP.TabIndex = 109;
            this.lblCP.Text = "CP:";
            // 
            // lblColonia
            // 
            this.lblColonia.AutoSize = true;
            this.lblColonia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblColonia.Location = new System.Drawing.Point(15, 284);
            this.lblColonia.Name = "lblColonia";
            this.lblColonia.Size = new System.Drawing.Size(59, 17);
            this.lblColonia.TabIndex = 108;
            this.lblColonia.Text = "Colonia:";
            // 
            // uscCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtCP);
            this.Controls.Add(this.txtColonia);
            this.Controls.Add(this.lblCP);
            this.Controls.Add(this.lblColonia);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.btnBusqueda);
            this.Controls.Add(this.txtBusqueda);
            this.Controls.Add(this.dgvClientes);
            this.Controls.Add(this.lblCorreo);
            this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.gboTipoCliente);
            this.Controls.Add(this.lblIndicacion);
            this.Controls.Add(this.txtCelular);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.txtApellidoPaterno);
            this.Controls.Add(this.txtApellidoMaterno);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblCelular);
            this.Controls.Add(this.lblRfc);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.lblDireccion);
            this.Controls.Add(this.lblApellidoMaterno);
            this.Controls.Add(this.lblApellidoPaterno);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtRfc);
            this.Controls.Add(this.btnAccion);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblTitulo);
            this.Name = "uscCliente";
            this.Size = new System.Drawing.Size(884, 504);
            this.Resize += new System.EventHandler(this.uscAgregarClientes_Resize);
            this.gboTipoCliente.ResumeLayout(false);
            this.gboTipoCliente.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnAccion;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox gboTipoCliente;
        private System.Windows.Forms.RadioButton rdbDistribuidor;
        private System.Windows.Forms.RadioButton rdbPublico;
        private System.Windows.Forms.Label lblIndicacion;
        private System.Windows.Forms.TextBox txtCelular;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.TextBox txtApellidoPaterno;
        private System.Windows.Forms.TextBox txtApellidoMaterno;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCelular;
        private System.Windows.Forms.Label lblRfc;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.Label lblApellidoMaterno;
        private System.Windows.Forms.Label lblApellidoPaterno;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtRfc;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.DataGridView dgvClientes;
        private System.Windows.Forms.Button btnBusqueda;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn id_cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFC;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn apellido;
        private System.Windows.Forms.TextBox txtCP;
        private System.Windows.Forms.TextBox txtColonia;
        private System.Windows.Forms.Label lblCP;
        private System.Windows.Forms.Label lblColonia;
    }
}
