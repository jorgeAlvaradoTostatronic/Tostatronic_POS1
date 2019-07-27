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
    public partial class uscRecuperaFactura : UserControl
    {
        string fechaT = "";
        string fol = "";
        float impuesto = 1;
        float bigTotal = 0;
        string folAux = "0";
        List<ProductoSat> articulos = new List<ProductoSat>();
        public uscRecuperaFactura()
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
            string consulta = "SELECT productos_de_venta.id_producto, productos.nombre, productos_de_venta.cantidad_comprada, productos_de_venta.precio_al_momento, productos_de_venta.descuento " +
                "FROM productos_de_venta, productos " +
                "WHERE productos_de_venta.id_venta=" + folio + " AND productos.codigo=productos_de_venta.id_producto;";
            productos = Sql.BuscarDatos(consulta);
            consulta = "SELECT clientes.rfc, clientes.nombres,clientes.apellido_paterno, clientes.id_cliente,clientes.apellido_materno, clientes.correo_electronico " +
                "FROM venta, clientes " +
                "WHERE (venta.id_venta=" + folio + " AND venta.cancelada=0) " +
                "AND clientes.id_cliente=venta.id_cliente " +
                "GROUP BY venta.id_venta;";
            venta = Sql.BuscarDatos(consulta);
            consulta = "SELECT impuesto FROM venta WHERE id_venta=" + folio + ";";
            impuesto = float.Parse(Sql.BuscarDatos(consulta)[0][0]);
            txtRfc.Text = venta[0][0];
            txtNombre.Text = venta[0][1] + " " + venta[0][2] + " " + venta[0][4];
            txtMail.Text = venta[0][5];
            float sub;
            articulos = new List<ProductoSat>();
            for (int i = 0; i < productos.Count; i++)
            {
                sub = float.Parse(productos[i][3]);
                sub *= float.Parse(productos[i][2]);
                sub -= sub * Descuento(productos[i][0], venta[0][3]) / 100;
                articulos.Add(new ProductoSat(productos[i][1], Sql.GetCodigoSATProducto(productos[i][0]), float.Parse(productos[i][2]), float.Parse(productos[i][3]), sub));
                dgvVentas.Rows.Add(productos[i][0], productos[i][1], productos[i][2], productos[i][3], sub.ToString(), productos[i][4]);

            }
            Total();
            ActivaCamposDeTexto();
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
            yxyIva.Text = (total * (0.16)).ToString("$0.00");
            bigTotal = total;
            total *= 1.16f;
            txtTotal.Text = total.ToString("$0.00");

        }
        private string ValorCelda(int fila, string columna)
        {
            return dgvVentas.Rows[fila].Cells[columna].Value.ToString();
        }

        private void BtnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.rfc, venta.fecha_de_venta" +
           " FROM venta, clientes" +
           " WHERE (venta.cancelada=0 AND venta.facturada=1 AND (venta.id_venta LIKE '%" + txtBusqueda.Text + "%'))" +
           " AND (clientes.id_cliente=venta.id_cliente) " +
           "GROUP BY venta.id_venta ");
        }

        private void DtpFecha_ValueChanged(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.rfc, venta.fecha_de_venta " +
           " FROM venta, clientes" +
           " WHERE (venta.cancelada=0 AND venta.facturada=1 AND (venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' )) " +
           " AND (clientes.id_cliente=venta.id_cliente) " +
           "GROUP BY venta.id_venta ");
        }

        private void DgvVerVentas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVerVentas.RowCount > 0)
            {
                dgvVentas.Rows.Clear();
                fechaT = dgvVerVentas.Rows[e.RowIndex].Cells[2].Value.ToString();
                fol = dgvVerVentas.Rows[e.RowIndex].Cells[0].Value.ToString();
                folAux = dgvVerVentas.Rows[e.RowIndex].Cells[0].Value.ToString();
                LlenaVenta(dgvVerVentas.Rows[e.RowIndex].Cells[0].Value.ToString());

            }
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnBusqueda_Click(sender, new EventArgs());
        }

        private void ActivaCamposDeTexto()
        {
            txtNombre.Enabled = true;
            txtRfc.Enabled = true;
            txtMail.Enabled = true;
        }

        void DesactivaCampos()
        {
            txtNombre.Text = "";
            txtRfc.Text = "";
            txtNombre.Enabled = false;
            txtRfc.Enabled = false;
            txtMail.Enabled = false;

            txtSubTotal.Text = "$0.00";
            txtTotal.Text = "$0.00";
            yxyIva.Text = "$0.00";

            dgvVerVentas.Rows.Clear();
            dgvVentas.Rows.Clear();

            bigTotal = 0;

            folAux = "0";

            txtMail.Text = "";
        }
    }
}
