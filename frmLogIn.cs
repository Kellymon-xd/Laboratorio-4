using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string email = txtCorreo.Text.Trim();
            string password = txtContraseña.Text.Trim();

            try
            {
                var conexion = AppContexto.Instancia.Conexion;
                conexion.Conectar();

                var usuariosDAO = new UsuariosDAO(conexion);
                var usuario = usuariosDAO.LoginUsuario(email, password);

                if (usuario["id_usuario"] != null)
                {
                    AppContexto.Instancia.UsuarioActual = usuario;
                    this.Close(); // ✅ Cierra el login (AppContexto abrirá el menú)
                }
                else
                {
                    MessageBox.Show(usuario["mensaje"],
                        "Inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frmR = new frmRegistroCliente();
            frmR.ShowDialog();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
