using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace InveComv1.ViewModels
{
    internal class VentasViewModel : Bases.BindableBase
    {
        #region Propiedades de manejo de la ventana

        /// <summary>
        /// Propiedad de muestreo de la ventana de cambio en cantidad de producto
        /// </summary>
        private Visibility editCantidad = Visibility.Collapsed;
        public Visibility EditCantidad { get => editCantidad; set => SetProperty(ref editCantidad, value); }

        /// <summary>
        /// Propiedad de seleccion del producto de venta actual
        /// </summary>
        private int selectedProduct;
        public int SelectedProduct { get => selectedProduct; set => SetProperty(ref selectedProduct, value); }

        /// <summary>
        /// Propiedad de muestreo de las ventanas emergentes
        /// </summary>
        private Visibility popUpWindow = Visibility.Collapsed;
        public Visibility PopUpWindow { get => popUpWindow; set => SetProperty(ref popUpWindow, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la cantidad de producto seleccionado
        /// </summary>
        private string cantidadProducto;
        public string CantidadProducto { get => cantidadProducto; set => SetProperty(ref cantidadProducto, value); }

        /// <summary>
        /// Mensaje de carga de datos en la ventana
        /// </summary>
        private bool isBusy;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        /// <summary>
        /// Texto de la ventana de estado de calculo de tasa de cambio
        /// </summary>
        private string textVentanaTasa = "Calculando tasa de cambio, por favor espere";
        public string TextVentanaTasa { get => textVentanaTasa; set => SetProperty(ref textVentanaTasa, value); }

        private Visibility ventanaVentasEnable;
        public Visibility VentanaVentasEnable { get => ventanaVentasEnable; set => SetProperty(ref ventanaVentasEnable, value); }

        private FrameworkElement userControl;
        public FrameworkElement UserControl { get { return userControl; } set { userControl = value; OnPropertyChanged("UserControl"); } }

        private Views.Ventas vistaVentas;
        public Views.Ventas VistaVentas { get { return vistaVentas; } set { vistaVentas = value;} }

        /// <summary>
        /// Propiedad de ejecucion de comandos
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Porpiedad de manejo del producto a buscar
        /// </summary>
        private string buscarProducto;
        public string BuscarProducto { get => buscarProducto; set => SetProperty(ref buscarProducto, value); }

        /// <summary>
        /// Propiedad de deteccion del codigo de producto actual
        /// </summary>
        private string tBCodigoActual;
        public string TBCodigoActual { get => tBCodigoActual; set => SetProperty(ref tBCodigoActual, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la cedula del cliente
        /// </summary>
        private string cedulaRifeCliente;
        public string CedulaRifeCliente { get => cedulaRifeCliente; set => SetProperty(ref cedulaRifeCliente, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la cedula del cliente
        /// </summary>
        private string nombreCliente;
        public string NombreCliente { get => nombreCliente; set => SetProperty(ref nombreCliente, value); }

        /// <summary>
        /// Propiedad de almacenamiento del telefono del cliente
        /// </summary>
        private string telefonoCliente;
        public string TelefonoCliente { get => telefonoCliente; set => SetProperty(ref telefonoCliente, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la direccion del cliente
        /// </summary>
        private string direccionCliente;
        public string DireccionCliente { get => direccionCliente; set => SetProperty(ref direccionCliente, value); }

        /// <summary>
        /// Tabla de productos de la venta actual
        /// </summary>
        private DataTable productoVenta;
        public DataTable ProductoVenta { get => productoVenta; set => SetProperty(ref productoVenta, value); }

        /// <summary>
        /// Total de la venta actual
        /// </summary>
        private double total;
        public double Total { get => total; set => SetProperty(ref total, value); }

        /// <summary>
        /// Subtotal de la venta actual
        /// </summary>
        private double subtotal;
        public double Subtotal { get => subtotal; set => SetProperty(ref subtotal, value); }

        /// <summary>
        /// IVA de la venta actual
        /// </summary>
        private double iva;
        public double IVA { get => iva; set => SetProperty(ref iva, value); }

        /// <summary>
        /// Valor en USD de la venta actual
        /// </summary>
        private double totalUSD;
        public double TotalUSD { get => totalUSD; set => SetProperty(ref totalUSD, value); }

        /// <summary>
        /// Propiedad de muestreo de la tasa de cambio diaria
        /// </summary>
        private string tasaUSD;
        public string TasaUSD { get => tasaUSD; set => SetProperty(ref tasaUSD, value); }

        /// <summary>
        /// Propiedad de muestreo de la tasa de cambio diaria
        /// </summary>
        private string tasaFechaUSD;
        public string TasaFechaUSD { get => tasaFechaUSD; set => SetProperty(ref tasaFechaUSD, value); }

        #endregion

        #region Constructor de clase
        public VentasViewModel()
        {
            IsBusy = true;
            PopUpWindow = Visibility.Visible;
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);

            /// <summary>
            /// Metodo de deteccion de la tasa de cambio actual
            /// </summary>
            /// <returns></returns>
            var task = Task.Run(async () => 
            {
                WebClient client = new WebClient();
                
                try
                {
                    if(await Task.Run(() => Bases.TestInternetConnection.TestInternet()))
                    {
                        string websiteCode = await client.DownloadStringTaskAsync("https://www.bcv.org.ve");
                        string test;
                        websiteCode.Normalize();
                        test = websiteCode.Substring(websiteCode.IndexOf("<span> USD</span>"), 140);
                        test = test.Substring(test.IndexOf("<strong>"), 20);
                        test = test.Remove(test.IndexOf("<strong>"), 8);
                        if (CultureInfo.CurrentCulture.ToString() == "en-US")
                        {
                            test = test.Replace(',', '.');
                        }
                        double rate = Double.Parse(test, CultureInfo.CurrentCulture);
                        TasaUSD = rate.ToString();
                        TasaFechaUSD = $"{rate.ToString()} {DateTime.Today.ToString("dd/MM/yyyy")}";
                        IsBusy = false;
                        ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
                        PopUpWindow = Visibility.Collapsed;
                    }
                    else
                    {
                        TextVentanaTasa = "No se pudo obtener la tasa, favor insertar manualmente";
                    }
                }
                catch (Exception ex)
                {
                    //IsBusy = false;
                    TextVentanaTasa ="No se pudo obtener la tasa, favor insertar manualmente";
                }
            });
        }
        #endregion


        private bool ViewCommandCanExecute(string action)
        {
            if (!isBusy)
            {
                return true;
            } 
            else if(action == "guardartasa")
            {
                return true;
            }else
            { 
                return false; 
            }
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action.ToLower())
            {

                #region Cambiar cantidad de un producto de la venta
                case "cantidad":

                    TextVentanaTasa = "Ingrese la cantidad de producto requerida:";
                    PopUpWindow = Visibility.Visible;
                    EditCantidad = Visibility.Visible;

                    break;

                case "cambiarcantidad":
                    try
                    {
                        int.Parse(CantidadProducto);
                        Total = Math.Round((Total - Convert.ToDouble(ProductoVenta.Rows[SelectedProduct]["PrecioTotal"]) * Convert.ToDouble(TasaUSD)), 2);
                        IVA = Math.Round(Total * 0.16, 2);
                        Subtotal = Math.Round(Total - IVA, 2);
                        //TotalUSD = Math.Round(Total / Convert.ToDouble(TasaUSD), 2);
                        TotalUSD = Math.Round(TotalUSD - Convert.ToDouble(ProductoVenta.Rows[SelectedProduct]["PrecioTotal"]), 2);
                        ProductoVenta.Rows[SelectedProduct]["Cantidad"] = CantidadProducto;
                        ProductoVenta.Rows[SelectedProduct]["PrecioTotal"] = Convert.ToDouble(ProductoVenta.Rows[SelectedProduct]["PrecioTotal"]) * Convert.ToDouble(CantidadProducto);
                        Total = Math.Round((Convert.ToDouble(ProductoVenta.Rows[SelectedProduct]["PrecioTotal"]) * Convert.ToDouble(TasaUSD) + Total), 2);
                        IVA = Math.Round(Total * 0.16, 2);
                        Subtotal = Math.Round(Total - IVA, 2);
                        //TotalUSD = Math.Round(Total / Convert.ToDouble(TasaUSD), 2);
                        TotalUSD = Math.Round(Convert.ToDouble(ProductoVenta.Rows[SelectedProduct]["PrecioTotal"]) + TotalUSD, 2);
                        PopUpWindow = Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {
                        CantidadProducto = null;
                        TextVentanaTasa = "Ingrese una cantidad de producto valida";
                    }
                    break;
                #endregion

                #region Eliminar producto de la venta
                case "eliminar":
                    if (Properties.Settings.Default.Tipo == 1 || Properties.Settings.Default.Tipo == 2)
                    {
                        try
                        {
                            int.Parse(ProductoVenta.Rows[SelectedProduct]["Cantidad"].ToString());
                            ProductoVenta.Rows.RemoveAt(SelectedProduct);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Por favor seleccione un producto a modificar");
                        }
                        break;
                    }
                    else
                    {
                        int tipoTemp = Properties.Settings.Default.Tipo;
                        Views.ValidarUsuario validarUsuario = new Views.ValidarUsuario();
                        validarUsuario.DataContext = new ValidarUsuarioViewModel()
                        {
                            Usuario = "superadmin",
                            Window = validarUsuario
                        };
                        validarUsuario.ShowDialog();
                        if (Properties.Settings.Default.Tipo == 1 || Properties.Settings.Default.Tipo == 2)
                        {
                            try
                            {
                                int.Parse(ProductoVenta.Rows[SelectedProduct]["Cantidad"].ToString());
                                ProductoVenta.Rows.RemoveAt(SelectedProduct);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Por favor seleccione un producto a modificar");
                            }
                            Properties.Settings.Default.Tipo = tipoTemp;
                        }
                        break;
                    }

                #endregion

                #region Buscar codigo de producto para venta
                case "buscar":

                    Views.EditWindow window = new Views.EditWindow();
                    Views.BuscarProducto pagina = new Views.BuscarProducto();
                    pagina.DataContext = new BuscarProductosViewModel()
                    {
                        WindowBuscar = window,
                        PagBuscarProducto = pagina,
                    };
                    window.FramEditWindow.Content = pagina;
                    window.ShowDialog();
                    break;
                #endregion

                #region Pagar
                case "pagar":

                    if (String.IsNullOrEmpty(CedulaRifeCliente))
                    {
                        MessageBox.Show("Ingrese una cedula de cliente");
                        break;
                    }
                    else
                    {
                        try
                        {
                            #region Almacenamiento de los detalles de la venta actual

                            TypesLayer.TL_Ventas ventaActual = new TypesLayer.TL_Ventas();
                            ventaActual.CantidadProductos = productoVenta.Rows.Count;
                            ventaActual.CedulaCliente = CedulaRifeCliente;
                            ventaActual.Fecha = DateTime.Today.ToString("yyyy/MM/dd");
                            ventaActual.FacturaNo = "FR-" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                            ventaActual.TotalVenta = Total;
                            ventaActual.IVA = iva;

                            TypesLayer.TL_Detalle_Ventas Detalle = new TypesLayer.TL_Detalle_Ventas();
                            TypesLayer.TL_Producto_Vendido producto = new TypesLayer.TL_Producto_Vendido();

                            foreach (DataRow products in ProductoVenta.Rows)
                            {
                                Detalle.CurrentSale.Add(new TypesLayer.TL_Producto_Vendido
                                {
                                    CantidadProductoVenta = Convert.ToInt32(products["Cantidad"]),
                                    CodigoProductoVenta = products[1].ToString(),
                                    NombreProductoVenta = products[2].ToString(),
                                    PrecioUnitProductoVente = Convert.ToDouble(products[3]),
                                    TotalProductoVenta = Convert.ToDouble(products["PrecioTotal"]),
                                }
                            );
                            }

                            Detalle.CedulaCliente = CedulaRifeCliente;
                            Detalle.Fecha = DateTime.Today.ToString("yyyy/MM/dd");
                            Detalle.FacturaNo = ventaActual.FacturaNo;
                            #endregion

                            Views.EditWindow window1 = new Views.EditWindow();
                            Views.Pago pago = new Views.Pago();

                            #region Inicializacion de las propiedades de la ventana de pago
                            pago.DataContext = new PagosViewModel()
                            {
                                PagoActual = window1,
                                Venta = ventaActual,
                                DetalleVenta = Detalle,
                                Window = window1,
                                Total = Total.ToString(),
                                TasaUSD = TasaUSD
                            };
                            #endregion

                            #region Reseteo de los campos de la ventana actual en pantalla
                            TBCodigoActual = null;
                            NombreCliente = null;
                            DireccionCliente = null;
                            TelefonoCliente = null;
                            ProductoVenta = null;
                            IVA = 0;
                            Subtotal = 0;
                            Total = 0;
                            TotalUSD = 0;

                            #endregion
                            window1.FramEditWindow.Content = pago;
                            window1.ShowDialog();
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("La venta no puede ser procesada, Intente de nuevo");
                            //MessageBox.Show(ex.Message);
                            isBusy = false;
                            break;
                        }
                    }
 
                #endregion

                #region Agregar producto a venta actual
                case "agregar":

                    if (ProductoVenta == null)
                    {
                        ProductoVenta = new DataTable();
                    }
                    DataRow row;
                    SetUpLayer.SUL_Ventas obj_sul_ventas = new SetUpLayer.SUL_Ventas();
                    row = await obj_sul_ventas.AgregarProductoVenta(TBCodigoActual);

                    if (ProductoVenta.Rows.Count > 0 && !String.IsNullOrEmpty(TasaUSD))
                    {
                        try
                        {
                            ProductoVenta.ImportRow(row.Table.Rows[row.Table.Rows.Count - 1]);
                            Total = Math.Round((Convert.ToDouble(ProductoVenta.Rows[ProductoVenta.Rows.Count - 1]["PrecioTotal"]) * Convert.ToDouble(TasaUSD) + Total) , 2);
                            IVA = Math.Round(Total * 0.16, 2);
                            Subtotal = Math.Round(Total - IVA, 2);
                            //TotalUSD = Math.Round(Total / Convert.ToDouble(TasaUSD), 2);
                            TotalUSD = Math.Round(Convert.ToDouble(ProductoVenta.Rows[ProductoVenta.Rows.Count - 1]["PrecioTotal"]) + TotalUSD, 2);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No se ha encontrado el producto");
                        }

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(TasaUSD))
                        {
                            try
                            {
                                ProductoVenta = row.Table;
                                Total = Math.Round((Convert.ToDouble(ProductoVenta.Rows[0]["PrecioTotal"]) + Total) * Convert.ToDouble(TasaUSD), 2);
                                IVA = Math.Round(Total * 0.16, 2);
                                Subtotal = Math.Round(Total - IVA, 2);
                                TotalUSD = Math.Round(Convert.ToDouble(ProductoVenta.Rows[0]["PrecioTotal"]) + TotalUSD, 2);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                MessageBox.Show("No se ha encontrado el producto");
                            }
                        }
                        else MessageBox.Show("Espere a que se calcule la tasa del dia");
                    }

                    break;
                #endregion

                #region Regresar a Home
                case "regresar":
                    UserControl = new Views.homepage();
                    VentanaVentasEnable = Visibility.Collapsed;
                    UserControl.DataContext = new homepageViewmodel();
                    break;
                #endregion

                #region Guardar tasa manualmente
                case "guardartasa":
                    try
                    {
                        Double.Parse(TasaUSD);
                        IsBusy = false;
                        ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
                        TasaFechaUSD = $"{TasaUSD} {DateTime.Today.ToString("dd/MM/yyyy")}";
                        PopUpWindow = Visibility.Collapsed;
                    }
                    catch (Exception ex)
                    {
                        TasaUSD = null;
                        TextVentanaTasa = "Ingrese un valor de cambio valido para continuar";
                    }

                    break;
                #endregion

                #region Region cierra de caja
                case "cierracaja":
                    Views.ValidarUsuario validarUsuario1 = new Views.ValidarUsuario();
                    validarUsuario1.DataContext = new ValidarUsuarioViewModel()
                    {
                        Usuario = Properties.Settings.Default.NameUsuario,
                        Window = validarUsuario1
                    };
                    
                    bool validado = (bool)validarUsuario1.ShowDialog();

                    if (validado)
                    {
                        Views.EditWindow edit = new Views.EditWindow();

                        Views.CierraCaja cierre = new Views.CierraCaja();
                        cierre.DataContext = new CierreCajaViewModel()
                        {
                            Window = edit
                        };

                        edit.FramEditWindow.Content = cierre;
                        if ((bool)edit.ShowDialog())
                        {
                            Views.UserLogin login = new Views.UserLogin();
                            login.Show();
                            Window mainWindow = Window.GetWindow(VistaVentas);
                            mainWindow.Close();
                        }

                    };


                    break;

                #endregion

                default:
                    break;
            }
        }
    }
}