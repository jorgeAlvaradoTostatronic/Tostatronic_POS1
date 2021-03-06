﻿using System;
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
    public partial class uscProductos : UserControl
    {
        private Redimension redimension;
        private string id_Producto="";

        public uscProductos(Accion accion)
        {
            InitializeComponent();
            redimension = new Redimension(this);
            Cargar(accion);
        }

        private void Cargar(Accion accion)
        {
            switch (accion)
            {
                case Accion.Agregar:
                    dgvProductos.Visible = false;
                    txtBusqueda.Visible = false;
                    btnBusqueda.Visible = false;
                    btnAccion.Text = "Agregar Producto";
                    btnAccion.Click += new EventHandler(btnAccionAgregar_Click);
                    break;
                case Accion.Modificar:
                    btnAccion.Text = "Modificar Producto";
                    btnAccion.Click += new EventHandler(btnAccionModificar_Click);
                    break;
                case Accion.Eliminar:
                    txtCodigo.Enabled = false;
                    txtDescripcion.Enabled = false;
                    nudCantidad.Enabled = false;
                    txtPublico.Enabled = false;
                    txtDistribuidor.Enabled = false;
                    btnAccion.Text = "Eliminar Producto";
                    btnAccion.Click += new EventHandler(btnAccionEliminar_Click);
                    txtPrecioDeCompra.Enabled = false;
                    txtCantidadMinima.Enabled = false;
                    txtPrecioDeVentaMinimo.Enabled = false;
                    break;
            }
            lblTitulo.Text = btnAccion.Text;
        }

        private void btnAccionAgregar_Click(object sender, EventArgs e)
        {
            if (Verificar())
            {
                if (!Sql.Existe("SELECT codigo FROM productos WHERE codigo = '" + txtCodigo.Text + "'"))
                {
                    String name=CargaImagen();
                    Sql.InsertarDatos("Insert into productos VALUES('" + txtCodigo.Text + "','" + txtDescripcion.Text + "','" +
                        nudCantidad.Value + "','" + txtCantidadMinima.Value + "','" + txtPrecioDeCompra.Text + "','" + txtPublico.Text + "','" + txtDistribuidor.Text + "','" + txtPrecioDeVentaMinimo.Text + "',0,'" + name + "')");
                    MessageBox.Show("Agregado con exito", "Agregado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    Limpiar();
                }
                else
                {
                    MessageBox.Show("Código ya existente. Ingrese otro código.", "Advertencia", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtCodigo.Focus();    
                }
            }
        }

        private void btnAccionModificar_Click(object sender, EventArgs e)
        {
            if (id_Producto.Equals(""))
                MessageBox.Show("Seleccione un producto.", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
            else if (Verificar())
            {
                if (!Sql.Existe("SELECT codigo FROM productos WHERE codigo = '" + txtCodigo.Text + "'"))
                {
                    String name = CargaImagen();
                    Sql.InsertarDatos("UPDATE productos SET codigo='" + txtCodigo.Text + "', nombre='" + txtDescripcion.Text +
                        "', existencia= '" + nudCantidad.Value.ToString() + "', cantidad_minima='"+txtCantidadMinima.Value.ToString()+"', precio_publico='" + txtPublico.Text + "', precio_distribuidor='" +
                        txtDistribuidor.Text + "', precio_minimo='" + txtPrecioDeVentaMinimo .Text+"', imagen='"+name+"' WHERE codigo = '" + id_Producto + "'");
                    MessageBox.Show("Modificado con exito", "Modificado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    Buscador("SELECT  codigo, nombre FROM productos " +
                        "WHERE codigo = '" + id_Producto + "'"); ;
                    Limpiar();
                }
                else
                {
                    String name = CargaImagen();
                    Sql.InsertarDatos("UPDATE productos SET nombre='" + txtDescripcion.Text +
                        "', existencia= '" + nudCantidad.Value.ToString() + "', cantidad_minima='" + txtCantidadMinima.Value.ToString() + "', precio_publico='" + txtPublico.Text + "', precio_distribuidor='" +
                        txtDistribuidor.Text + "', precio_minimo='" + txtPrecioDeVentaMinimo.Text + "', precio_compra='"+txtPrecioDeCompra.Text+ "', imagen='" + name + "' WHERE codigo = '" + id_Producto + "'");
                    MessageBox.Show("Modificado con exito", "Modificado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    Buscador("SELECT  codigo, nombre FROM productos " +
                        "WHERE codigo = '" + id_Producto + "'"); ;
                    Limpiar();
                }
            }
        }

        private String CargaImagen()
        {
            String name = "No_image.png";
            if (!txtImageName.Text.Equals("Seleccionar imagen..."))
            {
                try
                {
                    name = txtCodigo.Text;
                    name += Path.GetExtension(txtImageName.Text);
                    File.Copy(txtImageName.Text, @"Imagenes\" + name,true);
                }
                catch (IOException)
                {
                    MessageBox.Show("La imagen no pudo ser cargada al sistema", "Error al cargar imagen", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    name = "No_image.png";
                }
            }
            return name;
        }
        private void btnAccionEliminar_Click(object sender, EventArgs e)
        {
            if (id_Producto.Equals(""))
                MessageBox.Show("Seleccione un producto.", "Advertencia", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
            else
            {
                DialogResult confirmacion = MessageBox.Show(this, "¿Esta seguro que desea eliminar " +
                    "este producto?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                {
                    Sql.InsertarDatos("UPDATE productos SET eliminado  = 1 WHERE codigo = '" + id_Producto + "'");
                    MessageBox.Show("Eliminando con exito", "Eliminado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    dgvProductos.Rows.Clear();
                }
                Limpiar();
            }
        }

        private bool Verificar()
        {
            float precioMinimo, precioCompra;
            if (!String.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                if (!String.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    if (!String.IsNullOrWhiteSpace(nudCantidad.Text))
                    {
                        if (!String.IsNullOrWhiteSpace(txtPublico.Text))
                        {
                            float precioPublico;
                            if (float.TryParse(txtPublico.Text, out precioPublico))
                            {
                                if (precioPublico >= 0)
                                {
                                    if (!String.IsNullOrWhiteSpace(txtDistribuidor.Text))
                                    {
                                        float precioDistribuidor;
                                        if (float.TryParse(txtDistribuidor.Text, out precioDistribuidor))
                                        {
                                            if (precioDistribuidor >= 0)
                                            {
                                                if (float.TryParse(txtPrecioDeCompra.Text, out precioCompra))
                                                {
                                                    if (float.TryParse(txtPrecioDeVentaMinimo.Text, out precioMinimo))
                                                    {
                                                        if(precioMinimo>precioCompra)
                                                        {
                                                            if (precioDistribuidor > precioMinimo)
                                                            {
                                                                if (precioPublico > precioMinimo)
                                                                {
                                                                    if (precioMinimo > 0)
                                                                    {
                                                                        if (txtCantidadMinima.Value >= 0)
                                                                            return true;
                                                                        else
                                                                        {
                                                                            MessageBox.Show(this, "La cantidad minima debe ser mayo o igual a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                                            txtCantidadMinima.Focus();
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        MessageBox.Show(this, "El precio minimo debe de ser mayor a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                                        txtPrecioDeVentaMinimo.Focus();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show(this, "El precio publico debe ser mayor al precio minimo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                                    txtPublico.Focus();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show(this, "El precio distribuido debe ser mayor al precio minimo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                                txtDistribuidor.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show(this, "El precio minimo debe de ser mayor a al precio de compra", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                            txtPrecioDeVentaMinimo.Focus();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(this, "El precio minimo debe ser un numero", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                        txtPrecioDeVentaMinimo.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show(this, "El precio de compra debe ser un numero", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    txtPrecioDeCompra.Focus();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(this, "El precio para distribuidor debe ser positivo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                txtDistribuidor.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(this, "El precio para distribuidor debe ser númerico", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            txtDistribuidor.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "Ingrece el precio para distribuidor", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        txtDistribuidor.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, "El precio para el público debe ser positivo", "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    txtPublico.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "El precio para el público debe ser númerico", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtPublico.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Ingrece el precio para el público", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPublico.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Ingrece la cantidad", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        nudCantidad.Focus();
                    }
                }
                else
                {
                    MessageBox.Show(this, "Ingrece la descripción", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescripcion.Focus();
                }
            }
            else
            {
                MessageBox.Show(this, "Ingrece el código", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
            }
            return false;
        }

        private void Limpiar()
        {
            foreach (Control control in Controls)
                if (control is TextBox)
                    control.Text = "";
            nudCantidad.Value = 0;
            txtCantidadMinima.Value = 0;
            txtImageName.Text = "Seleccionar imagen...";
            pcbImage.Image.Dispose();
            pcbImage.Image= Image.FromFile(@"Imagenes\No_image.png");
            dgvProductos.Rows.Clear();
        }

        private void Buscador(string consulta)
        {
            dgvProductos.Rows.Clear();
            List<string[]> lista;
            lista = Sql.BuscarDatos(consulta);
            for (int i = 0; i < lista.Count; i++)
                dgvProductos.Rows.Add(lista[i]);
        }

        private void uscProductos_Resize(object sender, EventArgs e)
        {
            redimension.Redimencionar();
        }


        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBusqueda_Click(sender, new EventArgs());
        }

        private void btnBusqueda_Click(object sender, EventArgs e)
        {
            Buscador("SELECT codigo, nombre FROM productos " +
                "WHERE (codigo LIKE '%" + txtBusqueda.Text + "%' OR nombre LIKE '%" + txtBusqueda.Text + "%') AND eliminado = 0");
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                List<string[]> lista = Sql.BuscarDatos("SELECT codigo, nombre, " +
                    "existencia,cantidad_minima, precio_compra, precio_publico, precio_distribuidor,precio_minimo, imagen FROM productos WHERE codigo = '" +
                    dgvProductos.Rows[e.RowIndex].Cells["codigo"].Value + "'");
                id_Producto = lista[0][0];
                txtCodigo.Text = lista[0][0];
                txtDescripcion.Text = lista[0][1];
                nudCantidad.Value = decimal.Parse(lista[0][2]);
                txtCantidadMinima.Value = decimal.Parse(lista[0][3]);
                txtPrecioDeCompra.Text = lista[0][4];
                txtPublico.Text = lista[0][5];
                txtDistribuidor.Text = lista[0][6];
                txtPrecioDeVentaMinimo.Text = lista[0][7];
                String imageName = lista[0][8];
                pcbImage.Image.Dispose();
                pcbImage.Image = Image.FromFile(@"Imagenes\" + imageName);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void uscProductos_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string imagen = openFileDialog1.FileName;
                    txtImageName.Text = imagen;
                    pcbImage.Image.Dispose();
                    pcbImage.Image = Image.FromFile(imagen);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
        }
    }
}