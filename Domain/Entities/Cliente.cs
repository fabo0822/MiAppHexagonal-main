using System;

namespace MiAppHexagonal.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string? Nombre { get; set; }

    // Asociación simple con Dirección (1:1)
    public Direccion? Direccion { get; set; }

    // Agregación: un cliente puede tener varios pedidos
    public List<Pedido> Pedidos { get; set; } = new();
}
