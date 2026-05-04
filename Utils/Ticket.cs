using System;
using GestionTiendaUTN.Models;

namespace GestionTiendaUTN.Utils
{
    // Clase estática para generar un ticket de compra
    public static class Ticket
    {
        public static void Generar(Producto p, int cantidad, decimal total)
        {
            Console.WriteLine("\n------ TICKET ------");
            Console.WriteLine($"Producto: {p.Nombre}");
            Console.WriteLine($"Precio Unitario: ${p.CalcularPrecioFinal():N2}");
            Console.WriteLine($"Cantidad: {cantidad}");
            Console.WriteLine($"Total: ${total:N2}");
            Console.WriteLine("Consumidor Final");
            Console.WriteLine("--------------------\n");
        }
    }
}