using System;

namespace GestionTiendaUTN.Models
{
    // HERENCIA
    // Lavarropas hereda de Producto → reutiliza propiedades comunes
    public class Lavarropas : Producto
    {
        // Propiedades específicas del lavarropas
        public int CargaKg { get; set; }
        public string Tipo { get; set; }

        // CONSTRUCTOR
        // Inicializa tanto atributos propios como los heredados
        public Lavarropas(int c, string n, string m, decimal p, int s, int kg, string tipo)
            : base(c, n, m, p, s) // llamada al constructor de Producto
        {
            CargaKg = kg;
            Tipo = tipo;
        }

        // POLIMORFISMO (override)
        // Implementación del método abstracto definido en Producto
        // Usa sintaxis de expresión lambda (forma corta)
        public override decimal CalcularPrecioFinal() => Precio * 1.10m; 
        // Aplica un recargo del 10%

        // POLIMORFISMO (override método virtual)
        public override void MostrarDetalles()
        {
            // Muestra los datos comunes definidos en Producto
            base.MostrarDetalles();

            // Agrega los datos específicos del lavarropas
            Console.WriteLine($" | Lavarropas: {CargaKg}kg - {Tipo}");
        }
    }
}