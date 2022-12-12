using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class TL_Products
    {
        private int idArticulo;
        private string codigo;
        private string nombre;
        private int grupo;
        private double costo;
        private double precio;
        private int cantidad;
        private bool Activo;
        private byte[] imagen;
        private int proveedor;
        private string descripcion;

        public int IdArticulo { get => idArticulo; set => idArticulo = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Grupo { get => grupo; set => grupo = value; }
        public double Costo { get => costo; set => costo = value; }
        public double Precio { get => precio; set => precio = value; }
        public bool Activo1 { get => Activo; set => Activo = value; }
        public byte[] Imagen { get => imagen; set => imagen = value; }
        public int Proveedor { get => proveedor; set => proveedor = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
    }
}