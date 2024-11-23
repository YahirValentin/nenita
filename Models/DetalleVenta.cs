using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Models
{
    internal class DetalleVenta
    {

        public int idProductoVenta { get; set; }
        public int idVenta { get; set; }
        public int codigoBarras { get; set; }
        public int cantidad { get; set; }
        public decimal precioVenta { get; set; }
        public decimal subtotal { get; set; }

    }
}
