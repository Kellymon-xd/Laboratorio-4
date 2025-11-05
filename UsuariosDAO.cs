using Npgsql;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Laboratorio_4
{
    public class UsuariosDAO
    {
        private readonly ConexionBD _conexion;

        public UsuariosDAO(ConexionBD conexion)
        {
            _conexion = conexion;
            _conexion.Conectar();
        }

        public Dictionary<string, string> LoginUsuario(string email, string password)
        {
            var usuario = new Dictionary<string, string>();

            string sql = "SELECT * FROM login_usuario(@p_email, @p_password)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("p_email", email);
                cmd.Parameters.AddWithValue("p_password", HashSHA256(password));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Leer con GetOrdinal (obtiene el índice de cada columna)
                        int colIdUsuario = reader.GetOrdinal("id_usuario");
                        int colNombre = reader.GetOrdinal("nombre");
                        int colApellido = reader.GetOrdinal("apellido");
                        int colIdRol = reader.GetOrdinal("id_rol");
                        int colMensaje = reader.GetOrdinal("mensaje");

                        // Verificar si alguna columna viene NULL (DBNull)
                        usuario["id_usuario"] = reader.IsDBNull(colIdUsuario) ? null : reader.GetString(colIdUsuario);
                        usuario["nombre"] = reader.IsDBNull(colNombre) ? null : reader.GetString(colNombre);
                        usuario["apellido"] = reader.IsDBNull(colApellido) ? null : reader.GetString(colApellido);
                        usuario["id_rol"] = reader.IsDBNull(colIdRol) ? null : reader.GetInt32(colIdRol).ToString();
                        usuario["mensaje"] = reader.IsDBNull(colMensaje) ? null : reader.GetString(colMensaje);
                    }
                }
            }

            return usuario;
        }


        public (bool exito, string mensaje) CrearUsuario(string nombre, string apellido, string email, string password, int idRol)
        {
            string passwordHashed = HashSHA256(password);
            string sql = "SELECT * FROM crear_usuario(@p_nombre, @p_apellido, @p_email, @p_password, @p_id_rol)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("p_nombre", nombre);
                cmd.Parameters.AddWithValue("p_apellido", apellido);
                cmd.Parameters.AddWithValue("p_email", email);
                cmd.Parameters.AddWithValue("p_password", passwordHashed);
                cmd.Parameters.AddWithValue("p_id_rol", idRol);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool exito = reader.GetBoolean(reader.GetOrdinal("exito"));
                        string mensaje = reader.GetString(reader.GetOrdinal("mensaje"));
                        return (exito, mensaje);
                    }
                }
            }

            return (false, "Error inesperado al crear el usuario.");
        }

        private string HashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();

                foreach (byte b in hash)
                    builder.Append(b.ToString("x2")); // convierte cada byte a hexadecimal

                return builder.ToString();
            }
        }

        public Dictionary<string, string> ObtenerUsuarioPorId(string idUsuario)
        {
            var datos = new Dictionary<string, string>();
            string sql = "SELECT * FROM usuarios WHERE id_usuario = @id";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("id", idUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        datos["id_usuario"] = reader["id_usuario"].ToString();
                        datos["nombre"] = reader["nombre"].ToString();
                        datos["apellido"] = reader["apellido"].ToString();
                        datos["email"] = reader["email"].ToString();
                        datos["id_rol"] = reader["id_rol"].ToString();
                    }
                }
            }

            return datos.Count > 0 ? datos : null;
        }


    }
}
