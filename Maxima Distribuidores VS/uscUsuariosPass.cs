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
    public partial class uscUsuariosPass : UserControl
    {

        private Redimension redimension;

        public uscUsuariosPass()
        {
            InitializeComponent();
            redimension = new Redimension(this);
        }

        private void uscUsuariosPass_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void uscUsuariosPass_Load(object sender, EventArgs e)
        {
            lblDisplayUsuario.Text = Usuario.Instancia().User;
            lblDisplayNombre.Text = Usuario.Instancia().Nombre;
            lblPaterno.Text = Usuario.Instancia().Paterno;
            lblMaterno.Text = Usuario.Instancia().Materno;
            lblDisplayCorreo.Text = Usuario.Instancia().Correo;
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            if (txtConfirmacion.Text == txtPassword.Text)
            {
                Sql.InsertarDatos("UPDATE usuarios SET password = '" + Encriptacion.Encriptar(txtPassword.Text) + "' WHERE id_usuario = '" +
                    Usuario.Instancia().Id + "'");
                MessageBox.Show("Password cambiado con exito", "Operacion concreatada", MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                txtPassword.Text = String.Empty;
                txtConfirmacion.Text = String.Empty;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtPassword.Text = String.Empty;
            txtConfirmacion.Text = String.Empty;
        }
    }
}
