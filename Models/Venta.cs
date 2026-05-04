using System;

namespace GestionTiendaUTN.Models
{
    // Clase para representar una venta realizada en la tienda
    public class Venta
    {
        public string Producto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }

   // Constructor para inicializar una venta con los detalles del producto, precio, cantidad y fecha
        public Venta(string producto, decimal precio, int cantidad)
        {
            Producto = producto;
            PrecioUnitario = precio;
            Cantidad = cantidad;
            Total = precio * cantidad;
            Fecha = DateTime.Now;
        }
        
        public void Mostrar() // Método para mostrar los detalles de la venta
        {
            Console.WriteLine($"{Fecha:g} | {Producto} | Cant: {Cantidad} | Total: ${Total:N2}");
        }
    }
}