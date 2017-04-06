using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql;
using System.Data;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class SqlValidacion
    {
        public static string Server = "localhost";
        public static string DB = "maxima_distribuidores";
        public static string User = "root";
        public static string Pass = "";
        public static string Puerto = "80";
        
        public static void Load()
        {
            InformacionServidor info = GuardarServidor.Leer();
            Server = info.Servidor.Ip;
            User = info.Servidor.User;
            Pass = Encriptacion.Desencriptar(info.Servidor.Password);
            Puerto = info.Servidor.Puerto.ToString();
            cadenaConexion =
                "Server=" + Server + ";" +
                //"Port="+Puerto+";" +
                "Database=seriales;" +
                "Uid=" + User + ";" +
                "Pwd=" + Pass + ";";
        }

        private static string cadenaConexion =
               "Server=localhost;" +
               "Database=seriales;" +
               "Uid=maxima;" +
               "Pwd=;";

        private static MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        public static void InsertarDatos(string consulta)
        {
            MySqlConnection conexion;
            MySqlCommand cmd;
            try
            {
                cmd = new MySqlCommand(consulta);
                conexion = ObtenerConexion();
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();
                cmd.Connection = conexion;
                cmd.ExecuteReader();

                conexion.Close();
            }
            catch (Exception) {  }
        }

        public static bool Existe(string comando)
        {
            MySqlConnection conexion;
            MySqlCommand cmd;
            try
            {
                cmd = new MySqlCommand(comando);
                conexion = ObtenerConexion();
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();
                cmd.Connection = conexion;
                MySqlDataReader lector = cmd.ExecuteReader();
                while (lector.Read())
                    if (lector.GetString(0) != null)
                    {
                        conexion.Close();
                        lector.Close();
                        return true;
                    }
                lector.Close();
                conexion.Close();
            }
            catch (Exception) { }
            return false;
        }
    }
}
