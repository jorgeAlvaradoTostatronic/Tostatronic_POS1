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
    public partial class uscUsuarios : UserControl
    {
        private Redimension redimension;

        public uscUsuarios(Accion accion)
        {
            InitializeComponent();
            redimension = new Redimension(this);
            CargarAciones(accion);
        }

        private void CargarAciones(Accion accion)
        {
            switch (accion)
            {
                case Accion.Agregar:
                    lblTitulo.Text = "NUEVO USUARIO";
                    btnAccion.Text = "Agregar Usuario";
                    btnAccion.Click += new EventHandler(btnAccionAgregar_Click);
                    foreach (Control item in Controls)
                        if (item is Label || item is TextBox)
                            item.Visible = true;
                    lvUsuarios.Visible = false;
                    break;
                case Accion.Modificar:
                    lblTitulo.Text = "MODIFICAR PERMISOS DE USUARIO";
                    btnAccion.Text = "Guardar Cambios";
                    btnAccion.Click += new EventHandler(btnAccionPermisos_Click);
                    this.Load += new EventHandler(uscUsuarios_Load);
                    lvUsuarios.SelectedIndexChanged += new EventHandler(lvUsuarios_SelectedIndexChanged);
                    foreach (Control item in Controls)
                        if(item is Label || item is TextBox)
                            item.Visible = false;
                    foreach (Control item in grbPermisos.Controls)
                        if (item is CheckBox)
                            ((CheckBox)item).Checked = false;
                    lblTitulo.Visible = true;
                    lvUsuarios.Visible = true;
                    break;
            }
        }

        private void lvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUsuarios.SelectedItems.Count == 1)
            {
                Usuario user = Usuario.Instancia();
                List<string[]> lista = Sql.BuscarDatos("SELECT permisos FROM usuarios WHERE id_usuario = '"
                    + lvUsuarios.SelectedItems[0].Text + "' AND id_usuario != '" + user.Id.ToString() + "'");
                int contador = 0;
                string cadena = lista[0][0];
                char[] array = cadena.ToCharArray();
                Array.Reverse(array);
                cadena = new string(array);
                foreach (Control item in grbPermisos.Controls)
                {
                    if (item is CheckBox)
                        ((CheckBox)item).Checked = (cadena[contador] == '1') ? true : false;
                    contador++;
                }
            }
            
        }

        private void uscUsuarios_Load(object sender, EventArgs e)
        {
            Usuario user = Usuario.Instancia();
            List<string[]> lista = Sql.BuscarDatos("SELECT id_usuario, usuario, nombre FROM usuarios WHERE id_usuario != '" 
                + user.Id.ToString() + "' AND id_usuario != '0'");
            for (int i = 0; i < lista.Count; i++)
                lvUsuarios.Items.Add(new ListViewItem(lista[i]));
        }

        private void btnAccionPermisos_Click(object sender, EventArgs e)
        {
            Sql.InsertarDatos("UPDATE usuarios SET permisos = '" + ExtraerPermisos() + "' WHERE id_usuario = '" +
                lvUsuarios.SelectedItems[0].Text + "'");
            MessageBox.Show("Guardado con exito", "Operacion Concretada", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void uscUsuarios_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void ckbClientes_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbClientes.Checked)
            {
                ckbClientesAgregar.Checked = true;
                ckbClientesVer.Checked = true;
                ckbClientesModificar.Checked = true;
                ckbClientesEliminar.Checked = true;
            }
        }

        private void ckbProductos_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbProductos.Checked)
            {
                ckbProductosAgregar.Checked = true;
                ckbProductosVer.Checked = true;
                ckbProductosModificar.Checked = true;
                ckbProductosEliminar.Checked = true;
            }
        }

        private void ckbClientesOpe_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbClientesAgregar.Checked || !ckbClientesVer.Checked ||
                !ckbClientesModificar.Checked || !ckbClientesEliminar.Checked)
                ckbClientes.Checked = false;
            else
                ckbClientes.Checked = true;
        }

        private void ckbProductosOpe_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbProductosAgregar.Checked || !ckbProductosVer.Checked ||
                !ckbProductosModificar.Checked || !ckbProductosEliminar.Checked)
                ckbProductos.Checked = false;
            else
                ckbProductos.Checked = true;
        }

        private void btnAccionAgregar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtUsuario.Text))
                MessageBox.Show("El usuario no puede estar vacio", String.Empty, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            else if (String.IsNullOrWhiteSpace(txtNombre.Text))
                MessageBox.Show("El nombre no puede estar vacio", String.Empty, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            else
            {
                bool confirmacion = Sql.Existe("SELECT usuario FROM usuarios WHERE usuario = '" + txtUsuario.Text + "'");

                if (!confirmacion)
                {
                    string operacion = "INSERT INTO usuarios VALUES(NULL, '" + txtUsuario.Text + "', '" +
                            Encriptacion.Encriptar(txtPassword.Text) + "', '" + txtNombre.Text + "', '" + txtApellidoPaterno.Text + "', '" +
                            txtApellidoMaterno.Text + "', '" + txtCorreo.Text + "', '" + ExtraerPermisos() + "')";
                    Sql.InsertarDatos(operacion);
                    MessageBox.Show("El usuario ha sido agregado con exito!", "Operacion Concretada", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    foreach (Control item in Controls)
                        if (item is TextBox)
                            item.Text = String.Empty;
                }
                else
                    MessageBox.Show("El usuario ya existe, intenta con otro", "Operacion cancelada", MessageBoxButtons.OK,
                       MessageBoxIcon.Information);

            }
        }

        private string ExtraerPermisos()
        {
            string permisos = String.Empty;
            foreach (Control item in grbPermisos.Controls)
                if (item is CheckBox)
                    if (((CheckBox)item).Checked)
                        permisos += "1";
                    else
                        permisos += "0";
            char[] array = permisos.ToCharArray();
            Array.Reverse(array);
            permisos = new string(array);
            return permisos;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            foreach (Control item in Controls)
                if (item is TextBox)
                    item.Text = String.Empty;
        }
    }
}
