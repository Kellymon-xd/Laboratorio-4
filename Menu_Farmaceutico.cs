using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class Menu_Farmaceutico : Form
    {
        Dictionary<string, string> usuario;
        public Menu_Farmaceutico(Dictionary<string, string> usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void AbrirFormulario(Form formHijo)
        {
            foreach (Form frm in this.MdiChildren)
                frm.Close();

            formHijo.MdiParent = this;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.StartPosition = FormStartPosition.Manual;
            formHijo.Show();

            int menuHeight = 0;
            foreach (Control c in this.Controls)
            {
                if (c is MenuStrip)
                {
                    menuHeight = c.Height;
                    break;
                }
            }

            int widthDiff = this.Width - this.ClientSize.Width;
            int heightDiff = this.Height - this.ClientSize.Height;

            this.Width = formHijo.Width + widthDiff;
            this.Height = formHijo.Height + heightDiff + menuHeight;

            formHijo.Dock = DockStyle.Fill;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppContexto.Instancia.CerrarSesion();
            this.Close();

        }


        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMedicamentos frm = new frmMedicamentos();
            AbrirFormulario(frm);
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPedidos frm = new frmPedidos();
            AbrirFormulario(frm);
        }
    }
}
