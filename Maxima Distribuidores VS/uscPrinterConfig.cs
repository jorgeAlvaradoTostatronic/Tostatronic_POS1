using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Maxima_Distribuidores_VS
{
    public partial class uscPrinterConfig : UserControl
    {
        public uscPrinterConfig()
        {
            InitializeComponent();
            loadPrinters();
        }
        private void loadPrinters()
        {
            foreach(string a in PrinterConfig.getPrinters())
                cmbPrinters.Items.Add(a);
            if(cmbPrinters.Items.Count==0)
            {
                MessageBox.Show("No tienen ninguna impresora instalada.\nPara configurar este aspecto es indispensable contar\nminimo con una impresora instalada","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                btnCancelar.Enabled = false;
                btnGuardar.Enabled = false;
                return;
            }
            if (PrinterConfig.getPriterName().Equals(" "))
                cmbPrinters.SelectedIndex = 0;
            else
            {
                try
                {
                    cmbPrinters.SelectedItem = PrinterConfig.getPriterName();
                }
                catch(Exception)
                {
                    MessageBox.Show("La impresora almacenada en su configuracion actual no se encuentra disponible.\n Favor de cambiarla", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbPrinters.SelectedIndex = 0;
                }
            }
        }

        private void brnGuardar_Click(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\printerConfig.xml"))
                File.Delete(Application.StartupPath + "\\printerConfig.xml");
            PrinterConfig.setDefaultThermalPrinter(cmbPrinters.SelectedItem.ToString());
            MessageBox.Show("Configuracion cambiada con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
