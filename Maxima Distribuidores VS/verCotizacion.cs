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
    public partial class verCotizacion : UserControl
    {
        float impuesto = 1;
        string fechaT = "";
        string fol = "";
        public verCotizacion()
        {
            InitializeComponent();
        }
        private void Buscador(string consulta)
        {
            dgvVerVentas.Rows.Clear();
            List<string[]> lista;
            //consulta = "SELECT id_venta,id_cliente,fecha_de_venta FROM venta WHERE fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%';";
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
                dgvVerVentas.Rows.Add(lista[i][0], lista[i][1], lista[i][2]);
        }

        private void LlenaVenta(string folio)
        {
            dgvVerVentas.Rows.Clear();
            List<string[]> productos;
            List<string[]> venta;
            string consulta = "SELECT productos_de_cotizacion.id_producto, productos.nombre, productos_de_cotizacion.cantidad_cotizada, productos_de_cotizacion.precio_al_momento, productos_de_cotizacion.descuento " +
                "FROM productos_de_cotizacion, productos " +
                "WHERE productos_de_cotizacion.id_cotizacion=" + folio + " AND productos.codigo=productos_de_cotizacion.id_producto;";
            productos = Sql.BuscarDatos(consulta);
            consulta = "SELECT clientes.rfc, clientes.nombres,clientes.apellido_paterno, clientes.id_cliente " +
                "FROM cotizacion, clientes " +
                "WHERE (cotizacion.id_cotizacion=" + folio + ") " +
                "AND clientes.id_cliente=cotizacion.id_cliente " +
                "GROUP BY cotizacion.id_cotizacion;";
            venta = Sql.BuscarDatos(consulta);
            consulta = "SELECT impuesto FROM cotizacion WHERE id_cotizacion=" + folio + ";";
            impuesto = float.Parse(Sql.BuscarDatos(consulta)[0][0]);
            txtRfc.Text = venta[0][0];
            txtNombre.Text = venta[0][1];
            txtApellidoPaterno.Text = venta[0][2];
            float sub;
            for (int i = 0; i < productos.Count; i++)
            {
                sub = float.Parse(productos[i][3]);
                sub *= float.Parse(productos[i][2]);
                sub -= sub * Descuento(productos[i][0], venta[0][3]) / 100;
                dgvVentas.Rows.Add(productos[i][0], productos[i][1], productos[i][2], productos[i][3], sub.ToString(), productos[i][4]);
            }
            Total();
        }
        private float Descuento(string id_producto, string id_cliente)
        {
            try
            {
                List<string[]> descuento = Sql.BuscarDatos("SELECT descuento FROM descuentos WHERE id_cliente = '" + id_cliente + "' AND id_producto = '" + id_producto + "'");
                if (descuento.Count > 0)
                    return float.Parse(descuento[0][0]);
                else
                    return 0;
            }
            catch (Exception) { return 0; }
        }
        private void Total()
        {
            float total = 0;
            for (int i = 0; i < dgvVentas.RowCount; i++)
                total += float.Parse(ValorCelda(i, "subtotal"));
            txtSubTotal.Text = total.ToString("$0.00");
            yxyIva.Text = (total * (impuesto-1)).ToString("$0.00");
            total *= impuesto;
            txtTotal.Text = total.ToString("$0.00");
        }
        private string ValorCelda(int fila, string columna)
        {
            return dgvVentas.Rows[fila].Cells[columna].Value.ToString();
        }

        private void btnBusqueda_Click_1(object sender, EventArgs e)
        {
            Buscador("SELECT cotizacion.id_cotizacion, clientes.rfc, cotizacion.fecha_cotizacion" +
          " FROM cotizacion, clientes" +
          " WHERE ((cotizacion.id_cotizacion LIKE '%" + txtBusqueda.Text + "%')) " +
          " AND (clientes.id_cliente=cotizacion.id_cliente) " +
          "GROUP BY cotizacion.id_cotizacion ");
        }

        private void dtpFecha_ValueChanged_1(object sender, EventArgs e)
        {
            Buscador("SELECT cotizacion.id_cotizacion, clientes.rfc, cotizacion.fecha_cotizacion " +
           " FROM cotizacion, clientes" +
           " WHERE ((cotizacion.fecha_cotizacion LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' )) " +
           " AND (clientes.id_cliente=cotizacion.id_cliente) " +
           "GROUP BY cotizacion.id_cotizacion ");
        }

        private void txtBusqueda_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click_1(sender, new EventArgs());
        }

        private void dgvVerVentas_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVerVentas.RowCount > 0)
            {
                dgvVentas.Rows.Clear();
                fechaT = dgvVerVentas.Rows[e.RowIndex].Cells[2].Value.ToString();
                fol = dgvVerVentas.Rows[e.RowIndex].Cells[0].Value.ToString();
                LlenaVenta(dgvVerVentas.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvVentas.RowCount > 1)
            {
                string id_cliente = Sql.BuscarDatos("SELECT id_cliente FROM clientes WHERE rfc = '" + txtRfc.Text + "'")[0][0];
                string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                float totalParcial = 0;
                List<ProductoCompleto> productos = new List<ProductoCompleto>();
                ProductoCompleto producto;
                for (int i = 0; i < dgvVentas.RowCount; i++)
                {
                    producto = new ProductoCompleto(dgvVentas.Rows[i].Cells["codigo"].Value.ToString(), dgvVentas.Rows[i].Cells["descripcion"].Value.ToString(),
                        float.Parse(dgvVentas.Rows[i].Cells["cantidad"].Value.ToString()), int.Parse(dgvVentas.Rows[i].Cells["descuentoPro"].Value.ToString()),
                        float.Parse(dgvVentas.Rows[i].Cells["subtotal"].Value.ToString()));
                    productos.Add(producto);
                    totalParcial += float.Parse(dgvVentas.Rows[i].Cells["subtotal"].Value.ToString());
                }
                ImpresionTickets.ImprimeTicket(fol, productos, totalParcial, totalParcial, fechaT, txtNombre.Text, txtApellidoPaterno.Text, impuesto);
                PDFFile.Ver(Application.StartupPath + "\\Ticket.pdf");
            }
        }
    }
}
