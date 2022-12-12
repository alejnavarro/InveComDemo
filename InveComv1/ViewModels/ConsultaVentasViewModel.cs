using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InveComv1.ViewModels
{
    public class ConsultaVentasViewModel : Bases.BindableBase
    {
        #region Propiedades de la vista
        /// <summary>
        /// Propiedad de almacenamiento del numero de factura a consultar
        /// </summary>
        private string facturaNo;
        public string FacturaNo { get => facturaNo; set => SetProperty(ref facturaNo, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la fecha inicial de busqueda de ventas
        /// </summary>
        private DateTime fechaInicio;
        public DateTime FechaInicio { get => fechaInicio; set => SetProperty(ref fechaInicio, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la fecha final de busqueda de ventas
        /// </summary>
        private DateTime fechaFinal;
        public DateTime FechaFinal { get => fechaFinal; set => SetProperty(ref fechaFinal, value); }

        /// <summary>
        /// Propiedad de almacenamiento del numero de venta a consultar
        /// </summary>
        private int selectedRow;
        public int SelectedRow { get => selectedRow; set => SetProperty(ref selectedRow, value); }

        /// <summary>
        /// Propiedad de almacenamiento del numero de factura a consultar
        /// </summary>
        private string cedulaCliente;
        public string CedulaCliente { get => cedulaCliente; set => SetProperty(ref cedulaCliente, value); }

        /// <summary>
        /// Propiedad de almacenamiento de las ventas consultadas
        /// </summary>
        private DataTable ventasConsultadas;
        public DataTable VentasConsultadas { get => ventasConsultadas; set => SetProperty(ref ventasConsultadas, value); }

        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Propiedad de declaracion del comando manejador de eventos para doble click
        /// </summary>
        private Bases.DelegateCommand<object> mouseDoubleClickEvent;
        public Bases.DelegateCommand<object> MouseDoubleClickEvent { get { return mouseDoubleClickEvent; } set { SetProperty(ref mouseDoubleClickEvent, value); } }

        #endregion

        #region Constructor de clase
        public ConsultaVentasViewModel()
        {
            FechaInicio = DateTime.Today;
            FechaFinal = DateTime.Today;
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
            MouseDoubleClickEvent = new Bases.DelegateCommand<object>(MouseDoubleClickEventExecute);

        }
        #endregion

        private async void MouseDoubleClickEventExecute (object parameter)
        {
            try
            {
                if (VentasConsultadas.Rows[SelectedRow] != null)
                {
                    SetUpLayer.SUL_Ventas obj_sul_ventas = new SetUpLayer.SUL_Ventas();
                    TypesLayer.TL_Detalle_Completo_Venta DetalleVenta = new TypesLayer.TL_Detalle_Completo_Venta();
                    DetalleVenta = await obj_sul_ventas.ConsultarDetalleVenta(VentasConsultadas.Rows[SelectedRow][3].ToString());
                    Views.EditWindow window = new Views.EditWindow();
                    Views.VistaDetalladaVenta vista = new Views.VistaDetalladaVenta();

                    vista.DataContext = new VistaDetalladaVentaViewModel()
                    {
                        Window = window,
                        DetalleVenta = DetalleVenta,
                        DetalleProductos = test(DetalleVenta)
                    };


                    window.FramEditWindow.Content = vista;
                    window.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                //MessageBox.Show("Por favor realice una consulta de Venta");
            }

        }

        public DataTable test(TypesLayer.TL_Detalle_Completo_Venta DetalleVenta)
        {
            DataTable test_table = new DataTable();
            test_table.Columns.Add("FacturaNo");
            test_table.Columns.Add("Codigo");
            test_table.Columns.Add("Nombre");
            test_table.Columns.Add("Precio");
            test_table.Columns.Add("CantidadProducto");
            test_table.Columns.Add("TotalProducto");
            int i = 0;
            foreach (var producto in DetalleVenta.DetalleProductos.CurrentSale)
            {
                test_table.Rows.Add();
                test_table.Rows[i]["FacturaNo"] = DetalleVenta.DetallePago.FacturaNo;
                test_table.Rows[i]["Codigo"] = producto.CodigoProductoVenta;
                test_table.Rows[i]["Nombre"] = producto.NombreProductoVenta;
                test_table.Rows[i]["Precio"] = producto.PrecioUnitProductoVente;
                test_table.Rows[i]["CantidadProducto"] = producto.CantidadProductoVenta;
                test_table.Rows[i]["TotalProducto"] = producto.TotalProductoVenta;
                i++;
            }
            return test_table;
        }
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "buscar":
                    SetUpLayer.SUL_Ventas obj_sul_ventas = new SetUpLayer.SUL_Ventas();
                    VentasConsultadas = await obj_sul_ventas.ConsultaGeneralVenta(FechaInicio.ToString("yyyy/MM/dd"),FechaFinal.ToString("yyyy/MM/dd"),CedulaCliente,FacturaNo);
                    break;

                default:
                    break;
            }
        }
    }
}
