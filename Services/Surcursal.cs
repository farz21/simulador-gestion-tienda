using System;
using System.Collections.Generic;
using GestionTiendaUTN.Models;

namespace GestionTiendaUTN.Services
{
    public class Sucursal
    {
        // Nombre identificativo de la sucursal
        public string Nombre { get; set; }

        // Cada sucursal tiene su propio inventario y registro de ventas
        private List<Producto> inventario = new List<Producto>();
        private List<Venta> ventas = new List<Venta>(); // Historial de ventas

        // Constructor para inicializar la sucursal con un nombre
        public Sucursal(string nombre)
        {
            Nombre = nombre;
        }

            // Agrega un producto al inventario de la sucursal
        public void AgregarProducto(Producto p) => inventario.Add(p);

        public void ListarProductos() // Muestra el inventario actual de la sucursal
        {
            Console.WriteLine($"\n--- Inventario {Nombre} ---");

            if (inventario.Count == 0) // Si no hay productos, se muestra un mensaje
            {
                Console.WriteLine("Sin stock.");
                return;
            }

            foreach (var p in inventario) // Recorre cada producto y muestra sus detalles
                p.MostrarDetalles();
        }

         // Busca un producto por su código en el inventario de la sucursal
        public Producto? BuscarPorCodigo(int codigo) 
{
    return inventario.Find(p => p.Codigo == codigo);
}

        // Registra una venta realizada en la sucursal, agregándola al historial de ventas
       public void RegistrarVenta(Venta v)
             {
               ventas.Add(v);
             }
        // Muestra el historial de ventas realizadas en la sucursal
        public void MostrarVentas()
        {
            Console.WriteLine($"\n--- Ventas {Nombre} ---");

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas.");
                return;
            }

            foreach (var v in ventas)
                v.Mostrar();
        }
    }
}