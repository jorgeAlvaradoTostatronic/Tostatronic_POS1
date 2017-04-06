using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public partial class uscBuscarCliente : UserControl
    {
        private Redimension redimension;

        public uscBuscarCliente()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            txtApellidoMaterno.Enabled = false;
            txtApellidoPaterno.Enabled = false;
            txtDireccion.Enabled = false;
            txtTelefono.Enabled = false;
            txtCelular.Enabled = false;
            rdbDistribuidor.Enabled = false;
            rdbPublico.Enabled = false;
        }

        private void txtRfc_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Buscando");
            }
        }

        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Buscando");
            }
        }

        private void uscBuscarCliente_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }
    }
}
