using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;

namespace Maxima_Distribuidores_VS
{
    public partial class uscAgregarCotizacion : UserControl
    {
        private Redimension redimension;
        private int id_cliente;
        private int tipoCliente;
        private string buscador;
        private float total;

        public uscAgregarCotizacion()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            dgvCotizacion.Columns[IndexColumna(dgvCotizacion, "codigo")].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvCotizacion.Columns[IndexColumna(dgvCotizacion, "subtotal")].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvCotizacion.Columns[IndexColumna(dgvCotizacion, "codigo")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCotizacion.Columns[IndexColumna(dgvCotizacion, "descripcion")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            Recuperar();
            total = 0;
        }

        private void uscAgregarCotizacion_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE codigo = '" +
                    ValorCelda(dgvProductos, e.RowIndex, "clmCodigo") + "'");

                dialogCantidad frmCantidad = new dialogCantidad(lista[0][0] + " (" + lista[0][1] + ")", 1000, float.Parse(lista[0][6 - tipoCliente]), Descuento(lista[0][0]));
                frmCantidad.ShowDialog();
                if (frmCantidad.Cantidad > 0)
                {
                    float subtotal;
                    subtotal = (float)frmCantidad.Cantidad * float.Parse(lista[0][6 - tipoCliente]);
                    subtotal -= subtotal * Descuento(lista[0][0]) / 100;

                    for (int i = 0; i < dgvCotizacion.RowCount; i++)
                        if (dgvCotizacion.Rows[i].Cells["codigo"].Value.ToString() == lista[0][0])
                        {
                            dgvCotizacion.Rows[i].Cells["cantidad"].Value = decimal.Parse(dgvCotizacion.Rows[i].Cells["cantidad"].Value.ToString()) + frmCantidad.Cantidad;
                            subtotal = float.Parse(dgvCotizacion.Rows[i].Cells["cantidad"].Value.ToString()) * float.Parse(dgvCotizacion.Rows[i].Cells["precio"].Value.ToString());
                            dgvCotizacion.Rows[i].Cells["subtotal"].Value = subtotal - subtotal * Descuento(lista[0][0]) / 100;
                            frmCantidad.Dispose();
                            GuardarCotizacion.Guardar(Guardar());
                            Total();
                            return;
                        }
                    dgvCotizacion.Rows.Add(new string[] { lista[0][0], lista[0][1], frmCantidad.Cantidad.ToString(), 
                        lista[0][6 - tipoCliente], subtotal.ToString(), Descuento(lista[0][0]).ToString() });
                    Total();
                    GuardarCotizacion.Guardar(Guardar());
                }
                frmCantidad.Dispose();
            }
            catch (Exception) { }
        }

        private void Total()
        {
            total = 0;
            for (int i = 0; i < dgvCotizacion.RowCount; i++)
                total += float.Parse(ValorCelda(dgvCotizacion, i, "subtotal"));
            txtSubTotal.Text = total.ToString("$0.00");
            yxyIva.Text = (total * 0.16).ToString("$0.00");
            total *= 1.16f;
            txtTotal.Text = total.ToString("$0.00");
        }

        private int IndexColumna(DataGridView dgv, string columna)
        {
            return dgv.Columns[columna].Index;
        }

        private string ValorCelda(DataGridView dgv, int fila, string columna)
        {
            return dgv.Rows[fila].Cells[columna].Value.ToString();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            buscador = txtBusqueda.Text;
            Buscador("SELECT  codigo, nombre, existencia FROM productos " +
                    "WHERE (codigo LIKE '%" + buscador + "%' OR nombre LIKE '%" + buscador + "%') AND eliminado  = 0");
        }

        private void Buscador(string consulta)
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
            {
                dgvProductos.Rows.Add(lista[i]);
                dgvProductos.Rows[dgvProductos.Rows.Count - 1].Cells["desc"].Value = Descuento(lista[i][0]).ToString();
            }
        }

        private float Descuento(string id_producto)
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

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            if (!btnCotizacion.Enabled || dgvCotizacion.Rows.Count <= 0)
            {
                dialogBusquedaClientes buscarCliente = new dialogBusquedaClientes();
                try
                {
                    dgvProductos.Rows.Clear();
                    buscarCliente.ShowDialog();
                    id_cliente = int.Parse(buscarCliente.Cliente[0]);
                    txtRfc.Text = buscarCliente.Cliente[1];
                    txtNombre.Text = buscarCliente.Cliente[2];
                    txtApellidoPaterno.Text = buscarCliente.Cliente[3];
                    tipoCliente = int.Parse(buscarCliente.Cliente[4]);
                    BorrarXML();
                    ActivarCotizacion();
                }
                catch (Exception)
                {
                    DesactivarCotizacion();
                }
                buscarCliente.Dispose();
            }
            else
            {
                DialogResult confirmacion = MessageBox.Show(this, "Si cambia de cliente perdera los datos de la cotización actual\n" +
                    "¿Desea continuar?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    Cancelar();
                    Total();
                    btnCotizacion.Enabled = false;
                    btnBuscarCliente_Click(sender, new EventArgs());
                }
            }

        }

        private void ActivarCotizacion()
        {
            btnBusqueda.Enabled = true;
            dgvProductos.Enabled = true;
            dgvCotizacion.Enabled = true;
            txtBusqueda.Enabled = true;
            btnEliminar.Enabled = true;
            btnCotizacion.Enabled = true;
            if (txtRfc.Text == "xxxxxxxxxxxxx")
            {
                txtNombre.Enabled = true;
                txtRfc.Enabled = true;
                txtApellidoPaterno.Enabled = true;
            }
            else
            {
                txtNombre.Enabled = false;
                txtRfc.Enabled = false;
                txtApellidoPaterno.Enabled = false;
            }
        }

        private void DesactivarCotizacion()
        {
            btnBusqueda.Enabled = false;
            dgvProductos.Enabled = false;
            dgvCotizacion.Enabled = false;
            txtBusqueda.Enabled = false;
            btnEliminar.Enabled = false;
            btnCotizacion.Enabled = false;
            txtNombre.Enabled = false;
            txtRfc.Enabled = false;
            txtApellidoPaterno.Enabled = false;
            Limpiar();
        }

        private void Limpiar()
        {
            dgvCotizacion.Rows.Clear();
            dgvProductos.Rows.Clear();
            foreach (Control control in Controls)
                if (control is TextBox)
                    control.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            for (int i = 0; i < dgvCotizacion.RowCount; i++)
                if (bool.Parse(dgvCotizacion.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                    listaEliminar.Add(new string[3] { dgvCotizacion.Rows[i].Cells["codigo"].Value.ToString(),
                        dgvCotizacion.Rows[i].Cells["descripcion"].Value.ToString(), dgvCotizacion.Rows[i].Cells["cantidad"].Value.ToString() });
            if (listaEliminar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaEliminar)
                    mensajeConfirmacion += datos[1] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " + "estos productos de la venta? \nListado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    for (int i = 0; i < dgvCotizacion.RowCount; i++)
                        if (bool.Parse(dgvCotizacion.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            dgvCotizacion.Rows.RemoveAt(i);
                            i--;
                        }
                    MessageBox.Show("Eliminados");
                    Buscador("SELECT  codigo, nombre, existencia FROM productos " +
                        "WHERE codigo LIKE '%" + buscador + "%' OR nombre LIKE '%" + buscador + "%'");
                    GuardarCotizacion.Guardar(Guardar());
                    Total();
                }
            }
            else
                MessageBox.Show("Seleccione al menos un producto a eliminar.");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            if (dgvCotizacion.RowCount > 0)
            {
                DialogResult confirmacion = MessageBox.Show(this, "¿Desea cancelar toda la cotizacion?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    BorrarXML();
                    Cancelar();
                    Limpiar();
                    DesactivarCotizacion();
                    Total();
                }
            }
            else
                MessageBox.Show("No hay nada en la venta.");
        }

        private void Cancelar()
        {
            dgvCotizacion.Rows.Clear();
            dgvProductos.Rows.Clear();
        }

        private void btnCotizacion_Click(object sender, EventArgs e)
        {
            if (dgvCotizacion.Rows.Count > 0)
            {
                string id_cliente = Sql.BuscarDatos("SELECT id_cliente FROM clientes WHERE rfc = '" + txtRfc.Text + "'")[0][0];
                string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //este es un contador de productos para la divicion de los mismos
                int contadorProductos = 0;
                float subTotal = 0;
                ProductoCompleto producto;
                List<ProductoCompleto> productos = new List<ProductoCompleto>();
                for (int i = 0; i < dgvCotizacion.RowCount; i++)
                {
                    producto = new ProductoCompleto(dgvCotizacion.Rows[i].Cells["codigo"].Value.ToString(), dgvCotizacion.Rows[i].Cells["descripcion"].Value.ToString(),
                            float.Parse(dgvCotizacion.Rows[i].Cells["cantidad"].Value.ToString()), Descuento(dgvCotizacion.Rows[i].Cells["codigo"].Value.ToString()),
                            float.Parse(dgvCotizacion.Rows[i].Cells["subtotal"].Value.ToString()));
                    productos.Add(producto);
                    subTotal += float.Parse(dgvCotizacion.Rows[i].Cells["subtotal"].Value.ToString());
                }
                if(productos.Count>0)
                {
                    subTotal *= 1.16f;
                    Sql.InsertarCotizacion(productos, Usuario.Instancia().Id.ToString(), id_cliente);
                }
                BorrarXML();
                Cancelar();
                DesactivarCotizacion();
                Limpiar();
            }
        }

        private string ReporteCotizacion(string fecha)
        {
            try
            {
                List<ProductoCompleto> listaProductos = new List<ProductoCompleto>();
                string[] cotizacion, usuario, cliente;

                try
                {
                    cotizacion = Sql.BuscarDatos("SELECT * FROM cotizaciones WHERE fecha = '" + fecha + "'")[0];
                }
                catch (Exception)
                {
                    cotizacion = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
                }

                try
                {
                    usuario = Sql.BuscarDatos("SELECT usuario, nombre, paterno, materno" +
                        " FROM usuarios WHERE id_usuario = '" + cotizacion[1] + "'")[0];
                }
                catch (Exception)
                {
                    usuario = new string[] { " ", " ", " ", " " };
                }

                try
                {
                    cliente = Sql.BuscarDatos("SELECT nombre, paterno, materno, rfc, telefono, domicilio, correo " +
                        " FROM clientes WHERE id_cliente = '" + cotizacion[2] + "'")[0];
                }
                catch (Exception)
                {
                    cliente = new string[] { " ", " ", " ", " ", " ", " ", " " };
                }

                foreach (string datos in cotizacion[3].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] temp = datos.Split(new char[] { ':' });
                    string[] producto = Sql.BuscarDatos("SELECT productos.codigo, productos.descripcion, descuentos.descuento FROM productos,descuentos WHERE productos.id_producto = '" + temp[0] + "' AND descuentos.id_producto = '" + temp[0] + "' AND descuentos.id_cliente= '" + cotizacion[2] + "'")[0];
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

        private void dgvCotizacion_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string codigo = ValorCelda(e.RowIndex, "codigo");
                float minPrice = float.Parse(Sql.BuscarDatos("SELECT precio_minimo FROM productos WHERE codigo = '" + codigo + "';")[0][0]);
                if (float.Parse(ValorCelda(e.RowIndex, "precio")) < minPrice)
                {
                    dgvCotizacion.Rows[e.RowIndex].Cells[3].ErrorText = "El precio minimo es de: " + minPrice.ToString();
                    dgvCotizacion.Rows[e.RowIndex].Cells[3].Value = ValorAnterior(codigo).ToString();
                }
                else
                {
                    float sub = float.Parse(dgvCotizacion.Rows[e.RowIndex].Cells[3].Value.ToString());
                    sub *= float.Parse(dgvCotizacion.Rows[e.RowIndex].Cells[2].Value.ToString());
                    sub -= sub * Descuento(codigo) / 100;
                    dgvCotizacion.Rows[e.RowIndex].Cells[4].Value = sub;
                    Total();
                    GuardarCotizacion.Guardar(Guardar());
                    dgvCotizacion.Rows[e.RowIndex].Cells[3].ErrorText = "";
                }
            }
        }
        public InformacionVenta Guardar()
        {
            List<Producto> productos = new List<Producto>();
            for (int i = 0; i < dgvCotizacion.RowCount; i++)
                productos.Add(new Producto(dgvCotizacion.Rows[i].Cells["codigo"].Value.ToString(),
                    float.Parse(dgvCotizacion.Rows[i].Cells["cantidad"].Value.ToString()),
                    float.Parse(dgvCotizacion.Rows[i].Cells["des"].Value.ToString())));
            return new InformacionVenta(id_cliente, productos);
        }
        private string ValorCelda(int fila, string columna)
        {
            return dgvCotizacion.Rows[fila].Cells[columna].Value.ToString();
        }
        private float ValorAnterior(string codigo)
        {
            if (tipoCliente == 1)
                return float.Parse(Sql.BuscarDatos("SELECT precio_distribuidor FROM productos WHERE codigo ='" + codigo + "'")[0][0]);
            return float.Parse(Sql.BuscarDatos("SELECT precio_publico FROM productos WHERE codigo ='" + codigo + "'")[0][0]);
        }
        public void Recuperar()
        {
            if (File.Exists(Application.StartupPath + "\\Tostatronic_Cotizacion.xml"))
            {
                try
                {
                    InformacionVenta informacionVenta = GuardarCotizacion.Leer();

                    List<string[]> listaCliente = Sql.BuscarDatos("SELECT id_cliente, rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes WHERE id_cliente = '" +
                        informacionVenta.Id_cliente + "'");

                    id_cliente = int.Parse(listaCliente[0][0]);
                    txtRfc.Text = listaCliente[0][1];
                    txtNombre.Text = listaCliente[0][2];
                    txtApellidoPaterno.Text = listaCliente[0][3];
                    tipoCliente = int.Parse(listaCliente[0][4]);

                    foreach (Producto producto in informacionVenta.Productos)
                    {
                        List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE codigo = '" +
                            producto.Id_producto + "'");
                        float descuentoTemp = Descuento(lista[0][0]);
                        float subTotalTemp = producto.Cantidad * float.Parse(lista[0][6 - tipoCliente]);
                        dgvCotizacion.Rows.Add(new string[] { lista[0][0], lista[0][1], 
                            producto.Cantidad.ToString(), lista[0][6 - tipoCliente], 
                            (subTotalTemp - subTotalTemp*descuentoTemp/100).ToString(),  lista[0][2], descuentoTemp.ToString() });
                    }
                    Total();
                }
                catch (Exception) { }
            }
        }
        private void BorrarXML()
        {
            if (File.Exists(Application.StartupPath + "\\Tostatronic_Cotizacion.xml"))
                File.Delete(Application.StartupPath + "\\Tostatronic_Cotizacion.xml");
        }
    }
}
