using System;

namespace GestionTiendaUTN.Models
{
    // Clase para representar una heladera como producto en la tienda
    public class Heladera : Producto
    {
        // Propiedades específicas de la heladera
        public string Tipo { get; set; }
        public int CapacidadLitros { get; set; }

        // 1. Eliminamos "string m" del constructor y no se lo pasamos al base()
        public Heladera(int c, string n, decimal p, int s, string tipo, int litros)
            : base(c, n, p, s)
        {
            Tipo = tipo;
            CapacidadLitros = litros;
        }

        public override decimal CalcularPrecioFinal()
        {
            // 2. Como no hay "Marca", buscamos si el "Nombre" incluye la palabra Samsung
            if (Nombre.ToUpper().Contains("SAMSUNG"))
            {
                return Precio * 1.21m;
            }

            return Precio;
        }

        public override void MostrarDetalles()
        {
            base.MostrarDetalles();
            Console.WriteLine($" | Heladera: {Tipo} - {CapacidadLitros}L");
        }
    }
}