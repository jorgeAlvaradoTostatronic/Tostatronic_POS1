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
    public partial class uscImages : UserControl
    {
        public uscImages()
        {
            InitializeComponent();
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }
        private void Buscador()
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista = Sql.BuscarDatos("SELECT codigo, nombre, imagen FROM productos WHERE (codigo LIKE '%" + txtBusqueda.Text +
                "%' OR nombre LIKE '%" + txtBusqueda.Text + "%') AND eliminado  = 0");

            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
        }

        private String CargaImagen()
        {
            String name = "No_image.png";
            if (!txtImageName.Text.Equals("Seleccionar imagen..."))
            {
                try
                {
                    name = dgvProductos.Rows[dgvProductos.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    name += Path.GetExtension(txtImageName.Text);
                    File.Copy(txtImageName.Text, @"Imagenes\" + name,true);
                    Sql.InsertarDatos("UPDATE productos SET imagen  = '"+name+"' WHERE codigo = '" + dgvProductos.Rows[dgvProductos.CurrentCell.RowIndex].Cells[0].Value.ToString() + "'");
                }
                catch (IOException e)
                {
                    MessageBox.Show("La imagen no pudo ser cargada al sistema\n" + e.Message, "Error al cargar imagen", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    name = "No_image.png";
                }
            }
            return name;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofdImagen.ShowDialog() == DialogResult.OK)
                {
                    string imagen = ofdImagen.FileName;
                    txtImageName.Text = imagen;
                    pcbImagen.Image.Dispose();
                    pcbImagen.Image = Image.FromFile(imagen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
        }

        private void brnGuardar_Click(object sender, EventArgs e)
        {
            if(!txtImageName.Text.Equals("Seleccionar imagen..."))
            {
                dgvProductos.Rows[dgvProductos.CurrentCell.RowIndex].Cells[2].Value = CargaImagen();
                txtImageName.Text = "Seleccionar imagen...";
                MessageBox.Show("Imagen cambiada correctamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Buscador();
        }

        private void dgvProductos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            txtImageName.Text = "Seleccionar imagen...";
            String imageName = dgvProductos.Rows[RowIndex()].Cells[2].Value.ToString();
            pcbImagen.Image.Dispose();
            pcbImagen.Image = Image.FromFile(@"Imagenes\" + imageName);
        }

        int RowIndex()
        {
            return dgvProductos.CurrentCell.RowIndex;
        }
    }
}
