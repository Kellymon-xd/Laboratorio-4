using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio3
{
    public partial class Dialogo : Form
    {
        public string ValorIngresado { get; set; }

        public Dialogo()
        {
            InitializeComponent();
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ValorIngresado = txtInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Titulo
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public void AplicarEstiloForm(Form formulario)
        {
            this.BackColor = formulario.BackColor;
            this.ForeColor = formulario.ForeColor;

            foreach (Control ctl in this.Controls)
            {
                if (ctl is Label || ctl is TextBox)
                {
                    ctl.BackColor = formulario.BackColor;
                    ctl.ForeColor = formulario.ForeColor;
                }
                else if (ctl is Button btn)
                {
                    btn.BackColor = formulario.BackColor;
                    btn.ForeColor = formulario.ForeColor;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = formulario.ForeColor; 
                    btn.UseVisualStyleBackColor = false;
                }
            }
        }
        public void AplicarEstiloBoton(Dialogo dlg, Color colorBoton)
        {
            Color colorFuente = this.ForeColor;
            Color colorHover = ControlPaint.Light(colorBoton, 0.2f);

            btnAceptar.BackColor = colorBoton;
            btnAceptar.ForeColor = colorFuente;
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.FlatAppearance.BorderColor = colorFuente;
            btnAceptar.UseVisualStyleBackColor = false;

            btnAceptar.MouseEnter += (s, e) => btnAceptar.BackColor = colorHover;
            btnAceptar.MouseLeave += (s, e) => btnAceptar.BackColor = colorBoton;
        }

    }
}
