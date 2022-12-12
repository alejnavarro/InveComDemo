using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class Devoluciones
    {
        string facturaNo;
        string cedulaCliente;
        DataTable productos;

        public string FacturaNo { get => facturaNo; set => facturaNo = value; }
        public string CedulaCliente { get => cedulaCliente; set => cedulaCliente = value; }
        public DataTable Productos { get => productos; set => productos = value; }
    }
}
