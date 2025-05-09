using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Ports;

namespace MiAppHexagonal.Application.Services
{

    public class ProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }
        public void CrearProducto(Producto producto)
        {
            _repo.Crear(producto);
        }

        public void MostrarTodos()
        {
            var productos = _repo.ObtenerTodos();
            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos registrados.");
                return;
            }

            Console.WriteLine("\nLista de Productos:");
            Console.WriteLine("ID\tNombre\t\tStock\tPrecio");
            Console.WriteLine("----------------------------------------");
            foreach (var producto in productos)
            {
                Console.WriteLine($"{producto.Id}\t{producto.Nombre}\t\t{producto.Stock}\t${producto.Precio:F2}");
            }
        }

        public void ActualizarProducto(int id, string nombre, int stock, decimal precio)
        {
            var producto = new Producto
            {
                Id = id,
                Nombre = nombre,
                Stock = stock,
                Precio = precio
            };
            _repo.Actualizar(producto);
            Console.WriteLine("Producto actualizado correctamente.");
        }

        public void EliminarProducto(int id)
        {
            _repo.Eliminar(id);
            Console.WriteLine("Producto eliminado correctamente.");
        }

        public Producto? ObtenerProducto(int id)
        {
            return _repo.ObtenerPorId(id);
        }
    }
}