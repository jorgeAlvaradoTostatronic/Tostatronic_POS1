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
    public partial class uscCredito : UserControl
    {
        int id_cliente;
        public uscCredito()
        {
            InitializeComponent();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (dgvCredito.Rows.Count <= 0)
            {
                dialogBusquedaClientes buscarCliente = new dialogBusquedaClientes();
                try
                {
                    dgvCredito.Rows.Clear();
                    buscarCliente.ShowDialog();
                    id_cliente = int.Parse(buscarCliente.Cliente[0]);
                    txtRfc.Text = buscarCliente.Cliente[1];
                    txtNombre.Text = buscarCliente.Cliente[2];
                    txtApellidoPaterno.Text = buscarCliente.Cliente[3];
                    llenaData();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error");
                }
                buscarCliente.Dispose();
            }
            else
            {
                DialogResult confirmacion = MessageBox.Show(this, "Si cambia de cliente perdera los datos de la consulta actual\n" +
                    "¿Desea continuar?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    dgvCredito.Rows.Clear();
                    txtTotal.Text = "$0.00";
                    btnBuscarCliente_Click(sender, new EventArgs());
                }
            }
        }

        private void llenaData()
        {
            string consulta = "SELECT  venta.id_venta, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.impuesto" +
           " FROM venta,productos_de_venta" +
           " WHERE (venta.cancelada=0  AND venta.pagada=0 AND venta.id_cliente="+id_cliente+")" +
           " AND (productos_de_venta.id_venta=venta.id_venta)" +
           "GROUP BY venta.id_venta ";
            List<string[]> datos = Sql.BuscarDatos(consulta);
            float total,totalDeuda=0;
            float impuesto;
            float restante;
            List<string[]> abonos = new List<string[]>();
            foreach (string[] a in datos)
            {
                try
                {
                    abonos = Sql.BuscarDatos("SELECT SUM(cantidad_abonada) FROM abonos WHERE id_venta='" + a[0] + "' ");
                }
                catch (Exception) { }
                impuesto = (float.Parse(a[3]) - 1) * 100;
                total = float.Parse(a[2]) * float.Parse(a[3]);
                restante = total - float.Parse(abonos[0][0]);
                totalDeuda += restante;
                dgvCredito.Rows.Add(a[0],a[1],a[2],impuesto.ToString(), total.ToString("$0.00"), float.Parse(abonos[0][0]).ToString("$0.00"), restante.ToString("$0.00"));
            }
            txtTotal.Text = totalDeuda.ToString("$0.00");
        }
    }
}
