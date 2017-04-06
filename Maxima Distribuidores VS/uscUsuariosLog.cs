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
    public partial class uscUsuariosLog : UserControl
    {
        private Redimension redimension;

        public uscUsuariosLog()
        {
            InitializeComponent();
            redimension = new Redimension(this);
            cmbUsuarios.SelectedIndex = 0;
            List<string[]> lista = Sql.BuscarDatos("SELECT usuario FROM usuarios");
            for (int i = 0; i < lista.Count; i++)
                cmbUsuarios.Items.Add(lista[i][0]);
        }

        private void CargarRegistro() 
        {
            if (cmbUsuarios.Text != "Todos")
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT * FROM log WHERE usuario = '" +
                    cmbUsuarios.Text + "'ORDER BY id_log DESC");
                for (int i = 0; i < lista.Count; i++)
                    lista[i][3] = lista[i][3].Substring(0, 10);
                for (int i = 0; i < lista.Count; i++)
                    lvLog.Items.Add(new ListViewItem(lista[i]));
            }
            else
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT * FROM log ORDER BY id_log DESC");
                for (int i = 0; i < lista.Count; i++)
                    lista[i][3] = lista[i][3].Substring(0, 10);
                for (int i = 0; i < lista.Count; i++)
                    lvLog.Items.Add(new ListViewItem(lista[i]));
            }            
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
            CargarRegistro();
        }

        private void uscUsuariosLog_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void cmbUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
            CargarRegistro();
        }
    }
}
