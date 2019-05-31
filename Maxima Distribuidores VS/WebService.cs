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
        public static string ActualizaProductosApp()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT nombre,codigo,existencia,precio_publico, imagen FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoWebServiceApp p = new ProductoWebServiceApp();
            List<ProductoWebServiceApp> pr = new List<ProductoWebServiceApp>();
            foreach (string[] a in productos)
            {
                p.Nombre = a[0];
                p.Codigo = a[1];
                p.Cantidad = float.Parse(a[2]);
                p.PrecioPublico = float.Parse(a[3]);
                p.Imagen = a[4];
                pr.Add(p);
            }
            string json = JsonConvert.SerializeObject(pr);
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/appWeb.php") as HttpWebRequest;
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
        public static string PromocionDistribuidor()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, existencia, precio_distribuidor FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
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
        public static string[] GetNoInWebPage()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, nombre FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoNoInWebPage p = new ProductoNoInWebPage();
            List<ProductoNoInWebPage> pr = new List<ProductoNoInWebPage>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Nombre = a[1];
                pr.Add(p);
            }
            string json = JsonConvert.SerializeObject(pr);
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/noWeb.php") as HttpWebRequest;
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
            string[] stringSeparators = new string[] { "\\n" };
            string[] result = json1.Split(stringSeparators,
                           StringSplitOptions.RemoveEmptyEntries);
            return result;
        }
        public static string[] GetGoogleSheet()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo, nombre FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoNoInWebPage p = new ProductoNoInWebPage();
            List<ProductoNoInWebPage> pr = new List<ProductoNoInWebPage>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                p.Nombre = a[1];
                pr.Add(p);
            }
            HttpWebRequest request = WebRequest.Create("http://tostatronic.com/store/newwebservice/p.php") as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            string json1 = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                    json1 += reader.ReadLine();
            }
            string[] stringSeparators = new string[] { "\\n" };
            string[] result = json1.Split(stringSeparators,
                           StringSplitOptions.RemoveEmptyEntries);
            return result;
        }
        public static string[] UpdateDescriptionApp()
        {
            List<string[]> productos = Sql.BuscarDatos("SELECT codigo FROM productos WHERE eliminado=0 ORDER BY nombre ASC;");
            ProductoNoInWebPage p = new ProductoNoInWebPage();
            List<ProductoNoInWebPage> pr = new List<ProductoNoInWebPage>();
            foreach (string[] a in productos)
            {
                p.Codigo = a[0];
                pr.Add(p);
            }
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/desApp.php") as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            string json1 = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                    json1 += reader.ReadLine();
            }
            string[] stringSeparators = new string[] { "\\n" };
            string[] result = json1.Split(stringSeparators,
                           StringSplitOptions.RemoveEmptyEntries);
            return result;
        }
        public static string UpdateDescriptionApp2(List<ProductoCompleto> p)
        {
            string json = JsonConvert.SerializeObject(p);
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/updateDes.php") as HttpWebRequest;
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
        public static string UpdatePriceApp(List<ProductoCompleto> p)
        {
            string json = JsonConvert.SerializeObject(p);
            HttpWebRequest request = WebRequest.Create("http://tostratonic.com/store/newwebservice/updatePriceApp.php") as HttpWebRequest;
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
