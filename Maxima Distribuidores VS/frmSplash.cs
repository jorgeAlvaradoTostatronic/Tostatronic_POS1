using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Maxima_Distribuidores_VS
{
    public partial class frmSplash : Form
    {
        private Timer tmrSplash;
        private int tiempo;

        public frmSplash()
        {
            InitializeComponent();
            tmrSplash = new Timer();
            tmrSplash.Tick += new System.EventHandler(this.tmrSplash_Tick);
            tmrSplash.Interval = 100;
            tiempo = 0;
            tmrSplash.Start();
        }

        private void tmrSplash_Tick(object sender, EventArgs e)
        {
            tiempo += tmrSplash.Interval;
            if (tiempo >= 3000)
            {
                tmrSplash.Stop();
                tiempo = 0;

                PrimeroUso();
                this.Hide();
                frmLogin login = new frmLogin();
                login.Show();
            }
        }

        private void PrimeroUso()
        {
            try
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor(true).ShowDialog();

                Iteracion:
                if (!Sql.ConectaBD())
                {
                    dialogBaseDatos dbd = new dialogBaseDatos(Operacion.Importar);
                    dbd.ShowDialog();
                    if (!dbd.Respuesta)
                        goto Iteracion;
                }
            }
            catch (Exception) { }
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            SqlValidacion.Load();
            Sql.Load();
        }
    }
}
