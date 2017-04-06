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
    public partial class dialogVenta : Form
    {
        private bool cancelar;

        public bool Cancelar
        {
            get { return cancelar; }
        }

        public float Pago
        {
            get 
            {
                float temp = 0;
                if (float.TryParse(txtPago.Text, out temp))
                    return temp;
                else return 0;
            }
        }
        public float Total
        {
            get
            {
                float temp = 0;
                if (float.TryParse(txtTotal.Text, out temp))
                    return temp;
                else return 0;
            }
        }

        public float Descuento
        {
            get
            {
                if (String.IsNullOrEmpty(txtDescuento.Text))
                    return 0;
                else
                {
                    float temp = 0;
                    float.TryParse(txtDescuento.Text, out temp);
                    return temp;
                }
            }
        }

        public dialogVenta(string subtotal)
        {
            InitializeComponent();
            txtSubtotal.Text = subtotal.Replace("$", "");
            txtTotal.Text = txtSubtotal.Text;
        }

        private void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Pago < Total)
            {
                DialogResult confirmacion = MessageBox.Show(this, "No se esta realizando el pago en una sola exhibición.\nLa venta quedara pendiente.\n¿Desea continuar aún asi?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.No)
                    return;
            }
            cancelar = false;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
            this.Close();
        }

        private void txtDescuento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDescuento.Text))
                {
                    txtTotal.Text = txtSubtotal.Text;
                    txtDescuento.ForeColor = Color.Black;
                    if (!String.IsNullOrWhiteSpace(txtPago.Text))
                        btnAceptar.Enabled = true;
                }
                else if (float.Parse(txtDescuento.Text) > float.Parse(txtSubtotal.Text))
                {
                    txtDescuento.ForeColor = Color.Red;
                    btnAceptar.Enabled = false;
                }
                else
                {
                    txtTotal.Text = (float.Parse(txtSubtotal.Text) - float.Parse(txtDescuento.Text)).ToString("0.00");
                    txtDescuento.ForeColor = Color.Black;
                    if (!String.IsNullOrWhiteSpace(txtPago.Text))
                        btnAceptar.Enabled = true;
                }
            }
            catch (Exception) { }
        }

        private void txtPago_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtPago.Text))
                btnAceptar.Enabled = false;
            else
                btnAceptar.Enabled = true;
            txtDescuento_TextChanged(sender, new EventArgs());
        }
    }
}
