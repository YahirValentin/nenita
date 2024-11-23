using nenita.Data;
using nenita.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nenita.Forms
{
    public partial class FrmRegistrarVenta : Form
    {
        public FrmRegistrarVenta()
        {
            InitializeComponent();
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            var formListaProductos = new FrmListaProductos();
            if (formListaProductos.ShowDialog() == DialogResult.OK)
            {
                var producto = formListaProductos.ProductoSeleccionado;
                txtCodigoProducto.Text = producto.codigoBarras.ToString();
                txtProducto.Text = producto.nombre;
                txtPrecio.Text = producto.precioVenta.ToString("0.00");
            }



        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int cantidad = (int)nudCantidad.Value;
            decimal precio = decimal.Parse(txtPrecio.Text);
            dgvDetalles.Rows.Add(txtCodigoProducto.Text, txtProducto.Text, cantidad, precio, cantidad * precio);

        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {

            try
            {
                // Crear objeto Venta
                var venta = new Venta
                {
                    fecha = DateTime.Now,
                    monto = decimal.Parse(txtTotal.Text),
                    Detalles = new List<DetalleVenta>()
                };

                // Agregar los detalles desde el DataGridView
                foreach (DataGridViewRow row in dgvDetalles.Rows)
                {
                    var detalle = new DetalleVenta
                    {
                        codigoBarras = int.Parse(row.Cells["codigoBarras"].Value.ToString()),
                        cantidad = int.Parse(row.Cells["cantidad"].Value.ToString()),
                        precioVenta = decimal.Parse(row.Cells["precio"].Value.ToString()),
                        subtotal = decimal.Parse(row.Cells["subtotal"].Value.ToString())
                    };

                    venta.Detalles.Add(detalle);
                }

                // Guardar en la base de datos
                var ventaDAL = new VentaDAL();
                int idVenta = ventaDAL.GuardarVenta(venta);

                MessageBox.Show($"Venta registrada con éxito. ID de Venta: {idVenta}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpiar formulario
                dgvDetalles.Rows.Clear();
                txtTotal.Clear();
                txtPagaCon.Clear();
                txtCambio.Clear();
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
    }
}
