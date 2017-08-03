using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    public class InformacionVenta
    {
        public int Id_cliente { get; set; }
        public List<Producto> Productos { get; set; }

        public InformacionVenta() { }

        public InformacionVenta(int id_cliente, List<Producto> productos)
        {
            Id_cliente = id_cliente;
            Productos = productos;
        }
    }

    public struct Producto
    {
        public string Id_producto;
        public float Cantidad, Subtotal, Descuento;

        public Producto(string id_producto, float cantidad, float descuento)
        {
            Id_producto = id_producto;
            Cantidad = cantidad;
            Descuento = descuento;
            Subtotal = default(float);
        }

        public Producto(string id_producto, float cantidad, float descuento, float subtotal)
        {
            Id_producto = id_producto;
            Cantidad = cantidad;
            Descuento = descuento;
            Subtotal = subtotal;
        }
    }

    public struct ProductoCompleto
    {
        public float Descuento;
        public string Codigo, Descripcion;
        public float Cantidad, Precio, Subtotal;

        public ProductoCompleto(string codigo, string descripcion, float cantidad,float descuento, float subtotal)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Cantidad = cantidad;
            Precio = (100*subtotal)/(100-descuento) / cantidad;
            Descuento = descuento;
            Subtotal = subtotal;
        }

        public ProductoCompleto(string codigo, string descripcion, float cantidad, float subtotal)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Cantidad = cantidad;
            Precio = subtotal / cantidad;
            Descuento = 0;
            Subtotal = subtotal;
        }
    }

    public struct ProductoCatalogo
    {
        public string Codigo, Descripcion, Imagen;
        public float PrecioPublico, PrecioDistribuidor, PrecioMinimo;

        public ProductoCatalogo(string codigo, string descripcion, float precioPublico, float precioDistribuidor,
            float precioMinimo, string imagen)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            PrecioPublico = precioPublico;
            PrecioDistribuidor = precioDistribuidor;
            PrecioMinimo = precioMinimo;
            Imagen = imagen;
        }
    }

    public struct Clientes
    {
        public int tipo_cliente;
        public string nombre, paterno, materno, rfc, telefono, domicilio, correo;

        public Clientes(string nombre, string paterno, string materno, string rfc, string telefono,
            string domicilio, string correo)
        {
            tipo_cliente = 0;
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.rfc = rfc;
            this.telefono = telefono;
            this.domicilio = domicilio;
            this.correo = correo;
        }

        public Clientes(int tipo_cliente, string nombre, string paterno, string materno, string rfc, string telefono,
            string domicilio, string correo)
        {
            this.tipo_cliente = tipo_cliente;
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.rfc = rfc;
            this.telefono = telefono;
            this.domicilio = domicilio;
            this.correo = correo;
        }
    }

    public struct ClienteCompleto
    {
        public int tipo_cliente;
        public string nombre, paterno, materno, rfc, telefono, domicilio, cp, colonia, correo;

        public ClienteCompleto(string nombre, string paterno, string materno, string rfc, string telefono,
            string domicilio,string cp, string colonia, string correo)
        {
            tipo_cliente = 0;
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.rfc = rfc;
            this.telefono = telefono;
            this.domicilio = domicilio;
            this.cp = cp;
            this.colonia = colonia;
            this.correo = correo;
        }

        public ClienteCompleto(int tipo_cliente, string nombre, string paterno, string materno, string rfc, string telefono,
            string domicilio, string cp, string colonia, string correo)
        {
            this.tipo_cliente = tipo_cliente;
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.rfc = rfc;
            this.telefono = telefono;
            this.domicilio = domicilio;
            this.cp = cp;
            this.colonia = colonia;
            this.correo = correo;
        }
    }

    public struct Venta
    {
        public int Id_pedido;
        public Usuarios Usuario;
        public Clientes Cliente;
        public bool Pagado;
        public string Fecha;
        public List<ProductoCompleto> Productos;
        public float Abono;
        public float Descuento;
        public float Total;

        public Venta(int id_pedido, Usuarios usuario, Clientes cliente, bool pagado, string fecha, List<ProductoCompleto> productos,
            float abono, float descuento, float total)
        {
            Id_pedido = id_pedido;
            Usuario = usuario;
            Cliente = cliente;
            Pagado = pagado;
            Fecha = fecha;
            Abono = abono;
            Descuento = descuento;
            Total = total;
            Productos = productos;
        }
    }

    public struct Cotizacion
    {
        public int Id_cotizacion;
        public Usuarios Usuario;
        public Clientes Cliente;
        public string Fecha;
        public List<ProductoCompleto> Productos;
        public float Total;

        public Cotizacion(int id_cotizacion, Usuarios usuario, Clientes cliente, string fecha, List<ProductoCompleto> productos, float total)
        {
            Id_cotizacion = id_cotizacion;
            Usuario = usuario;
            Cliente = cliente;
            Fecha = fecha;
            Total = total;
            Productos = productos;
        }
    }

    public struct Usuarios
    {
        public string Usuario, NombreCompleto;

        public Usuarios(string usuario, string nombre)
        {
            Usuario = usuario;
            NombreCompleto = nombre;
        }
    }
}
