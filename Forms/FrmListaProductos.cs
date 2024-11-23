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
    public partial class FrmListaProductos : Form
    {
        Producto ProductoSeleccionado { get;  set; }

        public FrmListaProductos()
        {
            InitializeComponent();

        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {

           

        }


        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0)
            {
                ProductoSeleccionado = new Producto
                {
                    codigoBarras = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["codigoBarras"].Value),
                    nombre = dgvProductos.Rows[e.RowIndex].Cells["nombre"].Value.ToString(),
                    marca = dgvProductos.Rows[e.RowIndex].Cells["marca"].Value.ToString(),
                    precioVenta = Convert.ToDecimal(dgvProductos.Rows[e.RowIndex].Cells["precioVenta"].Value),
                    existencia = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["existencia"].Value),
                    iva = Convert.ToDecimal(dgvProductos.Rows[e.RowIndex].Cells["iva"].Value)
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void FrmListaProductos_Load(object sender, EventArgs e)
        {

        }
    }

}
