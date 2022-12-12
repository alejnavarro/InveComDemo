using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class TL_Usuario
    {
        private int idUsuario;
        private string usuario;
        private string clave;
        private int tipo;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public string Clave { get => clave; set => clave = value; }
        public int Tipo { get => tipo; set => tipo = value; }
    }
}
