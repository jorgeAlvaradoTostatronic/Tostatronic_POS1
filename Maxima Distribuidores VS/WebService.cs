using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    class WebService
    {
        public static string ActualizaExistencias()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, existencia, precio_publico FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoWebService p = new ProductoWebService();
            List<ProductoWebService> pr = new List<ProductoWebService>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Cantidad = float.Parse(a[1]);
                p.PrecioPublico = float.Parse(a[2]);
                pr.Add(p);
            }
            string json = JsonConvert.SerializeObject(pr);
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/") as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            using (StreamWriter writes = new StreamWriter(request.GetRequestStream()))
            {
                writes.Write(json);
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            string json1 = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                    json1 += reader.ReadLine();
            }
            return json1;
        }
    }
}
