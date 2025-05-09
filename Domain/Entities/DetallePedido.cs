using System;

namespace MiAppHexagonal.Domain.Entities;

/*Composición (Pedido → DetallePedido)
La clase DetallePedido no puede existir sin Pedido, por eso usamos composición.*/
public class DetallePedido
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? NombreProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total => Cantidad * PrecioUnitario;
}
