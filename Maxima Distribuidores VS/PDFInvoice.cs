using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class PDFInvoice
    {
        public static MemoryStream CreatePDF(TipoInvoice t, string folio, string fecha, List<ProductoCompleto> productos, string nombre, string patenro)
        {
            // Create a Document object
            string tv;
            if (t == TipoInvoice.Cotizacions)
                tv = "Cotizacion";
            else
                tv = "Venta";

            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Application.StartupPath + "\\Invoice.pdf", FileMode.Create));

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            PdfPTable title = new PdfPTable(1);
            title.HorizontalAlignment = 0;
            title.WidthPercentage = 200;
            title.SetWidths(new float[] { 20 });  // then set the column's __relative__ widths
            title.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell tileName = new PdfPCell(new Phrase(tv, titleFont));
            tileName.HorizontalAlignment = 2;
            tileName.Border = Rectangle.NO_BORDER;
            title.AddCell(tileName);
            document.Add(title);

            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 200;
            headertable.SetWidths(new float[] { 10, 10, 10 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable.SpacingAfter = 30;
            iTextSharp.text.Image logo;
            //Agregando la imagen
            if (File.Exists(Application.StartupPath + "\\Resources\\ticket.png"))
                 logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\Resources\\ticket.png");
            else
                logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\ticket.png");

            logo.SetAbsolutePosition(0, 0);
            logo.SetAbsolutePosition(80,document.PageSize.Height-100);
            logo.ScaleToFit(logo.Width/2, 200);
            document.Add(logo);
            PdfPCell invoiceCell = new PdfPCell(new Phrase(tv, titleFont));
            invoiceCell.HorizontalAlignment = 2;
            invoiceCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(invoiceCell);

            PdfPCell sp = new PdfPCell(new Phrase("", titleFont));
            sp.HorizontalAlignment = 2;
            sp.Border = Rectangle.NO_BORDER;
            headertable.AddCell(sp);
            sp = new PdfPCell(new Phrase("", titleFont));
            sp.HorizontalAlignment = 2;
            sp.Border = Rectangle.NO_BORDER;
            headertable.AddCell(sp);

            PdfPCell noCell = new PdfPCell(new Phrase("No :", bodyFont));
            noCell.HorizontalAlignment = 2;
            noCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(noCell);
            headertable.AddCell(new Phrase(folio, bodyFont));

            sp = new PdfPCell(new Phrase("", titleFont));
            sp.HorizontalAlignment = 2;
            sp.Border = Rectangle.NO_BORDER;
            headertable.AddCell(sp);

            PdfPCell billCell = new PdfPCell(new Phrase("Para: ", bodyFont));
            billCell.HorizontalAlignment = 2;
            billCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase(nombre + "\t" + patenro, bodyFont));

            sp = new PdfPCell(new Phrase("", titleFont));
            sp.HorizontalAlignment = 2;
            sp.Border = Rectangle.NO_BORDER;
            headertable.AddCell(sp);

            PdfPCell dateCell = new PdfPCell(new Phrase("Fecha :", bodyFont));
            dateCell.HorizontalAlignment = 2;
            dateCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(dateCell);
            headertable.AddCell(new Phrase(fecha, bodyFont));

            

            
            document.Add(headertable);
            //Create body table
            PdfPTable itemTable = new PdfPTable(5);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 40, 80, 20, 40 ,40});  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("Codigo", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("Nombre", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Cantidad", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Precio Unitario", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Sub Total", boldTableFont));
            cell5.HorizontalAlignment = 1;
            itemTable.AddCell(cell5);

            float total=0;
            int cont =1;
            foreach (ProductoCompleto producto in productos)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(producto.Codigo, bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    numberCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(producto.Descripcion, bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    descCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(producto.Cantidad.ToString(), bodyFont));
                qtyCell.HorizontalAlignment = 1;
                qtyCell.PaddingLeft = 10f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    qtyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(qtyCell);

                PdfPCell puCell = new PdfPCell(new Phrase(producto.Precio.ToString("$0.00"), bodyFont));
                puCell.HorizontalAlignment = 1;
                puCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    puCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(puCell);

                PdfPCell amtCell = new PdfPCell(new Phrase(producto.Subtotal.ToString("$0.00"), bodyFont));
                amtCell.HorizontalAlignment = 1;
                amtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    amtCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(amtCell);
                total += producto.Subtotal;
                cont++;

            }
            // Table footer
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell3 = new PdfPCell(new Phrase(""));
            totalAmtCell3.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell3);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Sub Total", boldTableFont));
            totalAmtStrCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(total.ToString("$0.00"), boldTableFont));
            totalAmtStrCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER; 
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);
            PdfPCell espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            PdfPCell iva = new PdfPCell(new Phrase("IVA", boldTableFont));
            iva.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            iva.HorizontalAlignment = 1;
            itemTable.AddCell(iva);
            PdfPCell ivaV = new PdfPCell(new Phrase((total*0.16).ToString("$0.00"), boldTableFont));
            ivaV.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            ivaV.HorizontalAlignment = 1;
            itemTable.AddCell(ivaV);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            PdfPCell totalV = new PdfPCell(new Phrase("Total", boldTableFont));
            totalV.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalV.HorizontalAlignment = 1;
            itemTable.AddCell(totalV);
            PdfPCell totalVV = new PdfPCell(new Phrase((total * 1.16).ToString("$0.00"), boldTableFont));
            totalVV.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            totalVV.HorizontalAlignment = 1;
            itemTable.AddCell(totalVV);

            PdfPCell cell = new PdfPCell(new Phrase("***Garantia d 1 mes despues de efectuada la venta***", bodyFont));
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            itemTable.AddCell(cell);
            document.Add(itemTable);

            

            //Approved by
            PdfContentByte cb = new PdfContentByte(writer);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);

            cb = new PdfContentByte(writer);
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(pageSize.GetLeft(70), 80);
            cb.ShowText("Tejedores #680, Col. La paz, Guadalajara, Jalisco");
            cb.EndText();
            document.Close();
            return PDFData;
        }

        public static MemoryStream ListaDePrecios(List<ProductoCompleto> p)
        {

            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Application.StartupPath + "\\ListaPrecios.pdf", FileMode.Create));

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here


            //Agregando la imagen
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\ticket.png");
            float pos = document.PageSize.Width;
            pos -= logo.Width;
            pos /= 2;
            logo.SetAbsolutePosition(pos, 0);
            logo.SetAbsolutePosition(80, document.PageSize.Height - 100);
            logo.ScaleToFit(logo.Width, 200);
            document.Add(logo);

            PdfPTable Ti = new PdfPTable(1);
            Ti.PaddingTop = 200;
            Ti.HorizontalAlignment = 0;
            Ti.WidthPercentage = 100;
            Ti.SetWidths(new float[] { 100 });  // then set the column's __relative__ widths
            Ti.SpacingBefore = 200;
            Ti.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cel = new PdfPCell(new Phrase(" ", titleFont));
            cel.Border = Rectangle.NO_BORDER;
            cel.HorizontalAlignment = 1;

            Ti.AddCell(cel);
            document.Add(Ti);
            
            Ti = new PdfPTable(1);
            Ti.PaddingTop = 20;
            Ti.HorizontalAlignment = 0;
            Ti.WidthPercentage = 100;
            Ti.SetWidths(new float[] { 100 });  // then set the column's __relative__ widths
            Ti.SpacingBefore = 100;
            Ti.DefaultCell.Border = Rectangle.NO_BORDER;
            cel = new PdfPCell(new Phrase("Lista de precios", titleFont));
            cel.HorizontalAlignment = 1;
            cel.Border = Rectangle.NO_BORDER;
            Ti.AddCell(cel);
            document.Add(Ti);

            //Create body table
            PdfPTable itemTable = new PdfPTable(3);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 100, 200, 40 });  // then set the column's __relative__ widths
            itemTable.SpacingBefore = 50;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("Codigo", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("Nombre", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Precio", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);


            int cont = 1;
            foreach (ProductoCompleto producto in p)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(producto.Codigo, bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    numberCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(producto.Descripcion, bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    descCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(producto.Precio.ToString(), bodyFont));
                qtyCell.HorizontalAlignment = 0;
                qtyCell.PaddingLeft = 10f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    qtyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(qtyCell);
                
                cont++;

            }
            document.Add(itemTable);
            document.Close();
            return PDFData;
        }

        public static bool FormatoInventario(List<ProductoCompleto> p)
        {

            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Application.StartupPath + "\\Formato.pdf", FileMode.Create));

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here


            //Agregando la imagen
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\ticket.png");
            float pos = document.PageSize.Width;
            pos -= logo.Width;
            pos /= 2;
            logo.SetAbsolutePosition(pos, 0);
            logo.SetAbsolutePosition(80, document.PageSize.Height - 100);
            logo.ScaleToFit(logo.Width, 200);
            document.Add(logo);

            PdfPTable Ti = new PdfPTable(1);
            Ti.PaddingTop = 200;
            Ti.HorizontalAlignment = 0;
            Ti.WidthPercentage = 100;
            Ti.SetWidths(new float[] { 100 });  // then set the column's __relative__ widths
            Ti.SpacingBefore = 200;
            Ti.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cel = new PdfPCell(new Phrase(" ", titleFont));
            cel.Border = Rectangle.NO_BORDER;
            cel.HorizontalAlignment = 1;

            Ti.AddCell(cel);
            document.Add(Ti);

            Ti = new PdfPTable(1);
            Ti.PaddingTop = 20;
            Ti.HorizontalAlignment = 0;
            Ti.WidthPercentage = 100;
            Ti.SetWidths(new float[] { 100 });  // then set the column's __relative__ widths
            Ti.SpacingBefore = 100;
            Ti.DefaultCell.Border = Rectangle.NO_BORDER;
            cel = new PdfPCell(new Phrase("Formato inventario", titleFont));
            cel.HorizontalAlignment = 1;
            cel.Border = Rectangle.NO_BORDER;
            Ti.AddCell(cel);
            document.Add(Ti);

            //Create body table
            PdfPTable itemTable = new PdfPTable(3);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 100, 200, 40 });  // then set the column's __relative__ widths
            itemTable.SpacingBefore = 50;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("Codigo", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("Nombre", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Cantida", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);


            int cont = 1;
            foreach (ProductoCompleto producto in p)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(producto.Codigo, bodyFont));
                numberCell.HorizontalAlignment = 1;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    numberCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(producto.Descripcion, bodyFont));
                descCell.HorizontalAlignment = 1;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    descCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(" ", bodyFont));
                qtyCell.HorizontalAlignment = 0;
                qtyCell.PaddingLeft = 10f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                if (cont % 2 == 0)
                    qtyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(qtyCell);

                cont++;

            }
            document.Add(itemTable);
            document.Close();
            return true;
        }

    }
}
