using System;
using GestionTiendaUTN.Models;

namespace GestionTiendaUTN.Utils
{
    // CLASE ESTÁTICA
    // No se instancia → contiene solo comportamiento
    // Responsabilidad: generar la salida (impresión) del ticket
    public static class Ticket
    {
        // MÉTODO ESTÁTICO
        // Genera e imprime un ticket de venta
        public static void Generar(Producto p, int cantidad, decimal total)
        {
            Console.WriteLine("\n------ TICKET ------");

            // Datos del producto
            Console.WriteLine($"Producto: {p.Nombre}");

            // POLIMORFISMO:
            // Se llama al método del tipo real (Televisor, Heladera, Lavarropas)
            Console.WriteLine($"Precio Unitario: ${p.CalcularPrecioFinal():N2}");

            Console.WriteLine($"Cantidad: {cantidad}");

            // Total ya viene calculado desde afuera (Program)
            Console.WriteLine($"Total: ${total:N2}");

            // Texto fijo del comprobante
            Console.WriteLine($"Consumidor Final");

            Console.WriteLine("--------------------\n");
        }
    }
}