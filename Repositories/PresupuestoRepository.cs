using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SistemaVentas.Web.Models;
using System;
using MVC.Interfaces;

namespace SistemaVentas.Web.Repository
{
    public class PresupuestoRepository : IPresupuestoRepository
    {
        private readonly string _cadenaConexion = "Data Source=Db/Tienda.db;";

        // Mantenemos Acoplamiento Fuerte: El Repositorio de Presupuestos
        // instancia al Repositorio de Productos para obtener los detalles.
        private readonly ProductoRepository _productoRepo = new ProductoRepository();


        // Listar todos los presupuestos
        public List<Presupuesto> GetAllPresupuesto()
        {
            var lista = new List<Presupuesto>();

            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            const string sql = "SELECT IdPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos";

            using var cmd = new SqliteCommand(sql, conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var p = new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(0),
                    NombreDestinatario = reader.GetString(1),
                    FechaCreacion = DateOnly.TryParse(reader.GetString(2), out var fecha)
                        ? fecha.ToDateTime(TimeOnly.MinValue)
                        : DateTime.Now,
                    Detalle = new List<PresupuestoDetalle>()
                };

                lista.Add(p);
            }

            reader.Close();

            // -----------------------------------------
            // 🔥 CARGAR DETALLES + PRODUCTOS PARA CADA PRESUPUESTO
            // -----------------------------------------

            foreach (var presupuesto in lista)
            {
                const string sqlDet = @"
            SELECT pd.IdProducto, pd.Cantidad,
                   pr.Descripcion, pr.Precio
            FROM PresupuestosDetalle pd
            JOIN Productos pr ON pr.IdProducto = pd.IdProducto
            WHERE pd.IdPresupuesto = @id";

                using var cmdDet = new SqliteCommand(sqlDet, conexion);
                cmdDet.Parameters.AddWithValue("@id", presupuesto.IdPresupuesto);

                using var rdDet = cmdDet.ExecuteReader();
                while (rdDet.Read())
                {
                    var prod = new Producto
                    {
                        IdProducto = rdDet.GetInt32(0),
                        Precio = rdDet.GetDecimal(3),
                        Descripcion = rdDet.GetString(2)
                    };

                    presupuesto.Detalle.Add(new PresupuestoDetalle
                    {
                        Producto = prod,
                        Cantidad = rdDet.GetInt32(1)
                    });
                }
            }

            return lista;
        }


        // Crear nuevo presupuesto
        public int Crear(Presupuesto p)
        {
            try
            {
                using var conexion = new SqliteConnection(_cadenaConexion);
                conexion.Open();

                string sqlInsert = @"INSERT INTO Presupuestos  (nombreDestinatario, FechaCreacion) VALUES (@nombre, @fecha);";

                using var cmd = new SqliteCommand(sqlInsert, conexion);
                cmd.Parameters.AddWithValue("@nombre", p.NombreDestinatario);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT last_insert_rowid();";
                p.IdPresupuesto = Convert.ToInt32(cmd.ExecuteScalar());

                return p.IdPresupuesto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear presupuesto: {ex.Message}");
                return 0;
            }
        }



        // Obtener presupuesto por ID con su detalle
        public Presupuesto GetByIdPresupuesto(int id)
        {
            Presupuesto? p = null;

            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            string sqlPres = "SELECT IdPresupuesto, nombreDestinatario, FechaCreacion FROM Presupuestos WHERE IdPresupuesto = @id";
            using var cmdPres = new SqliteCommand(sqlPres, conexion);
            cmdPres.Parameters.AddWithValue("@id", id);

            using var reader = cmdPres.ExecuteReader();
            if (reader.Read())
            {
                p = new Presupuesto
                {
                    IdPresupuesto = reader.GetInt32(0),
                    NombreDestinatario = reader.GetString(1),
                    FechaCreacion = DateOnly.TryParse(reader.GetString(2), out var fecha)  // otra forma general es : FechaCreacion = DateTime.Parse(reader.GetString(2));
                    ? fecha.ToDateTime(TimeOnly.MinValue) // convierte a DateTime
                    : DateTime.Now,
                    Detalle = new List<PresupuestoDetalle>()
                };
            }

            reader.Close();
            if (p == null) return null;

            string sqlDet = @"SELECT pd.IdProducto, pr.descripcion, pr.precio, pd.cantidad
                              FROM PresupuestosDetalle pd
                              JOIN Productos pr ON pd.IdProducto = pr.idProducto
                              WHERE pd.IdPresupuesto = @id";

            using var cmdDet = new SqliteCommand(sqlDet, conexion);
            cmdDet.Parameters.AddWithValue("@id", id);

            using var readerDet = cmdDet.ExecuteReader();
            while (readerDet.Read())
            {
                var prod = new Producto
                {
                    IdProducto = readerDet.GetInt32(0),
                    Descripcion = readerDet.GetString(1),
                    Precio = readerDet.GetDecimal(2)
                };

                p.Detalle.Add(new PresupuestoDetalle
                {
                    Producto = prod,
                    Cantidad = readerDet.GetInt32(3)
                });
            }

            return p;
        }

        // Modificar  producto al presupuesto
        public void Modificar(Presupuesto presupuesto)
        {
            string queryPresupuesto = "UPDATE Presupuestos SET NombreDestinatario = @nombre, FechaCreacion = @fecha WHERE idPresupuesto = @id";

            using (var connection = new SqliteConnection(_cadenaConexion))
            {
                connection.Open();

                var command = new SqliteCommand(queryPresupuesto, connection);
                command.Parameters.AddWithValue("@nombre", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@id", presupuesto.IdPresupuesto);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }



        // Agregar producto al presupuesto
        public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            string sql = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, cantidad) VALUES (@idPres, @idProd, @cant)";
            using var cmd = new SqliteCommand(sql, conexion);
            cmd.Parameters.AddWithValue("@idPres", idPresupuesto);
            cmd.Parameters.AddWithValue("@idProd", idProducto);
            cmd.Parameters.AddWithValue("@cant", cantidad);

            cmd.ExecuteNonQuery();
        }

        // Eliminar un presupuesto y su detalle
        public bool Eliminar(int id)
        {
            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            string sqlDet = "DELETE FROM PresupuestosDetalle WHERE IdPresupuesto = @id";
            using var cmdDet = new SqliteCommand(sqlDet, conexion);
            cmdDet.Parameters.AddWithValue("@id", id);
            cmdDet.ExecuteNonQuery();

            string sqlPres = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";
            using var cmdPres = new SqliteCommand(sqlPres, conexion);
            cmdPres.Parameters.AddWithValue("@id", id);

            return cmdPres.ExecuteNonQuery() > 0;
        }

        // ------------------------------------------------------------------
        // LÓGICA RELACIONAL (Detalle y N:M)
        // ------------------------------------------------------------------

        // Método auxiliar para cargar los detalles de un presupuesto (usado en GetById)
        public List<PresupuestoDetalle> GetDetallesByPresupuestoId(int idPresupuesto)
        {
            var detalles = new List<PresupuestoDetalle>();
            const string sql = "SELECT IdPresupuesto, IdProducto, Cantidad FROM PresupuestoDetalle WHERE IdPresupuesto = @IdPresupuesto";

            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            using var comando = new SqliteCommand(sql, conexion);
            comando.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);

            using var lector = comando.ExecuteReader();

            // Cargamos todos los productos de la BD de una vez para optimizar
            var todosLosProductos = _productoRepo.GetAll().ToDictionary(p => p.IdProducto);

            while (lector.Read())
            {
                int idProducto = lector.GetInt32(1);

                // Mapeo
                var detalle = new PresupuestoDetalle
                {
                    // Buscamos el objeto Producto en el diccionario cargado previamente
                    Producto = todosLosProductos.ContainsKey(idProducto) ? todosLosProductos[idProducto] : null,
                    Cantidad = lector.GetInt32(2)
                };

                if (detalle.Producto != null)
                {
                    detalles.Add(detalle);
                }
            }

            return detalles;
        }

        // ❗ MÉTODO CLAVE DEL TP: Agrega un registro a la tabla de relación N:M
        public void AddDetalle(int idPresupuesto, int idProducto, int cantidad)
        {
            using var conexion = new SqliteConnection(_cadenaConexion);
            conexion.Open();

            // COn esto se verifica verificar si ya existe ese detalle
            const string sqlExiste = @"SELECT Cantidad FROM PresupuestosDetalle WHERE IdPresupuesto = @IdPresupuesto AND IdProducto = @IdProducto";

            using (var cmdExiste = new SqliteCommand(sqlExiste, conexion))
            {
                cmdExiste.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
                cmdExiste.Parameters.AddWithValue("@IdProducto", idProducto);

                var resultado = cmdExiste.ExecuteScalar();

                if (resultado != null)
                {
                    // si existe Actualiza la cantidad
                    const string sqlUpdate = @"UPDATE PresupuestosDetalle SET Cantidad = Cantidad + @Cantidad WHERE IdPresupuesto = @IdPresupuesto AND IdProducto = @IdProducto";

                    using var cmdUpdate = new SqliteCommand(sqlUpdate, conexion);
                    cmdUpdate.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmdUpdate.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
                    cmdUpdate.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmdUpdate.ExecuteNonQuery();
                    return;
                }
            }

            // Caso contrario lo carga
            const string sqlInsert = @"INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @Cantidad)";

            using var cmdInsert = new SqliteCommand(sqlInsert, conexion);
            cmdInsert.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
            cmdInsert.Parameters.AddWithValue("@IdProducto", idProducto);
            cmdInsert.Parameters.AddWithValue("@Cantidad", cantidad);
            cmdInsert.ExecuteNonQuery();
        }



    }
}