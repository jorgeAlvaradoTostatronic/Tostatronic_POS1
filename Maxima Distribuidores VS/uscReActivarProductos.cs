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
    public partial class uscReActivarProductos : UserControl
    {
        public uscReActivarProductos()
        {
            InitializeComponent();
        }
        private void Buscador()
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista = Sql.BuscarDatos("SELECT *FROM productos WHERE (codigo LIKE '%" + txtBusqueda.Text +
                "%' OR nombre LIKE '%" + txtBusqueda.Text + "%') AND eliminado  = 1");

            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaReActivar = new List<string[]>();

            for (int i = 0; i < dgvProductos.RowCount; i++)
                if (bool.Parse(dgvProductos.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                    listaReActivar.Add(new string[1] {dgvProductos.Rows[i].Cells["codigo"].Value.ToString() });
            if (listaReActivar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaReActivar)
                    mensajeConfirmacion += datos[0] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea re-activar " + "estos productos? \nListado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    foreach (string[] datos in listaReActivar)
                        Sql.InsertarDatos("UPDATE productos SET eliminado  = 0 WHERE codigo = '" + datos[0] + "'");
                    for (int i = 0; i < dgvProductos.RowCount; i++)
                        if (bool.Parse(dgvProductos.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            dgvProductos.Rows.RemoveAt(i);
                            i--;
                        }
                    MessageBox.Show("El listado de productos ha sido re-activado con exito.", "Productos re-activados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Seleccione al menos un producto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        
        }

    }
}
