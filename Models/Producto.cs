using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nenita.Models
{
    internal class Producto
    {

        public int codigoBarras { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public decimal precioVenta { get; set; }
        public int existencia { get; set; }
        public decimal iva { get; set; }



    }
}
