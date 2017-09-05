using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    class tempClass
    {
        public static void generaNombres()
        {
            using (var tw = new StreamWriter("info.txt", true))
            {
                List<string[]> productos = Sql.BuscarDatos("SELECT  imagen FROM productos WHERE eliminado=0;");
                foreach (string[] a in productos)
                {
                    tw.WriteLine("http://tostratonic.com/Imagenes/"+a[0]);
                }
                tw.Close();
            }
        }
    }
}
