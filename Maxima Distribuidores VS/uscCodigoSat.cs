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
    public partial class uscCodigoSat : UserControl
    {
        public uscCodigoSat()
        {
            InitializeComponent();
            CompleteCodes();
            txtCodigoProducto.Enabled = false;
        }

        private void Buscador(string consulta)
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnBusqueda_Click(sender, new EventArgs());
        }

        private void BtnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("select a.codigo, a.nombre from productos a NATURAL LEFT JOIN codigo_producto b WHERE (a.eliminado=0 AND (a.codigo LIKE '%" + txtBusqueda.Text + "%' OR a.nombre LIKE '%" + txtBusqueda.Text + "%')) AND b.codigo IS NULL");
        }

        private void DgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                List<string[]> lista = Sql.BuscarDatos("select a.codigo, a.nombre " +
                    "from productos a NATURAL LEFT JOIN codigo_producto b where a.eliminado=0 AND a.codigo = '" +
                    dgvProductos.Rows[e.RowIndex].Cells["codigo"].Value + "'");
                txtCodigo.Text = lista[0][0];
                txtDescripcion.Text = lista[0][1];
                txtCodigoProducto.Enabled = true;
            }

        }

        void CompleteCodes()
        {
            List<string[]> lista;
            lista = Sql.BuscarDatos("SELECT c_ClaveProdServ, Descripción FROM f4_c_claveprodserv");
            txtCodigoProducto.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtCodigoProducto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            for (int i = 0; i < lista.Count; i++)
                col.Add(lista[i][1] + " -" + lista[i][0]);
            txtCodigoProducto.AutoCompleteCustomSource = col;
        }

        private void BtnAccion_Click(object sender, EventArgs e)
        {
            if (txtCodigoProducto.Text.Length == 0)
            {
                MessageBox.Show(this, "Error", "Debe agregar un codigo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtCodigo.Text.Length == 0)
            {
                MessageBox.Show(this, "Error", "Debe seleccionar un producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string[] separadas;
                separadas = txtCodigoProducto.Text.Split('-');
                string code = separadas[separadas.Count() - 1];
                Sql.InsertarDatos("INSERT INTO `codigo_producto`(`codigo`, `codigo_pro`) VALUES ('" + txtCodigo.Text + "','" + code + "')");
                Clear();
            }
        }
        void Clear()
        {
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            txtCodigoProducto.Text = "";
            txtBusqueda.Text = "";
            Buscador("select a.codigo, a.nombre from productos a NATURAL LEFT JOIN codigo_producto b WHERE (a.eliminado=0 AND (a.codigo LIKE '%" + txtBusqueda.Text + "%' OR a.nombre LIKE '%" + txtBusqueda.Text + "%')) AND b.codigo IS NULL");
        }
    }
}
