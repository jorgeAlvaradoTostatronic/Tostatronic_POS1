using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    class PDFCatalogo
    {
        public static bool Catalogo(List<ProductoCatalogo> productos)
        {

            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Application.StartupPath + "\\Catalogo.pdf", FileMode.Create));
            

            // First, create our fonts
            var portadaFont = FontFactory.GetFont("Arial", 25, Font.BOLD,BaseColor.WHITE);
            var portadaDescriptionFont = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.WHITE);
            var productNameFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK);
            var productNameFontLess = FontFactory.GetFont("Arial", 15, Font.BOLD, BaseColor.BLACK);
            var productNameFontLess12 = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
            var productDescriptionFont = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLACK);
            var productPricesFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here
            #region PortadaCatalogo
            //Seccion para la creacion de la portada del catalogo
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\Recursos\\Agosto.png");
            logo.SetAbsolutePosition(0, 0);
            document.Add(logo);
            #endregion

#region DeclaracionTablasYCeldas
            PdfPTable Ti = new PdfPTable(2);
            PdfPTable subTable = new PdfPTable(1);
            PdfPTable tableNameImage = new PdfPTable(1);
            PdfPTable tableprices = new PdfPTable(3);
            PdfPCell principalCell = new PdfPCell();
            PdfPCell cel = new PdfPCell();
            PdfPCell productCel = new PdfPCell();
            #endregion


            int cont = -1;
            string fileName;
            DateTime fileCreationDatetime;
            foreach (ProductoCatalogo aux in productos)
            {
                if(cont==4 || cont==-1)
                {
                    if(cont!=-1)
                        document.Add(Ti);
                    #region Cabecera
                    //Aqui se pretende generar el codigo que nos permitira agregar los productos
                    document.NewPage();
                    fileName = string.Empty;
                    fileCreationDatetime = DateTime.Now;
                    document.AddHeader("Principal", "Guadalajara, Jalisco, México a " + fileCreationDatetime.ToString(@"yyyyMMdd"));
                    logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\Recursos\\Cabecera.png");
                    logo.SetAbsolutePosition(10, document.PageSize.Height - 30);
                    logo.ScaleToFit(pageSize);
                    document.Add(logo);
                    Ti = new PdfPTable(2);
                    Ti.HorizontalAlignment = 0;
                    Ti.WidthPercentage = 100;
                    Ti.SetWidths(new float[] { 100, 100 });  // then set the column's __relative__ widths
                    Ti.SpacingBefore = 100;
                    Ti.DefaultCell.Border = Rectangle.NO_BORDER;
                    #endregion

                    cont = 0;
                }
                #region AgregarProductos
                //Agregamos las propiedades de la tabla de nombre e imagenes
                tableNameImage = new PdfPTable(1);
                tableNameImage.SpacingBefore = 15;
                tableNameImage.HorizontalAlignment = 0;
                tableNameImage.WidthPercentage = 100;
                tableNameImage.SetWidths(new float[] { 100 });  // then set the column's __relative__ widths
                tableNameImage.SpacingBefore = 15;
                tableNameImage.DefaultCell.Border = Rectangle.NO_BORDER;
                //Agregamos el nombre del producto
                if (aux.Descripcion.Length > 32)
                    cel = new PdfPCell(new Phrase(aux.Descripcion, productNameFontLess12));
                else if (aux.Descripcion.Length>28)
                    cel = new PdfPCell(new Phrase(aux.Descripcion, productNameFontLess));
                else
                    cel = new PdfPCell(new Phrase(aux.Descripcion, productNameFont));
                cel.FixedHeight = 20;
                cel.MinimumHeight = 20;
                cel.Border = Rectangle.NO_BORDER;
                cel.BackgroundColor = BaseColor.WHITE;
                cel.HorizontalAlignment = 1;
                tableNameImage.AddCell(cel);
                //Agregamos la imagen del producto
                cel = new PdfPCell();
                cel.Border = Rectangle.NO_BORDER;
                cel.BackgroundColor = BaseColor.WHITE;
                cel.HorizontalAlignment = Element.ALIGN_CENTER;
                cel.VerticalAlignment = Element.ALIGN_MIDDLE;
                cel.Image = Image.GetInstance(Imagenes.ResizeImage(System.Drawing.Image.FromFile(Application.StartupPath + "\\Imagenes\\" + aux.Imagen), 500, 500),BaseColor.WHITE);
                tableNameImage.AddCell(cel);


                //Agregamos el codigo del producto
                cel = new PdfPCell(new Phrase(aux.Codigo, productDescriptionFont));
                cel.Border = Rectangle.NO_BORDER;
                cel.BackgroundColor = BaseColor.WHITE;
                cel.HorizontalAlignment = 1;
                tableNameImage.AddCell(cel);
                //cel = new PdfPCell(tableNameImage);
                //subTable.SetWidths(new float[] { 100 });
                //subTable.DefaultCell.Border = Rectangle.NO_BORDER;
                //subTable.AddCell(cel);
                //Agregamos las propiedades de la tabla de productos
                tableprices = new PdfPTable(3);
                tableprices.SetWidths(new float[] { 25, 25, 25 });
                tableprices.DefaultCell.Border = Rectangle.NO_BORDER;
                //Agregamos las celdas de productos
                //celda de precio publico
                productCel = new PdfPCell(new Phrase("Publico", productDescriptionFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableprices.AddCell(productCel);
                //Celda de precio distribuidor
                productCel = new PdfPCell(new Phrase("Distribuidor", productDescriptionFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableprices.AddCell(productCel);
                //Celda de precio minimo
                productCel = new PdfPCell(new Phrase("Minimo", productDescriptionFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableprices.AddCell(productCel);
                //Info de las celdas
                //publico
                productCel = new PdfPCell(new Phrase(aux.PrecioPublico.ToString("$0.00"), productPricesFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.WHITE;
                tableprices.AddCell(productCel);
                //distribuidor
                productCel = new PdfPCell(new Phrase(aux.PrecioDistribuidor.ToString("$0.00"), productPricesFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.WHITE;
                tableprices.AddCell(productCel);
                //Minimo
                productCel = new PdfPCell(new Phrase(aux.PrecioMinimo.ToString("$0.00"), productPricesFont));
                productCel.HorizontalAlignment = 1;
                productCel.Border = Rectangle.RECTANGLE;
                productCel.BackgroundColor = BaseColor.WHITE;
                tableprices.AddCell(productCel);
                //Agregamos la tabla de nombre a imagen a la celda principal y lo agregamos a la tabla auxiliar
                principalCell = new PdfPCell(tableNameImage);
                subTable = new PdfPTable(1);
                subTable.AddCell(principalCell);
                //Agregamos la tabla de productos a la celda principal
                principalCell = new PdfPCell(tableprices);
                //Agregamos la celda principal a la sub tabla
                subTable.AddCell(principalCell);
                //Convertimos la sub tabla a una celda y lo agregamos a la tabla principal
                principalCell = new PdfPCell(subTable);
                //Agregamos la primer celda a la tabla principal
                Ti.AddCell(principalCell);
                #endregion
                cont++;
            }
            document.Add(Ti);
            #region ContraPortada
            //Seccion para la creacion de la portada del catalogo
            document.NewPage();
            logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\Recursos\\ContraPortada.png");
            logo.SetAbsolutePosition(0, 0);
            document.Add(logo);
            #endregion
            document.Close();
            return true;
        }
    }
}
