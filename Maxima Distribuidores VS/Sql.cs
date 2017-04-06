using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;
using MySql;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public static class Sql
    {
        public static string Server = "localhost";
        public static string DB = "maxima_distribuidores";
        public static string User = "root";
        public static string Pass = "";
        public static string Puerto = "80";

        private static string cadenaConexion =
                "Server=localhost;" +
                "Database=maxima_distribuidores;" +
                "Uid=root;" +
                "Pwd=";

        public static void Load()
        {
            InformacionServidor info = GuardarServidor.Leer();
            Server = info.Servidor.Ip;
            User = info.Servidor.User;
            Pass = Encriptacion.Desencriptar(info.Servidor.Password);
            Puerto = info.Servidor.Puerto.ToString();
            cadenaConexion =
                "Server="+Server+";" +
                //"Port="+Puerto+";" +
                "Database=salepoint;" +
                "Uid="+User+";" +
                "Pwd="+Pass+";";
        }

        public static MySqlConnection ObtenerConexion() 
        {
            return new MySqlConnection(cadenaConexion);
        }

        public static MySqlConnection ObtenerConexion(string conexion)
        {
            return new MySqlConnection(conexion);
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
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
            return false;
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
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
        }
        public static void InsertarVenta(List<ProductoCompleto> productos,string idUsuario,string idCliente,bool pagada)
        {
            MySqlConnection conexion;
            MySqlCommand cmd;
            MySqlTransaction transaccion = null;
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string consulta = "INSERT INTO venta (NULL,'" + idUsuario + "','" + idCliente + "','1','" + fecha + "','0');";
            if(pagada)
                consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`) VALUES (NULL, '"+idUsuario+"', '"+idCliente+"', '1', '"+fecha+"', '0');";
            else
                consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '0', '" + fecha + "', '0');";
            try
            {
                InsertarDatos(consulta);
                List<string[]> idList = BuscarDatos("SELECT id_venta FROM venta;");
                string id = idList[idList.Count - 1][0];
                long idVenta = long.Parse(id);
                foreach(ProductoCompleto producto in productos)
                {
                    consulta = "INSERT INTO `salepoint`.`productos_de_venta` (`id_venta`, `id_producto`, `cantidad_comprada`, `precio_al_momento`, `descuento`) VALUES ('"+idVenta+"', '"+producto.Codigo+"', '"+producto.Cantidad+"', '"+producto.Precio+"', '"+producto.Descuento+"');";
                    cmd = new MySqlCommand(consulta);
                    conexion = ObtenerConexion();
                    if (conexion.State == ConnectionState.Closed)
                        conexion.Open();
                    cmd.Connection = conexion;
                    cmd.ExecuteReader();
                    conexion.Close();
                }
            }
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { transaccion.Rollback(); }
        }
        public static void InsertarCotizacion(List<ProductoCompleto> productos, string idUsuario, string idCliente)
        {
            MySqlConnection conexion;
            MySqlCommand cmd;
            MySqlTransaction transaccion = null;
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string consulta = "INSERT INTO `salepoint`.`cotizacion` (`id_cotizacion`, `id_usuario`, `id_cliente`, `fecha_cotizacion`) VALUES (NULL, '"+idUsuario+"', '"+idCliente+"', '"+fecha+"');";
            try
            {
                InsertarDatos(consulta);
                List<string[]> idList = BuscarDatos("SELECT id_cotizacion FROM cotizacion;");
                string id = idList[idList.Count - 1][0];
                foreach (ProductoCompleto producto in productos)
                {
                    consulta = "INSERT INTO `salepoint`.`productos_de_cotizacion` (`id_cotizacion`, `id_producto`, `cantidad_cotizada`, `precio_al_momento`, `descuento`) VALUES ('" + id + "', '" + producto.Codigo + "', '" + producto.Cantidad + "', '" + producto.Precio + "', '" + producto.Descuento + "');";
                    cmd = new MySqlCommand(consulta);
                    conexion = ObtenerConexion();
                    if (conexion.State == ConnectionState.Closed)
                        conexion.Open();
                    cmd.Connection = conexion;
                    cmd.ExecuteReader();
                    conexion.Close();
                }
            }
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) 
            { //transaccion.Rollback(); 
            }
        }
        public static DataTable CrearTablaDatos(string comando)
        {
            DataTable t = new DataTable();
            try
            {
                MySqlConnection conexion = ObtenerConexion();
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(comando, conexion);
                conexion.Close();
                adapter.Fill(t);
            }
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
            return t;

        }
        public static List<string[]> BuscarDatos(string comando) 
        {
            List<string[]> lista = new List<string[]>();
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
                {
                    string[] datos = new string[lector.FieldCount];
                    for (int i = 0; i < lector.FieldCount; i++)
                        datos[i] = lector.GetString(i);
                    lista.Add(datos);
                }

                lector.Close();
                conexion.Close();
            }
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
            return lista;
        }

        public static string ChecarStatusServidor()
        {
            MySqlConnection conexion = ObtenerConexion();
            string resultado;
            try
            {
                conexion.Open();
                resultado = "Server Version: " + conexion.ServerVersion + "\nEstado: " + conexion.State.ToString();
                conexion.Close();
            }
            catch (MySqlException msql)
            {
                resultado = "Error de conexion\n" + msql.Message;
            }
            return resultado;
        }

        public static bool ConectaBD()
        {
            MySqlConnection conexion = ObtenerConexion();
            try
            {
                conexion.Open();
                conexion.Close();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public static bool ConectaServidor()
        {
            MySqlConnection conexion = ObtenerConexion("Server=" + Sql.Server + ";" + "Uid=" + Sql.User + ";"
                + "Pwd=" + Sql.Pass + ";");
            try
            {
                conexion.Open();
                MySqlCommand cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS prueba /*!40100 DEFAULT CHARACTER SET latin1 */;", conexion);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("DROP DATABASE prueba;", conexion);
                cmd.ExecuteNonQuery();

                conexion.Close();
                return true;
            }
            catch (Exception){ }
            return false;
        }

        public static void ExisteServidor(string comando)
        {
            MySqlConnection conexion = new MySqlConnection(comando);
            conexion.Open();
            conexion.Ping();
            conexion.Close();
        }

        public static string ObtenerFolio() 
        {
            string folio="";
            MySqlConnection conexion;
            MySqlCommand cmd;

            try
            {
                cmd = new MySqlCommand("SELECT id_venta FROM venta ORDER BY id_venta DESC");
                conexion = ObtenerConexion();
                if (conexion.State == ConnectionState.Closed)
                    conexion.Open();
                cmd.Connection = conexion;
                MySqlDataReader lector = cmd.ExecuteReader();
                lector.Read();
                folio = lector.GetString(0);

                lector.Close();
                conexion.Close();
            }
            catch (MySqlException msql)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception) { }
            return folio;
        }
    }
}
