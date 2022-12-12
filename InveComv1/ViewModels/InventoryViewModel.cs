using InveComv1.Bases;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InveComv1.ViewModels
{
    public class InventoryViewModel : Bases.BindableBase
    {
        #region Constructor
        public InventoryViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
            CargarInventario();
        }
        #endregion

        #region Carga de productos en inventario
        public async Task CargarInventario ()
        {
            SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
            Products = await obj_sul_products.Buscar("");
    }
        #endregion

        #region Propiedades de la vista

        private FrameworkElement userControl;
        public FrameworkElement UserControl { get { return userControl; } set { userControl = value; OnPropertyChanged("UserControl"); } }

        /// <summary>
        /// Titulo de la vista, traida por binding desde la pagina de inicio
        /// </summary>
        private string texto;
        public string Texto
        {
            get { return texto; }
            set
            {
                texto = value;
                OnPropertyChanged("Texto");
            }
        }

        /// <summary>
        /// Propiedad de habilitacion de la vista de inventario
        /// </summary>
        private Visibility inventoryEnabled;
        public Visibility InventoryEnabled { get => inventoryEnabled; set => SetProperty(ref inventoryEnabled, value); }

        /// <summary>
        /// Propiedad de identificacion del producto seleccionado del inventario
        /// </summary>
        private int selectedRow;
        public int SelectedRow { get => selectedRow; set => SetProperty(ref selectedRow, value); }

        /// <summary>
        /// Propiedd de llenado del data grid Inventario
        /// </summary>
        private DataTable products;
        public DataTable Products { get => products; set => SetProperty(ref products, value); }

        /// <summary>
        /// Propiedad de deteccion del producto a buscar en el inventario
        /// </summary>
        private string codigoBusqueda;
        public string CodigoBusqueda { get => codigoBusqueda; set => SetProperty(ref codigoBusqueda, value); }

        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand
        {
            get { return viewCommand; }
            set { SetProperty(ref viewCommand, value); }
        }

        #endregion


        #region Reguion ejecuion de comandos para botones
        private bool ViewCommandCanExecute(string action)
        {
            if (action == null)
                return false;

            if (action?.ToLower() == "botoninicio")
            {
                return true;
            }
            else if (action?.ToLower() == "save")
            {
                return true;
            }

            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            #region Agregar nuevo producto
            if (action?.ToLower() == "agregar")
            {
                Views.EditWindow window = new Views.EditWindow();
                Views.CRUDproducts CRUD = new Views.CRUDproducts();
                CRUD.DataContext = new ViewModels.CrudProductsViewModel()
                {
                    TituloCrudProductos = "InveCom Seccion Agregar Productos",
                    CRUD = CRUD,
                    Window = window,
                    CRUDCloseEnable = Visibility.Collapsed,
                    CRUDbtnActEnable = Visibility.Collapsed,
                    CRUDbtnDelEnable = Visibility.Collapsed,
            };
                CRUD.CloseBorder.Visibility = Visibility.Collapsed;
                window.FramEditWindow.Content = CRUD;
                window.ShowDialog();
            }
            #endregion

            #region Consultar producto
            if (action?.ToLower() == "consultar")
            {
                try
                {
                    TypesLayer.TL_Products producto = new TypesLayer.TL_Products();
                    SetUpLayer.SUL_Products obj_sul_Products = new SetUpLayer.SUL_Products();
                    producto = await obj_sul_Products.Consultar(Convert.ToInt32(Products.Rows[selectedRow][0]));

                    TypesLayer.TL_Grupos grupo = new TypesLayer.TL_Grupos();
                    SetUpLayer.SUL_Grupos onj_sul_Groups = new SetUpLayer.SUL_Grupos();
                    grupo = await onj_sul_Groups.ConsultarGrupo(producto.Grupo);

                    TypesLayer.TL_Proveedores proveedor = new TypesLayer.TL_Proveedores();
                    SetUpLayer.SUL_Proveedores obj_sul_Providers = new SetUpLayer.SUL_Proveedores();
                    proveedor = await obj_sul_Providers.ConsultarProveedor(producto.Proveedor);

                    Views.EditWindow window = new Views.EditWindow();
                    Views.CRUDproducts CRUD = new Views.CRUDproducts();

                    #region Inicializacion de propiedades del CRUD
                    CRUD.DataContext = new ViewModels.CrudProductsViewModel()
                    {
                        TituloCrudProductos = "InveCom Seccion Consultar Productos",
                        CRUD = CRUD,
                        Window = window,
                        Producto = producto,
                        CRUDbtnActEnable = Visibility.Collapsed,
                        CRUDAcptCancEnable = Visibility.Collapsed,
                        CRUDbtnDelEnable = Visibility.Collapsed,
                        TtextNombreProducto = producto.Nombre,
                        TtexCodigoProducto = producto.Codigo,
                        CostoProducto = producto.Costo.ToString(),
                        PrecioProducto = producto.Precio.ToString(),
                        CantidadProducto = producto.Cantidad.ToString(),
                        ProveedorProducto = proveedor.NombreProveedor,
                        GrupoProducto = grupo.NombreGrupo,
                    };
                    CRUD.TBdescripcionProducto.Text = producto.Descripcion;
                    #endregion

                    #region Inicializacion de valores del CRUD
                    CRUD.TBcantidadProducto.IsEnabled = false;
                    CRUD.TBcodigoProducto.IsEnabled = false;
                    CRUD.TBnombreProducto.IsEnabled = false;
                    CRUD.TBcodigoProducto.IsEnabled = false;
                    CRUD.TBdescripcionProducto.IsEnabled = false;
                    CRUD.TBcoostoProducto.IsEnabled = false;
                    CRUD.TBprecioProducto.IsEnabled = false;
                    CRUD.TBdescripcionProducto.IsEnabled = false;
                    CRUD.CBProveedor.IsEnabled = false;
                    CRUD.CBGrupo.IsEnabled = false;
                    #endregion

                    window.FramEditWindow.Content = CRUD;
                    window.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //MessageBox.Show("Seleccione un producto a consultar");
                }

            }
            #endregion

            #region Modificar producto
            if (action?.ToLower() == "modificar")
            {
                TypesLayer.TL_Products producto = new TypesLayer.TL_Products();
                SetUpLayer.SUL_Products obj_sul_Products = new SetUpLayer.SUL_Products();
                producto = await obj_sul_Products.Consultar(Convert.ToInt32(Products.Rows[selectedRow][0]));

                TypesLayer.TL_Grupos grupo = new TypesLayer.TL_Grupos();
                SetUpLayer.SUL_Grupos onj_sul_Groups = new SetUpLayer.SUL_Grupos();
                grupo = await onj_sul_Groups.ConsultarGrupo(producto.Grupo);

                TypesLayer.TL_Proveedores proveedor = new TypesLayer.TL_Proveedores();
                SetUpLayer.SUL_Proveedores obj_sul_Providers = new SetUpLayer.SUL_Proveedores();
                proveedor = await obj_sul_Providers.ConsultarProveedor(producto.Proveedor);

                Views.EditWindow window = new Views.EditWindow();
                Views.CRUDproducts CRUD = new Views.CRUDproducts();

                #region Inicializacion de propiedades del CRUD
                CRUD.DataContext = new ViewModels.CrudProductsViewModel()
                {
                    TituloCrudProductos = "InveCom Seccion Modificar Productos",
                    CRUD = CRUD,
                    Window = window,
                    Producto = producto,
                    CRUDbtnActEnable = Visibility.Visible,
                    CRUDAcptCancEnable = Visibility.Collapsed,
                    CRUDbtnDelEnable = Visibility.Collapsed,
                    TtextNombreProducto = producto.Nombre,
                    TtexCodigoProducto = producto.Codigo,
                    CostoProducto = producto.Costo.ToString(),
                    PrecioProducto = producto.Precio.ToString(),
                    CantidadProducto = producto.Cantidad.ToString(),
                    ProveedorProducto = proveedor.NombreProveedor,
                    GrupoProducto = grupo.NombreGrupo,
                };
                CRUD.TBdescripcionProducto.Text = producto.Descripcion;
                #endregion

                window.FramEditWindow.Content = CRUD;
                window.ShowDialog();
            }
            #endregion

            #region Eliiminar producto
            if (action?.ToLower() == "eliminar")
            {
                TypesLayer.TL_Products producto = new TypesLayer.TL_Products();
                SetUpLayer.SUL_Products obj_sul_Products = new SetUpLayer.SUL_Products();
                producto = await obj_sul_Products.Consultar(Convert.ToInt32(Products.Rows[selectedRow][0]));

                TypesLayer.TL_Grupos grupo = new TypesLayer.TL_Grupos();
                SetUpLayer.SUL_Grupos onj_sul_Groups = new SetUpLayer.SUL_Grupos();
                grupo = await onj_sul_Groups.ConsultarGrupo(producto.Grupo);

                TypesLayer.TL_Proveedores proveedor = new TypesLayer.TL_Proveedores();
                SetUpLayer.SUL_Proveedores obj_sul_Providers = new SetUpLayer.SUL_Proveedores();
                proveedor = await obj_sul_Providers.ConsultarProveedor(producto.Proveedor);

                Views.EditWindow window = new Views.EditWindow();
                Views.CRUDproducts CRUD = new Views.CRUDproducts();

                #region Inicializacion de propiedades del CRUD
                CRUD.DataContext = new ViewModels.CrudProductsViewModel()
                {
                    TituloCrudProductos = "InveCom Seccion Eliminar Productos",
                    CRUD = CRUD,
                    Window = window,
                    Producto = producto,
                    CRUDbtnActEnable = Visibility.Collapsed,
                    CRUDAcptCancEnable = Visibility.Collapsed,
                    CRUDbtnDelEnable = Visibility.Visible,
                    TtextNombreProducto = producto.Nombre,
                    TtexCodigoProducto = producto.Codigo,
                    CostoProducto = producto.Costo.ToString(),
                    PrecioProducto = producto.Precio.ToString(),
                    CantidadProducto = producto.Cantidad.ToString(),
                    ProveedorProducto = proveedor.NombreProveedor,
                    GrupoProducto = grupo.NombreGrupo,
                };
                CRUD.TBdescripcionProducto.Text = producto.Descripcion;
                #endregion

                #region Inicializacion de valores del CRUD
                CRUD.TBcantidadProducto.IsEnabled = false;
                CRUD.TBcodigoProducto.IsEnabled = false;
                CRUD.TBnombreProducto.IsEnabled = false;
                CRUD.TBcodigoProducto.IsEnabled = false;
                CRUD.TBdescripcionProducto.IsEnabled = false;
                CRUD.TBcoostoProducto.IsEnabled = false;
                CRUD.TBprecioProducto.IsEnabled = false;
                CRUD.TBdescripcionProducto.IsEnabled = false;
                CRUD.CBProveedor.IsEnabled = false;
                CRUD.CBGrupo.IsEnabled = false;
                #endregion

                window.FramEditWindow.Content = CRUD;
                window.ShowDialog();
            }
            #endregion

            #region Actulizar inventario de productos
            if (action?.ToLower() == "actualizar")
            {
                SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
                Products = await obj_sul_products.Buscar("");
            }
            #endregion

            #region Buscar Producto
            if(action.ToLower() == "buscar")
            {
                SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
                Products = await obj_sul_products.ConsultarProducto(CodigoBusqueda);
            }
            #endregion

            #region Regresar a pantalla inicial
            if (action?.ToLower() == "regresar")
            {
                UserControl = new Views.homepage();
                InventoryEnabled = Visibility.Collapsed;
                UserControl.DataContext = new homepageViewmodel();
            }
            #endregion

        }





        #endregion
    }


}
