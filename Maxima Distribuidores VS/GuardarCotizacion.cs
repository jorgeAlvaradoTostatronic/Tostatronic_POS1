using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class GuardarCotizacion
    {
        public static void Guardar(InformacionVenta informacionVenta)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InformacionVenta));
            TextWriter textWriter = new StreamWriter(Application.StartupPath + "\\Tostatronic_Cotizacion.xml");
            serializer.Serialize(textWriter, informacionVenta);
            textWriter.Close();
        }

        public static InformacionVenta Leer()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InformacionVenta));
            FileStream fs = new FileStream(Application.StartupPath + "\\Tostatronic_Cotizacion.xml", FileMode.Open);
            TextReader textReader = new StreamReader(fs);
            InformacionVenta informacionVenta = (InformacionVenta)serializer.Deserialize(textReader);
            fs.Close();
            File.Delete(Application.StartupPath + "\\Tostatronic_Cotizacion.xml");
            return informacionVenta;
        }
    }
}
