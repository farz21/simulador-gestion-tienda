using System;
using GestionTiendaUTN.Models;
using GestionTiendaUTN.Services;
using GestionTiendaUTN.UI;
using GestionTiendaUTN.Utils;
using GestionTiendaUTN.Repositories;
using System.Collections.Generic;

namespace GestionTiendaUTN
{
    class Program
    {
        static void Main()
        {
            bool salir = false;
            // =========================
            // CREACIÓN DE SUCURSALES
            // =========================
            Sucursal sucursalCentro = new Sucursal("Centro");
            Sucursal sucursalNorte = new Sucursal("Norte");

            // =========================================
            // CARGAR PRODUCTOS DESDE MYSQL
            // =========================================
            ProductoRepository repo = new ProductoRepository(); // Instancia del repositorio para acceder a la base de datos

            List<Producto> productosDB = repo.ObtenerProductos(); // Obtenemos la lista de productos desde la base de datos

            foreach (Producto p in productosDB) // Recorremos cada producto obtenido
            {
                // Según el código decidimos sucursal
                if (p.Codigo < 200)
                {
                    sucursalCentro.AgregarProducto(p);
                }
                else // Si el código es 200 o más, va a la sucursal Norte
                {
                    sucursalNorte.AgregarProducto(p);
                }
            }

            // =========================
            // MENÚ PRINCIPAL
            // =========================
            while (!salir)
            {
                Console.Clear();

                MenuUI.MostrarMenuPrincipal();

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        EjecutarMenu(sucursalCentro);
                        break;

                    case "2":
                        EjecutarMenu(sucursalNorte);
                        break;

                    case "0":
                        salir = true;
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        Pausa();
                        break;
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
                    case "1":
                        MenuCargar(s);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine($"--- PRODUCTOS EN SUCURSAL {s.Nombre.ToUpper()} ---\n");
                        s.ListarProductos();
                        Pausa();
                        break;
                    case "3":
                        MenuModificar(s); // Nueva opción
                        break;
                    case "4":
                        MenuEliminar(s); // Nueva opción
                        break;
                    case "5":
                        MenuVender(s);
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine($"--- VENTAS EN SUCURSAL {s.Nombre.ToUpper()} ---\n");
                        s.MostrarVentas();
                        Pausa();
                        break;
                    case "7": // Cambiamos el volver al número 7
                    case "0":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("\nOpción inválida.");
                        Pausa();
                        break;
                }
            }
        }

        // =========================
        // CARGAR PRODUCTO
        // =========================
        static void MenuCargar(Sucursal s)
        {
            ProductoRepository repo = new ProductoRepository();
            try
            {
                Console.Clear();

                // =========================
                // VALIDAR CÓDIGO ÚNICO
                // =========================
                int c;

                while (true)
                {
                    c = LeerEntero("Cod: ");

                    // Si el código NO existe en esta sucursal, se permite continuar
                    if (s.BuscarPorCodigo(c) == null)
                    {
                        break;
                    }

                    Console.WriteLine("Código duplicado en esta sucursal. Ingrese otro.");
                }

                // =========================
                // VALIDAR TIPO DE PRODUCTO
                // =========================
                string t;

                while (true)
                {
                    Console.Write("Tipo (1:TV, 2:Hel, 3:Lav): ");

                    t = Console.ReadLine() ?? "";

                    if (t == "1" || t == "2" || t == "3")
                    {
                        break;
                    }

                    Console.WriteLine("Ingrese una opción válida.");
                }

                // =========================
                // DATOS GENERALES
                // =========================
                Console.Write("Nombre: ");
                string n = Console.ReadLine() ?? "";

                decimal p = LeerDecimal("Precio: ");
                int st = LeerEntero("Stock: ");

                // =========================
                // TELEVISOR
                // =========================
                if (t == "1")
                {
                    int pulg = LeerEntero("Pulgadas: ");

                    string tipoPantalla;

                    while (true)
                    {
                        Console.Write("Tipo Pantalla (LED/Smart): ");

                        tipoPantalla = Console.ReadLine() ?? "";
                        tipoPantalla = tipoPantalla.ToUpper();

                        if (tipoPantalla == "LED" || tipoPantalla == "SMART")
                        {
                            break;
                        }

                        Console.WriteLine("Ingrese las opciones disponibles (LED/Smart)");
                    }
                    // Creamos el objeto televisor (ya no le pasamos "Sin Marca")
                    Televisor tv = new Televisor(
                       c, n, p, st, pulg, tipoPantalla
                    );

                    int idSucursal = s.Nombre == "Centro" ? 1 : 2;

                    // PRIMERO a la base de datos
                    repo.AgregarProducto(tv, idSucursal);
                    // SI TODO SALE BIEN, a la memoria
                    s.AgregarProducto(tv);
                }

                // =========================
                // HELADERA
                // =========================
                else if (t == "2")
                {
                    string tipo;

                    while (true)
                    {
                        Console.Write("Tipo (Freezer/No Frost): ");

                        tipo = Console.ReadLine() ?? "";
                        tipo = tipo.ToUpper();

                        if (tipo == "FREEZER" || tipo == "NO FROST")
                        {
                            break;
                        }

                        Console.WriteLine("Ingrese las opciones disponibles (Freezer/No Frost)");
                    }

                    int litros = LeerEntero("Capacidad (Litros): ");

                    // Creamos el objeto heladera (ya no le pasamos "Sin Marca")
                    Heladera h = new Heladera(
                     c, n, p, st, tipo, litros
                    );

                    int idSucursal = s.Nombre == "Centro" ? 1 : 2;

                    // PRIMERO a la base de datos
                    repo.AgregarProducto(h, idSucursal);
                    // SI TODO SALE BIEN, a la memoria
                    s.AgregarProducto(h);
                }

                // =========================
                // LAVARROPAS
                // =========================
                else if (t == "3")
                {
                    int kg = LeerEntero("Carga Kg: ");

                    string tipo;

                    while (true)
                    {
                        Console.Write("Tipo (Automatico/Semi): ");

                        tipo = Console.ReadLine() ?? "";
                        tipo = tipo.ToUpper();

                        if (tipo == "AUTOMATICO" || tipo == "SEMI")
                        {
                            break;
                        }

                        Console.WriteLine("Ingrese las opciones disponibles (Automatico/Semi)");
                    }
                    // Creamos el objeto lavarropas (ya no le pasamos "Sin Marca")
                    Lavarropas l = new Lavarropas(
                      c, n, p, st, kg, tipo
                    );

                    int idSucursal = s.Nombre == "Centro" ? 1 : 2;

                    // PRIMERO a la base de datos
                    repo.AgregarProducto(l, idSucursal);
                    // SI TODO SALE BIEN, a la memoria
                    s.AgregarProducto(l);
                }

                Console.WriteLine("\nProducto cargado correctamente.");
            }
            catch (Exception ex)
            {
                // Ahora los errores de la BD caen acá y los vas a poder ver
                Console.WriteLine("\nError:");
                Console.WriteLine(ex.Message);
            }

            Pausa();
        }

        // =========================
        // MODIFICAR PRODUCTO
        // =========================
        static void MenuModificar(Sucursal s)
        {
            Console.Clear();
            Console.WriteLine($"--- MODIFICAR PRODUCTO EN {s.Nombre.ToUpper()} ---");

            int cod = LeerEntero("Ingrese el código del producto a modificar: ");
            Producto? p = s.BuscarPorCodigo(cod);

            if (p != null)
            {
                Console.WriteLine($"\nEncontrado: {p.Nombre} | Precio actual: ${p.Precio} | Stock actual: {p.Stock}");

                decimal nuevoPrecio = LeerDecimal("Nuevo Precio: ");
                int nuevoStock = LeerEntero("Nuevo Stock: ");

                try
                {
                    ProductoRepository repo = new ProductoRepository();
                    // 1. Guardamos en la base de datos
                    repo.ModificarProducto(cod, nuevoPrecio, nuevoStock);

                    // 2. Si la BD no dio error, actualizamos la memoria local
                    p.Precio = nuevoPrecio;
                    p.Stock = nuevoStock;

                    Console.WriteLine("\n¡Producto modificado con éxito!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("\nProducto no encontrado en esta sucursal.");
            }
            Pausa();
        }

        // =========================
        // ELIMINAR PRODUCTO
        // =========================
        static void MenuEliminar(Sucursal s)
        {
            Console.Clear();
            Console.WriteLine($"--- ELIMINAR PRODUCTO EN {s.Nombre.ToUpper()} ---");

            int cod = LeerEntero("Ingrese el código del producto a eliminar: ");
            Producto? p = s.BuscarPorCodigo(cod);

            if (p != null)
            {
                Console.WriteLine($"\nEncontrado: {p.Nombre}");
                Console.Write("¿Está seguro que desea eliminarlo permanentemente? (S/N): ");
                string confirmacion = Console.ReadLine()?.ToUpper() ?? "";

                if (confirmacion == "S")
                {
                    try
                    {
                        ProductoRepository repo = new ProductoRepository();
                        // 1. Borramos de la base de datos
                        repo.EliminarProducto(cod);

                        // 2. Borramos de la lista en memoria
                        s.RemoverProductoLocal(p);

                        Console.WriteLine("\n¡Producto eliminado con éxito!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            else
            {
                Console.WriteLine("\nProducto no encontrado en esta sucursal.");
            }
            Pausa();
        }

        // =========================
        // VENDER PRODUCTO
        // =========================
        static void MenuVender(Sucursal s)
        {
            Console.Clear();
            Console.WriteLine($"--- VENDER PRODUCTO EN {s.Nombre.ToUpper()} ---");

            int cod = LeerEntero("Código del producto a vender: ");

            Producto? p = s.BuscarPorCodigo(cod);

            if (p != null && p.Stock > 0)
            {
                Console.WriteLine($"\nProducto: {p.Nombre} | Stock disponible: {p.Stock}");
                int cant = LeerEntero("Cantidad: ");

                if (cant <= p.Stock)
                {
                    decimal precioFinal = p.CalcularPrecioFinal();
                    decimal total = precioFinal * cant;
                    int idSucursal = s.Nombre == "Centro" ? 1 : 2;

                    try
                    {
                        ProductoRepository repo = new ProductoRepository();

                        // 1. INTENTAMOS GUARDAR EN LA BASE DE DATOS (TRANSACCIÓN)
                        repo.RegistrarVentaBD(idSucursal, p.Codigo, cant, precioFinal);

                        // 2. SI LA BD FUE EXITOSA, ACTUALIZAMOS LA MEMORIA LOCAL
                        p.Stock -= cant;

                        // Registrar venta localmente (historial de memoria)
                        s.RegistrarVenta(
                            new Venta(p.Nombre, precioFinal, cant)
                        );

                        Console.WriteLine("\n¡Venta registrada exitosamente en el sistema!");

                        // Generar ticket
                        Ticket.Generar(p, cant, total);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError al procesar la venta:");
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("\nStock insuficiente para realizar la venta.");
                }
            }
            else
            {
                Console.WriteLine("\nProducto no encontrado o sin stock disponible.");
            }

            Pausa();
        }

        // =========================
        // MÉTODOS AUXILIARES
        // =========================

        // LEE ENTEROS Y OBLIGA A INGRESAR
        // UN NÚMERO VÁLIDO
        static int LeerEntero(string mensaje)
        {
            int valor;

            while (true)
            {
                Console.Write(mensaje);

                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out valor))
                {
                    return valor;
                }

                Console.WriteLine("Número inválido. Intente nuevamente.");
            }
        }

        // LEE DECIMALES Y OBLIGA A INGRESAR
        // UN VALOR VÁLIDO
        static decimal LeerDecimal(string mensaje)
        {
            decimal valor;

            while (true)
            {
                Console.Write(mensaje);

                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out valor))
                {
                    return valor;
                }

                Console.WriteLine("Número inválido. Intente nuevamente.");
            }
        }

        // =========================
        // PAUSA DE CONSOLA
        // =========================
        static void Pausa()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
    }
}