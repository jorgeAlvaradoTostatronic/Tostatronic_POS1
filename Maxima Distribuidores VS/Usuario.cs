using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public class Usuario
    {
        private int id;
        public int Id { get { return id; } }

        private string user;
        public string User { get { return user; } }

        private string nombre;
        public string Nombre { get { return nombre; } }

        private string password;
        public string Password { get { return password; } }        

        private string paterno;
        public string Paterno { get { return paterno; } }

        private string materno;
        public string Materno { get { return materno; } }

        private string correo;
        public string Correo { get { return correo; } }

        private Permisos permisos;
        public Permisos Permisos { get { return permisos; } }

        private static Usuario usuario;

        private Usuario(int id, string user, string nombre, string password, 
            string paterno, string materno, string correo, string permisos)
        {
            this.id = id;
            this.user = user;
            this.nombre = nombre;
            this.password = password;
            this.paterno = paterno;
            this.materno = materno;
            this.correo = correo;
            this.permisos = new Permisos().SetByString(permisos);
        }

        public static void CrearInstancia(int id, string user, string nombre, string password,
            string paterno, string materno, string correo, string permisos)
        {
                usuario = new Usuario(id, user, nombre, password, paterno,
                    materno, correo, permisos);
        }

        public static void DestruirInstancia() { usuario = null; }

        public static Usuario Instancia()
        {
            if (usuario != null)
                return usuario;
            else
                return null;
        }

        public static void RegistrarOperacion(string descripcion)
        {
            Usuario usuario = Usuario.Instancia();
            DateTime fecha = new DateTime();
            Sql.InsertarDatos("INSERT INTO log VALUES(NULL, '" + usuario.User + "', '" + descripcion + "', '" +
                fecha.Date.ToString("yyyy-mm-dd") + "')");
        }
    }

    public struct Permisos
    {
        public bool Clientes;
        public bool AgregarClientes;
        public bool VerClientes;
        public bool ModificarClientes;
        public bool EliminarClientes;

        public bool Productos;
        public bool AgregarProductos;
        public bool VerProductos;
        public bool ModificarProductos;
        public bool EliminarProductos;

        public bool Ventas;
        public bool Cotizaciones;
        public bool Servidor;
        public bool Reportes;
        public bool Inventario;
        public bool Log;
        public bool ExportarBD;
        public bool ImportarBD;
        public bool ControlUsuario;
        public bool Factura;

        public Permisos SetByString(string permiso)
        {
            Permisos permisos = new Permisos();
            permisos.Clientes = (permiso[1] == '1' || permiso[2] == '1' 
                || permiso[3] == '1' || permiso[4] == '1') ? true : false;

            permisos.AgregarClientes = (permiso[1] == '1') ? true : false;
            permisos.VerClientes = (permiso[2] == '1') ? true : false;
            permisos.ModificarClientes = (permiso[3] == '1') ? true : false;
            permisos.EliminarClientes = (permiso[4] == '1') ? true : false;
            permisos.Productos = (permiso[6] == '1' || permiso[7] == '1' 
                || permiso[8] == '1' || permiso[9] == '1') ? true : false;

            permisos.AgregarProductos = (permiso[6] == '1') ? true : false;
            permisos.VerProductos = (permiso[7] == '1') ? true : false;
            permisos.ModificarProductos = (permiso[8] == '1') ? true : false;
            permisos.EliminarProductos = (permiso[9] == '1') ? true : false;
            permisos.Ventas = (permiso[10] == '1') ? true : false;
            permisos.Cotizaciones = (permiso[11] == '1') ? true : false;
            permisos.Servidor = (permiso[12] == '1') ? true : false;
            permisos.Reportes = (permiso[13] == '1') ? true : false;
            permisos.Inventario = (permiso[14] == '1') ? true : false;
            permisos.Log = (permiso[15] == '1') ? true : false;
            permisos.ExportarBD = (permiso[16] == '1') ? true : false;
            permisos.ImportarBD = (permiso[17] == '1') ? true : false;
            permisos.ControlUsuario = (permiso[18] == '1') ? true : false;
            permisos.Factura = (permiso[19] == '1') ? true : false;
            return permisos;
        }
    }
}
