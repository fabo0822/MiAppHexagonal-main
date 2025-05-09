using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Ports;
using MiAppHexagonal.Infrastructure.Mysql;
using MySql.Data.MySqlClient;

namespace MiAppHexagonal.Infrastructure.Repositories;

public class ProductoRepository : IGenericRepository<Producto>, IProductoRepository
{
    private readonly ConexionSingleton _conexion;

    public ProductoRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public void Actualizar(Producto entity)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE Producto SET nombre = @nombre, stock = @stock, precio = @precio WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
        cmd.Parameters.AddWithValue("@stock", entity.Stock);
        cmd.Parameters.AddWithValue("@precio", entity.Precio);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        cmd.ExecuteNonQuery();
    }

    public void Crear(Producto producto)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO Producto (nombre, stock, precio) VALUES (@nombre, @stock, @precio)";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@stock", producto.Stock);
        cmd.Parameters.AddWithValue("@precio", producto.Precio);
        cmd.ExecuteNonQuery();
       
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM Producto WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre, stock, precio FROM Producto";
        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            productos.Add(new Producto
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Stock = reader.GetInt32("stock"),
                Precio = reader.GetDecimal("precio")
            });
        }

        return productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "SELECT id, nombre, stock, precio FROM Producto WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Producto
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Stock = reader.GetInt32("stock"),
                Precio = reader.GetDecimal("precio")
            };
        }

        return null;
    }
}
