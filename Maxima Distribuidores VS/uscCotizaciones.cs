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
    public partial class uscCotizaciones : UserControl
    {
        private Redimension redimension;
        private uscAgregarVenta agregarVenta;

        public uscCotizaciones(uscAgregarVenta agregarVenta)
        {
            InitializeComponent();
            redimension = new Redimension(this);
            this.agregarVenta = agregarVenta;
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            //Buscador("SELECT id_cotizacion, id_cliente, fecha_cotizacion FROM cotizacion " +
                   // "WHERE id_cotizacion LIKE '%" + txtBusqueda.Text + "%'");
            Buscador("SELECT cotizacion.id_cotizacion, clientes.nombres,clientes.apellido_paterno," +
                   "clientes.apellido_materno, cotizacion.fecha_cotizacion, SUM(productos_de_cotizacion.cantidad_cotizada*productos_de_cotizacion.precio_al_momento)-(SUM(productos_de_cotizacion.cantidad_cotizada*productos_de_cotizacion.precio_al_momento*(productos_de_cotizacion.descuento/100)))" +
                   " FROM cotizacion,clientes,productos_de_cotizacion" +
                   " WHERE (cotizacion.id_cotizacion LIKE '%" + txtBusqueda.Text + "%')" +
                   " AND (clientes.id_cliente=cotizacion.id_cliente)" +
                   " AND (productos_de_cotizacion.id_cotizacion=cotizacion.id_cotizacion)" +
                   " GROUP BY cotizacion.id_cotizacion");
        }

        private void uscCotizaciones_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void Buscador(string consulta)
        {
            dgvCotizaciones.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvCotizaciones.Rows.Add(lista[i][0], lista[i][1] + " " + lista[i][2] + " " + lista[i][3],
                    lista[i][4].Split(new char[] { ' ' })[0]);
                dgvCotizaciones.Rows[i].Cells["generar"].Value = "Generar Venta";
            }
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            Buscador("SELECT cotizacion.id_cotizacion, clientes.nombres,clientes.apellido_paterno,"+
                   "clientes.apellido_materno, cotizacion.fecha_cotizacion, SUM(productos_de_cotizacion.cantidad_cotizada*productos_de_cotizacion.precio_al_momento)-(SUM(productos_de_cotizacion.cantidad_cotizada*productos_de_cotizacion.precio_al_momento*(productos_de_cotizacion.descuento/100)))"+ 
                   " FROM cotizacion,clientes,productos_de_cotizacion"+
                   " WHERE (cotizacion.fecha_cotizacion LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%')" +
                   " AND (clientes.id_cliente=cotizacion.id_cliente)"+
                   " AND (productos_de_cotizacion.id_cotizacion=cotizacion.id_cotizacion)"+
                   " GROUP BY cotizacion.id_cotizacion");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            for (int i = 0; i < dgvCotizaciones.RowCount; i++)
                if (bool.Parse(dgvCotizaciones.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                    listaEliminar.Add(new string[2] { dgvCotizaciones.Rows[i].Cells["id_cotizacion"].Value.ToString(),
                        dgvCotizaciones.Rows[i].Cells["cliente"].Value.ToString() });
            if (listaEliminar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaEliminar)
                    mensajeConfirmacion += datos[0] + " " + datos[1] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " + "estas cotizaciones?\n" +
                    "Listado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    foreach (string[] datos in listaEliminar)
                        Sql.InsertarDatos("DELETE FROM cotizacion WHERE id_cotizacion = '" + datos[0] + "'");
                    for (int i = 0; i < dgvCotizaciones.RowCount; i++)
                        if (bool.Parse(dgvCotizaciones.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            dgvCotizaciones.Rows.RemoveAt(i);
                            i--;
                        }
                    MessageBox.Show("Estas cotizaciones han sido borradas con exito.", "Ventas eliminadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Seleccione al menos una cotizacion.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void dgvCotizaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.ColumnIndex != IndexColumna("seleccionador"))
            {
                DialogResult confirmacion = MessageBox.Show(this, "Si convierte esta cotización en pedido se eliminara la venta actual\n¿Desea continuar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    agregarVenta.Cancelar();
                    if (IndexColumna("generar") == e.ColumnIndex)
                    {
                        try
                        {
                            List<ProductoCompleto> listaProductos = new List<ProductoCompleto>();
                            string[] cotizacion, usuario;
                            List<string[]> productos;
                            try
                            {
                                cotizacion = Sql.BuscarDatos("SELECT cotizacion.id_cotizacion,clientes.id_tipo_cliente , clientes.nombres,clientes.apellido_paterno," +
                               "clientes.apellido_materno, clientes.rfc, clientes.telefono, clientes.domicilio, clientes.correo_electronico, cotizacion.fecha_cotizacion" +
                               " FROM cotizacion,clientes,productos_de_cotizacion" +
                               " WHERE (cotizacion.id_cotizacion = '" + ValorCelda(e.RowIndex, "id_cotizacion") + "')" +
                               " AND (clientes.id_cliente=cotizacion.id_cliente)" +
                               " AND (productos_de_cotizacion.id_cotizacion=cotizacion.id_cotizacion)" +
                               " GROUP BY cotizacion.id_cotizacion")[0];
                            }
                            catch (Exception)
                            {
                                cotizacion = new string[] { " ", " ", " ", " ", " ", " " };
                            }

                            usuario = new string[] { " ", " ", " ", " " };
                            productos = Sql.BuscarDatos("SELECT productos_de_cotizacion.id_producto, productos_de_cotizacion.cantidad_cotizada, "+
                                        "productos_de_cotizacion.precio_al_momento, productos_de_cotizacion.descuento, "+
                                        "productos.nombre  "+
                                        "FROM productos_de_cotizacion, productos "+
                                        "WHERE (productos_de_cotizacion.id_cotizacion="+cotizacion[0]+") "+
                                        "AND (productos.codigo=productos_de_cotizacion.id_producto) ");

                            foreach (string[] datos in productos)
                            {
                                listaProductos.Add(new ProductoCompleto(datos[0], datos[4], float.Parse(datos[1]), float.Parse(datos[3]), float.Parse(datos[1]) * float.Parse(datos[2])));
                            }

                            Venta venta = new Venta(int.Parse(cotizacion[0]), new Usuarios(usuario[0], usuario[1] + " " + usuario[2] + " " + usuario[3]),
                                    new Clientes(int.Parse(cotizacion[1]), cotizacion[2], cotizacion[3], cotizacion[4], cotizacion[5], cotizacion[6], cotizacion[7], cotizacion[8]),
                                    false, cotizacion[9], listaProductos, 0, 0, 0);

                            agregarVenta.GenerarCotizacion(venta);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private int IndexColumna(string columna)
        {
            return dgvCotizaciones.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvCotizaciones.Rows[fila].Cells[columna].Value.ToString();
        }
    }
}
