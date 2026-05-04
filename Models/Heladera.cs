using System;

namespace GestionTiendaUTN.Models
{
    // Clase para representar una heladera como producto en la tienda
    public class Heladera : Producto
    {
        // Propiedades específicas de la heladera
        public string Tipo { get; set; }
        public int CapacidadLitros { get; set; }

        public Heladera(int c, string n, string m, decimal p, int s, string tipo, int litros)
            : base(c, n, m, p, s)
        {
            Tipo = tipo;
            CapacidadLitros = litros;
        }

        public override decimal CalcularPrecioFinal()
        {
            // Ejemplo de lógica: Samsung paga IVA
            if (Marca.ToUpper() == "SAMSUNG")
                return Precio * 1.21m;

            return Precio;
        }

        public override void MostrarDetalles()
        {
            base.MostrarDetalles();
            Console.WriteLine($" | Heladera: {Tipo} - {CapacidadLitros}L");
        }
    }
}