using System;

namespace GestionTiendaUTN.Models
{
    // =========================
    // HERENCIA
    // =========================
    // Televisor hereda de Producto → reutiliza atributos comunes
    public class Televisor : Producto
    {
        // Propiedades específicas del televisor
        public int Pulgadas { get; set; }
        public string TipoPantalla { get; set; }

        // CONSTRUCTOR
        // Inicializa atributos propios y heredados
        public Televisor(int c, string n, string m, decimal p, int s, int pulg, string tipoPantalla)
            : base(c, n, m, p, s) // llamada al constructor base
        {
            Pulgadas = pulg;
            TipoPantalla = tipoPantalla;
        }


        // POLIMORFISMO (override)
        // Implementa el método abstracto de Producto
        public override decimal CalcularPrecioFinal() => Precio * 0.90m;
        // Aplica un descuento del 10%

        
        // POLIMORFISMO (override método virtual)
        public override void MostrarDetalles()
        {
            // Muestra datos comunes (Producto)
            base.MostrarDetalles();

            // Agrega información específica del televisor
            Console.WriteLine($" | TV: {Pulgadas}\" - {TipoPantalla} (Promo -10%)");
        }
    }
}