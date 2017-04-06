using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public struct Reporte
    {
        public string rfc, direccion, telefono, leyenda, banco;

        public Reporte(string rfc, string direccion, string telefono, string leyenda, string banco)
        {
            this.rfc = rfc;
            this.direccion = direccion;
            this.telefono = telefono;
            this.leyenda = leyenda;
            this.banco = banco;
        }
    }

    public class InfoReporte
    {
        public Reporte Reporte { get; set; }

        public InfoReporte() { }

        public InfoReporte(Reporte reporte)
        {
            Reporte = reporte;
        }
    }
}
