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
    public partial class uscVerProductos : UserControl
    {
        private Redimension redimension;

        public uscVerProductos()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            Cargar();
        }

        private void Cargar()
        {
            dgvProductos.Columns[IndexColumna("seleccionador")].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvProductos.Columns[IndexColumna("codigo")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvProductos.Columns[IndexColumna("descripcion")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void uscBuscarProducto_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void Buscador()
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE (codigo LIKE '%" + txtBusqueda.Text + 
                "%' OR nombre LIKE '%" + txtBusqueda.Text + "%') AND eliminado  = 0");

            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            for (int i = 0; i < dgvProductos.RowCount; i++)
                if (bool.Parse(dgvProductos.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                    listaEliminar.Add(new string[1] { dgvProductos.Rows[i].Cells["codigo"].Value.ToString()});
            if (listaEliminar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaEliminar)
                    mensajeConfirmacion += datos[0] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " + "estos productos? \nListado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    foreach (string[] datos in listaEliminar)
                        Sql.InsertarDatos("UPDATE productos SET eliminado  = 1 WHERE codigo = '" + datos[0] + "'");
                    for (int i = 0; i < dgvProductos.RowCount; i++)
                        if (bool.Parse(dgvProductos.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            dgvProductos.Rows.RemoveAt(i);
                            i--;
                        }
                    MessageBox.Show("El listado de productos ha sido eliminado con exito.", "Productos eliminados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Seleccione al menos un producto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void dgvProductos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            float publico,distribuidor,precioMinimo;
            try
            {
                if (e.ColumnIndex == IndexColumna("seleccionador"))
                    return;
                if (dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText.ToString() == "")
                {
                    if (e.ColumnIndex != IndexColumna("codigo"))
                    {
                        float.TryParse(ValorCelda(e.RowIndex, "distribuidor"),out distribuidor);
                        float.TryParse(ValorCelda(e.RowIndex, "publico"),out publico);
                        float.TryParse(ValorCelda(e.RowIndex, "precioMinimo"),out precioMinimo);
                        if (distribuidor >= precioMinimo)
                        {
                            if (publico > precioMinimo)
                            {
                                Sql.InsertarDatos("UPDATE productos SET codigo='" + ValorCelda(e.RowIndex, "codigo") +
                                "', nombre='" + ValorCelda(e.RowIndex, "descripcion") +
                                "', existencia='" + ValorCelda(e.RowIndex, "cantidad") +
                                "', cantidad_minima='" + ValorCelda(e.RowIndex, "cantidadMinima") +
                                "', precio_publico='" + ValorCelda(e.RowIndex, "publico") +
                                "', precio_distribuidor='" + ValorCelda(e.RowIndex, "distribuidor") +
                                "', precio_minimo='" + ValorCelda(e.RowIndex, "precioMinimo") +
                                "' WHERE codigo = '" + ValorCelda(e.RowIndex, "codigo") + "'");
                            }
                            else
                                dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: El precio publico debe ser mayor al precio minimo.";
                        }
                        else
                            dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: El precio de distribuidor debe ser igual o mayor a precio minimo.";
                    }
                    else
                    {
                        dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: El codigo no se puede modificar.";
                    }
                }
            }
            catch (Exception) { }
        }

        private void dgvProductos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == IndexColumna("seleccionador"))
                return;
            if (!string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                if (e.ColumnIndex == IndexColumna("codigo") || e.ColumnIndex == IndexColumna("descripcion"))
                    dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
                else
                {
                    float temp;
                    if ((e.ColumnIndex == IndexColumna("cantidad") || e.ColumnIndex == IndexColumna("publico") ||
                        e.ColumnIndex == IndexColumna("distribuidor") || e.ColumnIndex == IndexColumna("cantidadMinima") || e.ColumnIndex == IndexColumna("precioMinimo")) && float.TryParse(e.FormattedValue.ToString(), out temp))
                    {
                        if (temp < 0)
                            dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Este dato debe ser mayor o igual a 0.";
                        else
                            dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
                    }
                    else
                        dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Este dato debe ser númerico.";
                }
            }
            else
                dgvProductos.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "Error: Inserte el dato.";
        }

        private int IndexColumna(string columna)
        {
            return dgvProductos.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvProductos.Rows[fila].Cells[columna].Value.ToString();
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Buscador();
        }
    }
}
