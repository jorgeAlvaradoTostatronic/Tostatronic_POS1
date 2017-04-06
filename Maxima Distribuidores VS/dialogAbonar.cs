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
    public partial class dialogAbonar : Form
    {
        private float total;
        private float abonado;
        private bool cancelar;
        private bool pagado;
        private float nuevoAbono;
        private float abonadoActual;

        public bool Cancelar
        {
            get { return cancelar; }
        }

        public bool Pagado
        {
            get { return pagado; }
        }

        public float Abono
        {
            get { return nuevoAbono; }
        }
        public float Abonado
        {
            get { return abonadoActual; }
        }

        public dialogAbonar(float total, float abonado)
        {
            InitializeComponent();
            this.total = total;
            this.abonado = abonado;
            txtTotal.Text = total.ToString();
            txtAbonado.Text = abonado.ToString();
            txtPendiente.Text = (total - abonado).ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cancelar = true;
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            float.TryParse(txtPago.Text, out nuevoAbono);
            abonadoActual = nuevoAbono;
            nuevoAbono += abonado;
            if (nuevoAbono < total)
            {
                DialogResult confirmacion = MessageBox.Show(this, "No se esta finalizando la venta.\nAún seguira quedando pendiente.\n¿Desea continuar aún asi?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmacion == DialogResult.Yes)
                    pagado = false;
                else
                    return;
            }
            else
                pagado = true;
            if (nuevoAbono > total)
            {
                nuevoAbono = total;
                float temp1 = 0;
                float.TryParse(txtAbonado.Text, out temp1);
                abonadoActual = total - temp1;
            }
            cancelar = false;
            this.Close();
        }

        private void txtPago_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtPago.Text))
                {
                    btnAceptar.Enabled = false;
                    txtPendiente.Text = (total - abonado).ToString();
                }
                else
                {
                    btnAceptar.Enabled = true;
                    float pendiente = total - (float.Parse(txtPago.Text) + abonado);
                    if (pendiente > 0)
                        txtPendiente.Text = pendiente.ToString();
                    else
                        txtPendiente.Text = "0";
                }
            }
            catch (Exception) { }
        }

        private void txtPago_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar == '.')
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
