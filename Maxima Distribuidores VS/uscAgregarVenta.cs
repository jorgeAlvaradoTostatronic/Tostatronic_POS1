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
    public partial class uscAgregarVenta : UserControl
    {
        private Redimension redimension;
        private int id_cliente;
        private int tipoCliente;
        private string buscador;
        private float total;

        public bool HayProductos
        {
            get
            {
                if (dgvVentas.RowCount > 0)
                    return true;
                return false;
            }
        }

        public uscAgregarVenta()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            dgvVentas.Columns[IndexColumna(dgvVentas, "codigo")].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvVentas.Columns[IndexColumna(dgvVentas, "subtotal")].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvVentas.Columns[IndexColumna(dgvVentas, "codigo")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvVentas.Columns[IndexColumna(dgvVentas, "descripcion")].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            total = 0;
        }

        private void uscAgregarVenta_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE codigo = '" +
                    ValorCelda(dgvProductos, e.RowIndex, "clmCodigo") + "'");

                dialogCantidad frmCantidad = new dialogCantidad(lista[0][0] + " (" + lista[0][1] + ")", Decimal.Parse(lista[0][2]), float.Parse(lista[0][6 - tipoCliente]), Descuento(lista[0][0]));
                frmCantidad.ShowDialog();
                if (frmCantidad.Cantidad > 0)
                {
                    decimal cantidad = Decimal.Parse(lista[0][2]) - frmCantidad.Cantidad;
                    float subtotal;
                    subtotal = (float)frmCantidad.Cantidad * float.Parse(lista[0][6 - tipoCliente]);
                    subtotal -= subtotal * Descuento(lista[0][0]) / 100;
                    Sql.InsertarDatos("UPDATE productos SET existencia ='" + cantidad +
                        "' WHERE codigo = '" + lista[0][0] + "'");

                    dgvProductos.Rows[e.RowIndex].Cells["clmCantidad"].Value = decimal.Parse(dgvProductos.Rows[e.RowIndex].Cells["clmCantidad"].Value.ToString()) - frmCantidad.Cantidad;

                    for (int i = 0; i < dgvVentas.RowCount; i++)
                        if (dgvVentas.Rows[i].Cells["codigo"].Value.ToString() == lista[0][0])
                        {
                            dgvVentas.Rows[i].Cells["cantidad"].Value = decimal.Parse(dgvVentas.Rows[i].Cells["cantidad"].Value.ToString()) + frmCantidad.Cantidad;
                            subtotal = float.Parse(dgvVentas.Rows[i].Cells["cantidad"].Value.ToString()) * float.Parse(dgvVentas.Rows[i].Cells["precio"].Value.ToString());
                            dgvVentas.Rows[i].Cells["existencia"].Value = decimal.Parse(dgvVentas.Rows[i].Cells["existencia"].Value.ToString()) - frmCantidad.Cantidad;
                            dgvVentas.Rows[i].Cells["subtotal"].Value = subtotal - subtotal * Descuento(lista[0][0]) / 100;
                            frmCantidad.Dispose();
                            GuardarVenta.Guardar(Guardar());
                            Total();
                            return;
                        }
                    dgvVentas.Rows.Add(new string[] { lista[0][0], lista[0][1], frmCantidad.Cantidad.ToString(), 
                        lista[0][6 - tipoCliente], subtotal.ToString(), cantidad.ToString(), Descuento(lista[0][0]).ToString()});
                    GuardarVenta.Guardar(Guardar());
                    Total();
                }
                frmCantidad.Dispose();

            }
            catch (Exception) { }
        }

        private void Total()
        {
            total = 0;
            for (int i = 0; i < dgvVentas.RowCount; i++)
                total += float.Parse(ValorCelda(dgvVentas, i, "subtotal"));
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
                    "WHERE (codigo LIKE '%" + buscador + "%' OR nombre LIKE '%" + buscador + "%') AND eliminado = 0");
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
            if (!btnVenta.Enabled || dgvVentas.Rows.Count <= 0)
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
                    ActivarVenta();
                }
                catch (Exception)
                {
                    DesactivarVenta();
                }
                buscarCliente.Dispose();
            }
            else
            {
                DialogResult confirmacion = MessageBox.Show(this, "Si cambia de cliente perdera los datos de la venta actual\n" +
                    "¿Desea continuar?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    Cancelar();
                    Total();
                    btnVenta.Enabled = false;
                    btnBuscarCliente_Click(sender, new EventArgs());
                }
            }

        }

        private void ActivarVenta()
        {
            btnBusqueda.Enabled = true;
            dgvProductos.Enabled = true;
            dgvVentas.Enabled = true;
            txtBusqueda.Enabled = true;
            btnVenta.Enabled = true;
            btnEliminar.Enabled = true;
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

        public void DesactivarVenta()
        {
            btnBusqueda.Enabled = false;
            dgvProductos.Enabled = false;
            dgvVentas.Enabled = false;
            txtBusqueda.Enabled = false;
            btnVenta.Enabled = false;
            btnEliminar.Enabled = false;
            txtNombre.Enabled = false;
            txtRfc.Enabled = false;
            txtApellidoPaterno.Enabled = false;
            Limpiar();
        }

        public void Limpiar()
        {
            dgvVentas.Rows.Clear();
            dgvProductos.Rows.Clear();
            foreach (Control control in Controls)
                if (control is TextBox)
                    control.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            for (int i = 0; i < dgvVentas.RowCount; i++)
                if (bool.Parse(dgvVentas.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                    listaEliminar.Add(new string[4] { dgvVentas.Rows[i].Cells["codigo"].Value.ToString(),
                        dgvVentas.Rows[i].Cells["descripcion"].Value.ToString(), dgvVentas.Rows[i].Cells["cantidad"].Value.ToString(),
                        dgvVentas.Rows[i].Cells["existencia"].Value.ToString() });
            if (listaEliminar.Count > 0)
            {
                string mensajeConfirmacion = "";
                foreach (string[] datos in listaEliminar)
                    mensajeConfirmacion += datos[0] + "\n";
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " + "estos productos de la venta? \nListado: \n" +
                    mensajeConfirmacion, "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    foreach (string[] datos in listaEliminar)
                        Sql.InsertarDatos("UPDATE productos SET existencia ='" + (float.Parse(datos[2]) + float.Parse(datos[3])) + "' WHERE codigo = '" + datos[0] + "'");
                    for (int i = 0; i < dgvVentas.RowCount; i++)
                        if (bool.Parse(dgvVentas.Rows[i].Cells["seleccionador"].EditedFormattedValue.ToString()))
                        {
                            dgvVentas.Rows.RemoveAt(i);
                            i--;
                        }
                    MessageBox.Show("Eliminados");
                    Buscador("SELECT  codigo, nombre, existencia FROM productos " +
                        "WHERE codigo LIKE '%" + buscador + "%' OR nombre LIKE '%" + buscador + "%'");
                    Total();
                    GuardarVenta.Guardar(Guardar());
                }
            }
            else
                MessageBox.Show("Seleccione al menos un producto a eliminar.");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            List<string[]> listaEliminar = new List<string[]>();

            if (dgvVentas.RowCount > 0)
            {
                DialogResult confirmacion = MessageBox.Show(this, "¿Desea cancelar toda la venta?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    Cancelar();
                    Limpiar();
                    DesactivarVenta();
                    Total();
                }
            }
            else
                MessageBox.Show("No hay nada en la venta.");
        }

        public InformacionVenta Guardar()
        {
            List<Producto> productos = new List<Producto>();
            for (int i = 0; i < dgvVentas.RowCount; i++)
                productos.Add(new Producto(dgvVentas.Rows[i].Cells["codigo"].Value.ToString(),
                    float.Parse(dgvVentas.Rows[i].Cells["cantidad"].Value.ToString()),
                    float.Parse(dgvVentas.Rows[i].Cells["descuentoPro"].Value.ToString())));
            return new InformacionVenta(id_cliente, productos);
        }

        public void Cancelar()
        {
            for (int i = 0; i < dgvVentas.RowCount; i++)
                Sql.InsertarDatos("UPDATE productos SET existencia ='" + (float.Parse(dgvVentas.Rows[i].Cells["cantidad"].Value.ToString())
                    + float.Parse(dgvVentas.Rows[i].Cells["existencia"].Value.ToString())) +
                    "' WHERE codigo = '" + dgvVentas.Rows[i].Cells["codigo"].Value.ToString() + "'");
            dgvVentas.Rows.Clear();
            dgvProductos.Rows.Clear();
            BorrarXML();
        }

        public void Recuperar()
        {
            if (File.Exists(Application.StartupPath + "\\Tostatronic_Venta.xml"))
            {
                try
                {
                    InformacionVenta informacionVenta = GuardarVenta.Leer();

                    List<string[]> listaCliente = Sql.BuscarDatos("SELECT id_cliente, rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes WHERE id_cliente = '" +
                        informacionVenta.Id_cliente + "'");

                    id_cliente = int.Parse(listaCliente[0][0]);
                    txtRfc.Text = listaCliente[0][1];
                    txtNombre.Text = listaCliente[0][2];
                    txtApellidoPaterno.Text = listaCliente[0][3];
                    tipoCliente = int.Parse(listaCliente[0][4]);
                    ActivarVenta();

                    foreach (Producto producto in informacionVenta.Productos)
                    {
                        List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE codigo = '" +
                            producto.Id_producto + "'");
                        float descuentoTemp = Descuento(lista[0][0]);
                        float subTotalTemp = producto.Cantidad * float.Parse(lista[0][6 - tipoCliente]);
                        dgvVentas.Rows.Add(new string[] { lista[0][0], lista[0][1], 
                            producto.Cantidad.ToString(), lista[0][6 - tipoCliente], 
                            (subTotalTemp - subTotalTemp*descuentoTemp/100).ToString(),  lista[0][2], descuentoTemp.ToString() });
                    }
                    Total();
                }
                catch (Exception) { }
            }
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            if (dgvVentas.Rows.Count > 0)
            {
                dialogVenta dgVenta = new dialogVenta(txtTotal.Text);
                dgVenta.ShowDialog();
                if (!dgVenta.Cancelar)
                {
                    string id_cliente = Sql.BuscarDatos("SELECT id_cliente FROM clientes WHERE rfc = '" + txtRfc.Text + "'")[0][0];
                    string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //Este es un contador de productos para la división de los mismos
                    int contadorProductos = 0;
                    //Aquí va una variable para el costo parcial en caso de que la venta lleve mas de 10 productos
                    float pago = dgVenta.Pago;
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
                    totalParcial *= 1.16f;
                    if (pago < totalParcial)
                    {
                        Sql.InsertarVenta(productos, Usuario.Instancia().Id.ToString(), id_cliente, false);
                        List<string[]> idList = Sql.BuscarDatos("SELECT id_venta FROM venta;");
                        string id = idList[idList.Count - 1][0];
                        long idVenta = long.Parse(id);
                        string comando = "INSERT INTO `salepoint`.`abonos` (`id_abono`, `id_venta`, `cantidad_abonada`, `fecha_abono`) VALUES (NULL, '"+idVenta+"', '"+pago+"', '"+fecha+"');";
                        Sql.InsertarDatos(comando);
                    }
                    else
                        Sql.InsertarVenta(productos, Usuario.Instancia().Id.ToString(), id_cliente, true);
                    string folio = Sql.ObtenerFolio();
                    string date=DateTime.Now.ToShortDateString() + " " +DateTime.Now.ToShortTimeString();
                    ImpresionTickets.ImprimeTicket(folio, productos, pago, totalParcial,date,txtNombre.Text,txtApellidoPaterno.Text);
                    PDFFile.Imprimir(this);
                    DesactivarVenta();
                    BorrarXML();
                    Limpiar();
                }
                dgVenta.Dispose();
            }
        }

        

        private string ReporteVenta(string fecha)
        {
            try
            {
                List<ProductoCompleto> listaProductos = new List<ProductoCompleto>();
                string[] pedido, usuario, cliente;

                try
                {
                    pedido = Sql.BuscarDatos("SELECT * FROM pedido WHERE fecha = '" + fecha + "'")[0];
                }
                catch (Exception)
                {
                    pedido = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };
                }

                try
                {
                    usuario = Sql.BuscarDatos("SELECT usuario, nombre, paterno, materno" +
                        " FROM usuarios WHERE id_usuario = '" + pedido[1] + "'")[0];
                }
                catch (Exception)
                {
                    usuario = new string[] { " ", " ", " ", " " };
                }

                try
                {
                    cliente = Sql.BuscarDatos("SELECT nombre, paterno, materno, rfc, telefono, domicilio, correo " +
                        " FROM clientes WHERE id_cliente = '" + pedido[2] + "'")[0];
                }
                catch (Exception)
                {
                    cliente = new string[] { " ", " ", " ", " ", " ", " ", " " };
                }

                foreach (string datos in pedido[5].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] temp = datos.Split(new char[] { ':' });
                    string[] producto = Sql.BuscarDatos("SELECT productos.codigo, productos.nombre, descuentos.descuento FROM productos,descuentos WHERE productos.codigo = '" + temp[0] + "' AND descuentos.id_producto = '" + temp[0] + "' AND descuentos.id_cliente= '" + pedido[2] + "'")[0];
                    listaProductos.Add(new ProductoCompleto(producto[0], producto[1], float.Parse(temp[1]),int.Parse(temp[3]), float.Parse(temp[2])));
                }

                Venta venta = new Venta(int.Parse(pedido[0]), new Usuarios(usuario[0], usuario[1] + " " + usuario[2] + " " + usuario[3]),
                    new Clientes(cliente[0], cliente[1], cliente[2], cliente[3], cliente[4], cliente[5], cliente[6]),
                    bool.Parse(pedido[3]), pedido[4], listaProductos, float.Parse(pedido[6]),
                    float.Parse(pedido[7]), float.Parse(pedido[8]));

                InfoReporte rep = GuardarInfoReporte.Leer();
                

                PDFFile pdf = new PDFFile("reportes", "reporte-");
                pdf.CrearPDF();
                string[,] data = {{"Cotización Tienda ", "Fecha: ", "RFC: ", "Direccion: ","Telefono: ", "Atendido por: "},
                            {pedido[0], venta.Fecha, rep.Reporte.rfc, rep.Reporte.direccion ,rep.Reporte.telefono, venta.Usuario.NombreCompleto}};
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
                    pdf.AgregarProductos(venta,false);
                    pdf.AgregarAnotaciones(" ");
                    pdf.NuevaPagina();

                }
                data[0, 0] = "Almacen";
                pdf.CrearCabecera(data);
                pdf.AgregarInfoCliente(venta.Cliente);
                pdf.AgregarProductos(venta,true);
                pdf.AgregarAnotaciones(" ");
                pdf.Cerrar();
                return pdf.Ruta;
            }
            catch (Exception) { }
            return null;
        }

        private void BorrarXML()
        {
            if (File.Exists(Application.StartupPath + "\\Tostatronic_Venta.xml"))
                File.Delete(Application.StartupPath + "\\Tostatronic_Venta.xml");
        }

        public void GenerarCotizacion(Venta venta)
        {
            try
            {
                try
                {
                    List<string[]> listaCliente = Sql.BuscarDatos("SELECT id_cliente, rfc, nombres, apellido_paterno, id_tipo_cliente FROM clientes WHERE rfc = '" +
                        venta.Cliente.rfc + "'");
                    id_cliente = int.Parse(listaCliente[0][0]);
                    txtRfc.Text = listaCliente[0][1];
                    txtNombre.Text = listaCliente[0][2];
                    txtApellidoPaterno.Text = listaCliente[0][3];
                    tipoCliente = int.Parse(listaCliente[0][4]);
                }
                catch (Exception)
                {
                    MessageBox.Show("La operación ha fallado porque el cliente ha sido eliminado.", "Error: Archivos corrompidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (ProductoCompleto producto in venta.Productos)
                {
                    List<string[]> lista = Sql.BuscarDatos("SELECT * FROM productos WHERE codigo = '" + producto.Codigo + "'");
                    float cantidad = 0;
                    float.TryParse(lista[0][2], out cantidad);
                    if (cantidad > 0)
                    {
                        if (cantidad >= producto.Cantidad)
                            Sql.InsertarDatos("UPDATE productos SET existencia=" + (cantidad - producto.Cantidad) +
                                " WHERE codigo = '" + producto.Codigo + "'");
                        float subtotal = producto.Cantidad * producto.Precio;
                        float descuento = Descuento(lista[0][0]);
                        subtotal -= subtotal * descuento / 100;
                        dgvVentas.Rows.Add(new string[] { lista[0][0], lista[0][1], producto.Cantidad.ToString(), 
                        producto.Precio.ToString(), subtotal.ToString(), (cantidad - producto.Cantidad).ToString(), descuento.ToString()});
                    }
                    else
                        MessageBox.Show("El producto: " + producto.Codigo + " ha sido eliminado o no tiene la cantidad necesaria para generarse",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo transladar la cotización", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            GuardarVenta.Guardar(Guardar());
            ActivarVenta();
            Total();
            MessageBox.Show("Se ha convertido esta cotización a venta.\nEn caso de ya no requerirla eliminela.", "Exito en la conversión", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvVentas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string codigo=ValorCelda(e.RowIndex,"codigo");
                float minPrice = float.Parse(Sql.BuscarDatos("SELECT precio_minimo FROM productos WHERE codigo = '" + codigo + "';")[0][0]);
                if (float.Parse(ValorCelda(e.RowIndex, "precio")) < minPrice)
                {
                    dgvVentas.Rows[e.RowIndex].Cells[3].ErrorText = "El precio minimo es de: " + minPrice.ToString();
                    dgvVentas.Rows[e.RowIndex].Cells[3].Value = ValorAnterior(codigo).ToString();
                }
                else
                {
                    float sub = float.Parse(dgvVentas.Rows[e.RowIndex].Cells[3].Value.ToString());
                    sub *= float.Parse(dgvVentas.Rows[e.RowIndex].Cells[2].Value.ToString());
                    sub-=sub * Descuento(codigo) / 100;
                    dgvVentas.Rows[e.RowIndex].Cells[4].Value = sub;
                    Total();
                    GuardarVenta.Guardar(Guardar());
                    dgvVentas.Rows[e.RowIndex].Cells[3].ErrorText = "";
                }
            }
        }
        private string ValorCelda(int fila, string columna)
        {
            return dgvVentas.Rows[fila].Cells[columna].Value.ToString();
        }
        private float ValorAnterior(string codigo)
        {
            if(tipoCliente==1)
                return float.Parse(Sql.BuscarDatos("SELECT precio_distribuidor FROM productos WHERE codigo ='"+codigo+"'")[0][0]);
            return float.Parse(Sql.BuscarDatos("SELECT precio_publico FROM productos WHERE codigo ='" + codigo + "'")[0][0]);
        }
    }
}
