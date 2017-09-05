using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Maxima_Distribuidores_VS
{
    class PrinterConfig
    {
        public static List<string> getPrinters()
        {
            List<string> printesr = new List<string>(); 
            foreach(string name in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                printesr.Add(name);
            }
            return printesr;
        }
       public static void setDefaultThermalPrinter(string printerName)
        {
            Impresora pr = new Impresora(printerName);
            InformacionImpresora info = new InformacionImpresora(pr);
            XmlSerializer serializer = new XmlSerializer(typeof(InformacionImpresora));
            TextWriter textWriter = new StreamWriter(Application.StartupPath + "\\pc.xml");
            serializer.Serialize(textWriter, info);
            textWriter.Close();
        }

        public static string getPriterName()
        {
            Impresora name = new Impresora(" ");
            InformacionImpresora ip;
            if (File.Exists(Application.StartupPath + "\\pc.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InformacionImpresora));
                FileStream fs = new FileStream(Application.StartupPath + "\\pc.xml", FileMode.Open);
                TextReader textReader = new StreamReader(fs);
                ip = (InformacionImpresora)serializer.Deserialize(textReader);
                fs.Close();
                name.Nombre = ip.Printer.Nombre;
            }
            return name.Nombre;
        }
    }
}
