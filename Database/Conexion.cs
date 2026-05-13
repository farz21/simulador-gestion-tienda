using MySql.Data.MySqlClient;

namespace GestionTiendaUTN.Database
{
    public class Conexion
    {
        // Cadena de conexión a MySQL
        private string connectionString = 
            "server=localhost;" +
            "database=ElectrodomesticosDB;" +
            "user=root;" +
            "password=1234;" +
            "port=3306;";

        // Devuelve una conexión lista para usar
        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(connectionString);
        }
    }
}