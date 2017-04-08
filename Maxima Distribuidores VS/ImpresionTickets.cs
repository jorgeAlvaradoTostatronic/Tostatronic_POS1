using System;
using System.Collections.Generic;
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
            catch (Exception ex) { throw (ex); }
        } 
    }
}
