using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public class Impresora
    {
        public string Nombre;
        public Impresora() { }
        public Impresora(string nombre)
        {
            Nombre = nombre;
        }
    }
    public class InformacionImpresora
    {
        public Impresora Printer { get; set; }

        public InformacionImpresora() { }

        public InformacionImpresora(Impresora printer)
        {
            Printer = printer;
        }
    }
}
