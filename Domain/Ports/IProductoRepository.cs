using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiAppHexagonal.Domain.Entities;

namespace MiAppHexagonal.Domain.Ports
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        Producto? ObtenerPorId(int id);
    }
}