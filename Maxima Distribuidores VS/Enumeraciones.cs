using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public enum Accion 
    { 
        Agregar, 
        Modificar, 
        Eliminar
    }

    public enum TipoProducto
    {
        Publico,
        Distribuidor
    }

    public enum Operacion
    { 
        Importar,
        Exportar
    }

    public enum TipoInvoice
    {
        Venta,
        Cotizacions
    }
}
