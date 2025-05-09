using System;

namespace MiAppHexagonal.Domain.Entities;

public class Direccion
{
    /*
    Asociación (1:1): Direccion
    Relación de asociación simple. Cliente tiene una referencia a Direccion, 
    pero esta puede existir por sí sola.
    */
    public int Id { get; set; }
    public string? Ciudad { get; set; }
    public string? Calle { get; set; }
    public string? CodigoPostal { get; set; }
}
