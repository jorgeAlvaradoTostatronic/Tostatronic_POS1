using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql;

namespace Maxima_Distribuidores_VS
{
    public partial class dialogBaseDatos : Form
    {

        private string ruta;
        private int maximo, _maximo;
        private int curBytes, totalBytes;
        Operacion operacion;
        public bool Respuesta;
        Timer timer;

        public dialogBaseDatos(Operacion operacion)
        {
            InitializeComponent();
            this.operacion = operacion;
        }

        private void Cargar()
        {
            timer = new Timer();
            timer.Interval = 50;
            switch (operacion)
            {
                case Operacion.Importar:
                    curBytes = 0;
                    totalBytes = 0;
                    Importar();
                    bgwWorker.DoWork += new DoWorkEventHandler(bgwWorkerImportar_DoWork);
                    bgwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorkerImportar_RunWorkerCompleted);
                    timer.Tick += new EventHandler(timer_TickImportar);
                    lblMensaje.Text = "Importando Base de Datos";
                    break;
                case Operacion.Exportar:
                    maximo = 0;
                    _maximo = 0;
                    Exportar();
                    bgwWorker.DoWork += new DoWorkEventHandler(bgwWorkerExportar_DoWork);
                    bgwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorkerExportar_RunWorkerCompleted);
                    timer.Tick += new EventHandler(timer_TickExportar);
                    lblMensaje.Text = "Exportando Base de Datos";
                    break;
            }
            this.Text = lblMensaje.Text;
            timer.Start();
        }

        private void dialogBaseDatos_Load(object sender, EventArgs e)
        {
            Cargar();
        }

        #region Exportar

        private void timer_TickExportar(object sender, EventArgs e)
        {
            pgbProgreso.Maximum = maximo;
            if (_maximo <= pgbProgreso.Maximum)
            pgbProgreso.Value = _maximo;
        }

        private void bgwWorkerExportar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pgbProgreso.Value = pgbProgreso.Maximum;
            if (Respuesta)
                MessageBox.Show("Respaldo concretado", "Operacion exitosa!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se descargo la base de datos, razones:\nError de conexion o de servidor", "Operacion cancelada",
                    MessageBoxButtons.OK, MessageBoxIcon.Question);
            this.Close();
        }

        public void Exportar()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Escoge la ruta donde se guardara el respaldo de la base de datos";
            switch (fbd.ShowDialog())
            {
                case DialogResult.Cancel:
                    MessageBox.Show("Se cancelo la operacion de respaldo de base de datos", "Operacion Cancelada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    break;
                case DialogResult.OK:
                    ruta = fbd.SelectedPath;
                    bgwWorker.RunWorkerAsync();
                    break;
                default:
                    MessageBox.Show("No se realizo ninguna accion", "Operacion Cancelada",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    break;
            }
            fbd.Dispose();
        }

        private void bgwWorkerExportar_DoWork(object sender, DoWorkEventArgs e)
        {
            Respuesta = ExportarBaseDatos(ruta, "backup_maxima_distribuidores");
        }

        public bool ExportarBaseDatos(string ruta, string nombreArchivo)
        {
            MySqlConnection conexion;
            conexion = Sql.ObtenerConexion();

            if (String.IsNullOrWhiteSpace(nombreArchivo))
                nombreArchivo = "backup " + DateTime.Now.ToString("dd-mm-ss MM-yyyy") + ".sql";
            else
                nombreArchivo += DateTime.Now.ToString("dd-mm-ss MM-yyyy") + ".sql";

            if (String.IsNullOrWhiteSpace(ruta))
                ruta = Application.ExecutablePath;
            else
                ruta += "\\";

            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup backup = new MySqlBackup(cmd))
                {
                    backup.ExportInfo.AddCreateDatabase = true;
                    backup.ExportInfo.IntervalForProgressReport = 50;
                    backup.ExportInfo.GetTotalRowsBeforeExport = true;
                    backup.ExportProgressChanged += backup_ExportProgressChanged;
                    cmd.Connection = conexion;
                    try
                    {
                        conexion.Open();
                        backup.ExportToFile(ruta + nombreArchivo);
                        conexion.Close();
                        cmd.Dispose();
                    }
                    catch (Exception) { }   
                }
            }

            return true;
        }

        private void backup_ExportProgressChanged(object sender, ExportProgressArgs e)
        {
            maximo = (int)e.TotalRowsInAllTables;
            _maximo = (int)e.CurrentRowIndexInAllTables;
        }

        #endregion

        #region Importar

        private void Importar()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Seleccionar el archivo de respaldo a importar";
            fd.Filter = "Archivos SQL (*.sql)|";
            fd.Multiselect = false;
            switch (fd.ShowDialog())
            {
                case DialogResult.Cancel:
                    MessageBox.Show("Se cancelo la operacion de respaldo de base de datos", "Operacion Cancelada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    break;
                case DialogResult.OK:
                    ruta = fd.FileName;
                    bgwWorker.RunWorkerAsync();
                    break;
                default:
                    MessageBox.Show("No se realizo ninguna accion", "Operacion Cancelada",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    break;
            }
        }

        private void bgwWorkerImportar_DoWork(object sender, DoWorkEventArgs e)
        {
            Respuesta = ImportarBaseDatos(ruta);
        }

        public bool ImportarBaseDatos(string archivo)
        {
            MySqlConnection conexion;
            conexion = Sql.ObtenerConexion("Server=" + Sql.Server + ";" + "Uid=" + Sql.User + ";" 
                + "Pwd=" + Sql.Pass + ";");

            if (String.IsNullOrWhiteSpace(archivo))
                return false;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup backup = new MySqlBackup(cmd))
                {
                    backup.ExportInfo.AddCreateDatabase = true;
                    backup.ExportInfo.IntervalForProgressReport = 50;
                    backup.ExportInfo.GetTotalRowsBeforeExport = true;
                    backup.ImportProgressChanged += backup_ImportProgressChanged;
                    cmd.Connection = conexion;
                    try
                    {
                        conexion.Open();
                        backup.ImportFromFile(archivo);
                        conexion.Close();
                        cmd.Dispose();
                    }
                    catch (Exception) { }       
                }
            }

            return true;
        }

        public static bool ImportarBaseDatos(string archivo, string comando)
        {
            MySqlConnection conexion;
            conexion = Sql.ObtenerConexion(comando);

            if (String.IsNullOrWhiteSpace(archivo))
                return false;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup backup = new MySqlBackup(cmd))
                {
                    backup.ExportInfo.AddCreateDatabase = true;
                    cmd.Connection = conexion;
                    try
                    {
                        conexion.Open();
                        backup.ImportFromFile(archivo);
                        conexion.Close();
                        cmd.Dispose();
                    }
                    catch (Exception) { }
                    
                }
            }

            return true;
        }

        private void backup_ImportProgressChanged(object sender, ImportProgressArgs e)
        {
            totalBytes = (int)e.TotalBytes;
            curBytes = (int)e.CurrentBytes;
        }

        private void bgwWorkerImportar_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer.Stop();

            pgbProgreso.Value = pgbProgreso.Maximum;
            if (Respuesta)
                MessageBox.Show("Se importo el archivo con exito", "Operacion exitosa!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se importo la base de datos, razones:\nLa base de datos que deseas importar es igual a la base de datos en el servidor\bError de conexion o de servidor", "Operacion cancelada",
                    MessageBoxButtons.OK, MessageBoxIcon.Question);
            this.Close();
        }

        private void timer_TickImportar(object sender, EventArgs e)
        {
            pgbProgreso.Maximum = totalBytes;
            if (curBytes < pgbProgreso.Maximum)
                pgbProgreso.Value = curBytes;
        }

        #endregion
    }
}
