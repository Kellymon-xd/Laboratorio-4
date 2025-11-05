using Npgsql;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Laboratorio_4
{
    public class ConexionBD : IDisposable
    {
        private string cadenaConexion;
        private NpgsqlConnection conexion;

        public NpgsqlConnection Conexion => conexion;

        public void Conectar()
        {
            try
            {
                LecturaXML();

                if (conexion == null)
                    conexion = new NpgsqlConnection(cadenaConexion);

                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar a la base de datos: " + ex.Message);
            }
        }

        public void Desconectar()
        {
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
        }

        public void Dispose()
        {
            Desconectar();
            conexion?.Dispose();
        }

        // ======= Métodos privados =======
        private void LecturaXML()
        {
            string rutaXML = "../../Config/ConfigDB.xml";
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement raiz;

            try
            {
                xmlDocument.Load(rutaXML);
                raiz = xmlDocument.DocumentElement;

                string servidor = Decode64(raiz.SelectSingleNode("host").InnerText);
                string puerto = Decode64(raiz.SelectSingleNode("puerto").InnerText);
                string baseDatos = Decode64(raiz.SelectSingleNode("dbname").InnerText);
                string usuario = Decode64(raiz.SelectSingleNode("usuario").InnerText);
                string contra = Decode64(raiz.SelectSingleNode("password").InnerText);

                cadenaConexion = $"Host={servidor};Port={puerto};Database={baseDatos};Username={usuario};Password={contra}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo ConfigDB.xml: " + ex.Message);
            }
        }

        private string Decode64(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
    }
}
