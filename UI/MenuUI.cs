using System;

namespace GestionTiendaUTN.UI
{
    // CLASE ESTÁTICA (UTILIDAD DE INTERFAZ DE USUARIO)
    // No se puede instanciar → solo contiene métodos estáticos
    // Se usa para centralizar toda la interacción visual (menús)
    public static class MenuUI
    {
        // MÉTODO ESTÁTICO
        // Muestra el menú principal del sistema
        public static void MostrarMenuPrincipal()
        {
            Console.WriteLine("\n--- SISTEMA DE GESTIÓN TIENDA UTN ---");
            Console.WriteLine("1 - Sucursal Centro");
            Console.WriteLine("2 - Sucursal Norte");
            Console.WriteLine("3 - Ver Reporte Global");
            Console.WriteLine("0 - Salir");
            Console.Write("Seleccione: ");
        }

        // MÉTODO ESTÁTICO
        // Muestra el menú de acciones para una sucursal específica
        public static void MostrarMenuAcciones(string sucursalNombre)
        {
            // ToUpper() → mejora visual (formato uniforme)
            Console.WriteLine($"\n--- SUCURSAL {sucursalNombre.ToUpper()} ---");

            Console.WriteLine("1 - Agregar producto");
            Console.WriteLine("2 - Listar productos");
            Console.WriteLine("3 - Vender producto");
            Console.WriteLine("4 - Volver");
            Console.Write("Acción: ");
        }
    }
}