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
    public partial class uscTipoProductos : UserControl
    {
        Redimension redimension;
        private TipoProducto tipoProducto;

        public uscTipoProductos(TipoProducto tipoProducto)
        {
            InitializeComponent();
            redimension = new Redimension(this);
            this.tipoProducto = tipoProducto;
            Cargar();
        }

        private void Cargar()
        {
            switch (tipoProducto)
            {
                case TipoProducto.Publico:
                    publico.Visible = true;
                    lblTitulo.Text = "Precios para el público";
                    dgvProductos.Columns[IndexColumna("publico")].DefaultCellStyle.BackColor = Color.LightGreen;
                    break;
                case TipoProducto.Distribuidor:
                    lblTitulo.Text = "Precios para distribuidores";
                    dgvProductos.Columns[IndexColumna("distribuidor")].DefaultCellStyle.BackColor = Color.LightGreen;
                    distribuidor.Visible = true;
                    break;
            }
            dgvProductos.Columns[IndexColumna("codigo")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvProductos.Columns[IndexColumna("descripcion")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void Buscador()
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista = Sql.BuscarDatos("SELECT  * FROM " +
                "productos WHERE codigo LIKE '%" + txtBusqueda.Text + "%' OR nombre LIKE '%" + txtBusqueda.Text + "%'");

            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
        }

        private void uscTipoProductos_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
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
