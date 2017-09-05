using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class ImpresionTickets
    {
        public static bool ImprimeTicket(string prmFolioTicket, List<ProductoCompleto> productos, float pagoCon, float totalL, string date, string nombre, string apellido, float impuesto)
        {
            try
            {
                double varEFECTIVO = 0;
                double varCAMBIO = 0;
                double varTOTAL = 0;
                double varIVA = 0;
                Ticket ticket = new Ticket();
                ticket.Path = Application.StartupPath;
                ticket.FileName = "\\Ticket.pdf";
                if(File.Exists(Application.StartupPath + "\\Resources\\ticket.png"))
                    ticket.HeaderImage = Application.StartupPath + "\\Resources\\ticket.png";
                else
                    ticket.HeaderImage = Application.StartupPath + "\\ticket.png";
                ticket.AddHeaderLine("              TOSTATRONIC");
                ticket.AddHeaderLine("    Venta de componentes electronicos");
                ticket.AddSubHeaderLine("\n");
                ticket.AddSubHeaderLine("Folio: " + prmFolioTicket);
                ticket.AddSubHeaderLine("Le atendió: " +
                    Usuario.Instancia().Nombre + " " + Usuario.Instancia().Paterno);
                ticket.AddSubHeaderLine("Fecha y Hora: " +
                    date + " ");
                ticket.AddSubHeaderLine("Cliente: " +
                    nombre + " " + apellido);
                varEFECTIVO = Convert.ToDouble(pagoCon);
                foreach (ProductoCompleto a in productos)
                {
                    ticket.AddItem(a.Cantidad.ToString(), a.Descripcion, a.Precio.ToString(), (a.Cantidad * a.Precio).ToString());
                }
                varTOTAL += Convert.ToDouble(totalL);
                varIVA +=
                    Convert.ToDouble(varTOTAL * (impuesto-1));
                varCAMBIO = ((varTOTAL + varIVA) - varEFECTIVO) * -1;

                //El metodo AddTotal requiere 2 parametros, 
                //la descripcion del total, y el precio 
                ticket.AddTotal("SUBTOTAL", varTOTAL.ToString("$0.00"));
                ticket.AddTotal("IVA", varIVA.ToString("$0.00"));
                ticket.AddTotal("TOTAL", (varTOTAL + varIVA).ToString("$0.00"));
                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                ticket.AddTotal("RECIBIDO", varEFECTIVO.ToString("$0.00"));
                if (varCAMBIO < 0)
                    ticket.AddTotal("Restante: ", (varCAMBIO * -1).ToString("$0.00"));
                else
                    ticket.AddTotal("Cambio: ", varCAMBIO.ToString("$0.00"));
                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                //El metodo AddFooterLine funciona igual que la cabecera 
                ticket.AddFooterLine("       Gracias por su preferencia");
                ticket.AddFooterLine("    Tostatronic le desea un buen dia");
                //Generamos
                if (ticket.Print()) { return (true); }
                else { return false; }
            }
            catch (Exception ) { return false; }
        }
        public static bool ImprimeTicketPago(string prmFolioTicket, float pagoCon, float totalL, string date, string nombre)
        {
            try
            {
                double cantidadPagada = 0;
                double restante = 0;
                double varTOTAL = 0;                Ticket ticket = new Ticket();
                ticket.Path = Application.StartupPath;
                ticket.FileName = "\\Pago.pdf";
                if (File.Exists(Application.StartupPath + "\\Resources\\ticket.png"))
                    ticket.HeaderImage = Application.StartupPath + "\\Resources\\ticket.png";
                else
                    ticket.HeaderImage = Application.StartupPath + "\\ticket.png";
                ticket.AddHeaderLine("              TOSTATRONIC");
                ticket.AddHeaderLine("    Venta de componentes electronicos");
                ticket.AddSubHeaderLine("\n");
                ticket.AddSubHeaderLine("Folio: " + prmFolioTicket);
                ticket.AddSubHeaderLine("Le atendió: " +
                    Usuario.Instancia().Nombre + " " + Usuario.Instancia().Paterno);
                ticket.AddSubHeaderLine("Fecha y Hora: " +
                    date + " ");
                ticket.AddSubHeaderLine("Cliente: " +
                    nombre);
                ticket.AddSubHeaderLine("          Abono a deuda");
                ticket.AddTotal("","");
                ticket.AddTotal("","");
                
                cantidadPagada = Convert.ToDouble(pagoCon);
                varTOTAL += Convert.ToDouble(totalL);
                varTOTAL += pagoCon;
                restante = varTOTAL - pagoCon ;
                ticket.AddTotal("PENDIENTE", varTOTAL.ToString("$0.00"));
                ticket.AddTotal("RECIBIDO", cantidadPagada.ToString("$0.00"));
                restante = varTOTAL - cantidadPagada;
                if (restante < 0)
                    restante = 0;
                ticket.AddTotal("RESTANTE: ", restante.ToString("$0.00"));
               
                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                //El metodo AddFooterLine funciona igual que la cabecera 
                ticket.AddFooterLine("    *******GRACIAS POR SU PAGO*******");
                ticket.AddFooterLine(" ");
                ticket.AddFooterLine("  --Tostatronic le desea un buen dia--");
                //Generamos
                if (ticket.PrintAbono())
                {
                    PDFFile.Ver(Application.StartupPath + "\\Pago.pdf");
                    return (true);
                }
                else { return false; }
            }
            catch (Exception ex) { throw (ex); }
        }




        public static bool ImprimeTicketN(string prmFolioTicket, List<ProductoCompleto> productos, float pagoCon, float totalL, string date, string nombre, string apellido, float impuesto)
        {
            try
            {
                double varEFECTIVO = 0;
                double varCAMBIO = 0;
                double varTOTAL = 0;
                double varIVA = 0;
                LibPrintTicket.Ticket ticket = new LibPrintTicket.Ticket();
                ticket.MaxChar = 25;
                ticket.MaxCharDescription = 10;
                string ticketRuta;
                if (File.Exists(Application.StartupPath + "\\Resources\\ticketN.png"))
                    ticketRuta = Application.StartupPath + "\\Resources\\ticketN.png";
                else
                    ticketRuta = Application.StartupPath + "\\ticketN.png";
                Image logo = Image.FromFile(ticketRuta);
                ticket.HeaderImage = logo;
                ticket.AddHeaderLine("\n\n");
                ticket.AddHeaderLine(setMiddle("TOSTATRONIC")+ "TOSTATRONIC");
                ticket.AddHeaderLine(setMiddle("Material electronico") + "Material electronico");
                ticket.AddHeaderLine("\n");
                ticket.AddSubHeaderLine("Folio: "+ prmFolioTicket);
                ticket.AddSubHeaderLine("Le atendio: " + Usuario.Instancia().Nombre + " " + Usuario.Instancia().Paterno);
                ticket.AddSubHeaderLine("Fecha: "+date);
                ticket.AddSubHeaderLine("Cliente: " + nombre+" "+ apellido);
                varEFECTIVO = Convert.ToDouble(pagoCon);
                foreach (ProductoCompleto a in productos)
                    ticket.AddItem(a.Cantidad.ToString(), a.Descripcion, (a.Cantidad * a.Precio).ToString("$0.00"));
                varTOTAL += Convert.ToDouble(totalL);
                varIVA +=
                    Convert.ToDouble(varTOTAL * (impuesto - 1));
                varCAMBIO = ((varTOTAL + varIVA) - varEFECTIVO) * -1;
                ticket.AddTotal("SUBTOTAL", varTOTAL.ToString("$0.00"));
                ticket.AddTotal("IVA", varIVA.ToString("$0.00"));
                ticket.AddTotal("TOTAL", (varTOTAL+varIVA).ToString("$0.00"));
                ticket.AddTotal("", "");
                if (varCAMBIO < 0)
                    ticket.AddTotal("Restante: ", (varCAMBIO * -1).ToString("$0.00"));
                else
                    ticket.AddTotal("Cambio: ", varCAMBIO.ToString("$0.00"));
                ticket.AddTotal("", "");

                ticket.AddFooterLine("***Gracias por su compra***");
                ticket.AddFooterLine("Informacion de tienda");
                ticket.AddFooterLine("Cel o whatsapp: 3314575853");

                ticket.PrintTicket(PrinterConfig.getPriterName()); //Nombre de la impresora de tickets
            }
            catch (Exception) { return false; }
            return true;
        }

        private static string setMiddle(string aux)
        {
            int i = (25 - aux.Length) / 2;
            aux = " ";
            for (int j = 1; j < i; j++)
                aux += " ";
            return aux;
        }

        public static bool ImprimeTicketPagoN(string prmFolioTicket, float pagoCon, float totalL, string date, string nombre)
        {
            try
            {
                double cantidadPagada = 0;
                double restante = 0;
                double varTOTAL = 0; 
                LibPrintTicket.Ticket ticket = new LibPrintTicket.Ticket();
                ticket.MaxChar = 25;
                ticket.MaxCharDescription = 10;
                string ticketRuta;
                if (File.Exists(Application.StartupPath + "\\Resources\\ticketN.png"))
                    ticketRuta = Application.StartupPath + "\\Resources\\ticketN.png";
                else
                    ticketRuta = Application.StartupPath + "\\ticketN.png";
                Image logo = Image.FromFile(ticketRuta);
                ticket.HeaderImage = logo;
                ticket.AddHeaderLine(setMiddle("TOSTATRONIC")+"TOSTATRONIC");
                ticket.AddHeaderLine(setMiddle("Componentes electronicos") +"Componentes electronicos");
                ticket.AddSubHeaderLine("\n");
                ticket.AddSubHeaderLine("Folio: " + prmFolioTicket);
                ticket.AddSubHeaderLine("Le atendió: " +
                    Usuario.Instancia().Nombre + " " + Usuario.Instancia().Paterno);
                ticket.AddSubHeaderLine("Fecha y Hora: " +
                    date + " ");
                ticket.AddSubHeaderLine("Cliente: " +
                    nombre);
                ticket.AddSubHeaderLine("Abono a deuda");
                ticket.AddTotal("", "");
                ticket.AddTotal("", "");

                cantidadPagada = Convert.ToDouble(pagoCon);
                varTOTAL += Convert.ToDouble(totalL);
                varTOTAL += pagoCon;
                restante = varTOTAL - pagoCon;
                ticket.AddTotal("PENDIENTE", varTOTAL.ToString("$0.00"));
                ticket.AddTotal("RECIBIDO", cantidadPagada.ToString("$0.00"));
                restante = varTOTAL - cantidadPagada;
                if (restante < 0)
                    restante = 0;
                ticket.AddTotal("RESTANTE: ", restante.ToString("$0.00"));

                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                //El metodo AddFooterLine funciona igual que la cabecera 
                ticket.AddFooterLine(setMiddle("GRACIAS POR SU PAGO")+ "GRACIAS POR SU PAGO");
                ticket.AddFooterLine(" ");
                ticket.AddFooterLine(setMiddle("*****Buen dia*****") +"*****Buen dia*****");
                //Generamos
                ticket.PrintTicket("ZJ-58"); //Nombre de la impresora de tickets
                return true;

            }
            catch (Exception) { return false; }
        }
    }
}
