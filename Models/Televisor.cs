using System;

namespace GestionTiendaUTN.Models
{
    // Clase para representar un televisor como producto en la tienda
    public class Televisor : Producto
    {
        // Propiedades específicas del televisor
        public int Pulgadas { get; set; }
        public string TipoPantalla { get; set; }

        public Televisor(int c, string n, decimal p, int s, int pulg, string tipoPantalla)
            : base(c, n, p, s)
        {
            Pulgadas = pulg;
            TipoPantalla = tipoPantalla;
        }
      
         // Se calcula el precio final aplicando un descuento del 10% para los televisores
        public override decimal CalcularPrecioFinal() => Precio * 0.90m;

        public override void MostrarDetalles()
        {
            base.MostrarDetalles();
            Console.WriteLine($" | TV: {Pulgadas}\" - {TipoPantalla} (Promo -10%)");
        }
    }
}