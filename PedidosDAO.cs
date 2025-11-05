using Npgsql;
using System;
using System.Collections.Generic;

namespace Laboratorio_4
{
    public class PedidosDAO
    {
        private readonly ConexionBD _conexion;

        public PedidosDAO(ConexionBD conexion)
        {
            _conexion = conexion;
            _conexion.Conectar();
        }

        public string RegistrarPedido(string idCliente, int[] medicamentos, int[] cantidades)
        {
            string sql = "SELECT registrar_pedido(@p_id_cliente, @p_medicamentos, @p_cantidades)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("p_id_cliente", idCliente);
                cmd.Parameters.AddWithValue("p_medicamentos", medicamentos);
                cmd.Parameters.AddWithValue("p_cantidades", cantidades);

                return cmd.ExecuteScalar()?.ToString();
            }
        }

        public Dictionary<string, string> ObtenerPedidoPorId(int idPedido)
        {
            var datos = new Dictionary<string, string>();
            string sql = "SELECT * FROM obtener_pedido_por_id(@id_pedido)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("id_pedido", idPedido);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        datos["id_pedido"] = reader["id_pedido"].ToString();
                        datos["cliente"] = reader["cliente"].ToString();
                        datos["fecha"] = reader["fecha"].ToString();
                        datos["total"] = reader["total"].ToString();
                    }
                }
            }

            return datos.Count > 0 ? datos : null;
        }

        public List<ComboboxItem> ObtenerClientes()
        {
            var clientes = new List<ComboboxItem>();
            string sql = "SELECT id_usuario, nombre_completo FROM vista_clientes ORDER BY nombre_completo";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientes.Add(new ComboboxItem
                    {
                        Text = reader.GetString(reader.GetOrdinal("nombre_completo")),
                        Value = reader.GetString(reader.GetOrdinal("id_usuario"))
                    });
                }
            }

            return clientes;
        }

        public List<dynamic> ObtenerPedidos(string idCliente = null)
        {
            var pedidos = new List<dynamic>();
            string sql = "SELECT id_pedido, cliente, total, fecha_pedido FROM vista_pedidos_resumen";

            if (!string.IsNullOrEmpty(idCliente))
                sql += " WHERE id_cliente = @idCliente";

            sql += " ORDER BY fecha_pedido DESC";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                if (!string.IsNullOrEmpty(idCliente))
                    cmd.Parameters.AddWithValue("idCliente", idCliente);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new
                        {
                            IdPedido = reader.GetInt32(0),
                            Cliente = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            Fecha = reader.GetDateTime(3)
                        });
                    }
                }
            }

            return pedidos;
        }

        public List<dynamic> ObtenerDetallePedido(int idPedido)
        {
            var detalle = new List<dynamic>();
            string sql = "SELECT medicamento, cantidad, subtotal FROM detalle_pedido_por_id(@idPedido)";

            using (var cmd = new NpgsqlCommand(sql, _conexion.Conexion))
            {
                cmd.Parameters.AddWithValue("idPedido", idPedido);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detalle.Add(new
                        {
                            Medicamento = reader.GetString(0),
                            Cantidad = reader.GetInt32(1),
                            Subtotal = reader.GetDecimal(2)
                        });
                    }
                }
            }

            return detalle;
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public override string ToString() => Text;
    }
}