/*using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Productos;
using Presupuestos;
using PresupuestoDetalles;
using System;

namespace RepositoriesP
{
    public class PresupuestoRepository
    {
        private readonly string _cadenaConexion = "Data Source=Db/Tienda.db;";

        // Crear nuevo presupuesto
        public int Crear(Presupuesto p)
        {
            try
            {
                using var conexion = new SqliteConnection(_cadenaConexion);
                conexion.Open();

                string sql = "INSERT INTO Presupuestos (nombreDestinatario, FechaCreacion) VALUES (@nombreDestinatario, @fecha); SELECT last_insert_rowid();";

                using var cmd = new SqliteCommand(sql, conexion);
                cmd.Parameters.AddWithValue("@nombreDestinatario", p.NombreDestinatario);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd"));

                p.IdPresupuesto = Convert.ToInt32(cmd.ExecuteScalar());
                return p.IdPresupuesto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear presupuesto: {ex.Message}");
                return 0;
            }
        }

        // Listar todos los presupuestos
        public List<Presupuesto> Listar()
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
                        ? fecha
                        : DateOnly.FromDateTime(DateTime.Now),
                    Detalle = new List<PresupuestoDetalle>()
                };
                lista.Add(p);
            }

            return lista;
        }

        // Obtener presupuesto por ID con su detalle
        public Presupuesto? ObtenerPorId(int id)
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
                    FechaCreacion = DateOnly.Parse(reader.GetString(2)),
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
                    Precio = readerDet.GetInt32(2)
                };

                p.Detalle.Add(new PresupuestoDetalle
                {
                    Producto = prod,
                    Cantidad = readerDet.GetInt32(3)
                });
            }

            return p;
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
    }
}*/