using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Maxima_Distribuidores_VS
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            List<string[]> lista = Sql.BuscarDatos("SELECT * FROM usuarios WHERE usuario = '" + cmbUsuarios.Text + "'");
            if (lista.Count != 0)
            {
                if (Encriptacion.Encriptar(txtPassword.Text) == lista[0][2])
                {
                    Usuario.CrearInstancia(int.Parse(lista[0][0]), lista[0][1], lista[0][3], lista[0][2],
                        lista[0][4], lista[0][5], lista[0][6], lista[0][7]);
                    this.Hide();
                    frmPrincipal principal = new frmPrincipal();
                    principal.Show();
                }
                else
                {
                    MessageBox.Show("El password no es correcto, intenta de nuevo", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            List<string[]> lista = Sql.BuscarDatos("SELECT usuario FROM usuarios");
            for (int i = 0; i < lista.Count; i++) 
                cmbUsuarios.Items.Add(lista[i][0]);
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\reportes\\");
            }
            catch (Exception)
            { }
        }

        private void cmbUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsuarios.Text != String.Empty)
                btnEntrar.Enabled = true;
            else
                btnEntrar.Enabled = false;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnEntrar_Click(sender, new EventArgs());
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cmbUsuarios.Items.Clear();
            List<string[]> lista = Sql.BuscarDatos("SELECT usuario FROM usuarios");
            for (int i = 0; i < lista.Count; i++)
                cmbUsuarios.Items.Add(lista[i][0]);
            if (lista.Count > 0)
                MessageBox.Show("Se ha refrescado con exito.\nEscriba su usuario y contraseña para ingresar.", "Conexion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            new dialogServidor().ShowDialog();
        }
    }
}
