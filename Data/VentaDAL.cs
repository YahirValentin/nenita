using nenita.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Data
{
    internal class VentaDAL
    {

        private readonly DBConnection db = new DBConnection();

        public int GuardarVenta(Venta venta)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var cmdVenta = new SqlCommand(
                            "INSERT INTO tbVentas (monto, descuento, iva, fecha) " +
                            "VALUES (@Monto, @Descuento, @iva, @fecha); " +
                            "SELECT SCOPE_IDENTITY();", conn, transaction);

                        cmdVenta.Parameters.AddWithValue("@monto", venta.monto);
                        cmdVenta.Parameters.AddWithValue("@descuento", venta.descuento);
                        cmdVenta.Parameters.AddWithValue("@iva", venta.iva);
                        cmdVenta.Parameters.AddWithValue("@fecha", venta.fecha);

                        int idVenta = Convert.ToInt32(cmdVenta.ExecuteScalar());

                        foreach (var detalle in venta.Detalles)
                        {
                            var cmdDetalle = new SqlCommand(
                                "INSERT INTO tbProductos_Ventas (idVenta, codigoBarras, cantidad, precioVenta) " +
                                "VALUES (@IdVenta, @CodigoBarras, @Cantidad, @PrecioVenta)", conn, transaction);

                            cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                            cmdDetalle.Parameters.AddWithValue("@codigoBarras", detalle.codigoBarras);
                            cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                            cmdDetalle.Parameters.AddWithValue("@precioVenta", detalle.precioVenta);

                            cmdDetalle.ExecuteNonQuery();

                            var cmdInventario = new SqlCommand(
                                "UPDATE tbProductos SET existencia = existencia - @cantidad " +
                                "WHERE codigoBarras = @CodigoBarras", conn, transaction);

                            cmdInventario.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                            cmdInventario.Parameters.AddWithValue("@codigoBarras", detalle.codigoBarras);

                            cmdInventario.ExecuteNonQuery();
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
