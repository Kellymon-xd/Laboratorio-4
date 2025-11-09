using Laboratorio_4.Modelos;
using Npgsql;
using System;
using System.Collections.Generic;

namespace Laboratorio_4
{
    public class MedicamentosDAO
    {
        private readonly ConexionBD _conexion;


        public MedicamentosDAO(ConexionBD conexion)
        {
            _conexion = conexion;
            _conexion.Conectar();
        }

        public List<Medicamento> ObtenerTodos()
        {
            var lista = new List<Medicamento>();
            string sql = "SELECT * FROM vista_inventario";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var med = new Medicamento
                    {
                        IdMedicamento = reader["id_medicamento"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        Imagen = reader["imagen"].ToString(),
                        CantidadDisponible = Convert.ToInt32(reader["cantidad_disponible"]),
                        PrecioUnitario = Convert.ToDecimal(reader["precio_unitario"])
                    };
                    lista.Add(med);
                }
            }

            return lista;
        }

        public string ModificarMedicamento(string id, string nombre = null, string imagen = null, int? cantidad = null, decimal? precio = null)
        {
            string resultado = "";
            string sql = "SELECT modificar_medicamento(@p_id, @p_nombre, @p_imagen, @p_cantidad, @p_precio)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("p_id", int.Parse(id));
                cmd.Parameters.AddWithValue("p_nombre", nombre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("p_imagen", imagen ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("p_cantidad", cantidad.HasValue ? (object)cantidad.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("p_precio", precio.HasValue ? (object)precio.Value : DBNull.Value);

                resultado = cmd.ExecuteScalar()?.ToString();
            }

            return resultado;
        }



        public string AgregarMedicamento(string nombre, string imagen, int cantidad, decimal precio)
        {
            string sql = "SELECT agregar_medicamento(@p_nombre, @p_imagen, @p_cantidad, @p_precio)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("p_nombre", nombre);
                cmd.Parameters.AddWithValue("p_imagen", imagen);
                cmd.Parameters.AddWithValue("p_cantidad", cantidad);
                cmd.Parameters.AddWithValue("p_precio", precio);

                return cmd.ExecuteScalar()?.ToString();
            }
        }
        public string EliminarMedicamento(string id)
        {
            try
            {
                string query = "SELECT eliminar_medicamento(@id)";
                using (var cmd = new NpgsqlCommand(query, _conexion.Conexion))
                {
                    cmd.Parameters.AddWithValue("@id", int.Parse(id));

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        return result.ToString();
                    else
                        return "No se recibió respuesta del servidor.";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar medicamento: " + ex.Message;
            }
        }
    }
}