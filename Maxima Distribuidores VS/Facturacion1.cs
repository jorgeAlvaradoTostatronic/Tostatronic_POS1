using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using XSDToXML.Utils;

namespace Maxima_Distribuidores_VS
{
    class Facturacion
    {
        static private string path =Directory.GetCurrentDirectory();
        static string pathXML = path + @"/miPrimerXML.xml";
        static string pathPDF = path + @"/miPrimerPDF.pdf";
        string realXMLName = path;
        string realPDFName = path;
        public static bool Certificado()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Certificado (*.cer)|*.cer";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                try
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string root = Directory.GetCurrentDirectory() + @"/Fiel";
                        var fileName = openFileDialog.FileName;
                        if (!Directory.Exists(root))
                        {
                            Directory.CreateDirectory(root);
                        }
                        System.IO.File.Copy(fileName, root + "/Certifiado.cer",true);
                    }
                    return true;
                }
                catch(Exception)
                {
                }
                return false;
            }
        }

        public static bool Llave()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Llave (*.key)|*.key";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                try
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string root = Directory.GetCurrentDirectory() + @"/Fiel";
                        var fileName = openFileDialog.FileName;
                        if (!Directory.Exists(root))
                        {
                            Directory.CreateDirectory(root);
                        }
                        System.IO.File.Copy(fileName, root + "/Key.key",true);
                    }
                    return true;
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        public static void CreaFactura()
        {
            string pathCer = Directory.GetCurrentDirectory() + @"/Fiel/Certifiado.cer";
            string pathKey = Directory.GetCurrentDirectory() + @"/Fiel/Key.key";
            string clavePrivada = "12345678a";

            //Obtenemos el numero
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(pathCer, out aa, out b, out c, out numeroCertificado);


            //Llenamos la clase COMPROBANTE--------------------------------------------------------
            Comprobante oComprobante = new Comprobante();
            oComprobante.Version = "3.3";
            oComprobante.Serie = "H";
            oComprobante.Folio = "1";
            oComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            // oComprobante.Sello = "faltante"; //sig video
            oComprobante.FormaPago = "1";
            oComprobante.NoCertificado = numeroCertificado;
            // oComprobante.Certificado = ""; //sig video
            oComprobante.SubTotal = 10m;
            oComprobante.Moneda = "MXN";
            oComprobante.Total = 10;
            oComprobante.TipoDeComprobante = "I";
            oComprobante.MetodoPago = "PUE";
            oComprobante.LugarExpedicion = "44860";



            ComprobanteEmisor oEmisor = new ComprobanteEmisor();

            oEmisor.Rfc = "AATJ9502061EA";
            oEmisor.Nombre = "Jorge Humberto Alvarado Tostado";
            oEmisor.RegimenFiscal = "612";

            ComprobanteReceptor oReceptor = new ComprobanteReceptor();
            oReceptor.Nombre = "Pepe SA DE CV";
            oReceptor.Rfc = "BIO091204LB1";
            oReceptor.UsoCFDI = "G03";

            //asigno emisor y receptor
            oComprobante.Emisor = oEmisor;
            oComprobante.Receptor = oReceptor;


            List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();
            ComprobanteConcepto oConcepto = new ComprobanteConcepto();
            oConcepto.Importe = 10m;
            oConcepto.ClaveProdServ = "92111704";
            oConcepto.Cantidad = 1;
            oConcepto.ClaveUnidad = "H87";
            oConcepto.Descripcion = "Un misil para la guerra";
            oConcepto.ValorUnitario = 10m;


            lstConceptos.Add(oConcepto);

            oComprobante.Conceptos = lstConceptos.ToArray();


            //Creamos el xml
            CreateXML(oComprobante);

            string cadenaOriginal = "";
            string pathxsl = Directory.GetCurrentDirectory() + @"/Fiel/cadenaoriginal_3_3.xslt";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(pathxsl);

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {

                transformador.Transform(pathXML, xwo);
                cadenaOriginal = sw.ToString();
            }


            SelloDigital oSelloDigital = new SelloDigital();
            oComprobante.Certificado = oSelloDigital.Certificado(pathCer);
            oComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, pathKey, clavePrivada);

            CreateXML(oComprobante);

            ////TIMBRE DEL XML
            ServiceReferenceFC.RespuestaCFDi respuestaCFDI = new ServiceReferenceFC.RespuestaCFDi();

            byte[] bXML = System.IO.File.ReadAllBytes(pathXML);

            ServiceReferenceFC.TimbradoClient oTimbrado = new ServiceReferenceFC.TimbradoClient();

            respuestaCFDI = oTimbrado.TimbrarTest("TEST010101ST1", "a", bXML);

            if (respuestaCFDI.Documento == null)
            {
                Console.WriteLine(respuestaCFDI.Mensaje);
            }
            else
            {

                System.IO.File.WriteAllBytes(pathXML, respuestaCFDI.Documento);
            }
        }

        public static string CreaFactura(string folio, string formaPago, string metodoDePago, List<ProductoSat> productos, float subtotal, string rfc, string rz, string usoCFDI, string mail)
        {
            string pathCer = Directory.GetCurrentDirectory() + @"/Fiel/Certifiado.cer";
            string pathKey = Directory.GetCurrentDirectory() + @"/Fiel/Key.key";
            string clavePrivada = "Jorge1995";

            //Obtenemos el numero
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(pathCer, out aa, out b, out c, out numeroCertificado);


            //Llenamos la clase COMPROBANTE--------------------------------------------------------
            string subt = subtotal.ToString();
            string impuetosImporte = (subtotal * 0.16).ToString();
            float t = subtotal * 1.16f;
            string ts = t.ToString();
            Comprobante oComprobante = new Comprobante();
            oComprobante.Version = "3.3";
            oComprobante.Serie = "H";
            oComprobante.Folio = folio;
            oComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            //oComprobante.Fecha = "2019-06-11T10:52:20";
            //oComprobante.Sello = "faltante"; //sig video
            oComprobante.FormaPago = formaPago;
            oComprobante.NoCertificado = numeroCertificado;
            // oComprobante.Certificado = ""; //sig video
            oComprobante.SubTotal = decimal.Parse(subt);
            oComprobante.Moneda = "MXN";
            oComprobante.Total = decimal.Parse(ts);
            oComprobante.TipoDeComprobante = "I";
            oComprobante.MetodoPago = "PUE";
            oComprobante.LugarExpedicion = "44860";



            ComprobanteEmisor oEmisor = new ComprobanteEmisor();

            oEmisor.Rfc = "AATJ9502061EA";
            oEmisor.Nombre = "Jorge Humberto Alvarado Tostado";
            oEmisor.RegimenFiscal = "612";

            ComprobanteReceptor oReceptor = new ComprobanteReceptor();
            oReceptor.Nombre = rz;
            oReceptor.Rfc = rfc;
            oReceptor.UsoCFDI = usoCFDI;

            //asigno emisor y receptor
            oComprobante.Emisor = oEmisor;
            oComprobante.Receptor = oReceptor;


            List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();
            ComprobanteConcepto oConcepto;
            ComprobanteConceptoImpuestos impuestos;
            ComprobanteConceptoImpuestosTraslado imAux;
            ComprobanteConceptoImpuestosTraslado[] impuestosTrasladados;

            foreach (ProductoSat a in productos)
            {
                oConcepto = new ComprobanteConcepto();
                impuestos = new ComprobanteConceptoImpuestos();
                imAux = new ComprobanteConceptoImpuestosTraslado();
                impuestosTrasladados = new ComprobanteConceptoImpuestosTraslado[1];
                oConcepto.Importe = Math.Round(decimal.Parse((a.Subtotal).ToString()),3);
                oConcepto.ClaveProdServ = a.CodigoSAT;
                oConcepto.Cantidad = decimal.Parse(a.Cantidad.ToString());
                oConcepto.ClaveUnidad = "H87";
                oConcepto.Descripcion = a.Descripcion;
                oConcepto.ValorUnitario = decimal.Parse((a.Precio).ToString());
                //Impuestos
                imAux.Base= decimal.Parse(a.Subtotal.ToString());
                imAux.ImporteSpecified = true;
                imAux.TasaOCuotaSpecified = true;
                imAux.TipoFactor = "Tasa";
                imAux.Importe = Math.Round(decimal.Parse((a.Subtotal * 0.16).ToString()),3);
                imAux.TasaOCuota = decimal.Parse("0.160000");
                imAux.Impuesto = "002";
                impuestosTrasladados[0] = imAux;
                impuestos.Traslados = impuestosTrasladados;
                oConcepto.Impuestos = impuestos;
                lstConceptos.Add(oConcepto);
            }
            oComprobante.Conceptos = lstConceptos.ToArray();

            ComprobanteImpuestos imComprobante = new ComprobanteImpuestos();
            ComprobanteImpuestosTraslado imComprobanteTraladados = new ComprobanteImpuestosTraslado();
            ComprobanteImpuestosTraslado[] imComprobanteTraladadosArray = new ComprobanteImpuestosTraslado[1];

            imComprobanteTraladados.TipoFactor = "Tasa";
            imComprobanteTraladados.TasaOCuota = decimal.Parse("0.160000");
            imComprobanteTraladados.Impuesto = "002";
            imComprobanteTraladados.Importe = Math.Round(decimal.Parse(impuetosImporte),3);

            imComprobanteTraladadosArray[0] = imComprobanteTraladados;

            imComprobante.Traslados = imComprobanteTraladadosArray;

            imComprobante.TotalImpuestosTrasladadosSpecified = true;
            imComprobante.TotalImpuestosTrasladados= Math.Round(decimal.Parse(impuetosImporte),2);
            oComprobante.Impuestos = imComprobante;



            //Creamos el xml
            CreateXML(oComprobante);

            string cadenaOriginal = "";
            string pathxsl = Directory.GetCurrentDirectory() + @"/Fiel/cadenaoriginal_3_3.xslt";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(pathxsl);

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {

                transformador.Transform(pathXML, xwo);
                cadenaOriginal = sw.ToString();
            }


            SelloDigital oSelloDigital = new SelloDigital();
            oComprobante.Certificado = oSelloDigital.Certificado(pathCer);
            oComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, pathKey, clavePrivada);

            CreateXML(oComprobante);

            ////TIMBRE DEL XML
            ServiceReferenceFC.RespuestaCFDi respuestaCFDI = new ServiceReferenceFC.RespuestaCFDi();

            byte[] bXML = System.IO.File.ReadAllBytes(pathXML);

            ServiceReferenceFC.TimbradoClient oTimbrado = new ServiceReferenceFC.TimbradoClient();

            respuestaCFDI = oTimbrado.Timbrar("AATJ9502061EA", "827984aaddd4126c9c67", bXML);

            if (respuestaCFDI.Documento == null)
            {
                return respuestaCFDI.Mensaje;
            }
            else
            {
                System.IO.File.WriteAllBytes(pathXML, respuestaCFDI.Documento);
                ServiceReferenceFC.TimbradoClient pdf = new ServiceReferenceFC.TimbradoClient();
                bXML = System.IO.File.ReadAllBytes(pathXML);
                respuestaCFDI=pdf.PDF("AATJ9502061EA", "827984aaddd4126c9c67", bXML,null); 
                System.IO.File.WriteAllBytes(pathPDF, respuestaCFDI.Documento);
                Sql.InsertarFactura(folio, bXML);
                Sql.InsertarDatos("UPDATE  `venta` SET `facturada`= 1 WHERE id_venta=" + folio);
                string pXMl = @path +"\\"+ folio + ".xml";
                string pPDF = @path + "\\" + folio + ".pdf";
                File.Move(pathXML, pXMl);
                File.Move(pathPDF, pPDF);
                try
                {
                    Email(mail,pXMl,pPDF);
                }
                catch(Exception e)
                {
                    return "Error: " + e.Message;
                }

                File.Delete(pXMl);
                File.Delete(pPDF);
            }
            return "";
        }

        private static void CreateXML(Comprobante oComprobante)
        {
            //SERIALIZAMOS.-------------------------------------------------

            XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");


            XmlSerializer oXmlSerializar = new XmlSerializer(typeof(Comprobante));

            string sXml = "";

            using (var sww = new StringWriterWithEncoding(Encoding.UTF8))
            {

                using (XmlWriter writter = XmlWriter.Create(sww))
                {

                    oXmlSerializar.Serialize(writter, oComprobante, xmlNameSpace);
                    sXml = sww.ToString();
                }

            }

            //guardamos el string en un archivo
            System.IO.File.WriteAllText(pathXML, sXml);
        }

        private static void Email(string email, string pXMl, string pPDF)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.dreamhost.com");
            mail.From = new MailAddress("jorge.alvarado@tostatronic.com");
            mail.To.Add(email);
            mail.Subject = "Factura Tostatronic";
            mail.Body = "Factura realizada por Tostatronic Software Desing";

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(pXMl);
            mail.Attachments.Add(attachment);
            attachment = new System.Net.Mail.Attachment(pPDF);
            mail.Attachments.Add(attachment);


            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("jorge.alvarado@tostatronic.com", "Jorge1995");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            mail.Dispose();
        }
    }
}
