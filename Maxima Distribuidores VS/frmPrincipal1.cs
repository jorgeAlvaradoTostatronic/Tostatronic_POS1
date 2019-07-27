using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace Maxima_Distribuidores_VS
{
    public partial class frmPrincipal : Form
    {
        private uscAgregarVenta agregarVenta;

        public frmPrincipal()
        {
            InitializeComponent();
            agregarVenta = new uscAgregarVenta();
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void rbnBtnFullScreen_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }

        private void rbnBtnNormalScreen_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            Theme.ColorTable = new RibbonProfesionalRendererColorTableBlack();
            uscAgregarVenta agregarVenta = new uscAgregarVenta();
            uscAgregarCotizacion agregarCotizacion = new uscAgregarCotizacion();
            agregarVenta.Dock = DockStyle.Fill;
            pnlContenedor.Controls.Add(agregarVenta);
            agregarVenta.Recuperar();
            Interfaz(agregarVenta);
            Permisos();
        }

        private void rbnBtnClientesAgregar_Click(object sender, EventArgs e)
        {
            uscCliente agregarCliente = new uscCliente(Accion.Agregar);
            Interfaz(agregarCliente);
        }

        private void rbnBtnClientesModificar_Click(object sender, EventArgs e)
        {
            uscCliente modificarCliente = new uscCliente(Accion.Modificar);
            Interfaz(modificarCliente);
        }

        private void rbnBtnClientesEliminar_Click(object sender, EventArgs e)
        {
            uscCliente eliminarCliente = new uscCliente(Accion.Eliminar);
            Interfaz(eliminarCliente);
        }

        private void rbnBtnClientesBuscar_Click(object sender, EventArgs e)
        {
            uscVerCliente buscarCliente = new uscVerCliente();
            Interfaz(buscarCliente);
        }

        private void rbnBtnProductosPublicos_Click(object sender, EventArgs e)
        {
            uscTipoProductos productosPublicos = new uscTipoProductos(TipoProducto.Publico);
            Interfaz(productosPublicos);
        }

        private void rbnBtnProductosDistribuidores_Click(object sender, EventArgs e)
        {
            uscTipoProductos productosDistribuidores = new uscTipoProductos(TipoProducto.Distribuidor);
            Interfaz(productosDistribuidores);
        }

        private void rbnBtnProductosEdicion_Click(object sender, EventArgs e)
        {
            uscVerProductos buscarProducto = new uscVerProductos();
            Interfaz(buscarProducto);
        }

        private void rbnBtnProductosAgregar_Click(object sender, EventArgs e)
        {
            uscProductos agregarProducto = new uscProductos(Accion.Agregar);
            Interfaz(agregarProducto);
        }

        private void rbnBtnProductosModificar_Click(object sender, EventArgs e)
        {
            uscProductos modificarProducto = new uscProductos(Accion.Modificar);
            Interfaz(modificarProducto);
        }

        private void rbnBtnProductosEliminar_Click(object sender, EventArgs e)
        {
            uscProductos eliminarProducto = new uscProductos(Accion.Eliminar); 
            Interfaz(eliminarProducto);
        }

        private void rbnBtnAgregarVentaCotizacion_Click(object sender, EventArgs e)
        {
            Interfaz(agregarVenta);
        }

        private void rbnBtnVentasRealizadas_Click(object sender, EventArgs e)
        {
            uscVentasRealizadas realizadas = new uscVentasRealizadas();
            Interfaz(realizadas);
        }

        private void rbnBtnVentasPendientes_Click_1(object sender, EventArgs e)
        {
            uscVentasPendientes pendientes = new uscVentasPendientes();
            Interfaz(pendientes);
        }

        private void rbnBtnNuevaCotizacion_Click(object sender, EventArgs e)
        {
            uscAgregarCotizacion agregarCotizacion = new uscAgregarCotizacion();
            Interfaz(agregarCotizacion);
        }

        private void Interfaz(UserControl userControl)
        {
            if (pnlContenedor.Controls.Count > 0)
                foreach (Control control in pnlContenedor.Controls)
                    if (control is uscAgregarVenta)
                        control.Visible = userControl is uscAgregarVenta;
                    else
                    {
                        pnlContenedor.Controls.Remove(control);
                        control.Dispose();
                    }
            userControl.Dock = DockStyle.Fill;
            if (!(userControl is uscAgregarVenta))
                pnlContenedor.Controls.Add(userControl);
            agregarVenta = (uscAgregarVenta)pnlContenedor.Controls[0];
        }

        #region PERMISOS
        private void Permisos()
        {
            
            Usuario usuario = Usuario.Instancia();
            rbtClientes.Visible = usuario.Permisos.Clientes;
            rbnBtnClientesAgregar.Visible = usuario.Permisos.AgregarClientes;
            rbnBtnClientesBuscar.Visible = usuario.Permisos.VerClientes;
            rbnBtnClientesEliminar.Visible = usuario.Permisos.EliminarClientes;
            rbnBtnClientesModificar.Visible = usuario.Permisos.ModificarClientes;

            rbtProductos.Visible = usuario.Permisos.Productos;
            rbnBtnProductosAgregar.Visible = usuario.Permisos.AgregarProductos;
            rbnBtnProductosPublicos.Visible = usuario.Permisos.VerProductos;
            rbnBtnProductosDistribuidores.Visible = usuario.Permisos.VerProductos;
            rbnBtnProductosPublicos.Visible = usuario.Permisos.VerProductos;
            rbnBtnProductosEliminar.Visible = usuario.Permisos.EliminarProductos;
            rbnBtnProductosModificar.Visible = usuario.Permisos.ModificarProductos;
            //rbnPnlProductosInventario.Visible = usuario.Permisos.Inventario;

            rbtReportes.Visible = usuario.Permisos.Reportes;
            rbnPnlCotizacion.Visible = usuario.Permisos.Cotizaciones;
            rbnPnlServidor.Visible = usuario.Permisos.Servidor;
            rbnPnlUsuarios.Visible = usuario.Permisos.ControlUsuario;
            rbnBtnUsuariosPermisos.Visible = usuario.Permisos.ControlUsuario;
            rbnBtnBDExportar.Enabled = usuario.Permisos.ExportarBD;
            rbnBtnBDImportar.Enabled = usuario.Permisos.ExportarBD;
            //rbnPnlRegistro.Visible = usuario.Permisos.Log;
            
        }
        #endregion

        private void rbnBtnUsuariosVer_Click(object sender, EventArgs e)
        {
            uscVerUsuarios uscVerUsers = new uscVerUsuarios();
            Interfaz(uscVerUsers);
        }

        private void rbnBtnCerrarSesion_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void rbnBtnUsuariosPermisos_Click(object sender, EventArgs e)
        {
            uscUsuarios uscPermisos = new uscUsuarios(Accion.Modificar);
            Interfaz(uscPermisos);
        }

        private void rbnBtnUsuariosPassword_Click(object sender, EventArgs e)
        {
            uscUsuariosPass uscUserPass = new uscUsuariosPass();
            Interfaz(uscUserPass);
        }

        private void rbnBtnUsuariosOperaciones_Click(object sender, EventArgs e)
        {
            uscUsuariosLog userLog = new uscUsuariosLog();
            Interfaz(userLog);
        }

        private void rbnBtnUsuariosAgregar_Click(object sender, EventArgs e)
        {
            uscUsuarios uscAgregarUsuario = new uscUsuarios(Accion.Agregar);
            Interfaz(uscAgregarUsuario);
        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (agregarVenta.HayProductos)
                {
                    DialogResult confirmacion = MessageBox.Show(this, "¿Desea guardar la venta actual?\n", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmacion == DialogResult.Yes)
                        GuardarVenta.Guardar(agregarVenta.Guardar());
                    else
                        agregarVenta.Cancelar();

                }
                else if (File.Exists(Application.StartupPath + "\\Maxima_Distribuidores_VS.xml"))
                    File.Delete(Application.StartupPath + "\\Maxima_Distribuidores_VS.xml");
                Directory.Delete(Application.StartupPath + "\\corte\\", true);
                Directory.Delete(Application.StartupPath + "\\reportes\\", true);
            }
            catch (Exception) { }
            Usuario.DestruirInstancia();
        }

        private void rbnBtnBDExportar_Click(object sender, EventArgs e)
        {
            dialogBaseDatos dbd = new dialogBaseDatos(Operacion.Exportar);
            dbd.ShowDialog();
        }

        private void rbnBtnBDImportar_Click(object sender, EventArgs e)
        {
            dialogBaseDatos dbd = new dialogBaseDatos(Operacion.Importar);
            dbd.ShowDialog();
        }

        private void rbnBtnServidorConfigurar_Click(object sender, EventArgs e)
        {
            dialogServidor ds = new dialogServidor();
            ds.ShowDialog();
        }

        private void rbnBtnServidorStatus_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Sql.ChecarStatusServidor(), "Estatus del servidor", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void rbnBtnReporteVer_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.ShowReadOnly = true;
            ofd.Title = "Ver archivo reciente";
            ofd.Filter = "Archivo PDF (*.pdf)|";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Application.StartupPath + "\\reportes\\";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                PDFFile.Ver(ofd.FileName);
            }
        }

        private void rbnBtnReporteImprimir_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.ShowReadOnly = true;
            ofd.Title = "Ver archivo reciente";
            ofd.Filter = "Archivo PDF (*.pdf)|";
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Application.StartupPath + "\\reportes\\";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                PDFFile.Imprimir(this, ofd.FileName);
            }
        }

        private void rbnBtnReporteGenerar_Click(object sender, EventArgs e)
        {
            uscReporte ur= new uscReporte();
            Interfaz(ur);
        }

        private void rbnBtnReporteInfo_Click(object sender, EventArgs e)
        {
            dialogInfoReporte dr = new dialogInfoReporte();
            dr.ShowDialog();
        }

        private void rbnBtnNuevaVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (agregarVenta.HayProductos)
                {
                    DialogResult confirmacion = MessageBox.Show(this, "Perdera la venta actual.\n¿Desea continuar aún asi?\n", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmacion == DialogResult.Yes)
                    {
                        agregarVenta.Cancelar();
                        agregarVenta.Limpiar();
                        agregarVenta.DesactivarVenta();
                        if (File.Exists(Application.StartupPath + "\\Maxima_Distribuidores_VS.xml"))
                            File.Delete(Application.StartupPath + "\\Maxima_Distribuidores_VS.xml");
                    }
                }
            }
            catch (Exception) { }
        }

        private void rbnBtnVerCotizaciones_Click(object sender, EventArgs e)
        {
            uscCotizaciones cotizaciones = new uscCotizaciones(agregarVenta);
            Interfaz(cotizaciones);
        }

        private void rbnBtnLicenciaInfo_Click(object sender, EventArgs e)
        {
            new acercaDe().ShowDialog();
        }

        private void rbnBtnClientesDescuento_Click(object sender, EventArgs e)
        {
            uscDescuentoClientes descuentos = new uscDescuentoClientes();
            Interfaz(descuentos);
        }

        private void rbnBtnClientesReActivar_Click(object sender, EventArgs e)
        {
            ReActivarClientes reactiva = new ReActivarClientes();
            Interfaz(reactiva);
        }

        private void rbnBtnProductosReActivar_Click(object sender, EventArgs e)
        {
            uscReActivarProductos reactivaProductos = new uscReActivarProductos();
            Interfaz(reactivaProductos);
        }

        private void rbnBtnProductosImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre,precio_distribuidor FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
                ProductoCompleto p= new ProductoCompleto();
                List<ProductoCompleto> pr=new List<ProductoCompleto>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.Precio = float.Parse(a[2]);
                    pr.Add(p);
                }
                PDFInvoice.ListaDePrecios(pr);
                PDFFile.Ver(Application.StartupPath+"\\ListaPrecios.pdf");
            }
            catch (Exception) { MessageBox.Show("Error"); }
        }

        private void rbnBtnClientesBuscar_DoubleClick(object sender, EventArgs e)
        {

        }

        private void rbnPnlVerVentas_Click(object sender, EventArgs e)
        {
            verVentas verVent = new verVentas();
            Interfaz(verVent);
        }

        private void rbnPnlVerCotizacion_Click(object sender, EventArgs e)
        {
            verCotizacion verCo = new verCotizacion();
            Interfaz(verCo);
        }

        private void rbnDistribuidor_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre,precio_publico FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
                ProductoCompleto p = new ProductoCompleto();
                List<ProductoCompleto> pr = new List<ProductoCompleto>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.Precio = float.Parse(a[2]);
                    pr.Add(p);
                }
                PDFInvoice.ListaDePrecios(pr);
                PDFFile.Ver(Application.StartupPath + "\\ListaPrecios.pdf");
            }
            catch (Exception) { MessageBox.Show("Error"); }
        }

        private void rbnFormato_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre,precio_publico FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
                ProductoCompleto p = new ProductoCompleto();
                List<ProductoCompleto> pr = new List<ProductoCompleto>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.Precio = float.Parse(a[2]);
                    pr.Add(p);
                }
                PDFInvoice.FormatoInventario(pr);
                PDFFile.Ver(Application.StartupPath + "\\Formato.pdf");
            }
            catch (Exception) { MessageBox.Show("Error"); }
        }

        private void rbnPnlTotalProductos_Click(object sender, EventArgs e)
        {
            List<string[]> p = Sql.BuscarDatos("SELECT SUM(precio_minimo*existencia) FROM productos ORDER BY codigo");
            float total = float.Parse(p[0][0]);
            MessageBox.Show("Total mejorado: " + total.ToString("$0.00"));

        }

        private void rbnBtnCredito_Click(object sender, EventArgs e)
        {
            uscCredito credito = new uscCredito();
            Interfaz(credito);
        }

        private void rbnModificarImagen_Click(object sender, EventArgs e)
        {
            uscImages images = new uscImages();
            Interfaz(images);
        }

        private void rbnBtnCatalogo_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre,precio_publico,precio_distribuidor, precio_minimo, imagen FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
                ProductoCatalogo p = new ProductoCatalogo();
                List<ProductoCatalogo> pr = new List<ProductoCatalogo>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.PrecioPublico = float.Parse(a[2]);
                    p.PrecioDistribuidor = float.Parse(a[3]);
                    p.PrecioMinimo = float.Parse(a[4]);
                    p.Imagen = a[5];
                    pr.Add(p);
                }
                PDFCatalogo.Catalogo(pr);
                PDFFile.Ver(Application.StartupPath + "\\Catalogo.pdf");
            }
            catch (Exception ae) { MessageBox.Show(ae.Message); }
            //tempClass.generaNombres();
        }

        private void rbnBtnActualizarExistencias_Click(object sender, EventArgs e)
        {
            string message=WebService.ActualizaExistencias();
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, precio_publico, existencia FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoCompleto p = new ProductoCompleto();
            List<ProductoCompleto> pr = new List<ProductoCompleto>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Precio = float.Parse(a[1]);
                p.Cantidad = float.Parse(a[2]);

                pr.Add(p);
            }
            message += "\n";
            MessageBox.Show(message + WebService.UpdatePriceApp(pr));
        }

        private void rbnBtnPrinter_Click(object sender, EventArgs e)
        {
            uscPrinterConfig printer = new uscPrinterConfig();
            Interfaz(printer);
        }

        private void rbnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre, imagen FROM productos WHERE eliminado=0 AND existencia=0 ORDER BY nombre ASC;");
                ProductoCatalogo p = new ProductoCatalogo();
                List<ProductoCatalogo> pr = new List<ProductoCatalogo>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.Imagen = a[2];
                    pr.Add(p);
                }
                ExportToExcel.DisplayInExcel(pr);
            }
            catch (Exception ae) { MessageBox.Show(ae.Message); }
        }

        private void rbnBtnGetData_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"noEstan.txt"))
                File.Delete(@"noEstan.txt");
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"noEstan.txt", true))
            {
                string[] data = WebService.GetNoInWebPage();
                foreach (string s in data)
                {
                    file.WriteLine(s);
                }
                file.Close();
            }
        }

        private void rbnGoogleSheet_Click(object sender, EventArgs e)
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre,existencia,precio_publico, imagen FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            string[] data = WebService.GetGoogleSheet();
            string[] aux;
            string[] stringSeparators = new string[] { " && " };
            List<ProductoGoogle> listaGoogle = new List<ProductoGoogle>();
            ProductoGoogle p;
            foreach (string s in data)
            {
                aux = s.Split(stringSeparators,
                           StringSplitOptions.RemoveEmptyEntries);
                foreach (string[] a in productos)
                {
                    if(a[0]==aux[0])
                    {
                        p.Codigo = a[0];
                        p.Nombre = a[1];
                        if (int.Parse(a[2]) != 0)
                            p.Estado = "en stock";
                        else
                            p.Estado = "agotado";
                        p.Precio = a[3];
                        p.EnlaceImagen = "https://tostatronic.com/Imagenes/" + a[4];
                        p.Enlace = aux[1];
                        if (aux.Length == 3)
                            p.Descripcion = aux[2];
                        else
                            p.Descripcion = "Por describir";
                        listaGoogle.Add(p);
                    }
                }
            }
            ExportToExcel.GoogleSheet(listaGoogle);
        }

        private void rbnBtnPromoDistribuidor_Click(object sender, EventArgs e)
        {
            String message=WebService.PromocionDistribuidor();
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, precio_distribuidor, existencia FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoCompleto p = new ProductoCompleto();
            List<ProductoCompleto> pr = new List<ProductoCompleto>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Precio = float.Parse(a[1]);
                p.Cantidad = float.Parse(a[2]);

                pr.Add(p);
            }
            message += "\n";
            MessageBox.Show(message + WebService.UpdatePriceApp(pr));
        }

        private void rbnBtnCorregirImagenes_Click(object sender, EventArgs e)
        {
            string newName;
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, imagen FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            string[] stringSeparators = new string[] { "." };
            string[] aux;
            foreach (string[] a in productos)
            {
                aux = a[1].Split(stringSeparators,
                           StringSplitOptions.RemoveEmptyEntries);
                newName = aux[0];
                newName += ".png";
                Sql.InsertarDatos("UPDATE productos SET imagen='" + newName + "' WHERE codigo='" + a[0] + "'");
            }
            MessageBox.Show("Coreccion correcta");
        }

        private void tbnAppProducts_Click(object sender, EventArgs e)
        {
            MessageBox.Show(WebService.ActualizaProductosApp());
          


            //De aqui en adelante no se requiere mas
            //List<string[]> productos = Sql.BuscarDatos("SELECT codigo, precio_publico, existencia FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            //ProductoCompleto p = new ProductoCompleto();
            //List<ProductoCompleto> pr = new List<ProductoCompleto>();
            //foreach (string[] a in productos)
            //{
            //    p.Codigo = a[0];
            //    p.Precio = float.Parse(a[1]);
            //    p.Cantidad = float.Parse(a[2]);

            //    pr.Add(p);
            //}
            //MessageBox.Show(WebService.UpdatePriceApp(pr));
        }

        private void rbnUdtDes_Click(object sender, EventArgs e)
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, precio_distribuidor, existencia FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoCompleto p = new ProductoCompleto();
            List<ProductoCompleto> pr = new List<ProductoCompleto>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Precio = float.Parse(a[1]);
                p.Cantidad = float.Parse(a[2]);

                pr.Add(p);
            }
            MessageBox.Show(WebService.UpdatePriceApp(pr));
            //string[] data = WebService.UpdateDescriptionApp();
            //string[] stringSeparators = new string[] { " && " };
            //string[] aux;
            //List<ProductoCompleto> lista = new List<ProductoCompleto>();
            //List<string[]> productos = Sql.BuscarDatos("SELECT codigo FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            //ProductoCompleto p;
            //foreach (string[] a in productos)
            //{
            //    foreach (string s in data)
            //    {
            //        p = new ProductoCompleto();
            //        aux = s.Split(stringSeparators,
            //                   StringSplitOptions.RemoveEmptyEntries);
            //        if(aux[0].Equals(a[0]))
            //        {
            //            p.Codigo = aux[0];
            //            p.Cantidad = int.Parse(aux[1]);
            //            if (aux.Length > 2)
            //                p.Descripcion = aux[2];
            //            else
            //                p.Descripcion = " Sin descripción.";
            //            byte[] bytes = Encoding.Default.GetBytes(p.Descripcion);
            //            p.Descripcion = Encoding.UTF8.GetString(bytes);
            //            p.Descripcion = p.Descripcion.Replace("'", " ");
            //            lista.Add(p);
            //        }
            //    }
            //}
            //MessageBox.Show(WebService.UpdateDescriptionApp2(lista),"Exito");
        }

        private void rbnBtnProductExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT codigo,nombre, imagen, precio_publico,precio_distribuidor,precio_minimo FROM productos WHERE eliminado=0  ORDER BY nombre ASC;");
                ProductoCatalogo p = new ProductoCatalogo();
                List<ProductoCatalogo> pr = new List<ProductoCatalogo>();
                foreach (string[] a in productos)
                {
                    p.Codigo = a[0];
                    p.Descripcion = a[1];
                    p.Imagen = a[2];
                    p.PrecioPublico = float.Parse(a[3]);
                    p.PrecioDistribuidor = float.Parse(a[4]);
                    p.PrecioMinimo = float.Parse(a[5]);
                    pr.Add(p);
                }
                ExportToExcel.ProductListPrices(pr);
            }
            catch (Exception ae) { MessageBox.Show(ae.Message); }
        }

        //En esta parte mandamos a llamar a user control para agregar y modificar los codigos de los productos
        private void RbnCodigoSat_Click(object sender, EventArgs e)
        {
            uscCodigoSat addSatCode = new uscCodigoSat();
            Interfaz(addSatCode);
        }

        private void RbnBtnCertificado_Click(object sender, EventArgs e)
        {
            if (Facturacion.Certificado())
                MessageBox.Show(this, "Archivo copiado correctamente", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, "Error al copiar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RbnBtnKey_Click(object sender, EventArgs e)
        {
            if (Facturacion.Llave())
                MessageBox.Show(this, "Archivo copiado correctamente", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, "Error al copiar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RbnPnlFacturacion_Click(object sender, EventArgs e)
        {
            uscFacturacion facturacion = new uscFacturacion();
            Interfaz(facturacion);
        }
    }
}
