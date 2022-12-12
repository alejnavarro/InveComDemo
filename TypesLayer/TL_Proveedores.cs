using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class TL_Proveedores
    {
        private int idPRoveedor;
        private string nombreProveedor;
        private int telefono;
        private string email;
        private string rif;
        private string web;

        public string NombreProveedor { get => nombreProveedor; set => nombreProveedor = value; }
        public int Telefono { get => telefono; set => telefono = value; }
        public string Email { get => email; set => email = value; }
        public string Rif { get => rif; set => rif = value; }
        public string Web { get => web; set => web = value; }
        public int IdPRoveedor { get => idPRoveedor; set => idPRoveedor = value; }
    }
}