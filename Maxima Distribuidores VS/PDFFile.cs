using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Reflection;

namespace Maxima_Distribuidores_VS
{
    public class PDFFile
    {
        private string archivo;
        private Clientes clientes;

        public string Archivo
        {
            get { return archivo; }
        }

        private string carpeta;

        public string Carpeta
        {
            get { return carpeta; }
        }

        private string ruta;

        public string Ruta
        {
            get { return ruta; }
        }

        private Document pdf;

        PdfWriter pdfw;

        public Document PDF
        {
            get { return pdf; }
            set { pdf = value; }
        }


        private Font bold = new Font(Font.FontFamily.COURIER, 9f, Font.BOLD);
        private Font normal = new Font(Font.FontFamily.COURIER, 9f, Font.NORMAL);
        private Font cotizacion = new Font(Font.FontFamily.COURIER, 20f, Font.NORMAL);
        private Font titulo = new Font(Font.FontFamily.COURIER, 20f, Font.BOLD);


        public PDFFile(string carpeta, string archivo)
        {
            this.carpeta = carpeta;
            this.archivo = archivo + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
            this.ruta = Application.StartupPath + "\\" + carpeta + "\\";
            try
            {
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);
                ruta += this.archivo;
            }
            catch (Exception) { }
        }

        public void CrearPDF()
        {
            FileStream fs = new FileStream(ruta, FileMode.Create);
            Document doc = new Document(PageSize.LETTER, 50, 25, 0, 25);
            PdfWriter pdfw = PdfWriter.GetInstance(doc, fs);
            this.pdfw = pdfw;
            doc.Open();
            pdf = doc;
        }

        public void CrearCabecera(string[,] datos)
        {
            Image img;
            if (File.Exists(Application.StartupPath + "\\tarjeta.png"))
                img = Image.GetInstance(Application.StartupPath + "\\tarjeta.png");
            else
                img = Image.GetInstance(Application.StartupPath + "\\Resources\\tarjeta.png");
            
            img.ScaleAbsolute(230, 100);
            img.Alignment = Image.TEXTWRAP | Image.ALIGN_LEFT;
            pdf.Add(img);
            pdf.Add(new Paragraph("\n"));
            Phrase ph = new Phrase();
            Paragraph p;
            for (int k = 2; k < datos.GetLength(1); k++)
            {
                ph = new Phrase();
                if(k==2)
                    ph.Add("\n");
                ph.Add("                          ");
                ph.Add(new Phrase(datos[0, k], bold));
                ph.Add(new Phrase(datos[1, k].ToUpper(), normal));
                p = new Paragraph(ph);
                p.Alignment = Element.ALIGN_LEFT;
                pdf.Add(p);
            }
            ph = new Phrase();
            ph.Add(" \n\n\n");
            ph.Add(new Phrase(datos[0, 0], cotizacion));
            ph.Add("\n");
            ph.Add(new Phrase(datos[1, 0], cotizacion));
            p = new Paragraph(ph);
            p.Alignment = Element.ALIGN_RIGHT;
            pdf.Add(p);

            ph = new Phrase();
            ph.Add(new Phrase(datos[0, 1], bold));
            ph.Add(new Phrase(datos[1, 1], normal));
            p = new Paragraph(ph);
            p.Alignment = Element.ALIGN_RIGHT;
            pdf.Add(p);
        }

        public Document AgregarInfoCliente(Clientes clientes)
        {
            this.clientes = clientes;
            pdf.Add(new Paragraph("INFORMACION DEL CLIENTE\n\n", bold));
            Phrase ph = new Phrase();
            ph.Add(new Phrase("NOMBRE: ", bold));
            ph.Add(new Phrase(clientes.nombre.ToUpper() + " " + clientes.paterno.ToUpper() + " " + clientes.materno.ToUpper(), normal));
            ph.Add(new Phrase(" RFC: ", bold));
            ph.Add(new Phrase(clientes.rfc.ToUpper() + "\n", normal));
            if (!String.IsNullOrWhiteSpace(clientes.telefono))
            {
                ph.Add(new Phrase("TELEFONO: ", bold));
                ph.Add(new Phrase(clientes.telefono + "\n", normal));
            }
            if (!String.IsNullOrWhiteSpace(clientes.domicilio))
            {
                ph.Add(new Phrase("DOMICILIO: ", bold));
                ph.Add(new Phrase(clientes.domicilio.ToUpper(), normal));
            }
            pdf.Add(new Paragraph(ph));
            return pdf;
        }

        public void AgregarProductos(Venta venta, bool almacen)
        {
            pdf.Add(new Paragraph("\n"));
            float[] relativeWidths;
            if(!almacen)
                 relativeWidths = new float[]{ 80, 250,80, 80, 90,100 };
            else
                relativeWidths = new float[] { 110, 250, 100, 250 };
            PdfPTable tabla = new PdfPTable(relativeWidths);
            tabla.DefaultCell.Border = Rectangle.NO_BORDER;
            tabla.TotalWidth = pdf.PageSize.Width - 80f;
            tabla.LockedWidth = true;

            //PdfPCell cell = new PdfPCell(new Phrase("LISTADO DE PRODUCTOS", bold));
            //cell.Colspan = 5;
            //cell.Border = Rectangle.NO_BORDER;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            ////cell.BackgroundColor = BaseColor.GRAY;
            //tabla.AddCell(cell);
            tabla.AddCell(Celda("CODIGO",Element.ALIGN_LEFT));
            tabla.AddCell(Celda("DESCRIPCION",Element.ALIGN_LEFT));
            tabla.AddCell(Celda("UNIDAD ML",Element.ALIGN_RIGHT));
            if (!almacen)
            {
                tabla.AddCell(Celda("PRECIO", Element.ALIGN_RIGHT));
                tabla.AddCell(Celda("DESCUENTO", Element.ALIGN_RIGHT));
                tabla.AddCell(Celda("IMPORTE", Element.ALIGN_RIGHT));
            }
            else
                tabla.AddCell(Celda(" "));

            if (venta.Productos != null)
            {
                int k = 0;
                for (k = 0; k < venta.Productos.Count; k++)
                {
                    tabla.AddCell(new Phrase(venta.Productos[k].Codigo.ToUpper(), normal));
                    tabla.AddCell(new Phrase(venta.Productos[k].Descripcion.ToUpper(), normal));
                    tabla.AddCell(Celda(venta.Productos[k].Cantidad.ToString("0.00")));
                    if (almacen)
                        tabla.AddCell(Celda(" "));
                    if (!almacen)
                    {
                        tabla.AddCell(Celda(venta.Productos[k].Precio.ToString("$0.00")));
                        tabla.AddCell(Celda(venta.Productos[k].Descuento.ToString() + "%"));
                        tabla.AddCell(Celda(venta.Productos[k].Subtotal.ToString("$0.00")));
                    }   
                }
                if (!String.IsNullOrWhiteSpace(clientes.telefono))
                    k += 2;
                if (!venta.Pagado)
                    k += 4;
                for (int i = 0; i < (26 - k); i++)
                {
                    for (int j = 0; j < 3; j++)
                        tabla.AddCell(new Phrase(" ", normal));
                    if (!almacen)
                        for (int j = 0; j < 3; j++)
                            tabla.AddCell(new Phrase(" ", normal));
                }
            }
            
            float subtotal = venta.Total + venta.Descuento;
            subtotal /= 1.16f;
            if(!almacen)
            {
                pdf.Add(tabla);

                float[] anchoRelativo;
                anchoRelativo = new float[] { 250, 400 };
                PdfPTable comentario = new PdfPTable(anchoRelativo);
                comentario.DefaultCell.Border = Rectangle.BOX;
                comentario.TotalWidth = pdf.PageSize.Width - 80f;
                comentario.LockedWidth = true;
                InfoReporte info = GuardarInfoReporte.Leer();
                PdfPCell leyenda = new PdfPCell(new Phrase(info.Reporte.leyenda));
                PdfPCell banco = new PdfPCell(new Phrase(info.Reporte.banco));
                leyenda.HorizontalAlignment = Element.ALIGN_CENTER;
                banco.HorizontalAlignment = Element.ALIGN_CENTER;
                
                comentario.AddCell(leyenda);
                comentario.AddCell(banco);
                pdf.Add(comentario);

                if (String.IsNullOrWhiteSpace(clientes.telefono) && String.IsNullOrWhiteSpace(clientes.domicilio))
                    vecesNuevoRenglon(2);
                else
                    vecesNuevoRenglon(1);

                PdfPTable tabla3 = new PdfPTable(relativeWidths);
                tabla3.DefaultCell.Border = Rectangle.NO_BORDER;
                tabla3.TotalWidth = pdf.PageSize.Width - 80f;
                tabla3.LockedWidth = true;
                PdfPCell sub = new PdfPCell(new Phrase("SUBTOTAL", bold));
                sub.Colspan = 5;
                sub.HorizontalAlignment = Element.ALIGN_RIGHT;
                sub.Border = Rectangle.NO_BORDER;
                tabla3.AddCell(sub);
                tabla3.AddCell(new Phrase(subtotal.ToString("$0.00"), normal));
                PdfPCell subIva = new PdfPCell(new Phrase("16% I.V.A.", bold));
                subIva.Colspan = 5;
                subIva.HorizontalAlignment = Element.ALIGN_RIGHT;
                subIva.Border = Rectangle.NO_BORDER;
                tabla3.AddCell(subIva);
                float iva = subtotal * .16f;
                tabla3.AddCell(new Phrase(iva.ToString("$0.00"), normal));
                if (venta.Descuento > 0)
                {
                    PdfPCell desc = new PdfPCell(new Phrase("DESCUENTO", bold));
                    desc.Colspan = 5;
                    desc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    desc.Border = Rectangle.NO_BORDER;
                    tabla3.AddCell(desc);
                    tabla3.AddCell(new Phrase(venta.Descuento.ToString("$0.00"), normal));
                }

                PdfPCell total = new PdfPCell(new Phrase("TOTAL", bold));
                total.Colspan = 5;
                total.HorizontalAlignment = Element.ALIGN_RIGHT;
                total.Border = Rectangle.NO_BORDER;
                tabla3.AddCell(total);
                subtotal *= 1.16f;
                tabla3.AddCell(new Phrase(subtotal.ToString("$0.00"), normal));
                pdf.Add(tabla3);
                //NuevoRenglon();
                if (!venta.Pagado)
                {
                    sub = new PdfPCell(new Phrase("PAGO", bold));
                    sub.Colspan = 5;
                    sub.HorizontalAlignment = Element.ALIGN_RIGHT;
                    sub.Border = Rectangle.NO_BORDER;
                    tabla3.Rows.Clear();
                    tabla3.AddCell(sub);
                    tabla3.AddCell(new Phrase(venta.Abono.ToString("$0.00"), normal));
                    sub = new PdfPCell(new Phrase("RESTANTE", bold));
                    sub.Colspan = 5;
                    sub.HorizontalAlignment = Element.ALIGN_RIGHT;
                    sub.Border = Rectangle.NO_BORDER;
                    tabla3.AddCell(sub);
                    tabla3.AddCell(new Phrase((venta.Total - venta.Abono).ToString("$0.00"), normal));

                    pdf.Add(tabla3);
                }
            }
            if(almacen)
            {
                pdf.Add(tabla);
                tabla.Rows.Clear();
            }
            
        }
        public void imprimirProductos(List<string[]> productos)
        {
            pdf.Add(new Paragraph("\n"));
            float[] relativeWidths;
            relativeWidths = new float[] { 150,350,180 };
            PdfPTable tabla = new PdfPTable(relativeWidths);
            tabla.DefaultCell.Border = Rectangle.NO_BORDER;
            tabla.TotalWidth = pdf.PageSize.Width - 80f;
            tabla.LockedWidth = true;
            tabla.AddCell(Celda("CODIGO", Element.ALIGN_LEFT));
            tabla.AddCell(Celda("DESCRIPCION", Element.ALIGN_LEFT));
            tabla.AddCell(Celda("PRECIO", Element.ALIGN_RIGHT));
            if (productos.Count > 0)
            {
                int k = 0;
                for (k = 0; k < productos.Count; k++)
                {
                    tabla.AddCell(new Phrase(productos[k][0].ToUpper(), normal));
                    tabla.AddCell(new Phrase(productos[k][1].ToUpper(), normal));
                    tabla.AddCell(Celda(float.Parse(productos[k][2]).ToString("$0.00")));
                }
                pdf.Add(tabla);
            }
        }
        PdfPCell Celda(string informacion)
        {
            PdfPCell cell = new PdfPCell(new Phrase(informacion, normal));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            return cell;
        }
        PdfPCell Celda(string informacion, int alineacion)
        {
            PdfPCell cell = new PdfPCell(new Phrase(informacion, bold));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = alineacion;
            return cell;
        }
        void vecesNuevoRenglon(int numeroRenglones)
        {
            for (int i = 0; i < numeroRenglones; i++)
                NuevoRenglon();
        }
        public void AgregarCorte(List<string[]> corte, List<string[]> pendiente, List<string[]> abonos)
        {
            pdf.Add(new Paragraph("\n"));

            float[] relativeWidths = { 90, 340, 90, 90, 100 };
            PdfPTable tabla = new PdfPTable(relativeWidths);
            tabla.DefaultCell.Border = Rectangle.NO_BORDER;
            tabla.TotalWidth = pdf.PageSize.Width - 80f;
            tabla.LockedWidth = true;

            PdfPCell cell = new PdfPCell(new Phrase("CORTE", bold));
            cell.Colspan = 5;
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            tabla.AddCell(cell);

            tabla.AddCell(new Phrase("FOLIO", bold));
            tabla.AddCell(new Phrase("NUM CLIENTE", bold));
            tabla.AddCell(new Phrase("RFC", bold));
            tabla.AddCell(new Phrase("FECHA", bold));
            tabla.AddCell(new Phrase("SUBTOTAL", bold));
            float total = 0;
            
            if (pendiente.Count > 0)
            {
                cell.Phrase = new Phrase("LISTA DE VENTAS PENDIENTES", bold);
                for (int k = 0; k < pendiente.Count; k++)
                {
                    tabla.AddCell(new Phrase(pendiente[k][0], normal));
                    try
                    {
                        String idCliente = pendiente[k][1];
                        List<string[]> rfc = Sql.BuscarDatos("SELECT rfc FROM clientes WHERE id_cliente = '" + idCliente + "'");
                        tabla.AddCell(new Phrase("PENDIENTE A: " + pendiente[k][2].ToUpper() + " " + pendiente[k][3].ToUpper() +" "+pendiente[k][4], normal));
                        tabla.AddCell(new Phrase(rfc[0][0].ToUpper(), normal));
                    
                    }
                    catch (Exception)
                    {

                        tabla.AddCell(new Phrase("PENDIENTE A: CLIENTE ELIMINADO", normal));
                        tabla.AddCell(new Phrase("INDETERMINADO", normal));
                    
                    }
                    tabla.AddCell(new Phrase(pendiente[k][5].ToUpper(), normal));
                    if (bool.Parse(pendiente[k][7]))
                        tabla.AddCell(new Phrase("CANC."+pendiente[k][6].ToUpper(), normal));
                    else
                    {
                        
                        tabla.AddCell(new Phrase(float.Parse(pendiente[k][6]).ToString("$0.00"), normal));
                        total += float.Parse(pendiente[k][6]);
                    }
                        
                }
            }
            
            if (abonos.Count > 0)
            {
                cell.Phrase = new Phrase("ABONOS REALIZADOS", bold);
                tabla.AddCell(cell);
                for (int k = 0; k < abonos.Count; k++)
                {
                    bool pendientes = true;
                    bool realizadas = true;
                    try
                    {
                        for (int j = 0; j < pendiente.Count; j++ )
                        {
                            if(abonos[k][0] == pendiente[j][0])
                                pendientes = false;        
                        }
                        for (int j = 0; j < corte.Count; j++)
                        {
                            if (abonos[k][0] == corte[j][0])
                                realizadas = false;
                        }    
                    }
                    catch (Exception) { }
                    if (pendientes && realizadas)
                    {
                            tabla.AddCell(new Phrase(abonos[k][0], normal));
                            try
                            {

                                List<string[]> idCliente = Sql.BuscarDatos("SELECT id_cliente FROM venta WHERE id_venta = '" + abonos[k][0] + "'");
                                List<string[]> nombreCliente = Sql.BuscarDatos("SELECT nombres, apellido_paterno, rfc FROM clientes WHERE id_cliente = '" + idCliente[0][0] + "'");
                                tabla.AddCell(new Phrase("ABONO DE: " + nombreCliente[0][0].ToUpper() + " " + nombreCliente[0][1].ToUpper(), normal));
                                tabla.AddCell(new Phrase(nombreCliente[0][2].ToUpper(), normal));
                            }
                            catch (Exception)
                            {

                                tabla.AddCell(new Phrase("PENDIENTE A: CLIENTE ELIMINADO", normal));
                                tabla.AddCell(new Phrase("INDETERMINADO", normal));
                            }

                            tabla.AddCell(new Phrase(abonos[k][1].ToUpper(), normal));
                            tabla.AddCell(new Phrase(float.Parse(abonos[k][2]).ToString("$0.00"), normal));
                            total += float.Parse(abonos[k][2]);
                        
                    }
                }
            }
                
            
            if (corte.Count > 0)
            {
                cell.Phrase = new Phrase("VENTAS REALIZADAS", bold);
                tabla.AddCell(cell);
                for (int k = 0; k < corte.Count; k++)
                {
                    tabla.AddCell(new Phrase(corte[k][0], normal));
                    String idCliente = corte[k][1];
                    try
                    {
                        List<string[]> rfc = Sql.BuscarDatos("SELECT rfc FROM clientes WHERE id_cliente = '" + idCliente + "'");
                        tabla.AddCell(new Phrase(corte[k][2].ToString().ToUpper() + " " + corte[k][3].ToString().ToUpper(), normal));
                        tabla.AddCell(new Phrase(rfc[0][0].ToString().ToUpper(), normal));
                    }
                    catch (Exception)
                    {

                        tabla.AddCell(new Phrase("CLIENTE ELIMINADO", normal));
                        tabla.AddCell(new Phrase("INDETERMINADO", normal));
                    }

                    tabla.AddCell(new Phrase(corte[k][5].ToUpper(), normal));
                    if (bool.Parse(corte[k][7]))
                        tabla.AddCell(new Phrase("CANC." + float.Parse(corte[k][6]).ToString("$0.00"), normal));
                    else
                    {
                        tabla.AddCell(new Phrase(float.Parse(corte[k][6]).ToString("$0.00"), normal));
                        total += float.Parse(corte[k][6]);
                    }

                }
            }
            PdfPCell p = new PdfPCell(new Phrase(" ", normal));
            p.Colspan = 5;
            p.Border = Rectangle.NO_BORDER;
            tabla.AddCell(p);
            PdfPCell sub = new PdfPCell(new Phrase("TOTAL", bold));
            sub.Border = Rectangle.NO_BORDER;
            sub.Colspan = 4;
            sub.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabla.AddCell(sub);
            tabla.AddCell(new Phrase(total.ToString("$0.00"), normal));
            pdf.Add(tabla);
        }

        public void AgregarProductos(Cotizacion cotizacion)
        {
            pdf.Add(new Paragraph("\n"));
            float[] relativeWidths;
            relativeWidths = new float[] { 80, 250, 80, 80, 90, 100 };
            PdfPTable tabla = new PdfPTable(relativeWidths);
            tabla.DefaultCell.Border = Rectangle.NO_BORDER;
            tabla.TotalWidth = pdf.PageSize.Width - 80f;
            tabla.LockedWidth = true;
            tabla.AddCell(Celda("CODIGO", Element.ALIGN_LEFT));
            tabla.AddCell(Celda("DESCRIPCION", Element.ALIGN_LEFT));
            tabla.AddCell(Celda("UNIDAD ML", Element.ALIGN_RIGHT));
            tabla.AddCell(Celda("PRECIO", Element.ALIGN_RIGHT));
            tabla.AddCell(Celda("DESCUENTO", Element.ALIGN_RIGHT));
            tabla.AddCell(Celda("IMPORTE", Element.ALIGN_RIGHT));
            if (cotizacion.Productos != null)
            {
                int k = 0;
                for (k = 0; k < cotizacion.Productos.Count; k++)
                {
                    tabla.AddCell(new Phrase(cotizacion.Productos[k].Codigo.ToUpper(), normal));
                    tabla.AddCell(new Phrase(cotizacion.Productos[k].Descripcion.ToUpper(), normal));
                    tabla.AddCell(Celda(cotizacion.Productos[k].Cantidad.ToString("0.00")));
                    tabla.AddCell(Celda(cotizacion.Productos[k].Precio.ToString("$0.00")));
                    tabla.AddCell(Celda(cotizacion.Productos[k].Descuento.ToString() + "%"));
                    tabla.AddCell(Celda(cotizacion.Productos[k].Subtotal.ToString("$0.00")));
                    
                }
                if (!String.IsNullOrWhiteSpace(clientes.telefono))
                    k += 2;
                for (int i = 0; i < (26 - k); i++)
                {
                    for (int j = 0; j < 6; j++)
                        tabla.AddCell(new Phrase(" ", normal));
                }

            }
            pdf.Add(tabla);


            float[] anchoRelativo;
            anchoRelativo = new float[] { 250, 400 };
            PdfPTable comentario = new PdfPTable(anchoRelativo);
            comentario.DefaultCell.Border = Rectangle.BOX;
            comentario.TotalWidth = pdf.PageSize.Width - 80f;
            comentario.LockedWidth = true;
            InfoReporte info = GuardarInfoReporte.Leer();
            PdfPCell leyenda = new PdfPCell(new Phrase(info.Reporte.leyenda));
            PdfPCell banco = new PdfPCell(new Phrase(info.Reporte.banco));
            leyenda.HorizontalAlignment = Element.ALIGN_CENTER;
            banco.HorizontalAlignment = Element.ALIGN_CENTER;

            comentario.AddCell(leyenda);
            comentario.AddCell(banco);
            pdf.Add(comentario);
            if (String.IsNullOrWhiteSpace(clientes.telefono))
                vecesNuevoRenglon(2);
            float subtotal = cotizacion.Total;
            subtotal /= 1.16f;

            if (clientes.telefono != "")
                vecesNuevoRenglon(2);

            PdfPTable tabla3 = new PdfPTable(relativeWidths);
            tabla3.DefaultCell.Border = Rectangle.NO_BORDER;
            tabla3.TotalWidth = pdf.PageSize.Width - 80f;
            tabla3.LockedWidth = true;
            PdfPCell sub = new PdfPCell(new Phrase("SUBTOTAL", bold));
            sub.Colspan = 5;
            sub.HorizontalAlignment = Element.ALIGN_RIGHT;
            sub.Border = Rectangle.NO_BORDER;
            tabla3.AddCell(sub);
            tabla3.AddCell(new Phrase(subtotal.ToString("$0.00"), normal));
            PdfPCell subIva = new PdfPCell(new Phrase("16% I.V.A.", bold));
            subIva.Colspan = 5;
            subIva.HorizontalAlignment = Element.ALIGN_RIGHT;
            subIva.Border = Rectangle.NO_BORDER;
            tabla3.AddCell(subIva);
            float iva = subtotal * .16f;
            tabla3.AddCell(new Phrase(iva.ToString("$0.00"), normal));
            PdfPCell total = new PdfPCell(new Phrase("TOTAL", bold));
            total.Colspan = 5;
            total.HorizontalAlignment = Element.ALIGN_RIGHT;
            total.Border = Rectangle.NO_BORDER;
            tabla3.AddCell(total);
            subtotal *= 1.16f;
            tabla3.AddCell(new Phrase(subtotal.ToString("$0.00"), normal));
            pdf.Add(tabla3);
            tabla.Rows.Clear();
            tabla3.Rows.Clear();
        }

        public void AgregarAnotaciones(string cadena)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            PdfContentByte cb = pdfw.DirectContent;
            PdfTemplate template = cb.CreateTemplate(50, 50);

            Rectangle pageSize = pdf.PageSize;
            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(72));
            cb.ShowText("ANOTACIONES");
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetLeft(40), pageSize.GetBottom(30));


            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(70));
            cb.ShowText("____________________________________________________________________________");
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(20));
            cb.ShowText("____________________________________________________________________________");
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, cadena, pageSize.GetRight(40), pageSize.GetBottom(30), 0);
            cb.EndText();
        }

        public static bool Imprimir(UserControl usc, string ruta)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = false,
                    Verb = "print",
                    FileName = ruta //put the correct path here
                };
                p.Start();
            }
            catch (Exception) { }
            return true;
        }

        public static bool Imprimir(Form frm, string ruta)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "print",
                    FileName = ruta //put the correct path here
                };
                p.Start();
            }
            catch (Exception) { }
            return true;
        }

        public static bool Imprimir(Form frm)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "print",
                    FileName = Application.StartupPath + "\\Ticket.pdf" //put the correct path here
                };
                p.Start();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool Imprimir(UserControl usc)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    Verb = "print",
                    FileName = Application.StartupPath + "\\Ticket.pdf" //put the correct path here
                };
                p.Start();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static void Ver(string ruta)
        {
            try
            {
                //Process.Start(ruta);
                Process proceso = Process.Start(ruta);
                proceso.WaitForInputIdle();
            }
            catch (Exception)
            { }
        }

        public void Cerrar() { pdf.Close(); }

        public void NuevaPagina() { pdf.NewPage(); }

        public void NuevoRenglon() { pdf.Add(new Paragraph("\n")); }
    }
}
