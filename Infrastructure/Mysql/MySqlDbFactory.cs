using System;
using MiAppHexagonal.Domain.Factory;
using MiAppHexagonal.Domain.Ports;
using MiAppHexagonal.Infrastructure.Repositories;

namespace MiAppHexagonal.Infrastructure.Mysql;

public class MySqlDbFactory : IDbFactory
{
    private readonly string _connectionString;

    public MySqlDbFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IClienteRepository CrearClienteRepository()
    {
        return new lmpClienteRepository(_connectionString);
    }

    public IProductoRepository CrearProductoRepository()
    {
        return new ProductoRepository(_connectionString);
    }

    public IPedidoRepository CrearPedidoRepository()
    {
        return new PedidoRepository(_connectionString);
    }
}
