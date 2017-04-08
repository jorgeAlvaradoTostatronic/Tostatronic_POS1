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
    public partial class uscReporte : UserControl
    {

        private Redimension redimension;
        private float impuesto = 1;
        public uscReporte()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            Buscador("SELECT id_venta, pagada, fecha_de_venta FROM venta WHERE " +
                " fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' ORDER BY id_venta DESC");
            cmbTipo.SelectedIndex = 1;
        }

        private void uscReporte_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (cmbTipo.Text == "Ventas")
                Buscador("SELECT id_venta, pagada, fecha_de_venta FROM venta WHERE id_venta LIKE '%" +
                    txtFolio.Text + "%' ORDER BY id_venta DESC");
            else
                BuscarCotizacion("SELECT id_cotizacion, fecha_cotizacion FROM cotizacion WHERE " +
            " id_cotizacion LIKE '%" + txtFolio.Text + "%' ORDER BY id_cotizacion DESC");
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (cmbTipo.Text == "Ventas")
                Buscador("SELECT  id_venta, pagada, fecha_de_venta FROM venta WHERE " +
                " fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' ORDER BY id_venta DESC");
            else
                BuscarCotizacion("SELECT id_cotizacion, fecha_cotizacion FROM cotizacion WHERE " +
                " fecha_cotizacion LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' ORDER BY id_cotizacion DESC");
        }

        private void txtFolio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cmbTipo.Text == "Ventas")
                    Buscador("SELECT id_venta, pagada, fecha_de_venta FROM venta WHERE id_venta LIKE '%" +
                        txtFolio.Text + "%' ORDER BY id_venta DESC");
                else
                    BuscarCotizacion("SELECT id_cotizacion, fecha_cotizacion FROM cotizacion WHERE " +
                " id_cotizacion LIKE '%" + txtFolio.Text + "%' ORDER BY id_cotizacion DESC");
            }
        }

        private void txtFolio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
                e.Handled = false;
        }

        private void Buscador(string consulta)
        {
            dgvDatos.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvDatos.Rows.Add(lista[i]);
                dgvDatos.Rows[i].Cells["clmVer"].Value = "Ver PDF";
                dgvDatos.Rows[i].Cells["clmImprimir"].Value = "Imprimir PDF";
                if (bool.Parse(dgvDatos.Rows[i].Cells["clmStatus"].Value.ToString()))
                    dgvDatos.Rows[i].Cells["clmStatus"].Value = "Pagado";
                else
                    dgvDatos.Rows[i].Cells["clmStatus"].Value = "Pendiente";
            }
        }

        private void BuscarCotizacion(string consulta)
        {
            dgvDatos.Rows.Clear();
            List<string[]> lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvDatos.Rows.Add(new string[] { lista[i][0], String.Empty, lista[i][1] });
                dgvDatos.Rows[i].Cells["clmVer"].Value = "Ver PDF";
                dgvDatos.Rows[i].Cells["clmImprimir"].Value = "Imprimir PDF";
            }
        }

        private int IndexColumna(string columna)
        {
            return dgvDatos.Columns[columna].Index;
        }

        private string ValorCelda(int fila, string columna)
        {
            return dgvDatos.Rows[fila].Cells[columna].Value.ToString();
        }
        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (IndexColumna("clmVer") == e.ColumnIndex)
                {
                    if (cmbTipo.Text == "Ventas")
                    {
                        PDFInvoice.CreatePDF(TipoInvoice.Venta, ValorCelda(e.RowIndex, "clmFolio"), ValorCelda(e.RowIndex, "clmFecha"), obtenerProductos(ValorCelda(e.RowIndex, "clmFolio")),c,impuesto );
                        PDFFile.Ver(Application.StartupPath + "\\Invoice.pdf");
                    }
                    else
                    {
                        PDFInvoice.CreatePDF(TipoInvoice.Cotizacions, ValorCelda(e.RowIndex, "clmFolio"), ValorCelda(e.RowIndex, "clmFecha"), obtenerProductosCo(ValorCelda(e.RowIndex, "clmFolio")),c,impuesto);
                        PDFFile.Ver(Application.StartupPath + "\\Invoice.pdf");
                    }
                }
                else if (IndexColumna("clmImprimir") == e.ColumnIndex)
                {
                    if (cmbTipo.Text == "Ventas")
                    {
                        PDFInvoice.CreatePDF(TipoInvoice.Venta, ValorCelda(e.RowIndex, "clmFolio"), ValorCelda(e.RowIndex, "clmFecha"), obtenerProductos(ValorCelda(e.RowIndex, "clmFolio")),c,impuesto);
                        PDFFile.Imprimir(this, Application.StartupPath + "\\Invoice.pdf");
                    }
                    else
                    {
                        PDFInvoice.CreatePDF(TipoInvoice.Cotizacions, ValorCelda(e.RowIndex, "clmFolio"), ValorCelda(e.RowIndex, "clmFecha"), obtenerProductosCo(ValorCelda(e.RowIndex, "clmFolio")),c,impuesto);
                        PDFFile.Imprimir(this, Application.StartupPath + "\\Invoice.pdf");
                    }
                }

            }
            catch (Exception) { }

        }
        Clientes c;
        private List<ProductoCompleto> obtenerProductosCo(string folio)
        {
            List<string[]> productos;
            List<string[]> venta;
            string consulta = "SELECT productos_de_cotizacion.id_producto, productos.nombre, productos_de_cotizacion.cantidad_cotizada, productos_de_cotizacion.precio_al_momento, productos_de_cotizacion.descuento " +
                "FROM productos_de_cotizacion, productos " +
                "WHERE productos_de_cotizacion.id_cotizacion=" + folio + " AND productos.codigo=productos_de_cotizacion.id_producto;";
            productos = Sql.BuscarDatos(consulta);
            consulta = "SELECT clientes.rfc, clientes.nombres,clientes.apellido_paterno, clientes.apellido_materno, clientes.telefono, clientes.domicilio, clientes.correo_electronico " +
                "FROM cotizacion, clientes " +
                "WHERE (cotizacion.id_cotizacion=" + folio + ") " +
                "AND clientes.id_cliente=cotizacion.id_cliente " +
                "GROUP BY cotizacion.id_cotizacion;";
            venta = Sql.BuscarDatos(consulta);
            c.rfc = venta[0][0];
            c.nombre = venta[0][1];
            c.paterno = venta[0][2];
            c.materno = venta[0][3];
            c.telefono = venta[0][4];
            c.domicilio = venta[0][5];
            c.correo = venta[0][6];
            consulta = "SELECT impuesto FROM cotizacion WHERE id_cotizacion="+folio+";";
            impuesto = float.Parse(Sql.BuscarDatos(consulta)[0][0]);
            float sub;
            List<ProductoCompleto> prod = new List<ProductoCompleto>();
            for (int i = 0; i < productos.Count; i++)
            {
                sub = float.Parse(productos[i][3]);
                sub *= float.Parse(productos[i][2]);
                sub -= sub * (float.Parse(productos[i][3]) * float.Parse(productos[0][4])) / 100;
                prod.Add(new ProductoCompleto(productos[i][0], productos[i][1], float.Parse(productos[i][2]), float.Parse(productos[i][4]), sub));
            }
            return prod;
        }
        private List<ProductoCompleto> obtenerProductos(string folio)
        {
            List<string[]> productos;
            List<string[]> venta;
            string consulta = "SELECT productos_de_venta.id_producto, productos.nombre, productos_de_venta.cantidad_comprada, productos_de_venta.precio_al_momento, productos_de_venta.descuento " +
                "FROM productos_de_venta, productos " +
                "WHERE productos_de_venta.id_venta=" + folio + " AND productos.codigo=productos_de_venta.id_producto;";
            productos = Sql.BuscarDatos(consulta);
            consulta = "SELECT clientes.rfc, clientes.nombres,clientes.apellido_paterno, clientes.apellido_materno, clientes.telefono, clientes.domicilio, clientes.correo_electronico  " +
                "FROM venta, clientes " +
                "WHERE (venta.id_venta=" + folio + " AND venta.cancelada=0) " +
                "AND clientes.id_cliente=venta.id_cliente " +
                "GROUP BY venta.id_venta;";
            venta = Sql.BuscarDatos(consulta);
            c.rfc = venta[0][0];
            c.nombre = venta[0][1];
            c.paterno = venta[0][2];
            c.materno = venta[0][3];
            c.telefono = venta[0][4];
            c.domicilio = venta[0][5];
            c.correo = venta[0][6];
            consulta = "SELECT impuesto FROM venta WHERE id_venta=" + folio + ";";
            impuesto = float.Parse(Sql.BuscarDatos(consulta)[0][0]);
            float sub;
            List<ProductoCompleto> prod = new List<ProductoCompleto>();
            for (int i = 0; i < productos.Count; i++)
            {
                sub = float.Parse(productos[i][3]);
                sub *= float.Parse(productos[i][2]);
                sub -= sub * (float.Parse(productos[i][3]) * float.Parse(productos[0][4])) / 100;
                prod.Add(new ProductoCompleto(productos[i][0], productos[i][1], float.Parse(productos[i][2]), float.Parse(productos[i][4]),sub));
            }
            return prod;
        }
        private string ReporteCotizacion(string id_cotizacion)
        {
            try
            {
                List<ProductoCompleto> listaProductos = new List<ProductoCompleto>();
                string[] cotizacion, usuario, cliente;

                try
                {
                    cotizacion = Sql.BuscarDatos("SELECT * FROM cotizacion WHERE id_cotizacion = '" + id_cotizacion + "'")[0];
                }
                catch (Exception)
                {
                    cotizacion = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
                }

                try
                {
                    usuario = Sql.BuscarDatos("SELECT usuario, nombre, apellido_paterno, apellido_materno" +
                        " FROM usuarios WHERE id_usuario = '" + cotizacion[1] + "'")[0];
                }
                catch (Exception)
                {
                    usuario = new string[] { " ", " ", " ", " " };
                }

                try
                {
                    cliente = Sql.BuscarDatos("SELECT nombres, apellido_paterno, apellido_materno, rfc, telefono, domicilio, correo_electronico " +
                        " FROM clientes WHERE id_cliente = '" + cotizacion[2] + "'")[0];
                }
                catch (Exception)
                {
                    cliente = new string[] { " ", " ", " ", " ", " ", " ", " " };
                }

                foreach (string datos in cotizacion[3].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] temp = datos.Split(new char[] { ':' });
                    string[] producto = Sql.BuscarDatos("SELECT codigo, nombre FROM productos WHERE codigo = '" + temp[0] + "'")[0];
                    listaProductos.Add(new ProductoCompleto(producto[0], producto[1], float.Parse(temp[1]),int.Parse(temp[3]), float.Parse(temp[2])));
                }

                Cotizacion cotizar = new Cotizacion(int.Parse(cotizacion[0]), new Usuarios(usuario[0], usuario[1] + " " + usuario[2] + " " + usuario[3]),
                    new Clientes(cliente[0], cliente[1], cliente[2], cliente[3], cliente[4], cliente[5], cliente[6]),
                    cotizacion[5], listaProductos, float.Parse(cotizacion[4]));

                InfoReporte rep = GuardarInfoReporte.Leer();
                string[,] data = {{"Precotización", "Fecha: ", "RFC: ", "Dirección: ","Telefono: ", "Atendido por: "},
                            { cotizar.Id_cotizacion.ToString(), cotizar.Fecha.ToString(), rep.Reporte.rfc, 
                                rep.Reporte.direccion ,rep.Reporte.telefono, cotizar.Usuario.NombreCompleto }};

                PDFFile pdf = new PDFFile("reportes", "reporte-");

                pdf.CrearPDF();
                pdf.CrearCabecera(data);
                pdf.AgregarInfoCliente(cotizar.Cliente);
                pdf.AgregarProductos(cotizar);
                pdf.AgregarAnotaciones(" ");
                pdf.Cerrar();
                return pdf.Ruta;
            }
            catch (Exception) { }
            return null;
        }

        private string Reporte(string folio)
        {
            try
            {
                List<ProductoCompleto> listaProductos = new List<ProductoCompleto>();
                string[] pedido, usuario, cliente;

                try
                {
                    pedido = Sql.BuscarDatos("SELECT * FROM venta WHERE id_venta = '" + folio + "'")[0];
                }
                catch (Exception)
                {
                    pedido = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
                }

                try
                {
                    usuario = Sql.BuscarDatos("SELECT usuario, nombre, apellido_paterno, apellido_materno" +
                    " FROM usuarios WHERE id_usuario = '" + pedido[1] + "'")[0];
                }
                catch (Exception)
                {
                    usuario = new string[] { " ", " ", " ", " " };
                }

                try
                {
                    cliente = Sql.BuscarDatos("SELECT nombres, apellido_paterno, apellido_materno, rfc, telefono, domicilio, correo_electronico " +
                    " FROM clientes WHERE id_cliente = '" + pedido[2] + "'")[0];
                }
                catch (Exception)
                {
                    cliente = new string[] { " ", " ", " ", " ", " ", " ", " " };
                }

                foreach (string datos in pedido[5].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] temp = datos.Split(new char[] { ':' });
                    if(Sql.Existe("SELECT codigo, nombre FROM productos WHERE codigo = '" + temp[0] + "' "))
                    {
                        string[] producto = Sql.BuscarDatos("SELECT codigo, nombre FROM productos WHERE codigo = '" + temp[0] + "' ")[0];
                        listaProductos.Add(new ProductoCompleto(producto[0], producto[1], float.Parse(temp[1]), int.Parse(temp[3]), float.Parse(temp[2])));
                    }
                    else
                    {
                        listaProductos.Add(new ProductoCompleto("Codigo elim.", "Productos elim.", float.Parse(temp[1]), int.Parse(temp[3]), float.Parse(temp[2])));
                    }
                   
                }

                Venta venta = new Venta(int.Parse(pedido[0]), new Usuarios(usuario[0], usuario[1] + " " + usuario[2] + " " + usuario[3]),
                    new Clientes(cliente[0], cliente[1], cliente[2], cliente[3], cliente[4], cliente[5], cliente[6]),
                    bool.Parse(pedido[3]), pedido[4], listaProductos, float.Parse(pedido[6]),
                    float.Parse(pedido[7]), float.Parse(pedido[8]));

                InfoReporte rep = GuardarInfoReporte.Leer();
                string[,] data = {{"Cotización Tienda", "Fecha: ", "RFC: ", "Dirección: ","Telefono: ", "Atendido por: "},
                            {folio, venta.Fecha, rep.Reporte.rfc, rep.Reporte.direccion ,rep.Reporte.telefono, venta.Usuario.NombreCompleto}};

                PDFFile pdf = new PDFFile("reportes", "reporte-");
                pdf.CrearPDF();

                for (int i = 0; i < 2; i++)
                {
                    if (i == 1)
                    {
                        data[0, 0] = "Cotización cliente";
                        pdf.NuevoRenglon();
                        pdf.CrearCabecera(data);
                    }
                    else
                    {
                        pdf.NuevoRenglon();
                        pdf.CrearCabecera(data);
                    }

                    pdf.AgregarInfoCliente(venta.Cliente);
                    pdf.AgregarProductos(venta, false);
                    pdf.AgregarAnotaciones(" ");
                    pdf.NuevaPagina();
                }
                data[0, 0] = "Almacen";
                pdf.CrearCabecera(data);
                pdf.AgregarInfoCliente(venta.Cliente);
                pdf.AgregarProductos(venta, true);
                pdf.AgregarAnotaciones(" ");
                pdf.Cerrar();

                return pdf.Ruta;
            }
            catch (Exception) { }
            return null;
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipo.Text == "Ventas")
            {
                txtFolio.Text = String.Empty;
                dgvDatos.Columns[IndexColumna("clmStatus")].Visible = true;
                dgvDatos.Columns[IndexColumna("clmFolio")].HeaderText = "Folio";
                Buscador("SELECT id_venta, pagada, fecha_de_venta FROM venta WHERE " +
                " fecha_de_venta LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' ORDER BY id_venta DESC");
            }
            else
            {
                txtFolio.Text = String.Empty;
                dgvDatos.Columns[IndexColumna("clmStatus")].Visible = false;
                dgvDatos.Columns[IndexColumna("clmFolio")].HeaderText = "Num Seguimiento";
                BuscarCotizacion("SELECT id_cotizacion, fecha_cotizacion FROM cotizacion WHERE " +
                " fecha_cotizacion LIKE '%" + dtpFecha.Value.ToString("yyyy-MM-dd") + "%' ORDER BY id_cotizacion DESC");
            }
        }
    }
}
