using System;
using MiAppHexagonal.Domain.Ports;

namespace MiAppHexagonal.Domain.Factory;

public interface IDbFactory
{
    IClienteRepository CrearClienteRepository();
    IProductoRepository CrearProductoRepository();
    IPedidoRepository CrearPedidoRepository();
}
