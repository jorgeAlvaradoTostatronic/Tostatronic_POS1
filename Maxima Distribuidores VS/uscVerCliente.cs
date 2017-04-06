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
    public partial class uscVerCliente : UserControl
    {
        private Redimension redimension;

        public uscVerCliente()
        {
            InitializeComponent();
            redimension = new Redimension(this);
        }

        private void uscBuscarCliente_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT id_cliente, rfc, nombres, apellido_paterno,apellido_materno, telefono,celular,domicilio,correo_electronico,id_tipo_cliente,descripcion FROM clientes " +
               "WHERE (rfc LIKE '%" + txtBuscar.Text + "%' OR nombres LIKE '%" + txtBuscar.Text + "%' OR apellido_paterno LIKE '%" + txtBuscar.Text + "%') AND rfc <> 'xxxxxxxxxxxxx' AND eliminado = 0");
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

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private int IndexColumna(string columna)
        {
            return dgvClientes.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvClientes.Rows[fila].Cells[columna].Value.ToString();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> listaEliminar = new List<string[]>();

                for (int i = 0; i < dgvClientes.RowCount; i++)
                    if (bool.Parse(dgvClientes.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        listaEliminar.Add(new string[2] { dgvClientes.Rows[i].Cells["id_cliente"].Value.ToString(),
                        dgvClientes.Rows[i].Cells["rfc"].Value.ToString() });
                if (listaEliminar.Count > 0)
                {
                    string mensajeConfirmacion = "";
                    foreach (string[] datos in listaEliminar)
                        mensajeConfirmacion += datos[1] + "\n";
                    DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " + "estos(es) cliente(s)? \nListado: \n" +
                        mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmacion == DialogResult.Yes)
                    {
                        foreach (string[] datos in listaEliminar)
                            Sql.InsertarDatos("UPDATE clientes SET eliminado = 1 WHERE id_cliente = '" + datos[0] + "'");
                        for (int i = 0; i < dgvClientes.RowCount; i++)
                            if (bool.Parse(dgvClientes.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                            {
                                dgvClientes.Rows.RemoveAt(i);
                                i--;
                            }
                        MessageBox.Show("Eliminados");
                    }
                }
                else
                    MessageBox.Show("Seleccione al menos un cliente.");
            }
            catch (Exception){}
        }

        private void dgvClientes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == IndexColumna("seleccionador"))
                    return;
                if (dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText.ToString() == "")
                {
                    if (e.ColumnIndex != IndexColumna("rfc") || (e.ColumnIndex == IndexColumna("rfc") &&
                        !Sql.Existe("SELECT id_cliente FROM clientes WHERE rfc = '" + ValorCelda(e.RowIndex, "rfc")
                        + "' AND id_cliente <> '" + ValorCelda(e.RowIndex, "id_cliente") + "'")))
                    {
                        Sql.InsertarDatos("UPDATE clientes SET rfc='" + ValorCelda(e.RowIndex, "rfc") +
                            "', nombres='" + ValorCelda(e.RowIndex, "nombre") +
                            "', descripcion='" + ValorCelda(e.RowIndex, "descripcion") +
                            "', apellido_paterno='" + ValorCelda(e.RowIndex, "paterno") +
                            "', apellido_materno='" + ValorCelda(e.RowIndex, "materno") +
                            "', telefono='" + ValorCelda(e.RowIndex, "telefono") +
                            "', celular='" + ValorCelda(e.RowIndex, "celular") +
                            "', domicilio='" + ValorCelda(e.RowIndex, "direccion") +
                            "', correo_electronico='" + ValorCelda(e.RowIndex, "correo") +
                            "' WHERE id_cliente = '" + ValorCelda(e.RowIndex, "id_cliente") + "'");
                    }
                    else
                    {
                        dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: El código ya existe.";
                    }
                        
                }
            }
            catch (Exception) { }
        }

        private void dgvClientes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvClientes.Columns[e.ColumnIndex].Name == "tipo")
                {
                    DataGridViewComboBoxCell tipoC = dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;
                    if (int.Parse(tipoC.Value.ToString()) == 1)
                    {
                        Sql.InsertarDatos("UPDATE clientes SET id_tipo_cliente = 1 WHERE  id_cliente = '" + ValorCelda(e.RowIndex, "id_cliente") + "'");
                    }
                    else if (int.Parse(tipoC.Value.ToString()) == 2)
                    {
                        Sql.InsertarDatos("UPDATE clientes SET id_tipo_cliente = 2 WHERE  id_cliente = '" + ValorCelda(e.RowIndex, "id_cliente") + "'");
                    }
                    else
                        MessageBox.Show("Seleccione un tipo de cliente");
                }
            }
            catch (Exception){}
            
        }

        private void dgvClientes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == IndexColumna("seleccionador"))
                    return;
                if (!string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    if (e.ColumnIndex == IndexColumna("rfc") || e.ColumnIndex == IndexColumna("nombre") || e.ColumnIndex == IndexColumna("paterno") || e.ColumnIndex == IndexColumna("materno") || e.ColumnIndex == IndexColumna("telefono") || e.ColumnIndex == IndexColumna("celular") || e.ColumnIndex == IndexColumna("direccion") || e.ColumnIndex == IndexColumna("correo") || e.ColumnIndex == IndexColumna("descripcion"))
                        dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
                }
                else
                    dgvClientes.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Inserte el dato.";
            }
            catch (Exception){}
            
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
