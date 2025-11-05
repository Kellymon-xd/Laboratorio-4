using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laboratorio_4
{
    public class AppContexto : ApplicationContext
    {
        public static AppContexto Instancia { get; private set; }
        public ConexionBD Conexion { get; private set; }
        public Dictionary<string, string> UsuarioActual { get; set; }

        public AppContexto()
        {
            Instancia = this;
            Conexion = new ConexionBD();
            MostrarLogin();
        }

        private void MostrarLogin()
        {
            var login = new frmLogIn();

            login.FormClosed += (s, e) =>
            {
                // Si no hay usuario logueado => se cierra toda la app
                if (UsuarioActual == null)
                {
                    ExitThread();
                }
                else
                {
                    MostrarMenuPorRol();
                }
            };

            login.Show();
        }

        private void MostrarMenuPorRol()
        {
            Form menu = null;
            string rol = UsuarioActual["id_rol"];

            switch (rol)
            {
                case "1":
                    menu = new Menu_Farmaceutico(UsuarioActual);
                    break;
                case "2":
                    menu = new Menu_Cliente(UsuarioActual);
                    break;
                default:
                    MessageBox.Show("Rol no reconocido.");
                    UsuarioActual = null;
                    MostrarLogin();
                    return;
            }

            // Si el usuario cierra sesión, se vuelve al login
            menu.FormClosed += (s, e) =>
            {
                if (UsuarioActual == null)
                {
                    MostrarLogin();
                }
                else
                {
                    ExitThread();
                }
            };

            menu.Show();
        }

        public void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
