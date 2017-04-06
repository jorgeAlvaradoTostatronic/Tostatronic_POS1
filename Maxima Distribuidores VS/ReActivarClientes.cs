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
    public partial class ReActivarClientes : UserControl
    {
        public ReActivarClientes()
        {
            InitializeComponent();
        }
        private void Buscador(string consulta)
        {
            dgvClientes.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            DataGridViewComboBoxColumn comboboxColumn = dgvClientes.Columns["tipo"] as DataGridViewComboBoxColumn;
            comboboxColumn.DataSource = Sql.CrearTablaDatos("SELECT * FROM tipo_cliente");
            comboboxColumn.DisplayMember = "descripcion";
            comboboxColumn.ValueMember = "id_tipo_cliente";
            for (int i = 0; i < lista.Count; i++)
            {
                dgvClientes.Rows.Add(lista[i][0], lista[i][1], lista[i][2], lista[i][3], lista[i][4], lista[i][5], lista[i][6], lista[i][7], lista[i][8], null, lista[i][10]);
                dgvClientes.Rows[i].Cells["tipo"].Value = int.Parse(lista[i][9]);
            }
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT id_cliente, rfc, nombres, apellido_paterno,apellido_materno, telefono,celular,domicilio,correo_electronico,id_tipo_cliente,descripcion FROM clientes " +
              "WHERE (rfc LIKE '%" + txtBuscar.Text + "%' OR nombres LIKE '%" + txtBuscar.Text + "%' OR apellido_paterno LIKE '%" + txtBuscar.Text + "%') AND rfc <> 'xxxxxxxxxxxxx' AND eliminado = 1");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> listaActivar = new List<string[]>();

                for (int i = 0; i < dgvClientes.RowCount; i++)
                    if (bool.Parse(dgvClientes.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        listaActivar.Add(new string[2] { dgvClientes.Rows[i].Cells["id_cliente"].Value.ToString(),
                        dgvClientes.Rows[i].Cells["rfc"].Value.ToString() });
                if (listaActivar.Count > 0)
                {
                    string mensajeConfirmacion = "";
                    foreach (string[] datos in listaActivar)
                        mensajeConfirmacion += datos[1] + "\n";
                    DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea re-activar " + "estos(es) cliente(s)? \nListado: \n" +
                        mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmacion == DialogResult.Yes)
                    {
                        foreach (string[] datos in listaActivar)
                            Sql.InsertarDatos("UPDATE clientes SET eliminado = 0 WHERE id_cliente = '" + datos[0] + "'");
                        for (int i = 0; i < dgvClientes.RowCount; i++)
                            if (bool.Parse(dgvClientes.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                            {
                                dgvClientes.Rows.RemoveAt(i);
                                i--;
                            }
                        MessageBox.Show("Re-activados");
                    }
                }
                else
                    MessageBox.Show("Seleccione al menos un cliente.");
            }
            catch (Exception) { }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == char.Parse(" "))
                e.Handled = false;
            else if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
