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
    static class GuardarServidor
    {
        public static void Guardar(InformacionServidor infoServer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(InformacionServidor));
            TextWriter textWriter = new StreamWriter(Application.StartupPath + "\\server.xml");
            serializer.Serialize(textWriter, infoServer);
            textWriter.Close();
        }

        public static InformacionServidor Leer()
        {
            InformacionServidor info;
            if (File.Exists(Application.StartupPath + "\\server.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InformacionServidor));
                FileStream fs = new FileStream(Application.StartupPath + "\\server.xml", FileMode.Open);
                TextReader textReader = new StreamReader(fs);
                info =  (InformacionServidor)serializer.Deserialize(textReader);
                textReader.Close();
            }
            else
                info = new InformacionServidor(new Servidor("localhost", Encriptacion.Encriptar(""), "root", 80));
            return info;
        }
    }
}
