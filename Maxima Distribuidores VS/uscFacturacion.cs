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
    public partial class uscFacturacion : UserControl
    {
        string fechaT = "";
        string fol = "";
        float impuesto = 1;
        float bigTotal = 0;
        string folAux = "0";
        List<ProductoSat> articulos = new List<ProductoSat>();
        public uscFacturacion()
        {
            InitializeComponent();
            CompleteCodes();
        }

        void CompleteCodes()
        {
            List<string[]> lista;
            lista = Sql.BuscarDatos("SELECT c_UsoCFDI, Descripción FROM f4_c_usocfdi");
            txtUsoCFDI.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtUsoCFDI.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            for (int i = 0; i < lista.Count; i++)
                col.Add(lista[i][1] + " -" + lista[i][0]);
            txtUsoCFDI.AutoCompleteCustomSource = col;

            lista = Sql.BuscarDatos("SELECT c_MetodoPago, Descripción FROM f4_c_metodopago");
            txtMetodo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtMetodo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            col = new AutoCompleteStringCollection();
            for (int i = 0; i < lista.Count; i++)
                col.Add(lista[i][1] + " -" + lista[i][0]);
            txtMetodo.AutoCompleteCustomSource = col;

            lista = Sql.BuscarDatos("SELECT c_FormaPago, Descripción FROM f4_c_formapago");
            txtForma.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtForma.AutoCompleteSource = AutoCompleteSource.CustomSource;
            col = new AutoCompleteStringCollection();
            for (int i = 0; i < lista.Count; i++)
                col.Add(lista[i][1] + " -" + lista[i][0]);
            txtForma.AutoCompleteCustomSource = col;
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
            txtNombre.Text = venta[0][1]+ " "+ venta[0][2]+ " " + venta[0][4];
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
           " WHERE (venta.cancelada=0 AND venta.facturada=0 AND (venta.id_venta LIKE '%" + txtBusqueda.Text + "%'))" +
           " AND (clientes.id_cliente=venta.id_cliente) " +
           "GROUP BY venta.id_venta ");
        }

        private void DtpFecha_ValueChanged(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.rfc, venta.fecha_de_venta " +
           " FROM venta, clientes" +
           " WHERE (venta.cancelada=0 AND venta.facturada=0 AND (venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' )) " +
           " AND (clientes.id_cliente=venta.id_cliente) " +
           "GROUP BY venta.id_venta ");
        }

        private void DgvVerVentas_DoubleClick(object sender, EventArgs e)
        {
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

        private void BtnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtUsoCFDI.Text.Equals(""))
                {
                    MessageBox.Show(this, "Seleccione el uso del CFDI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if(txtForma.Text.Equals(""))
                {
                    MessageBox.Show(this, "Seleccione la forma de pago", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txtMetodo.Text.Equals(""))
                {
                    MessageBox.Show(this, "Seleccione el metodo de pago", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (txtMail.Text.Equals(""))
                {
                    MessageBox.Show(this, "Agregue el correo al que se le enviara el paquete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (folAux!="0")
                {
                    string fp, mp, ucfdi;
                    string[] separadas;
                    string aux="0";
                    int aucN;
                    separadas = txtForma.Text.Split('-');
                    fp = separadas[separadas.Count() - 1];
                    int.TryParse(fp, out aucN);
                    if (aucN < 10)
                        fp = aux + fp;

                    separadas = txtMetodo.Text.Split('-');
                    mp = separadas[separadas.Count() - 1];

                    separadas = txtUsoCFDI.Text.Split('-');
                    ucfdi = separadas[separadas.Count() - 1];
                    string mess = Facturacion.CreaFactura(folAux, fp, mp, articulos, bigTotal, txtRfc.Text, txtNombre.Text, ucfdi,txtMail.Text);
                    if (mess=="")
                        MessageBox.Show(this, "Factura Creada con exito", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(this, "Error: Factura no creada\n"+mess, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DesactivaCampos();
        }

        private void TxtMetodo_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtForma_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtUsoCFDI_TextChanged(object sender, EventArgs e)
        {

        }

        private void ActivaCamposDeTexto()
        {
            txtForma.Enabled = true;
            txtMetodo.Enabled = true;
            txtNombre.Enabled = true;
            txtRfc.Enabled = true;
            txtUsoCFDI.Enabled = true;
            txtMail.Enabled = true;

            txtUsoCFDI.Text = "Gastos en general -G03";
            txtForma.Text = "Transferencia electrónica de fondos -3";
            txtMetodo.Text = "Pago en una sola exhibición -PUE";
        }

        void DesactivaCampos()
        {
            txtForma.Text = "";
            txtMetodo.Text = "";
            txtNombre.Text = "";
            txtRfc.Text = "";
            txtUsoCFDI.Text = "";

            txtForma.Enabled = false;
            txtMetodo.Enabled = false;
            txtNombre.Enabled = false;
            txtRfc.Enabled = false;
            txtUsoCFDI.Enabled = false;
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

        private void BtnRF_Click(object sender, EventArgs e)
        {

        }
    }
}
