using System;
using System.Collections.Generic;
using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Ports;
using MiAppHexagonal.Infrastructure.Mysql;
using MySql.Data.MySqlClient;

namespace MiAppHexagonal.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ConexionSingleton _conexion;

        public PedidoRepository(string connectionString)
        {
            _conexion = ConexionSingleton.Instancia(connectionString);
        }

        public List<Pedido> ObtenerTodos()
        {
            var pedidos = new List<Pedido>();
            var connection = _conexion.ObtenerConexion();

            string query = "SELECT id, cliente_id, estado, total FROM Pedido";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                pedidos.Add(new Pedido
                {
                    Id = reader.GetInt32("id"),
                    ClienteId = reader.GetInt32("cliente_id"),
                    Estado = reader.GetString("estado"),
                    Total = reader.GetDecimal("total")
                });
            }

            return pedidos;
        }

        public Pedido? ObtenerPorId(int id)
        {
            var connection = _conexion.ObtenerConexion();
            var pedido = new Pedido();

            // Obtener información del pedido
            string queryPedido = "SELECT id, cliente_id, estado, total FROM Pedido WHERE id = @id";
            using (var cmd = new MySqlCommand(queryPedido, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;

                pedido.Id = reader.GetInt32("id");
                pedido.ClienteId = reader.GetInt32("cliente_id");
                pedido.Estado = reader.GetString("estado");
                pedido.Total = reader.GetDecimal("total");
            }

            // Obtener detalles del pedido
            string queryDetalles = "SELECT id, producto_id, cantidad, precio_unitario, subtotal FROM DetallePedido WHERE pedido_id = @pedido_id";
            using (var cmd = new MySqlCommand(queryDetalles, connection))
            {
                cmd.Parameters.AddWithValue("@pedido_id", id);
                using var reader = cmd.ExecuteReader();
                pedido.Detalles = new List<DetallePedido>();

                while (reader.Read())
                {
                    pedido.Detalles.Add(new DetallePedido
                    {
                        Id = reader.GetInt32("id"),
                        ProductoId = reader.GetInt32("producto_id"),
                        Cantidad = reader.GetInt32("cantidad"),
                        PrecioUnitario = reader.GetDecimal("precio_unitario"),
                        Subtotal = reader.GetDecimal("subtotal")
                    });
                }
            }

            return pedido;
        }

        public void Crear(Pedido pedido)
        {
            var connection = _conexion.ObtenerConexion();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insertar pedido
                string queryPedido = "INSERT INTO Pedido (cliente_id, estado, total) VALUES (@cliente_id, @estado, @total)";
                using (var cmd = new MySqlCommand(queryPedido, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@cliente_id", pedido.ClienteId);
                    cmd.Parameters.AddWithValue("@estado", pedido.Estado);
                    cmd.Parameters.AddWithValue("@total", pedido.Total);
                    cmd.ExecuteNonQuery();
                    pedido.Id = (int)cmd.LastInsertedId;
                }

                // Insertar detalles
                string queryDetalle = "INSERT INTO DetallePedido (pedido_id, producto_id, cantidad, precio_unitario, subtotal) VALUES (@pedido_id, @producto_id, @cantidad, @precio_unitario, @subtotal)";
                foreach (var detalle in pedido.Detalles)
                {
                    using var cmd = new MySqlCommand(queryDetalle, connection, transaction);
                    cmd.Parameters.AddWithValue("@pedido_id", pedido.Id);
                    cmd.Parameters.AddWithValue("@producto_id", detalle.ProductoId);
                    cmd.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmd.Parameters.AddWithValue("@precio_unitario", detalle.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@subtotal", detalle.Subtotal);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Actualizar(Pedido pedido)
        {
            var connection = _conexion.ObtenerConexion();
            string query = "UPDATE Pedido SET estado = @estado, total = @total WHERE id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@estado", pedido.Estado);
            cmd.Parameters.AddWithValue("@total", pedido.Total);
            cmd.Parameters.AddWithValue("@id", pedido.Id);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            var connection = _conexion.ObtenerConexion();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Eliminar detalles primero
                string queryDetalles = "DELETE FROM DetallePedido WHERE pedido_id = @id";
                using (var cmd = new MySqlCommand(queryDetalles, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                // Eliminar pedido
                string queryPedido = "DELETE FROM Pedido WHERE id = @id";
                using (var cmd = new MySqlCommand(queryPedido, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
} 