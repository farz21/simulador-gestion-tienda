using System;
using GestionTiendaUTN.Models;   // Clases del dominio (Producto y derivados)
using GestionTiendaUTN.Services; // Lógica de negocio (Sucursal)
using GestionTiendaUTN.UI;       // Interfaz de usuario (menús)
using GestionTiendaUTN.Utils;    // Utilidades (Ticket)

namespace GestionTiendaUTN
{
  /* Trabajo Practico N1 Programacion 3
     Alumnos:  
     Coronel Gordillo Fabrizio Gabriel
     Santillan Lescano Pilar
     Duhalde Caetano
     Comision: 5
  */

  class Program
  {
    // =========================
    // VARIABLES ESTÁTICAS
    // =========================
    // Se comparten en toda la aplicación (no dependen de objetos)
    public static decimal RecaudacionGlobal = 0;
    public static int VentasTotales = 0;

    // =========================
    // MÉTODO ESTÁTICO
    // =========================
    // Registra cada venta en variables globales
    public static void RegistrarVenta(decimal monto)
    {
      VentasTotales++;             // incrementa cantidad de ventas
      RecaudacionGlobal += monto;  // acumula el dinero total
    }

    // MÉTODO PRINCIPAL (Main)
    // Punto de entrada del programa
    static void Main()
    {
      // CREACIÓN DE OBJETOS (instancias de sucursales)
      Sucursal sucursalCentro = new Sucursal("Centro");
      Sucursal sucursalNorte = new Sucursal("Norte");

      // CARGA INICIAL DE DATOS
      // Se usa POLIMORFISMO: todos son Producto pero de distintos tipos
      sucursalCentro.AgregarProducto(new Televisor(101, "Smart TV", "Samsung", 500000, 10, 55, "Smart"));
      sucursalCentro.AgregarProducto(new Heladera(102, "Frio Plus", "Samsung", 800000, 5, "No Frost", 400));
      sucursalNorte.AgregarProducto(new Lavarropas(201, "Carga Front", "Whirlpool", 400000, 8, 9, "Automatico"));

      bool salir = false;

      // BUCLE PRINCIPAL DEL SISTEMA
      while (!salir)
      {
        // Llamada a método ESTÁTICO de la UI (Interfaz de Usuario)
        MenuUI.MostrarMenuPrincipal();

        string? opcion = Console.ReadLine();

        // CONTROL DE FLUJO
        switch (opcion)
        {
          case "1": EjecutarMenu(sucursalCentro); break;
          case "2": EjecutarMenu(sucursalNorte); break;
          case "3": MostrarTotales(); break;
          case "0": salir = true; break;
        }
      }
    }

    // MÉTODO ESTÁTICO (REPORTE)
    static void MostrarTotales()
    {
      Console.WriteLine("\n--- REPORTE GLOBAL ---");
      Console.WriteLine($"Ventas: {VentasTotales} | Recaudación: ${RecaudacionGlobal:N2}");
    }

    // MÉTODO ESTÁTICO (MENÚ POR SUCURSAL)
    static void EjecutarMenu(Sucursal s)
    {
      bool volver = false;

      // Bucle interno para cada sucursal
      while (!volver)
      {
        MenuUI.MostrarMenuAcciones(s.Nombre); // método estático

        string? accion = Console.ReadLine();

        switch (accion)
        {
          case "1": MenuCargar(s); break;
          case "2": s.ListarProductos(); break; // método de instancia
          case "3": MenuVender(s); break;
          case "4": volver = true; break;
        }
      }
    }

    // MÉTODO ESTÁTICO (ALTA DE PRODUCTOS)
    static void MenuCargar(Sucursal s)
    {
      try
      {
        Console.Write("Cod: ");
        int c = int.Parse(Console.ReadLine() ?? "0");

        // VALIDACIÓN: evita códigos duplicados
        if (s.BuscarPorCodigo(c) != null)
        {
          Console.WriteLine("Código duplicado.");
          return;
        }

        Console.Write("Tipo (1:TV, 2:Hel, 3:Lav): ");
        string t = Console.ReadLine() ?? "";

        // DATOS GENERALES DEL PRODUCTO
        Console.Write("Nombre: "); string n = Console.ReadLine() ?? "";
        Console.Write("Marca: "); string m = Console.ReadLine() ?? "";
        Console.Write("Precio: "); decimal p = decimal.Parse(Console.ReadLine() ?? "0");
        Console.Write("Stock: "); int st = int.Parse(Console.ReadLine() ?? "0");

        // CREACIÓN SEGÚN TIPO (tipo "factory" manual)
        if (t == "1")
        {
          Console.Write("Pulgadas: ");
          int pulg = int.Parse(Console.ReadLine() ?? "0");

          Console.Write("Tipo Pantalla (LED/Smart): ");
          string tipoPantalla = Console.ReadLine() ?? "";

          s.AgregarProducto(new Televisor(c, n, m, p, st, pulg, tipoPantalla));
        }
        else if (t == "2")
        {
          Console.Write("Tipo (Freezer/No Frost): ");
          string tipo = Console.ReadLine() ?? "";

          Console.Write("Capacidad (Litros): ");
          int litros = int.Parse(Console.ReadLine() ?? "0");

          s.AgregarProducto(new Heladera(c, n, m, p, st, tipo, litros));
        }
        else if (t == "3")
        {
          Console.Write("Carga Kg: ");
          int kg = int.Parse(Console.ReadLine() ?? "0");

          Console.Write("Tipo (Automatico/Semi): ");
          string tipo = Console.ReadLine() ?? "";

          s.AgregarProducto(new Lavarropas(c, n, m, p, st, kg, tipo));
        }

        Console.WriteLine("Cargado correctamente.");
      }
      catch
      {
        // Captura errores de parseo o entrada inválida
        Console.WriteLine("Error en datos.");
      }
    }

    // MÉTODO ESTÁTICO (VENTA)
    static void MenuVender(Sucursal s)
    {
      try
      {
        Console.Write("Código: ");
        int cod = int.Parse(Console.ReadLine() ?? "0");

        // BÚSQUEDA DEL PRODUCTO
        Producto? p = s.BuscarPorCodigo(cod);

        if (p != null && p.Stock > 0)
        {
          Console.Write("Cantidad: ");
          int cant = int.Parse(Console.ReadLine() ?? "0");

          if (cant <= p.Stock)
          {
            // ACTUALIZA STOCK
            p.Stock -= cant;

            // POLIMORFISMO: cada producto calcula distinto su precio final
            decimal total = p.CalcularPrecioFinal() * cant;

            // REGISTRA VENTA GLOBAL
            RegistrarVenta(total);

            // CLASE ESTÁTICA → no se instancia
            Ticket.Generar(p, cant, total);
          }
          else
            Console.WriteLine("Stock insuficiente.");
        }
        else
          Console.WriteLine("Producto no encontrado.");
      }
      catch
      {
        Console.WriteLine("Error.");
      }
    }
  }
}