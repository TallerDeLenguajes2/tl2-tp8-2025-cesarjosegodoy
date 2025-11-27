using Microsoft.Data.Sqlite;
using SistemaVentas.Web.Models;


namespace SistemaVentas.Web.Repository
{


    public class ProductoRepository
    {
        private string _connectionString = "Data Source=Db/Tienda.db;"; // base de datos

        // - - - - - Alta
        public int Alta(Producto producto)
        {

            if (string.IsNullOrWhiteSpace(producto.Descripcion))
                throw new InvalidOperationException("La descripción no puede ser null o vacía.");

            if (producto.Precio <= 0)
                throw new InvalidOperationException("El precio debe ser mayor a cero.");
                
            int nuevoId = 0;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Productos (Descripcion, Precio) VALUES (@desc, @prec); SELECT last_insert_rowid();";


                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@desc", producto.Descripcion);
                    command.Parameters.AddWithValue("@prec", producto.Precio);
                    nuevoId = Convert.ToInt32(command.ExecuteScalar());
                }

            }

            return nuevoId;

        }
        // - - - - 
        public List<Producto> GetAll()
        {
            var productos = new List<Producto>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            const string query = "SELECT idProducto, Descripcion, Precio FROM Productos";
            using var command = new SqliteCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                productos.Add(new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetDecimal(2)
                });
            }

            return productos;
        }


        // 
        public Producto? GetById(int id)
        {
            using var Connection = new SqliteConnection(_connectionString);
            Connection.Open();

            var query = "SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id";
            var command = new SqliteCommand(query, Connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Producto
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetDecimal(2)
                };
            }
            return null;

        }


        /*public bool Eliminar(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var query = "DELETE FROM Productos WHERE idProducto=@id";
            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }*/


        public void Eliminar(int _id)
        {
            string query = "DELETE FROM Productos WHERE IDProducto = @id";

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", _id);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }



        /* public bool Modificar(int id, Producto prod)
         {
             using var connection = new SqliteConnection(_connectionString);
             connection.Open();

             var query = "UPDATE Productos SET Descripcion=@d, Precio=@p WHERE idProducto=@id";
             using var cmd = new SqliteCommand(query, connection);
             cmd.Parameters.AddWithValue("@id", id);
             cmd.Parameters.AddWithValue("@d", prod.Descripcion);
             cmd.Parameters.AddWithValue("@p", prod.Precio);

             return cmd.ExecuteNonQuery() > 0;
         }*/

        public void Modificar(Producto nuevo)
        {
            string query = "UPDATE Productos SET Descripcion = @nuevaDes, Precio = @nuevoPrecio WHERE IDProducto = @id";

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@nuevaDes", nuevo.Descripcion);
                command.Parameters.AddWithValue("@nuevoPrecio", nuevo.Precio);
                command.Parameters.AddWithValue("@id", nuevo.IdProducto);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }











    }
}






