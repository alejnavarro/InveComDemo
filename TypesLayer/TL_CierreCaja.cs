using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{
    public class TL_CierreCaja
    {
        private double efectivoUSD;
        private double efectivoBsF;
        private double tarjeta;
        private double otras;
        private double tasaDia;
        private double totalVentas;
        private string fecha;
        private string usuario;
        private int idUsuario;

        public double EfectivoUSD { get => efectivoUSD; set => efectivoUSD = value; }
        public double EfectivoBsF { get => efectivoBsF; set => efectivoBsF = value; }
        public double Tarjeta { get => tarjeta; set => tarjeta = value; }
        public double Otras { get => otras; set => otras = value; }
        public double TasaDia { get => tasaDia; set => tasaDia = value; }
        public double TotalVentas { get => totalVentas; set => totalVentas = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string Usuario { get => usuario; set => usuario = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    }
}
