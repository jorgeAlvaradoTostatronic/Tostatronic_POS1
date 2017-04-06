using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public partial class dialogBusquedaClientes : Form
    {
        private string[] cliente;

        public string[] Cliente
        {
            get { return cliente; }
        }

        public dialogBusquedaClientes()
        {
            InitializeComponent();
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT id_cliente,rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes " +
                    "WHERE (rfc LIKE '%" + txtBusqueda.Text + "%' OR nombres LIKE '%" + txtBusqueda.Text + "%' OR apellido_paterno LIKE '%" + txtBusqueda.Text + "%') AND eliminado = 0");
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
            {
                if (Int16.Parse(lista[i][4]) == 1)
                    lista[i][4] = "Distribuidor";
                else
                    lista[i][4] = "Publico";
                dgvClientes.Rows.Add(lista[i]);
            }
        }

        private void dialogBusquedaClientes_Load(object sender, EventArgs e)
        {
            Buscador("SELECT id_cliente, rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes WHERE eliminado = 0");
        }

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT id_cliente,rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes WHERE id_cliente = '" +
            ValorCelda(e.RowIndex, "clmId") + "'");
                cliente = lista[0];
            }
            catch (Exception){}
            
            this.Close();
        }
        private int IndexColumna(string columna)
        {
            return dgvClientes.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvClientes.Rows[fila].Cells[columna].Value.ToString();
        }

    }
}
