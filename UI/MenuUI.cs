using System;

namespace GestionTiendaUTN.UI
{
    public static class MenuUI
    {
        // Métodos para mostrar los menús de la aplicación
        public static void MostrarMenuPrincipal()
        {
            Console.Clear(); // Limpia pantalla automáticamente

            Console.WriteLine("--- SISTEMA TIENDA UTN ---");
            Console.WriteLine("1 - Sucursal Centro");
            Console.WriteLine("2 - Sucursal Norte");
            Console.WriteLine("0 - Salir");
            Console.Write("Opcion: ");
        }

        public static void MostrarMenuAcciones(string nombre)
        {
            Console.Clear(); // Limpia cada vez que entra al menú

            Console.WriteLine($"--- SUCURSAL {nombre.ToUpper()} ---");
            Console.WriteLine("1 - Agregar producto");
            Console.WriteLine("2 - Lista de productos");
            Console.WriteLine("3 - Vender producto");
            Console.WriteLine("4 - Ver ventas");
            Console.WriteLine("5 - Volver");
            Console.Write("Opcion: ");
        }
    }
}