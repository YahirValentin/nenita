using nenita.Data;
using nenita.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nenita.Forms
{
    public partial class FrmRegistrarVenta : Form
    {
        private Venta ventaActual;
        private BindingList<DetalleVenta> detallesVenta;
        private int idEmpleado;
        private readonly VentaDAL ventaDAL;
        public FrmRegistrarVenta()
        {
            InitializeComponent();
            ventaDAL = new VentaDAL();
            InicializarVenta();
        }
        private void InicializarVenta()
        {
            ventaActual = new Venta();
            detallesVenta = new BindingList<DetalleVenta>();
            // Configura el DataGridView para mostrar los detalles
            dgvDetalles.DataSource = detallesVenta;
            LimpiarCampos();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text.Trim();
                if (string.IsNullOrEmpty(codigo)) return;

                var producto = ProductoDAL.BuscarProducto(codigo);
                if (producto != null)
                {
                    txtProducto.Text = producto.nombre;
                    txtPrecio.Text = producto.precioVenta.ToString("C");
                    nudCantidad.Maximum = producto.existencia;
                    nudCantidad.Enabled = true;
                    btnAgregar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Producto no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var producto = ProductoDAL.BuscarProducto(txtCodigo.Text.Trim());
                if (producto == null) return;

                var detalle = new DetalleVenta
                {
                    codigoBarras = producto.codigoBarras,
                    precioVenta = producto.precioVenta,
                    cantidad = (int)nudCantidad.Value
                };

                detallesVenta.Add(detalle);
                CalcularTotales();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (detallesVenta.Count == 0)
                {
                    MessageBox.Show("Agregue productos a la venta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = DBConnection.ObtenerConexion())
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@descuento", ventaActual.descuento);
                        cmd.Parameters.AddWithValue("@iva", ventaActual.iva);
                        cmd.Parameters.AddWithValue("@monto", ventaActual.monto);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Now);

                        conn.Open();
                        int idVenta = Convert.ToInt32(cmd.ExecuteScalar());

                        MessageBox.Show($"Venta registrada correctamente. ID: {idVenta}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InicializarVenta();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmRegistrarVenta_Load(object sender, EventArgs e)
        {

        }
        private void CalcularTotales()
        {
            decimal subtotal = detallesVenta.Sum(d => d.precioVenta * d.cantidad);
            decimal iva = subtotal * 0.16m; // Ajusta según tu porcentaje de IVA
            decimal descuento = 0; // Implementa la lógica de descuentos según tus necesidades

            ventaActual.monto = subtotal;
            ventaActual.iva = iva;
            ventaActual.descuento = descuento;

            txtTotal.Text = (subtotal + iva - descuento).ToString("C");
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtProducto.Clear();
            txtPrecio.Clear();
            nudCantidad.Value = 0;
            nudCantidad.Enabled = false;
            btnAgregar.Enabled = false;
            txtCodigo.Focus();
        }

        private void txtCambio_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            try
            {
                // Verifica que el texto en txtPaga sea un valor numérico válido
                if (decimal.TryParse(txtPagaCon.Text.Trim(), out decimal pagaCon) &&
                    decimal.TryParse(txtTotal.Text.Replace("$","").Trim(), out decimal totalVenta))
                {
                    // Calcula el cambio
                    decimal cambio = pagaCon - totalVenta;

                    // Asegúrate de que el cambio no sea negativo
                    if (cambio >= 0)
                    {
                        txtCambio.Text = cambio.ToString("C"); // Formatea el cambio como moneda
                    }
                    else
                    {
                        txtCambio.Text = "Falta dinero"; // Mensaje para indicar falta de dinero
                    }
                }
                else
                {
                    txtCambio.Text = string.Empty; // Limpia el campo si la entrada no es válida
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular el cambio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}

