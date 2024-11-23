using nenita.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Data
{
    internal class ProductoDAL
    {

        private readonly DBConnection db = new DBConnection();

        public List<Producto> GetProductos(string searchTerm = "")
        {
            var productos = new List<Producto>();
            using (var conn = db.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT codigoBarras, nombre, marca, precioVenta, existencia, iva " +
                    "FROM tbProductos " +
                    "WHERE (@searchTerm = '' OR nombre LIKE @searchTerm + '%')", conn);

                cmd.Parameters.AddWithValue("@searchTerm", searchTerm ?? "");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            codigoBarras = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            marca = reader.GetString(2),
                            precioVenta = reader.GetDecimal(3),
                            existencia = reader.GetInt32(4),
                            iva = reader.GetDecimal(5)
                        });
                    }
                }
            }
            return productos;
        }

    }
}
