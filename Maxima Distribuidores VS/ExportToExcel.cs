using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Maxima_Distribuidores_VS
{
    class ExportToExcel
    {
        public static void DisplayInExcel(IEnumerable<ProductoCatalogo> productos)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true; 
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "ID Producto";
            workSheet.Cells[1, "B"] = "Name of the product";
            workSheet.Cells[1, "C"] = "Quantiy";
            workSheet.Cells[1, "D"] = "Image";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            foreach (var producto in productos)
            {
                row++;
                workSheet.Cells[row, "A"] = producto.Codigo;
                workSheet.Cells[row, "B"] = producto.Descripcion;
                workSheet.Cells[row, "C"] = 0;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 4];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                workSheet.Shapes.AddPicture(Application.StartupPath + "\\Imagenes\\" + producto.Imagen, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left+5, top+5, 32, 32);
                workSheet.Rows.RowHeight = 40;
                workSheet.Columns[1].AutoFit();
                workSheet.Columns[2].AutoFit();
                workSheet.Columns[3].AutoFit();
            }
        }
        public static void ProductListPrices(IEnumerable<ProductoCatalogo> productos)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Referencia";
            workSheet.Cells[1, "B"] = "Nombre Del producto";
            workSheet.Cells[1, "C"] = "Precio Publico";
            workSheet.Cells[1, "D"] = "Precio Distribuidor";
            workSheet.Cells[1, "E"] = "Precio Minimo";
            workSheet.Cells[1, "F"] = "Image";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            foreach (var producto in productos)
            {
                row++;
                workSheet.Cells[row, "A"] = producto.Codigo;
                workSheet.Cells[row, "B"] = producto.Descripcion;
                workSheet.Cells[row, "C"] = producto.PrecioPublico;
                workSheet.Cells[row, "D"] = producto.PrecioDistribuidor;
                workSheet.Cells[row, "E"] = producto.PrecioMinimo ;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 6];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                try
                {
                    workSheet.Shapes.AddPicture(Application.StartupPath + "\\Imagenes\\" + producto.Imagen, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 32, 32);
                }
                catch(Exception ex)
                {
                    workSheet.Shapes.AddPicture(Application.StartupPath + "\\Imagenes\\no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 32, 32);
                }
                
                workSheet.Rows.RowHeight = 40;
                workSheet.Columns[1].AutoFit();
                workSheet.Columns[2].AutoFit();
                workSheet.Columns[3].AutoFit();
                workSheet.Columns[5].AutoFit();
                workSheet.Columns[6].AutoFit();
            }
        }
        public static void GoogleSheet(IEnumerable<ProductoGoogle> productos)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "id";
            workSheet.Cells[1, "B"] = "titulo";
            workSheet.Cells[1, "C"] = "descripcion";
            workSheet.Cells[1, "G"] = "enlace";
            workSheet.Cells[1, "D"] = "estado";
            workSheet.Cells[1, "E"] = "precio";
            workSheet.Cells[1, "F"] = "disp";
            workSheet.Cells[1, "H"] = "enlace imagen";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            foreach (var producto in productos)
            {
                row++;
                workSheet.Cells[row, "A"] = producto.Codigo;
                workSheet.Cells[row, "B"] = producto.Nombre;
                workSheet.Cells[row, "C"] = producto.Descripcion;
                workSheet.Cells[row, "D"] = "Nuevo";
                workSheet.Cells[row, "E"] = "MXN "+producto.Precio;
                workSheet.Cells[row, "F"] = producto.Estado;
                workSheet.Cells[row, "G"] = producto.Enlace;
                workSheet.Cells[row, "H"] = producto.EnlaceImagen;
                workSheet.Rows.RowHeight = 40;
            }
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[4].AutoFit();
            workSheet.Columns[5].AutoFit();
            workSheet.Columns[6].AutoFit();
            workSheet.Columns[7].AutoFit();
            workSheet.Columns[8].AutoFit();
        }
    }
}
