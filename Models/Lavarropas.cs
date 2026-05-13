using System;

namespace GestionTiendaUTN.Models
{
    // Clase para representar un lavarropas como producto en la tienda
    public class Lavarropas : Producto
    {
        // Propiedades específicas del lavarropas
        public int CargaKg { get; set; }
        public string Tipo { get; set; }

        public Lavarropas(int c, string n, decimal p, int s, int kg, string tipo)
            : base(c, n, p, s)
        {
            CargaKg = kg;
            Tipo = tipo;
        }
         // Se calcula el precio final aplicando un recargo del 10% para los lavarropas
        public override decimal CalcularPrecioFinal() => Precio * 1.10m;
        
        //Muestra los detalles del producto
        public override void MostrarDetalles()
        {
            base.MostrarDetalles();
            Console.WriteLine($" | Lavarropas: {CargaKg}kg - {Tipo}");
        }
    }
}