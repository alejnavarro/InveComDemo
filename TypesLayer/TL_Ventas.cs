using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypesLayer
{

    #region Detalle de Venta Acutal
    public class TL_Detalle_Ventas
    {
        private List<TL_Producto_Vendido> currentSale = new List<TL_Producto_Vendido>();
        private string fecha;
        private string cedulaCliente;
        private string facturaNo;
        private double tasaDia;
        public List<TL_Producto_Vendido> CurrentSale { get => currentSale; set => currentSale = value; }
        public string Fecha { get => fecha; set => fecha = value; }
        public string CedulaCliente { get => cedulaCliente; set => cedulaCliente = value; }
        public string FacturaNo { get => facturaNo; set => facturaNo = value; }
        public double TasaDia { get => tasaDia; set => tasaDia = value; }
    }
    #endregion

    #region Clase producto Actual de la venta
    public class TL_Producto_Vendido
    {
        private int idProductoVenta;
        private string nombreProductoVenta;
        private string codigoProductoVenta;
        private int cantidadProductoVenta;
        private double precioUnitProductoVente;
        private double totalProductoVenta;

        public int IdProductoVenta { get => idProductoVenta; set => idProductoVenta = value; }
        public string NombreProductoVenta { get => nombreProductoVenta; set => nombreProductoVenta = value; }
        public string CodigoProductoVenta { get => codigoProductoVenta; set => codigoProductoVenta = value; }
        public int CantidadProductoVenta { get => cantidadProductoVenta; set => cantidadProductoVenta = value; }
        public double PrecioUnitProductoVente { get => precioUnitProductoVente; set => precioUnitProductoVente = value; }
        public double TotalProductoVenta { get => totalProductoVenta; set => totalProductoVenta = value; }
    }
    #endregion

    #region Venta Actual
    public class TL_Ventas
    {
        private int cantidadProductos;
        private double totalVenta;
        private double totalUSD;
        private double iVA;
        private string fecha;
        private string cedulaCliente;
        private string facturaNo;
        private string totalEfectivoBsF;
        private string totalEfectivoUSD;
        private string totalTarjeta;
        private string totalOtras;
        private double tasaDia;

        public string Fecha { get => fecha; set => fecha = value; }
        public string CedulaCliente { get => cedulaCliente; set => cedulaCliente = value; }
        public int CantidadProductos { get => cantidadProductos; set => cantidadProductos = value; }
        public double TotalVenta { get => totalVenta; set => totalVenta = value; }
        public double IVA { get => iVA; set => iVA = value; }
        public string FacturaNo { get => facturaNo; set => facturaNo = value; }

        public string TotalEfectivoBsF { get => totalEfectivoBsF; set => totalEfectivoBsF = value; }
        public string TotalEfectivoUSD { get => totalEfectivoUSD; set => totalEfectivoUSD = value; }
        public string TotalTarjeta { get => totalTarjeta; set => totalTarjeta = value; }
        public string TotalOtras { get => totalOtras; set => totalOtras = value; }
        public double TasaDia { get => tasaDia; set => tasaDia = value; }
        public double TotalUSD { get => totalUSD; set => totalUSD = value; }
    }
    #endregion

    #region Detalles Completos de la Venta

    public class TL_Detalle_Completo_Venta
    {
        private TL_Ventas detallePago;
        private TL_Detalle_Ventas detalleProductos;

        public TL_Ventas DetallePago { get => detallePago; set => detallePago = value; }
        public TL_Detalle_Ventas DetalleProductos { get => detalleProductos; set => detalleProductos = value; }
    }
    #endregion

}
