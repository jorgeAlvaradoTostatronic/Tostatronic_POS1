using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;

namespace Maxima_Distribuidores_VS
{
    public partial class uscVerUsuarios : UserControl
    {
        private Redimension redimension;

        public uscVerUsuarios()
        {
            InitializeComponent();
            redimension = new Redimension(this);
        }

        private void uscVerUsuarios_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }

        private void uscVerUsuarios_Load(object sender, EventArgs e)
        {
            CargarDataGrid();
        }

        private void CargarDataGrid()
        {
            dgvDatos.DataSource = Sql.CrearTablaDatos("SELECT id_usuario, usuario, nombre,"
                + " apellido_paterno, apellido_materno, correo FROM usuarios");
            foreach (DataGridViewColumn item in dgvDatos.Columns)
                if (item is DataGridViewTextBoxColumn)
                {
                    ((DataGridViewTextBoxColumn)item).HeaderText = ((DataGridViewTextBoxColumn)item).HeaderText.ToUpper();
                    ((DataGridViewTextBoxColumn)item).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    ((DataGridViewTextBoxColumn)item).Resizable = DataGridViewTriState.True;
                    ((DataGridViewTextBoxColumn)item).SortMode = DataGridViewColumnSortMode.Automatic;
                    ((DataGridViewTextBoxColumn)item).MaxInputLength = 50;
                    ((DataGridViewTextBoxColumn)item).MinimumWidth = 4;
                }
            dgvDatos.Columns["ID_USUARIO"].ReadOnly = true;
            dgvDatos.Columns["ID_USUARIO"].Visible = false;
            dgvDatos.Columns["USUARIO"].ReadOnly = true;
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            dgvDatos.Columns.Clear();
            CargarDataGrid();
        }

        private void dgvDatos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string campo = dgvDatos.Columns[e.ColumnIndex].HeaderText.ToLower();
            string id = dgvDatos.Rows[e.RowIndex].Cells["ID_USUARIO"].Value.ToString();
            string nuevoValor = dgvDatos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            Sql.InsertarDatos("UPDATE usuarios SET " + campo + " = '" + nuevoValor + "' WHERE id_usuario = '" + id + "'");
        }

        private void dgvDatos_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string id = e.Row.Cells["ID_USUARIO"].Value.ToString();
            Usuario user = Usuario.Instancia();
            if (int.Parse(id) == user.Id)
            {
                MessageBox.Show("No puedes eliminar tu propio usuario");
                e.Cancel = true;
            }
            else if (int.Parse(id) == 0)
            {
                MessageBox.Show("El usuario Administrador General no puede ser eliminado");
                e.Cancel = true;
            }
            else
            {
                DialogResult dr = MessageBox.Show("¿Estas seguro de que deseas eliminar al usuario?", "Confirmacion", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    dgvDatos.Rows.Remove(e.Row);
                    Sql.InsertarDatos("DELETE FROM usuarios WHERE id_usuario = '" + id + "'");
                }
                else
                    e.Cancel = true;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedRows.Count == 1)
                dgvDatos_UserDeletingRow(sender, new DataGridViewRowCancelEventArgs(dgvDatos.SelectedRows[0]));
            else
                MessageBox.Show("Debes seleccionar solo una fila", "Operacion cancelada", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
        }

    }
}
