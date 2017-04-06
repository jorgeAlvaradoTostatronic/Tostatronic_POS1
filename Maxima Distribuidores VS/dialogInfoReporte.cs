using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public partial class dialogInfoReporte : Form
    {
        public dialogInfoReporte()
        {
            InitializeComponent();
            InfoReporte info = GuardarInfoReporte.Leer();
            txtRFC.Text = info.Reporte.rfc;
            txtDireccion.Text = info.Reporte.direccion;
            txtTelefono.Text = info.Reporte.telefono;
            txtLeyenda.Text = info.Reporte.leyenda;
            txtDatosBancarios.Text = info.Reporte.banco;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtRFC.Text))
                MessageBox.Show("El RFC no puede estar en blanco", "Operacion cancelada",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if(String.IsNullOrWhiteSpace(txtTelefono.Text))
                MessageBox.Show("El telefono no puede estar en blanco", "Operacion cancelada",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (String.IsNullOrWhiteSpace(txtDireccion.Text))
                MessageBox.Show("La direccion no puede estar en blanco", "Operacion cancelada",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else 
            {
                GuardarInfoReporte.Guardar(new InfoReporte(new Reporte(txtRFC.Text, txtDireccion.Text, txtTelefono.Text, txtLeyenda.Text, txtDatosBancarios.Text)));
                MessageBox.Show("Se guardaron cambios con exito", "Operacion concretadas",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
