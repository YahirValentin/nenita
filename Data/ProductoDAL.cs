using nenita.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Data
{
    internal class ProductoDAL
    {


        public string CodigoBarras { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencia { get; set; }
        public decimal Iva { get; set; }

        public static Producto BuscarProducto(string codigoBarras)
        {
            using (SqlConnection conn = DBConnection.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand("sp_BuscarProducto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int existencia = 0;
                            // Intentamos convertir el valor de existencia
                            if (!int.TryParse(reader["existencia"].ToString(), out existencia))
                            {
                                // Si no se puede convertir, asignamos un valor predeterminado o manejamos el error
                                // Puedes elegir lanzar una excepción o asignar un valor predeterminado.
                                existencia = 0; // Asignamos 0 si no es posible convertirlo
                            }

                            return new Producto
                            {
                                codigoBarras = int.Parse(reader["codigoBarras"].ToString()),
                                nombre = reader["nombre"].ToString(),
                                marca = reader["marca"].ToString(),
                                precioVenta = Convert.ToDecimal(reader["precioVenta"]),
                                existencia = existencia, // Ahora `existencia` tiene un valor numérico seguro
                                iva = Convert.ToDecimal(reader["iva"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}


