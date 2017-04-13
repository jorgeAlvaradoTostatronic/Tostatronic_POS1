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
    public partial class uscVentasPendientes : UserControl
    {
        Redimension redimension;
        public uscVentasPendientes()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            dgvPendientes.Columns[IndexColumna("cliente")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void uscPendientes_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.cancelada, venta.impuesto FROM venta,clientes,productos_de_venta" +
           " WHERE (venta.cancelada=0 AND (venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' OR venta.id_venta LIKE '%"+txtBusqueda.Text+"%')) AND venta.pagada=0" +
           " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
           "GROUP BY venta.id_venta ");
        }

        private void Buscador(string consulta)
        {
            dgvPendientes.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                List<string[]> abonos = new List<string[]>();
                try
                {
                    abonos = Sql.BuscarDatos("SELECT SUM(cantidad_abonada) FROM abonos WHERE id_venta='"+lista[i][0]+"' ");
                }
                catch (Exception) { }
                float total = float.Parse(lista[i][5]) * float.Parse(lista[i][7]);
                float faltante = total - float.Parse(abonos[0][0]);
                dgvPendientes.Rows.Add(lista[i][0], lista[0][1] + " " + lista[0][2] + " " + lista[0][3],
                    lista[i][4].Split(new char[] { ' ' })[0], total, abonos[0][0], faltante.ToString());
                dgvPendientes.Rows[i].Cells["abonar"].Value = "Abonar";
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.cancelada FROM venta,clientes,productos_de_venta" +
           " WHERE (venta.cancelada=0 AND venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%') AND venta.pagada=0" +
           " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
           "GROUP BY venta.id_venta ");
        }

        private void dgvPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IndexColumna("abonar") == e.ColumnIndex)
            {
                dialogAbonar dgAbonar = new dialogAbonar(float.Parse(ValorCelda(e.RowIndex, "total")),
                    float.Parse(ValorCelda(e.RowIndex, "abono")));
                dgAbonar.ShowDialog();
                if (!dgAbonar.Cancelar)
                {
                    string fecha1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (dgAbonar.Pagado)
                    {
                        Sql.InsertarDatos("INSERT INTO abonos VALUES ('NULL','" + ValorCelda(e.RowIndex, "id_pedido") + "','" + dgvPendientes.Rows[e.RowIndex].Cells["faltante"].Value.ToString() + "','" + fecha1 + "' )");
                        Sql.InsertarDatos("UPDATE venta SET pagada=1 WHERE id_venta='" + ValorCelda(e.RowIndex, "id_pedido") + "';");
                        ImpresionTickets.ImprimeTicketPago(ValorCelda(e.RowIndex, "id_pedido"), dgAbonar.Abonado, 0, fecha1, dgvPendientes.Rows[e.RowIndex].Cells[1].Value.ToString());
                        dgvPendientes.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        Sql.InsertarDatos("INSERT INTO abonos VALUES ('NULL','" + ValorCelda(e.RowIndex, "id_pedido") + "','" + dgAbonar.Abonado.ToString() + "','" + fecha1 + "' )");
                        dgvPendientes.Rows[e.RowIndex].Cells["abono"].Value = dgAbonar.Abono.ToString();
                        dgvPendientes.Rows[e.RowIndex].Cells["faltante"].Value = (float.Parse(ValorCelda(e.RowIndex, "total")) -
                            float.Parse(ValorCelda(e.RowIndex, "abono"))).ToString();
                        ImpresionTickets.ImprimeTicketPago(ValorCelda(e.RowIndex, "id_pedido"), dgAbonar.Abonado, float.Parse(ValorCelda(e.RowIndex, "faltante")), fecha1, dgvPendientes.Rows[e.RowIndex].Cells[1].Value.ToString());
                    }
                    
                    //PDFFile.Ver(Application.StartupPath + "\\Pago.pdf");
                }
                dgAbonar.Dispose();
            }
        }

        private int IndexColumna(string columna)
        {
            return dgvPendientes.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvPendientes.Rows[fila].Cells[columna].Value.ToString();
        }
    }
}
