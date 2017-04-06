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
    public partial class uscDescuentoClientes : UserControl
    {
        private int id_cliente;
        private int tipoCliente;
        private Redimension redimension;

        public uscDescuentoClientes()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            dgvDescuentos.Columns[IndexColumna("codigo")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvDescuentos.Columns[IndexColumna("descripcion")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            dialogBusquedaClientes buscarCliente = new dialogBusquedaClientes();
            try
            {
                buscarCliente.ShowDialog();
                id_cliente = int.Parse(buscarCliente.Cliente[0]);
                txtRfc.Text = buscarCliente.Cliente[1];
                txtNombre.Text = buscarCliente.Cliente[2];
                txtApellidoPaterno.Text = buscarCliente.Cliente[3];
                tipoCliente = int.Parse(buscarCliente.Cliente[4]);
                btnBuscarProducto.Enabled = true;
                txtBusquedaProducto.Enabled = true;
                if (tipoCliente == 1)
                    Buscador("SELECT  codigo, nombre, precio_distribuidor FROM productos");
                else
                    Buscador("SELECT codigo, nombre, precio_publico FROM productos");
            }
            catch (Exception) { }
        }

        private void Buscador(string consulta)
        {
            dgvDescuentos.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvDescuentos.Rows.Add(lista[i]);
                List<string[]> descuento = Sql.BuscarDatos("SELECT descuento FROM descuentos WHERE id_cliente = '" + id_cliente + "' AND id_producto = '" + lista[i][0] + "'");
                if (descuento.Count > 0)
                    dgvDescuentos.Rows[dgvDescuentos.Rows.Count - 1].Cells["descuento"].Value = descuento[0][0];
                else
                    dgvDescuentos.Rows[dgvDescuentos.Rows.Count - 1].Cells["descuento"].Value = "0";
                    
            }
        }

        private void dgvDescuentos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText.ToString() == "")
                {
                    if (Sql.Existe("SELECT id_cliente FROM descuentos WHERE id_cliente = '" + id_cliente + "' AND id_producto = '" + ValorCelda(e.RowIndex, "codigo") + "'"))
                        Sql.InsertarDatos("UPDATE descuentos SET descuento='" + ValorCelda(e.RowIndex, "descuento") +
                            "' WHERE id_cliente = '" + id_cliente + "' AND id_producto= '" + ValorCelda(e.RowIndex, "codigo") + "'");
                    else
                        Sql.InsertarDatos("INSERT INTO descuentos VALUES (" + id_cliente + ",'" + ValorCelda(e.RowIndex, "codigo") + "'," + ValorCelda(e.RowIndex, "descuento") + ")");
                }
            }
            catch (Exception) { }
        }

        private int IndexColumna(string columna)
        {
            return dgvDescuentos.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvDescuentos.Rows[fila].Cells[columna].Value.ToString();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            if (tipoCliente == 1)
                Buscador("SELECT  codigo, nombre, precio_distribuidor FROM productos WHERE (codigo LIKE '%" +
                    txtBusquedaProducto.Text + "%' OR nombre LIKE '%" + txtBusquedaProducto.Text + "%') AND eliminado  = 0");
            else
                Buscador("SELECT  codigo, nombre, precio_publico FROM productos WHERE (codigo LIKE '%" +
                    txtBusquedaProducto.Text + "%' OR nombre LIKE '%" + txtBusquedaProducto.Text + "%') AND eliminado = 0");
        }

        private void txtBusquedaProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBuscarProducto_Click(sender, e);
        }

        private void dgvDescuentos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == IndexColumna("descuento"))
                if (!string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    float temp;
                    if (float.TryParse(e.FormattedValue.ToString(), out temp))
                    {
                        if (temp < 0)
                            dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Este dato debe ser mayor o igual a 0.";
                        else if (temp > 100)
                            dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Este dato debe ser menor o igual a 100.";
                        else
                            dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
                    }
                    else
                        dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Este dato debe ser númerico.";
                }
                else
                    dgvDescuentos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Inserte el dato.";
        }

        private void uscDescuentoClientes_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }
    }
}
