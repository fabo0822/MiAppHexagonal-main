using System;
using MiAppHexagonal.Domain.Entities;
using MiAppHexagonal.Domain.Ports;
using MiAppHexagonal.Infrastructure.Mysql;
using MySql.Data.MySqlClient;

namespace MiAppHexagonal.Infrastructure.Repositories;

public class lmpClienteRepository : IGenericRepository<Cliente>, IClienteRepository
{
    private readonly ConexionSingleton _conexion;

    public lmpClienteRepository(string connectionString)
    {
        _conexion = ConexionSingleton.Instancia(connectionString);
    }

    public List<Cliente> ObtenerTodos()
    {
        var clientes = new List<Cliente>();
        var connection = _conexion.ObtenerConexion();

        string query = "SELECT id, nombre FROM Cliente";
        using var cmd = new MySqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            clientes.Add(new Cliente
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre")
            });
        }

        return clientes;
    }

    public void Crear(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "INSERT INTO Cliente (nombre) VALUES (@nombre)";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Cliente cliente)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "UPDATE Cliente SET nombre = @nombre WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@nombre", cliente.Nombre);
        cmd.Parameters.AddWithValue("@id", cliente.Id);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        var connection = _conexion.ObtenerConexion();
        string query = "DELETE FROM Cliente WHERE id = @id";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}
