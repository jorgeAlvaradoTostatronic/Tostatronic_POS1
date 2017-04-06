using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class GuardarInfoReporte
    {
        public static void Guardar(InfoReporte infoServer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InfoReporte));
            TextWriter textWriter = new StreamWriter(Application.StartupPath + "\\reporte.xml");
            serializer.Serialize(textWriter, infoServer);
            textWriter.Close();
        }

        public static InfoReporte Leer()
        {
            InfoReporte info;
            if (File.Exists(Application.StartupPath + "\\reporte.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InfoReporte));
                FileStream fs = new FileStream(Application.StartupPath + "\\reporte.xml", FileMode.Open);
                TextReader textReader = new StreamReader(fs);
                info = (InfoReporte)serializer.Deserialize(textReader);
                textReader.Close();
            }
            else
                info = new InfoReporte(new Reporte(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty));
            return info;
        }
    }
}
