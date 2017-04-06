using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;

namespace Maxima_Distribuidores_VS
{
    public partial class frmValidacion : Form
    {
        public frmValidacion()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtSerie.Text))
            {
                if (SqlValidacion.Existe("SELECT id_serial FROM serialesdisponibles WHERE serial = '" + txtSerie.Text + "'"))
                {
                    SqlValidacion.InsertarDatos("DELETE FROM serialesdisponibles WHERE serial = '" + txtSerie.Text + "'");
                    NetworkInterface[] nic = NetworkInterface.GetAllNetworkInterfaces();
                    string mac = nic[0].GetPhysicalAddress().ToString();
                    SqlValidacion.InsertarDatos("INSERT INTO serialesusados VALUES (NULL,'" + txtSerie.Text + "', '" + mac + "')");
                    MessageBox.Show(this, "Gracias por su registro", "Registro de programa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    frmLogin login = new frmLogin();
                    login.Show();
                }
                else
                    MessageBox.Show(this, "Serial no valido", "Error de verificación", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                MessageBox.Show(this, "Debes ingresar un numero de serie", "Número de serie", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmValidacion_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
                
        private void txtSerie_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
