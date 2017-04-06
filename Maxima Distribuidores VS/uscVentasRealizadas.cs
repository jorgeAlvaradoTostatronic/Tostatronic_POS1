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
    public partial class uscVentasRealizadas : UserControl
    {
        private Redimension redimension;

        public uscVentasRealizadas()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            dgvRealizadas.Columns[IndexColumna("cliente")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
            "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
            " venta.cancelada FROM venta,clientes,productos_de_venta" +
            " WHERE (venta.cancelada=0 AND venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%') AND venta.pagada=1" +
            " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
            "GROUP BY venta.id_venta ");
            btnCorte.Visible = true;
        }

        private void Buscador(string consulta)
        {
            dgvRealizadas.Rows.Clear();
            List<string[]> lista;
            //consulta = "SELECT id_venta,id_cliente,fecha_de_venta FROM venta WHERE fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%';";
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvRealizadas.Rows.Add(lista[i][0], lista[i][1] + " " + lista[i][2] + " " + lista[i][3],
                    lista[i][4].Split(new char[] { ' ' })[0], lista[i][5]);

                DataGridViewCheckBoxCell checkbox = dgvRealizadas.Rows[dgvRealizadas.RowCount - 1].Cells["seleccionador"] as DataGridViewCheckBoxCell;
                if (bool.Parse(lista[i][6].ToString()))
                {
                    checkbox.Value = true;
                    checkbox.Style.BackColor = Color.Red;
                    checkbox.Style.SelectionBackColor = Color.Red;
                    checkbox.ReadOnly = true;
                }
                else
                    checkbox.Value = false;
            }
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT venta.id_venta, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.cancelada FROM venta,clientes,productos_de_venta" +
           " WHERE (venta.cancelada=0 AND (venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' OR venta.id_venta LIKE '%"+txtBusqueda.Text+"%')) AND venta.pagada=1" +
           " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
           "GROUP BY venta.id_venta ");
            btnCorte.Visible = false;
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private int IndexColumna(string columna)
        {
            return dgvRealizadas.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvRealizadas.Rows[fila].Cells[columna].Value.ToString();
        }

        private void uscVentasRealizadas_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaCancelar = new List<string[]>();

            DataGridViewCheckBoxCell checkbox;
            for (int i = 0; i < dgvRealizadas.RowCount; i++)
                if (bool.Parse(dgvRealizadas.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                {
                    checkbox = dgvRealizadas.Rows[i].Cells["seleccionador"] as DataGridViewCheckBoxCell;
                    if (checkbox.Style.BackColor != Color.Red)
                    {
                        listaCancelar.Add(new string[2] { dgvRealizadas.Rows[i].Cells["id_pedido"].Value.ToString(),
                            dgvRealizadas.Rows[i].Cells["cliente"].Value.ToString() });
                    }
                }
            if (listaCancelar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaCancelar)
                    mensajeConfirmacion += datos[0] + " " + datos[1] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea cancelar " + "estas ventas?\n" +
                    "Los productos actuales no surgiran cambios, solo se cancelara esta venta del registro.\nListado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    foreach (string[] datos in listaCancelar)
                    {
                        Sql.InsertarDatos("UPDATE venta SET cancelada = 1 WHERE id_venta = '" + datos[0] + "'");
                    }
                    for (int i = 0; i < dgvRealizadas.RowCount; i++)
                    {
                        if (bool.Parse(dgvRealizadas.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            checkbox = dgvRealizadas.Rows[i].Cells["seleccionador"] as DataGridViewCheckBoxCell;
                            checkbox.Value = true;
                            checkbox.Style.BackColor = Color.Red;
                            checkbox.Style.SelectionBackColor = Color.Red;
                            checkbox.ReadOnly = true;
                        }
                    }
                    MessageBox.Show("Estas ventas han sido canceladas con exito.", "Ventas canceladas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Seleccione al menos una venta.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private List<string[]> Corte(string consulta)
        {
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            return lista;
        }

        private List<string[]> Pendientes(string consulta)
        {
            List<string[]> pendientes;
            pendientes = Sql.BuscarDatos(consulta);
            return pendientes;
        }

        private List<string[]> Abonos(string consulta)
        {
            List<string[]> abonos;
            abonos = Sql.BuscarDatos(consulta);
            return abonos;
        }

        private void btnCorte_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> lista = Corte("SELECT venta.id_venta,clientes.id_cliente, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.cancelada FROM venta,clientes,productos_de_venta" +
           " WHERE (venta.cancelada=0 AND venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%') AND venta.pagada=1" +
           " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
           "GROUP BY venta.id_venta ");
                List<string[]> pendientes = Pendientes("SELECT venta.id_venta,clientes.id_cliente, clientes.nombres,clientes.apellido_paterno,clientes.apellido_materno, venta.fecha_de_venta," +
           "SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento)-(SUM(productos_de_venta.cantidad_comprada*productos_de_venta.precio_al_momento*(productos_de_venta.descuento/100)))," +
           " venta.cancelada FROM venta,clientes,productos_de_venta" +
           " WHERE (venta.cancelada=0 AND venta.fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%') AND venta.pagada=0" +
           " AND (clientes.id_cliente=venta.id_cliente) AND (productos_de_venta.id_venta=venta.id_venta) " +
           "GROUP BY venta.id_venta ");
                List<string[]> abonos = Abonos("SELECT abonos.id_venta, abonos.fecha_abono, abonos.cantidad_abonada FROM abonos " +
                    "WHERE abonos.fecha_abono LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%'");
                InfoReporte rep = GuardarInfoReporte.Leer();
                string[,] data = {{"Corte", "Fecha: ", "RFC: ", "Direccion: ","Telefono: "},
                            {" ", lista[0][2].ToString(), rep.Reporte.rfc, rep.Reporte.direccion ,rep.Reporte.telefono}};
                PDFFile pdf = new PDFFile("corte", "corte-");
                float total = 0;
                for (int i = 0; i < lista.Count; i++)
                    total += float.Parse(lista[i][6].ToString());
                pdf.CrearPDF();
                pdf.CrearCabecera(data);
                pdf.AgregarCorte(lista, pendientes, abonos);
                pdf.Cerrar();
                PDFFile.Ver(pdf.Ruta);
            }
            catch (Exception) { }
        }
    }
}
