using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maxima_Distribuidores_VS
{
    public class Redimension
    {
        private List<ControlInicial> controles;
        private SizeF dimension;
        private UserControl userControl;

        private struct ControlInicial
        {
            public Size Dimension;
            public Point Localizacion;
            public float Letra;

            public ControlInicial(Size dimension, Point localizacion, float letra)
            {
                Dimension = dimension;
                Localizacion = localizacion;
                Letra = letra;
            }
        }

        public Redimension(UserControl userControl)
        {
            this.userControl = userControl;
            dimension = userControl.Size;
            controles = new List<ControlInicial>();
            for (int i = 0; i < userControl.Controls.Count; i++)
                controles.Add(new ControlInicial(userControl.Controls[i].Size, 
                    userControl.Controls[i].Location, userControl.Controls[i].Font.SizeInPoints));
        }

        public void Redimencionar()
        {
            try
            {
                float ancho = this.userControl.Width / dimension.Width;
                float alto = this.userControl.Height / dimension.Height;
                SizeF escala = new SizeF(ancho, alto);

                for (int i = 0; i < userControl.Controls.Count; i++)
                {
                    userControl.Controls[i].Size = new Size((int)(controles[i].Dimension.Width * escala.Width),
                        (int)(controles[i].Dimension.Height * escala.Height));
                    userControl.Controls[i].Location = new Point((int)(controles[i].Localizacion.X * escala.Width),
                        (int)(controles[i].Localizacion.Y * escala.Height));
                    userControl.Controls[i].Font = new Font("Microsoft Sans Serif", controles[i].Letra *
                        (Math.Max(ancho, alto) - Math.Min(ancho, alto) + 1));
                }
            }
            catch (Exception) { }
        }
    }
}
