using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Models
{
    internal class Venta
    {

        public int idVenta { get; set; }
        public decimal monto { get; set; }
        public decimal descuento { get; set; }
        public decimal iva { get; set; }
        public DateTime fecha { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

    }
}
