using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

namespace Maxima_Distribuidores_VS
{
    public partial class dialogServidor : Form
    {
        private bool forzarServidor;

        public dialogServidor()
        {
            InitializeComponent();
            LeerServidor();
            forzarServidor = false;
        }

        public dialogServidor(bool forzarServidor)
        {
            InitializeComponent();
            LeerServidor();
            this.forzarServidor = forzarServidor;
            if (forzarServidor)
                this.ControlBox = false;
        }

        private void LeerServidor()
        {
            InformacionServidor info = GuardarServidor.Leer();
            txtServidor.Text = info.Servidor.Ip;
            txtUsuario.Text = info.Servidor.User;
            txtPass.Text = Encriptacion.Desencriptar(info.Servidor.Password);
            txtPuerto.Text = info.Servidor.Puerto.ToString();
        }

        private void txtPuerto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            { e.Handled = false; }
        }

        private void txtServidor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsWhiteSpace(e.KeyChar) || Char.IsSymbol(e.KeyChar)){ 
                e.Handled = false; 
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            InformacionServidor info = GuardarServidor.Leer();
            if (forzarServidor)
                MessageBox.Show("La configuración actual no establece comunicación con el servidor.\n" + 
                    "Introduzca una configuración valida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtServidor.Text))
                MessageBox.Show("El servidor no puede estar vacío.", "Advertencia", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            else if (String.IsNullOrWhiteSpace(txtUsuario.Text))
                MessageBox.Show("El  usuario no puede estar vacío.", "Advertencia", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            else if(String.IsNullOrWhiteSpace(txtPuerto.Text))
                MessageBox.Show("El puerto no puede estar vacío.", "Advertencia", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            else
            {
                DialogResult dr = MessageBox.Show("Si guarda los cambios se perdera la información del servidor anterior.\n" + 
                    "¿Deseas continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    InformacionServidor respaldoPorFalla = GuardarServidor.Leer();

                    try
                    {
                        GuardarServidor.Guardar(new InformacionServidor(new Servidor(txtServidor.Text, Encriptacion.Encriptar(txtPass.Text),
                            txtUsuario.Text, int.Parse(txtPuerto.Text))));
                        Cargar();
                    }
                    catch (Exception) { }

                    if (Sql.ConectaServidor())
                    {
                        MessageBox.Show("El servidor se cambio con éxito.", "Operación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        try
                        {
                            GuardarServidor.Guardar(respaldoPorFalla);
                            Cargar();
                        }
                        catch (Exception) { }
                        MessageBox.Show("Esta nueva configuración no establece comunicación con el servidor.\n" +
                            "Introduzca una configuración valida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (!forzarServidor)
                        this.Close();
                }
            }
        }

        private void Cargar()
        {
            Sql.Load();
            SqlValidacion.Load();
        }
    }
}
