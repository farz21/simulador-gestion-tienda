using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using GestionTiendaUTN.Models;
using GestionTiendaUTN.Database;
using System.Security.AccessControl;

namespace GestionTiendaUTN.Repositories
{
  public class ProductoRepository
  {
    private Conexion conexionDB = new Conexion();

    // =========================================
    // LISTAR PRODUCTOS (CON JOIN Y USING)
    // =========================================
    public List<Producto> ObtenerProductos()
    {
      List<Producto> productos = new List<Producto>();

      try
      {
        using (MySqlConnection conexion = conexionDB.ObtenerConexion())
        {
          conexion.Open();

          // CONSULTA SQL CON JOIN: Traemos datos generales y específicos
          string query = @"
                        SELECT p.*, t.Pulgadas, t.TipoPantalla, h.CapacidadLitros, h.Tipo as TipoHeladera, l.CargaKg, l.Tipo as TipoLavarropas
                        FROM Producto p
                        LEFT JOIN Televisor t ON p.IdProducto = t.IdProducto
                        LEFT JOIN Heladera h ON p.IdProducto = h.IdProducto
                        LEFT JOIN Lavarropas l ON p.IdProducto = l.IdProducto";

          MySqlCommand cmd = new MySqlCommand(query, conexion);

          using (MySqlDataReader reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              int codigo = reader.GetInt32("Codigo");
              string nombre = reader.GetString("Nombre");
              decimal precio = reader.GetDecimal("Precio");
              int stock = reader.GetInt32("Stock");
              string tipo = reader.GetString("TipoProducto");

              Producto producto;

              // SE ELIMINÓ EL PARÁMETRO "Marca" DE LAS INSTANCIACIONES
              if (tipo == "Televisor")
              {
                producto = new Televisor(
                    codigo, nombre, precio, stock,
                    reader.GetInt32("Pulgadas"), reader.GetString("TipoPantalla")
                );
              }
              else if (tipo == "Heladera")
              {
                producto = new Heladera(
                    codigo, nombre, precio, stock,
                    reader.GetString("TipoHeladera"), reader.GetInt32("CapacidadLitros")
                );
              }
              else
              {
                producto = new Lavarropas(
                    codigo, nombre, precio, stock,
                    reader.GetInt32("CargaKg"), reader.GetString("TipoLavarropas")
                );
              }

              productos.Add(producto);
            }
          }
        }
      }
      catch (MySqlException ex)
      {
        Console.WriteLine("Error al listar: " + ex.Message);
      }

      return productos;
    }

    // =========================================
    // AGREGAR PRODUCTO A MYSQL (CON TRANSACCIONES)
    // =========================================
    public void AgregarProducto(Producto producto, int idSucursal)
    {
      try
      {
        using (MySqlConnection conexion = conexionDB.ObtenerConexion())
        {
          conexion.Open();

          using (MySqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              // 1. INSERTAR EN TABLA BASE: Producto (Sin columna Marca)
              string queryProducto = @"
                                INSERT INTO Producto (Codigo, Nombre, Precio, Stock, TipoProducto, IdSucursal)
                                VALUES (@codigo, @nombre, @precio, @stock, @tipo, @sucursal)";

              MySqlCommand cmd = new MySqlCommand(queryProducto, conexion, transaccion);

              cmd.Parameters.AddWithValue("@codigo", producto.Codigo);
              cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
              cmd.Parameters.AddWithValue("@precio", producto.Precio);
              cmd.Parameters.AddWithValue("@stock", producto.Stock);
              cmd.Parameters.AddWithValue("@sucursal", idSucursal);

              string tipoStr = producto is Televisor ? "Televisor" :
                               producto is Heladera ? "Heladera" : "Lavarropas";
              cmd.Parameters.AddWithValue("@tipo", tipoStr);

              cmd.ExecuteNonQuery();

              // 2. OBTENER EL ID GENERADO
              int idProducto = Convert.ToInt32(cmd.LastInsertedId);

              // 3. INSERTAR DATOS ESPECÍFICOS SEGÚN EL TIPO
              if (producto is Televisor tv)
              {
                string queryTV = "INSERT INTO Televisor (IdProducto, Pulgadas, TipoPantalla) VALUES (@id, @pulg, @pant)";
                MySqlCommand cmdTV = new MySqlCommand(queryTV, conexion, transaccion);
                cmdTV.Parameters.AddWithValue("@id", idProducto);
                cmdTV.Parameters.AddWithValue("@pulg", tv.Pulgadas);
                cmdTV.Parameters.AddWithValue("@pant", tv.TipoPantalla);
                cmdTV.ExecuteNonQuery();
              }
              else if (producto is Heladera h)
              {
                string queryH = "INSERT INTO Heladera (IdProducto, CapacidadLitros, Tipo) VALUES (@id, @litros, @tipoH)";
                MySqlCommand cmdH = new MySqlCommand(queryH, conexion, transaccion);
                cmdH.Parameters.AddWithValue("@id", idProducto);
                cmdH.Parameters.AddWithValue("@litros", h.CapacidadLitros);
                cmdH.Parameters.AddWithValue("@tipoH", h.Tipo);
                cmdH.ExecuteNonQuery();
              }
              else if (producto is Lavarropas l)
              {
                string queryL = "INSERT INTO Lavarropas (IdProducto, CargaKg, Tipo) VALUES (@id, @kg, @tipoL)";
                MySqlCommand cmdL = new MySqlCommand(queryL, conexion, transaccion);
                cmdL.Parameters.AddWithValue("@id", idProducto);
                cmdL.Parameters.AddWithValue("@kg", l.CargaKg);
                cmdL.Parameters.AddWithValue("@tipoL", l.Tipo);
                cmdL.ExecuteNonQuery();
              }

              transaccion.Commit();
            }
            catch (Exception ex)
            {
              transaccion.Rollback();
              throw new Exception("Error en la base de datos (Transacción cancelada): " + ex.Message);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("No se pudo conectar a MySQL: " + ex.Message);
      }
    }
    // =========================================
    // ELIMINAR PRODUCTO
    // =========================================
    public void EliminarProducto(int codigo)
    {
      try
      {
        using (MySqlConnection conexion = conexionDB.ObtenerConexion())
        {
          conexion.Open();
          string query = "DELETE FROM Producto WHERE Codigo = @codigo";
          MySqlCommand cmd = new MySqlCommand(query, conexion);
          cmd.Parameters.AddWithValue("@codigo", codigo);
          cmd.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error al eliminar en la BD: " + ex.Message);
      }
    }
    // =========================================
    // MODIFICAR PRODUCTO
    // =========================================
    public void ModificarProducto(int codigo, decimal nuevoPrecio, int nuevoStock)
    {
      try
      {
        using (MySqlConnection conexion = conexionDB.ObtenerConexion())
        {
          conexion.Open();
          // Actualizamos los datos en la tabla principal
          string query = "UPDATE Producto SET Precio = @precio, Stock = @stock WHERE Codigo = @codigo";
          MySqlCommand cmd = new MySqlCommand(query, conexion);
          cmd.Parameters.AddWithValue("@precio", nuevoPrecio);
          cmd.Parameters.AddWithValue("@stock", nuevoStock);
          cmd.Parameters.AddWithValue("@codigo", codigo);
          cmd.ExecuteNonQuery(); // Ejecutamos la actualización
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error al modificar en la BD: " + ex.Message);
      }
    }
    // =========================================
    // REGISTRAR VENTA (CON TRANSACCIÓN OBLIGATORIA)
    // =========================================
    public void RegistrarVentaBD(int idSucursal, int codigoProducto, int cantidad, decimal precioUnitario)
    {
      try
      {
        using (MySqlConnection conexion = conexionDB.ObtenerConexion())
        {
          conexion.Open();

          // Iniciamos la transacción obligatoria del TP2
          using (MySqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              // 1. REGISTRAR LA CABECERA DE LA VENTA
              string queryVenta = "INSERT INTO Venta (IdSucursal) VALUES (@idSucursal)";
              MySqlCommand cmdVenta = new MySqlCommand(queryVenta, conexion, transaccion);
              cmdVenta.Parameters.AddWithValue("@idSucursal", idSucursal);
              cmdVenta.ExecuteNonQuery();

              // Obtenemos el ID de la venta recién creada
              int idVenta = Convert.ToInt32(cmdVenta.LastInsertedId);

              // 2. BUSCAR EL IdProducto INTERNO EN LA BD
              // En C# manejas el "Codigo", pero la tabla DetalleVenta pide el "IdProducto"
              string queryIdProd = "SELECT IdProducto FROM Producto WHERE Codigo = @codigo";
              MySqlCommand cmdIdProd = new MySqlCommand(queryIdProd, conexion, transaccion);
              cmdIdProd.Parameters.AddWithValue("@codigo", codigoProducto);
              int idProducto = Convert.ToInt32(cmdIdProd.ExecuteScalar());

              // 3. REGISTRAR EL DETALLE DE LA VENTA
              string queryDetalle = @"INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario) 
                                      VALUES (@idVenta, @idProducto, @cantidad, @precio)";
              MySqlCommand cmdDetalle = new MySqlCommand(queryDetalle, conexion, transaccion);
              cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
              cmdDetalle.Parameters.AddWithValue("@idProducto", idProducto);
              cmdDetalle.Parameters.AddWithValue("@cantidad", cantidad);
              cmdDetalle.Parameters.AddWithValue("@precio", precioUnitario);
              cmdDetalle.ExecuteNonQuery();

              // 4. DESCONTAR EL STOCK AUTOMÁTICAMENTE EN BD
              string queryStock = "UPDATE Producto SET Stock = Stock - @cantidad WHERE IdProducto = @idProducto";
              MySqlCommand cmdStock = new MySqlCommand(queryStock, conexion, transaccion);
              cmdStock.Parameters.AddWithValue("@cantidad", cantidad);
              cmdStock.Parameters.AddWithValue("@idProducto", idProducto);
              cmdStock.ExecuteNonQuery();

              // 5. CONFIRMAR LA OPERACIÓN SI TODO FUE CORRECTO
              transaccion.Commit();
            }
            catch (Exception ex)
            {
              // Si falla cualquiera de los 4 pasos anteriores, se deshace TODO
              transaccion.Rollback();
              throw new Exception("Error en la transacción de venta (Rollback ejecutado): " + ex.Message);
            }
          }
        }
      }
      // Si no se pudo conectar o hubo un error general, se lanza esta excepción
      catch (Exception ex)
      {
        throw new Exception("No se pudo conectar para registrar la venta: " + ex.Message);
      }
    }

  }
}