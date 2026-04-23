using System;

namespace GestionTiendaUTN.Models
{
    // =========================
    // HERENCIA
    // =========================
    // Heladera hereda de Producto → reutiliza atributos y métodos comunes
    public class Heladera : Producto
    {
        // Propiedades específicas de Heladera
        public string Tipo { get; set; }
        public int CapacidadLitros { get; set; }

        // =========================
        // CONSTRUCTOR
        // =========================
        // Llama al constructor de la clase base (Producto)
        public Heladera(int c, string n, string m, decimal p, int s, string tipo, int litros)
            : base(c, n, m, p, s) // reutiliza inicialización base
        {
            Tipo = tipo;
            CapacidadLitros = litros;
        }

        // POLIMORFISMO (override)
        // Implementa el método abstracto definido en Producto
        // Cada producto define su propia lógica de precio final
        public override decimal CalcularPrecioFinal()
        {
            // Regla de negocio:
            // Si la marca es Samsung → aplica recargo del 21%
            if (Marca.ToUpper() == "SAMSUNG")
                return Precio * 1.21m;

            // Caso general → precio sin cambios
            return Precio;
        }

        // POLIMORFISMO (override de método virtual)
        public override void MostrarDetalles()
        {
            // Reutiliza lo común definido en Producto
            base.MostrarDetalles();

            // Agrega información específica de Heladera
            Console.WriteLine($" | Heladera: {Tipo} - {CapacidadLitros}L");
        }
    }
}