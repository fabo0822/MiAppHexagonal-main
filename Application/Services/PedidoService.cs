using System;
using System.Collections.Generic;
using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Ports;

namespace MiAppHexagonal.Application.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _repo;

        public PedidoService(IPedidoRepository repo)
        {
            _repo = repo;
        }

        public void MostrarTodos()
        {
            var pedidos = _repo.ObtenerTodos();
            if (pedidos.Count == 0)
            {
                Console.WriteLine("No hay pedidos registrados.");
                return;
            }

            Console.WriteLine("\nLista de Pedidos:");
            Console.WriteLine("ID\tCliente\t\tEstado\t\tTotal");
            Console.WriteLine("----------------------------------------");
            foreach (var pedido in pedidos)
            {
                Console.WriteLine($"{pedido.Id}\t{pedido.ClienteId}\t\t{pedido.Estado}\t\t${pedido.Total:F2}");
            }
        }

        public void MostrarDetalles(int id)
        {
            var pedido = _repo.ObtenerPorId(id);
            if (pedido == null)
            {
                Console.WriteLine("Pedido no encontrado.");
                return;
            }

            Console.WriteLine($"\nDetalles del Pedido #{id}");
            Console.WriteLine($"Cliente: {pedido.ClienteId}");
            Console.WriteLine($"Estado: {pedido.Estado}");
            Console.WriteLine($"Total: ${pedido.Total:F2}");
            Console.WriteLine("\nProductos:");
            Console.WriteLine("Producto\tCantidad\tPrecio Unit.\tSubtotal");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var detalle in pedido.Detalles)
            {
                Console.WriteLine($"{detalle.ProductoId}\t{detalle.Cantidad}\t\t${detalle.PrecioUnitario:F2}\t${detalle.Subtotal:F2}");
            }
        }

        public void CrearPedido(Pedido pedido, List<DetallePedido> detalles)
        {
            pedido.Detalles = detalles;
            pedido.Total = detalles.Sum(d => d.Subtotal);
            _repo.Crear(pedido);
            Console.WriteLine("Pedido creado correctamente.");
        }

        public void CancelarPedido(int id)
        {
            var pedido = _repo.ObtenerPorId(id);
            if (pedido == null)
            {
                Console.WriteLine("Pedido no encontrado.");
                return;
            }

            pedido.Estado = "Cancelado";
            _repo.Actualizar(pedido);
            Console.WriteLine("Pedido cancelado correctamente.");
        }
    }
} 