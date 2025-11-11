using System;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public partial class frmRegistroCliente : Form
    {
        private UsuariosDAO u;

        public frmRegistroCliente()
        {
            InitializeComponent();
            u = new UsuariosDAO(AppContexto.Instancia.Conexion);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string password = txtContraseña.Text.Trim();

            try
            {
                (bool exito, string mensaje) resultado = u.CrearUsuario(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtCorreo.Text.Trim(),
                    password,
                    2 // Rol cliente
                );

                if (resultado.exito)
                {
                    MessageBox.Show(resultado.mensaje, "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show(resultado.mensaje, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblNombre_Click(object sender, EventArgs e)
        {

        }

        private void lblApellido_Click(object sender, EventArgs e)
        {

        }

        private void lblCorreo_Click(object sender, EventArgs e)
        {

        }

        private void lblContraseña_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
