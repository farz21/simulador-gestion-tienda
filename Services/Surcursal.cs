using System;
using System.Collections.Generic;
using GestionTiendaUTN.Models;

namespace GestionTiendaUTN.Services
{
    // COMPOSICIÓN
    // Una Sucursal "TIENE" productos (no hereda de ellos)
    // Relación: Sucursal → contiene una lista de Producto
    public class Sucursal
    {
        // Nombre de la sucursal
        public string Nombre { get; set; } = string.Empty;

        // Lista privada de productos (encapsulación)
        // Solo se puede modificar a través de métodos de la clase
        private List<Producto> inventario = new List<Producto>();

        // CONSTRUCTOR
        public Sucursal(string nombre)
        {
            Nombre = nombre;
        }

        // MÉTODO DE INSTANCIA
        // Agrega un producto al inventario
        public void AgregarProducto(Producto p) => inventario.Add(p);

        // LISTAR PRODUCTOS
        public void ListarProductos()
        {
            Console.WriteLine($"\n--- Inventario Sucursal {Nombre} ---");

            // Validación: lista vacía
            if (inventario.Count == 0)
            {
                Console.WriteLine("Sin stock.");
                return;
            }

            // POLIMORFISMO:
            // Cada producto ejecuta su propia versión de MostrarDetalles()
            foreach (var p in inventario)
                p.MostrarDetalles();
        }

        // BÚSQUEDA POR CÓDIGO
        public Producto? BuscarPorCodigo(int codigo)
        {
            // Uso de expresión lambda (búsqueda en lista)
            return inventario.Find(p => p.Codigo == codigo);
        }
    }
}