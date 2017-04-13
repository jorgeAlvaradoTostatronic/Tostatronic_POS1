using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public partial class uscCliente : UserControl
    {
        private Redimension redimension;
        private int idCliente;
        public uscCliente(Accion accion)
        {
            InitializeComponent();
            redimension = new Redimension(this);
            Cargar(accion);
        }
        private void Cargar(Accion accion)
        {
            switch (accion)
            {
                case Accion.Agregar:
                    dgvClientes.Visible = false;
                    btnAccion.Text = "Agregar Cliente";
                    btnAccion.Click += new EventHandler(btnAccionAgregar_Click);
                    txtBusqueda.Visible = false;
                    btnBusqueda.Visible = false;
                    dgvClientes.Visible = false;
                    break;
                case Accion.Modificar:
                    btnAccion.Text = "Modificar Cliente";
                    btnAccion.Click += new EventHandler(btnAccionModificar_Click);
                    dgvClientes.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgvClientesModificar_Click);
                    break;
                case Accion.Eliminar:
                    txtRfc.Enabled = false;
                    txtNombre.Enabled = false;
                    txtApellidoPaterno.Enabled = false;
                    txtApellidoMaterno.Enabled = false;
                    txtTelefono.Enabled = false;
                    txtCelular.Enabled = false;
                    txtDireccion.Enabled = false;
                    txtCP.Enabled = false;
                    txtColonia.Enabled = false;
                    gboTipoCliente.Enabled = false;
                    txtCorreo.Enabled = false;
                    btnAccion.Text = "Eliminar Cliente";
                    btnAccion.Click += new EventHandler(btnAccionEliminar_Click);
                    dgvClientes.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgvClientesModificar_Click);
                    break;
            }
            lblTitulo.Text = btnAccion.Text;
        }
        //Aqui se generan todos los eventos y acciones de la seccion de modificar

        private void dgvClientesModificar_Click(object sender, EventArgs e)
        {
            try
            {
                int tipoCliente = 0;
                tipoCliente = int.Parse(dgvClientes.SelectedCells[1].Value.ToString());
                if (tipoCliente == 1)
                    rdbDistribuidor.Checked = true;
                else
                    rdbPublico.Checked = true;

                txtNombre.Text = dgvClientes.SelectedCells[2].Value.ToString();
                txtApellidoPaterno.Text = dgvClientes.SelectedCells[3].Value.ToString();
                txtApellidoMaterno.Text = dgvClientes.SelectedCells[4].Value.ToString();
                txtRfc.Text = dgvClientes.SelectedCells[5].Value.ToString();
                txtTelefono.Text = dgvClientes.SelectedCells[6].Value.ToString();
                txtDireccion.Text = dgvClientes.SelectedCells[7].Value.ToString();
                txtCorreo.Text = dgvClientes.SelectedCells[8].Value.ToString();
                txtCelular.Text = dgvClientes.SelectedCells[9].Value.ToString();
            }
            catch (Exception){}
            
        }

        private void btnAccionModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Verificar())
                {
                    if (!Sql.Existe("SELECT id_cliente FROM clientes WHERE rfc = '" + txtRfc.Text + "' AND " +
                        "id_cliente <> '" + idCliente + "'"))
                    {
                        int tipoCliente = 0;
                        if (rdbDistribuidor.Checked == true)
                            tipoCliente = 1;
                        else
                            tipoCliente = 2;

                        Sql.InsertarDatos("UPDATE `clientes` SET `id_tipo_cliente`=" + tipoCliente + ",`nombres`='"
                            + txtNombre.Text + "',`apellido_paterno`='" + txtApellidoPaterno.Text + "',`apellido_materno`='"
                            + txtApellidoMaterno.Text + "',`rfc`='" + txtRfc.Text + "',`telefono`='"
                            + txtTelefono.Text + "',`domicilio`='" + txtDireccion.Text + "',`codigo_postal`='" + txtCP.Text + "', `colonia`='" + txtColonia.Text + "',`correo_electronico`='"
                            + txtCorreo.Text + "',`celular`='" + txtCelular.Text + "', descripcion= '" + txtDescripcion.Text + "' WHERE id_cliente="
                            + dgvClientes.SelectedCells[0].Value + "");
                        //"+dgvClientes.SelectedCells[0].Value+"
                        MessageBox.Show("Se modifico el registro exitosamente", "Modificado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(this, "El RFC: " + txtRfc.Text + "  que intenta introducir ya existe, verifiquelo", "Error de coincidencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtRfc.Focus();  
                    }
                }
            }
            catch (Exception) { MessageBox.Show(this, "EL RFC  " + txtRfc.Text + " no existe en la base de datos", "Error de coincidencia", MessageBoxButtons.OK, MessageBoxIcon.Error); Limpiar(); txtRfc.Focus(); }
            
        }

        //Aqui se generan todos los eventos y acciones de eliminar
        private void btnAccionEliminar_Click(object sender, EventArgs e)
        {
            DialogResult confirmacion;
            if (txtApellidoPaterno.Text != "")
            {
                confirmacion = MessageBox.Show(this, "Seguro de que dese eliminar este cliente?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    Sql.InsertarDatos("UPDATE clientes SET eliminado = 1 WHERE id_cliente=" + dgvClientes.SelectedCells[0].Value + "");
                    btnBusqueda_Click(sender, new EventArgs());
                    Limpiar();
                }
            }
            else
                MessageBox.Show("Ingrece algun cliente", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        //Aqui se generan todos los eventos y acciones de agregar
        private void btnAccionAgregar_Click(object sender, EventArgs e)
        {
            if (Verificar())
            {
                if(!Sql.Existe("SELECT id_cliente FROM clientes WHERE rfc = '"+txtRfc.Text+"'"))
                {
                    int tipoCliente = 0;
                    if (rdbDistribuidor.Checked == true)
                        tipoCliente = 1;
                    else
                        tipoCliente = 2;
                    Sql.InsertarDatos("Insert into clientes VALUES(NULL,'" + tipoCliente + "','" + txtNombre.Text + "','" +
                        txtApellidoPaterno.Text + "','" + txtApellidoMaterno.Text + "','" + txtRfc.Text + "','" + txtTelefono.Text + "','" + txtDireccion.Text + "', '"+txtCP.Text+"', '"+txtColonia.Text+"','" + txtCorreo.Text + "','" + txtCelular.Text + "', '" + txtDescripcion.Text + "',0)");
                    MessageBox.Show("Se agrego correctamente el registro", "Agregado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(this, "El RFC que intenta introducir ya existe, favor de verificarlo", "Error de coincidencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtRfc.Focus();  
                }
                
            }
        }
        private void uscAgregarClientes_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }
        public bool Verificar()
        {
            if (!String.IsNullOrWhiteSpace(txtRfc.Text))
            {
                if (!String.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    if (!String.IsNullOrWhiteSpace(txtApellidoPaterno.Text))
                    {
                        if (rdbDistribuidor.Checked != false || rdbPublico.Checked != false)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Seleccione tipo de cliente", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                            gboTipoCliente.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Agrgar Apellido paterno", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtApellidoPaterno.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Agregar Nombre", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    txtNombre.Focus();
                }
            }
            else
            {
                MessageBox.Show("Agrgar RFC", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                txtRfc.Focus();
            }
            return false;
        }

        //Aqui estan funciones complementarias
        private void Limpiar()
        {
            try
            {
                foreach (Control control in Controls)
                    if (control is TextBox)
                        control.Text = "";
                rdbDistribuidor.Checked = false;
                rdbPublico.Checked = false;
                dgvClientes.Rows.Clear();
            }
            catch (Exception){}
            
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Limpiar();
            Buscador("SELECT id_cliente, rfc, nombres, apellido_paterno,apellido_materno, telefono,celular,domicilio,correo_electronico,id_tipo_cliente,descripcion FROM clientes " +
              "WHERE (rfc LIKE '%" + txtBusqueda.Text + "%' OR nombres LIKE '%" + txtBusqueda.Text + "%' OR apellido_paterno LIKE '%" + txtBusqueda.Text + "%') AND rfc <> 'xxxxxxxxxxxxx' AND eliminado = 0");

        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }
        private void Buscador(string consulta)
        {
            dgvClientes.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
                dgvClientes.Rows.Add(lista[i]);
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int tipo;
                    List<string[]> lista = Sql.BuscarDatos("SELECT id_cliente, rfc, nombres, " +
                        "apellido_paterno, apellido_materno,telefono,domicilio,codigo_postal,colonia,correo_electronico,celular, descripcion,id_tipo_cliente FROM clientes WHERE id_cliente = " +
                        dgvClientes.Rows[e.RowIndex].Cells["id_cliente"].Value + "");
                    idCliente = int.Parse(lista[0][0]);
                    txtRfc.Text = lista[0][1];
                    txtNombre.Text = lista[0][2];
                    txtApellidoPaterno.Text = lista[0][3];
                    txtApellidoMaterno.Text = lista[0][4];
                    txtTelefono.Text = lista[0][5];
                    txtDireccion.Text = lista[0][6];
                    txtCP.Text = lista[0][7];
                    txtColonia.Text = lista[0][8];
                    txtCorreo.Text = lista[0][9];
                    txtCelular.Text = lista[0][10];
                    txtDescripcion.Text = lista[0][11];
                    tipo = int.Parse(lista[0][12]);
                    if (tipo == 1)
                        rdbDistribuidor.Checked = true;
                    else
                        rdbPublico.Checked = true;
                }
            }
            catch (Exception){}
            
        }

        private void txtRfc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtCorreo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == char.Parse("(") || e.KeyChar == char.Parse(")") || e.KeyChar == char.Parse("_") || e.KeyChar == char.Parse("-") || e.KeyChar == char.Parse("@") || e.KeyChar == char.Parse("#") || e.KeyChar == char.Parse(" ") || e.KeyChar == char.Parse("."))
            {
                e.Handled = false;
            }
            else
            {
                if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            txtRfc.Focus();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == char.Parse(" "))
            {
                e.Handled = false;
            }
            else
            {
                if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {

        }
    }
}