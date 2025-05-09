using System;
using System.Collections.Generic;

namespace MiAppHexagonal.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public decimal Total { get; set; }
    public List<DetallePedido> Detalles { get; set; } = new();

    // Composición con Detalles
    /*Agregación (1:N): Pedido
        Un cliente tiene múltiples pedidos. Si el cliente se elimina, los pedidos podrían 
        seguir existiendo si hay lógica para migrarlos o archivarlos.
    */
}
