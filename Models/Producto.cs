using System;

namespace GestionTiendaUTN.Models
{
    // =========================
    // ABSTRACCIÓN
    // =========================
    // Clase base abstracta → no se puede instanciar directamente
    // Sirve como plantilla para los distintos tipos de productos
    public abstract class Producto
    {
        // =========================
        // PROPIEDADES COMUNES
        // =========================
        // Atributos compartidos por todos los productos
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        // CONSTRUCTOR
        // Inicializa los atributos comunes
        public Producto(int codigo, string nombre, decimal precio, int stock)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }

        // POLIMORFISMO (MÉTODO ABSTRACTO)
        // Obliga a las clases hijas a implementar su propia lógica
        // Cada producto calcula su precio final de forma distinta
        public abstract decimal CalcularPrecioFinal();

        // MÉTODO VIRTUAL
        // Tiene implementación base pero puede ser sobrescrito (override)
        public virtual void MostrarDetalles()
        {
            Console.Write(
                $"Cod: {Codigo} | " +
                $"{Nombre.PadRight(12)} | " +
                $"Stock: {Stock.ToString().PadLeft(3)} | " +
                $"Precio: ${Precio:N2}"
            );
        }
    }
}