using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public partial class dialogCantidad : Form
    {
        private decimal cantidad, maximaCantidad;
        private float precio, descuento;

        public decimal Cantidad
        {
            get { return cantidad; }
        }

        public dialogCantidad(string producto, decimal maximaCantidad, float precio, float descuento)
        {
            InitializeComponent();
            this.Text = producto;
            nudCantidad.Maximum = maximaCantidad;
            this.maximaCantidad = maximaCantidad;
            this.precio = precio;
            this.descuento = descuento;
            lblMaximo.Text = "Máximo disponible: " + maximaCantidad.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            cantidad = nudCantidad.Value;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            float subtotal = precio * (float)nudCantidad.Value;
            MessageBox.Show("Total = cantidad x precio - descuento\nSubTotal = " + nudCantidad.Value.ToString()
                + " x " + precio.ToString() + "\nSubTotal = " + subtotal.ToString() + "\nDescuento = " + (subtotal * descuento / 100).ToString() +
                "\n16% I.V.A. = " + ((subtotal - subtotal * descuento / 100) * .16).ToString() +
            "\nTotal =" + ((subtotal - subtotal * descuento / 100)*1.16).ToString(), 
                "Precio calculado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
