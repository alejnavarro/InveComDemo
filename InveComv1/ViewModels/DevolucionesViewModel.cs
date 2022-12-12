using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InveComv1.ViewModels
{
    public class DevolucionesViewModel : Bases.BindableBase
    {

        /// <summary>
        /// Propiedad de almacenamiento del numero de factura a consultar
        /// </summary>
        private string facturaNo;
        public string FacturaNo { get => facturaNo; set => SetProperty(ref facturaNo, value); }

        /// <summary>
        /// Propiedad de almacenamiento de los productos de la venta consultada
        /// </summary>
        private DataTable productosVenta;
        public DataTable ProductosVenta { get => productosVenta; set => SetProperty(ref productosVenta, value); }

        /// <summary>
        /// Declaracion del objeto tipo ventas para manejo de la devolucion
        /// </summary>
        private SetUpLayer.SUL_Ventas ventaDevolucionDetalle;
        public SetUpLayer.SUL_Ventas VentaDevolucionDetalle { get => ventaDevolucionDetalle; set => SetProperty(ref ventaDevolucionDetalle, value); }

        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }


        #region Constructor de clase
        public DevolucionesViewModel()
        {
            VentaDevolucionDetalle = new SetUpLayer.SUL_Ventas();
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {
                #region consultar venta a devolver
                case "recibono":
                    TypesLayer.TL_Detalle_Ventas detalle_Venta = new TypesLayer.TL_Detalle_Ventas();
                    detalle_Venta = await VentaDevolucionDetalle.ConsultarDetalleProductosVentas(FacturaNo);

                    #region Generacion de columnas para el grid Detalle de Venta a conusltar
                    DataColumn Cedula = new DataColumn();
                    Cedula.ColumnName = "Cedula";
                    Cedula.DataType = typeof(string);

                    DataColumn ReciboNo = new DataColumn();
                    ReciboNo.ColumnName = "ReciboNo";
                    ReciboNo.DataType = typeof(string);

                    DataColumn Fecha = new DataColumn();
                    Fecha.ColumnName = "Fecha";
                    Fecha.DataType = typeof(string);

                    DataColumn Codigo = new DataColumn();
                    Codigo.ColumnName = "Codigo";
                    Codigo.DataType = typeof(string);

                    DataColumn Nombre = new DataColumn();
                    Nombre.ColumnName = "Nombre";
                    Nombre.DataType = typeof(string);

                    DataColumn Precio = new DataColumn();
                    Precio.ColumnName = "Precio";
                    Precio.DataType = typeof(Double);

                    DataColumn Cantidad = new DataColumn();
                    Cantidad.ColumnName = "Cantidad";
                    Cantidad.DataType = typeof(int);

                    DataColumn Precio_Total = new DataColumn();
                    Precio_Total.ColumnName = "Precio_Total";
                    Precio_Total.DataType = typeof(double);
                    #endregion

                    #region Creacion de columnas para la tabla detalle de Ventas
                    if (ProductosVenta == null)
                    {
                        ProductosVenta = new DataTable();
                        ProductosVenta.Columns.Add(Cedula);
                        ProductosVenta.Columns.Add(ReciboNo);
                        ProductosVenta.Columns.Add(Fecha);
                        ProductosVenta.Columns.Add(Codigo);
                        ProductosVenta.Columns.Add(Nombre);
                        ProductosVenta.Columns.Add(Precio);
                        ProductosVenta.Columns.Add(Cantidad);
                        ProductosVenta.Columns.Add(Precio_Total);
                    }
                    #endregion

                    #region Carga de datos en la tabla de Consulta de ventas
                    if (detalle_Venta != null)
                    {
                        foreach (TypesLayer.TL_Producto_Vendido producto in detalle_Venta.CurrentSale)
                        {
                            ProductosVenta.Rows.Add();
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Cedula"] = detalle_Venta.CedulaCliente;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["ReciboNo"] = detalle_Venta.FacturaNo;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Fecha"] = detalle_Venta.Fecha;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Codigo"] = producto.CodigoProductoVenta;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Nombre"] = producto.NombreProductoVenta;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Precio"] = producto.PrecioUnitProductoVente;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Cantidad"] = producto.CantidadProductoVenta;
                            ProductosVenta.Rows[productosVenta.Rows.Count - 1]["Precio_Total"] = producto.TotalProductoVenta;
                        }
                    }

                    #endregion
                    FacturaNo = "";
                    break;
                #endregion

                #region Realizar devolucion
                case "generarrecibo":

                    break;
                #endregion

            }
        }


    }
}
