using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    class Facturacion
    {
        public static bool Certificado()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Certificado (*.cer)";
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
                        System.IO.File.Copy(fileName, root + "/Certifiado.cer");
                    }
                    return true;
                }
                catch(Exception)
                {
                }
                return false;
            }
        }
    }
}
