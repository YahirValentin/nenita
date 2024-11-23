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
    internal class VentaDAL
    {

        public int IdVenta { get; set; }
        public decimal Monto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Iva { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

        public int RegistrarVenta(Venta venta, int idEmpleado)
        {
            using (SqlConnection conn = DBConnection.ObtenerConexion())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int idVenta;

                        // Registrar la venta
                        using (SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descuento", venta.descuento);
                            cmd.Parameters.AddWithValue("@iva", venta.iva);
                            cmd.Parameters.AddWithValue("@monto", venta.monto);
                            cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                            var result = cmd.ExecuteScalar();
                            idVenta = Convert.ToInt32(result);
                        }

                        // Registrar la relación venta-empleado
                        using (SqlCommand cmd = new SqlCommand("sp_RegistrarVentaEmpleado", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idVenta", idVenta);
                            cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                            cmd.ExecuteNonQuery();
                        }

                        // Registrar los productos de la venta
                        foreach (var detalle in venta.Detalles)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_AgregarProductoVenta", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idVenta", idVenta);
                                cmd.Parameters.AddWithValue("@codigoBarras", detalle.codigoBarras);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return idVenta;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}