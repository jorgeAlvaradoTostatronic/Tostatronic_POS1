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
        public static long InsertarVenta(List<ProductoCompleto> productos,string idUsuario,string idCliente,bool pagada, float impuesto)
        {
            #region NuevaAccion
            MySqlConnection conexion;
            MySqlCommand cmd;
            MySqlTransaction transaccion = null;
            long idVenta = 0;
            string consulta;
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            conexion = ObtenerConexion();
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
            cmd = new MySqlCommand();
            cmd.Connection = conexion;
            transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = transaccion;
            try
            {
                if (pagada)
                    consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`,`impuesto` ) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '1', '" + fecha + "', '0', " + impuesto + ");";
                else
                    consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`,`impuesto`) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '0', '" + fecha + "', '0', " + impuesto + ");";
                cmd.CommandText = consulta;
                cmd.ExecuteNonQuery();
                idVenta = cmd.LastInsertedId;
                foreach (ProductoCompleto producto in productos)
                {
                    cmd.Parameters.Clear();
                    consulta = "INSERT INTO `salepoint`.`productos_de_venta` (`id_venta`, `id_producto`, `cantidad_comprada`, `precio_al_momento`, `descuento`) VALUES (@id_venta, @codigo, @cantidad, @precio, @descuento);";
                    cmd.CommandText = consulta;
                    cmd.Parameters.AddWithValue("@id_venta", idVenta);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento);
                    cmd.ExecuteNonQuery();
                }
                transaccion.Commit();
                conexion.Close();
            }
            catch (MySqlException e)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        e.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                transaccion.Rollback();
            }
            finally
            {
                conexion.Close();
            }
            return idVenta;
            #endregion
            #region AccionAntigua
            //MySqlConnection conexion;
            //MySqlCommand cmd;
            //MySqlTransaction transaccion = null;

            //conexion = ObtenerConexion();
            //if (conexion.State == ConnectionState.Closed)
            //    conexion.Open();


            //string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string consulta = "INSERT INTO venta (NULL,'" + idUsuario + "','" + idCliente + "','1','" + fecha + "','0', " + impuesto + ");";
            //if(pagada)
            //    consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`,`impuesto` ) VALUES (NULL, '" + idUsuario+"', '"+idCliente+"', '1', '"+fecha+"', '0', "+impuesto+");";
            //else
            //    consulta = "INSERT INTO `salepoint`.`venta` (`id_venta`, `id_usuario`, `id_cliente`, `pagada`, `fecha_de_venta`, `cancelada`,`impuesto`) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '0', '" + fecha + "', '0', " + impuesto + ");";
            //try
            //{
            //    //InsertarDatos(consulta);
            //    #region InsertarDatos
            //    cmd = new MySqlCommand(consulta);
            //    //conexion = ObtenerConexion();
            //    //if (conexion.State == ConnectionState.Closed)
            //    //    conexion.Open();
            //    transaccion = conexion.BeginTransaction();
            //    cmd.Connection = conexion;
            //    cmd.ExecuteReader();
            //    cmd.Dispose();
            //    #endregion
            //    List<string[]> idList = BuscarDatos("SELECT id_venta FROM venta;");
            //    string id = idList[idList.Count - 1][0];
            //    long idVenta = long.Parse(id);
            //    idVenta++;
            //    cmd = new MySqlCommand();
            //    cmd.Connection = conexion;
            //    foreach (ProductoCompleto producto in productos)
            //    {
            //        cmd.Parameters.Clear();
            //        consulta = "INSERT INTO `salepoint`.`productos_de_venta` (`id_venta`, `id_producto`, `cantidad_comprada`, `precio_al_momento`, `descuento`) VALUES (@id_venta, @codigo, @cantidad, @precio, @descuento);";
            //        //cmd = new MySqlCommand(consulta);
            //        //conexion = ObtenerConexion();
            //        //if (conexion.State == ConnectionState.Closed)
            //        //    conexion.Open();
            //        //cmd.Connection = conexion;
            //        //cmd.ExecuteReader();
            //        //conexion.Close();
            //        cmd.CommandText = consulta;
            //        cmd.Parameters.AddWithValue("@id_venta", idVenta);
            //        cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
            //        cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
            //        cmd.Parameters.AddWithValue("@precio", producto.Precio);
            //        cmd.Parameters.AddWithValue("@descuento", producto.Descuento);

            //        cmd.ExecuteNonQuery();
            //    }
            //    transaccion.Commit();
            //    conexion.Close();
            //}
            //catch (MySqlException msql)
            //{
            //    if (!Sql.ConectaServidor())
            //        new dialogServidor().ShowDialog();
            //    else
            //        MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
            //            msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    transaccion.Rollback();
            //}
            //catch (Exception)
            //{
            //    conexion.Close();
            //    transaccion.Rollback();
            //}
            #endregion
        }
        public static void InsertarCotizacion(List<ProductoCompleto> productos, string idUsuario, string idCliente, float impuesto)
        {
            #region NuevaAccion
            MySqlConnection conexion;
            MySqlCommand cmd;
            MySqlTransaction transaccion = null;
            string consulta;
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            conexion = ObtenerConexion();
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
            cmd = new MySqlCommand();
            cmd.Connection = conexion;
            transaccion = conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            cmd.Transaction = transaccion;
            try
            {
                consulta = "INSERT INTO `salepoint`.`cotizacion` (`id_cotizacion`, `id_usuario`, `id_cliente`, `fecha_cotizacion`, `impuesto`) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '" + fecha + "', " + impuesto + ");";
                cmd.CommandText = consulta;
                cmd.ExecuteNonQuery();
                long idCotizacion = cmd.LastInsertedId;
                foreach (ProductoCompleto producto in productos)
                {
                    cmd.Parameters.Clear();
                    consulta = "INSERT INTO `salepoint`.`productos_de_cotizacion` (`id_cotizacion`, `id_producto`, `cantidad_cotizada`, `precio_al_momento`, `descuento`) VALUES (@id_venta, @codigo, @cantidad, @precio, @descuento);";
                    cmd.CommandText = consulta;
                    cmd.Parameters.AddWithValue("@id_venta", idCotizacion);
                    cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
                    cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@descuento", producto.Descuento);
                    cmd.ExecuteNonQuery();
                }
                transaccion.Commit();
                conexion.Close();
            }
            catch (MySqlException e)
            {
                if (!Sql.ConectaServidor())
                    new dialogServidor().ShowDialog();
                else
                    MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
                        e.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                transaccion.Rollback();
            }
            finally
            {
                conexion.Close();
            }
            #endregion
            #region AntiguoMetodo
            //MySqlConnection conexion;
            //MySqlCommand cmd;
            //MySqlTransaction transaccion = null;
            //string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string consulta = "INSERT INTO `salepoint`.`cotizacion` (`id_cotizacion`, `id_usuario`, `id_cliente`, `fecha_cotizacion`, `impuesto`) VALUES (NULL, '" + idUsuario + "', '" + idCliente + "', '" + fecha + "', " + impuesto + ");";
            //try
            //{
            //    InsertarDatos(consulta);
            //    List<string[]> idList = BuscarDatos("SELECT id_cotizacion FROM cotizacion;");
            //    string id = idList[idList.Count - 1][0];
            //    foreach (ProductoCompleto producto in productos)
            //    {
            //        consulta = "INSERT INTO `salepoint`.`productos_de_cotizacion` (`id_cotizacion`, `id_producto`, `cantidad_cotizada`, `precio_al_momento`, `descuento`) VALUES ('" + id + "', '" + producto.Codigo + "', '" + producto.Cantidad + "', '" + producto.Precio + "', '" + producto.Descuento + "');";
            //        cmd = new MySqlCommand(consulta);
            //        conexion = ObtenerConexion();
            //        if (conexion.State == ConnectionState.Closed)
            //            conexion.Open();
            //        cmd.Connection = conexion;
            //        cmd.ExecuteReader();
            //        conexion.Close();
            //    }
            //}
            //catch (MySqlException msql)
            //{
            //    if (!Sql.ConectaServidor())
            //        new dialogServidor().ShowDialog();
            //    else
            //        MessageBox.Show("Ha ocurrido un error en la consulta.\n" +
            //            msql.Message, "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //catch (Exception)
            //{ //transaccion.Rollback(); 
            //}
            #endregion
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
                        datos[i] = lector.GetString(i).ToString();
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
            catch (Exception es) { MessageBox.Show(es.Message); }
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
