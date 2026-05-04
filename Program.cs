using System;
using GestionTiendaUTN.Models;
using GestionTiendaUTN.Services;
using GestionTiendaUTN.UI;
using GestionTiendaUTN.Utils;

namespace GestionTiendaUTN
{
    class Program
    {
        static void Main() 
        {
          // Creación de sucursales
            Sucursal sucursalCentro = new Sucursal("Centro");
            Sucursal sucursalNorte = new Sucursal("Norte");

            // Carga inicial de productos en cada sucursal
            sucursalCentro.AgregarProducto(new Televisor(101, "Smart TV", "Samsung", 500000, 10, 55, "Smart"));
            sucursalCentro.AgregarProducto(new Heladera(102, "Frio Plus", "Samsung", 800000, 5, "No Frost", 400));
            sucursalNorte.AgregarProducto(new Lavarropas(201, "Carga Front", "Whirlpool", 400000, 8, 9, "Automatico"));

            bool salir = false;

            while (!salir)
            {
                Console.Clear(); // 🔹 Limpia consola automáticamente
                MenuUI.MostrarMenuPrincipal();

                string opcion = Console.ReadLine() ?? ""; // Evitar null con operador de fusión
                 // Manejo de opciones del menú principal
                switch (opcion)
                {
                    case "1": EjecutarMenu(sucursalCentro); break;
                    case "2": EjecutarMenu(sucursalNorte); break;
                    case "0": salir = true; break;
                }
            }
        }

        // =========================
        // MENÚ POR SUCURSAL
        // =========================
        static void EjecutarMenu(Sucursal s)
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                MenuUI.MostrarMenuAcciones(s.Nombre);

                string accion = Console.ReadLine() ?? "";

                switch (accion)
                {
                    case "1": MenuCargar(s); break;
                    case "2": s.ListarProductos(); Pausa(); break;
                    case "3": MenuVender(s); break;
                    case "4": s.MostrarVentas(); Pausa(); break;
                    case "5": volver = true; break;
                }
            }
        }

        // =========================
        // CARGAR PRODUCTO
        // =========================
        static void MenuCargar(Sucursal s)
        {
            try
            {
                Console.Clear();

                // Validación de código único para el nuevo producto
                if (!LeerEntero("Cod: ", out int c)) return;

                if (s.BuscarPorCodigo(c) != null)
                {
                    Console.WriteLine("Código duplicado.");
                    Pausa();
                    return;
                }

                Console.Write("Tipo (1:TV, 2:Hel, 3:Lav): ");
                string t = Console.ReadLine() ?? "";

                Console.Write("Nombre: ");
                string n = Console.ReadLine() ?? "";

                Console.Write("Marca: ");
                string m = Console.ReadLine() ?? "";

                if (!LeerDecimal("Precio: ", out decimal p)) return;
                if (!LeerEntero("Stock: ", out int st)) return;

                // Carga específica según tipo de producto
                 // TELEVISOR
                if (t == "1")
                {
                    if (!LeerEntero("Pulgadas: ", out int pulg)) return;

                    Console.Write("Tipo Pantalla (LED/Smart): ");
                    string tipoPantalla = Console.ReadLine() ?? "";

                    s.AgregarProducto(new Televisor(c, n, m, p, st, pulg, tipoPantalla));
                }
                // HELADERA
                else if (t == "2")
                {
                    Console.Write("Tipo (Freezer/No Frost): ");
                    string tipo = Console.ReadLine() ?? "";

                    if (!LeerEntero("Capacidad (Litros): ", out int litros)) return;

                    s.AgregarProducto(new Heladera(c, n, m, p, st, tipo, litros));
                }
                // LAVARROPAS
                else if (t == "3")
                {
                    if (!LeerEntero("Carga Kg: ", out int kg)) return;

                    Console.Write("Tipo (Automatico/Semi): ");
                    string tipo = Console.ReadLine() ?? "";

                    s.AgregarProducto(new Lavarropas(c, n, m, p, st, kg, tipo));
                }

                Console.WriteLine("Cargado correctamente.");
            }
            catch
            {
                Console.WriteLine("Error en datos.");
            }

            Pausa();
        }

        // =========================
        // VENDER PRODUCTO
        // =========================
        static void MenuVender(Sucursal s)
{
    try
    {
        Console.Clear();

        if (!LeerEntero("Código: ", out int cod)) return;

        Producto? p = s.BuscarPorCodigo(cod);

        if (p != null && p.Stock > 0)
        {
            if (!LeerEntero("Cantidad: ", out int cant)) return;

            if (cant <= p.Stock)
            {
                p.Stock -= cant;

                decimal precioFinal = p.CalcularPrecioFinal();
                decimal total = precioFinal * cant;

                // ✅ REGISTRAR VENTA EN LA SUCURSAL
                s.RegistrarVenta(new Venta(p.Nombre, precioFinal, cant));

                // ✅ GENERAR TICKET
                Ticket.Generar(p, cant, total);
            }
            else
            {
                Console.WriteLine("Stock insuficiente.");
            }
        }
        else
        {
            Console.WriteLine("Producto no encontrado.");
        }
    }
    catch
    {
        Console.WriteLine("Error.");
    }

    Pausa();
}

        // =========================
        // MÉTODOS AUXILIARES (STATIC)
        // =========================

        // Evita excepciones al leer enteros, muestra mensaje de error y pausa
        static bool LeerEntero(string mensaje, out int valor)
        {
            Console.Write(mensaje);
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out valor))
            {
                Console.WriteLine("Número inválido.");
                Pausa();
                return false;
            }
            return true;
        }

        // Evita excepciones al leer decimales, muestra mensaje de error y pausa
        static bool LeerDecimal(string mensaje, out decimal valor)
        {
            Console.Write(mensaje);
            string input = Console.ReadLine() ?? "";

            if (!decimal.TryParse(input, out valor))
            {
                Console.WriteLine("Número inválido.");
                Pausa();
                return false;
            }
            return true;
        }

        // Pausa la consola hasta que el usuario presione una tecla
        static void Pausa()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}