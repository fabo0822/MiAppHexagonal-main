using System.Collections.Generic;
using MiAppHexagonal.Domain.Entities;

namespace MiAppHexagonal.Domain.Ports
{
    public interface IPedidoRepository : IGenericRepository<Pedido>
    {
        Pedido? ObtenerPorId(int id);
    }
} 