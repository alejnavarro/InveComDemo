using InveComv1.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using TypesLayer;
using MessageBox = System.Windows.Forms.MessageBox;

namespace InveComv1.ViewModels
{
    class CrudProductsViewModel : Bases.BindableBase
    {

        #region Declaracion de propiedades del view model

        /// <summary>
        /// Generacion de una propiedad tipo CRUD para manejar los objetos dentro del mismo
        /// </summary>
        private Views.CRUDproducts cRUD;
        public CRUDproducts CRUD { get => cRUD; set => cRUD = value; }


        /// <summary>
        /// Generacion de una propiedad tipo Ventana para manejar los objetos dentro de la misma
        /// </summary>
        private Views.EditWindow _Window;
        public Views.EditWindow Window { get => _Window; set => _Window = value; }


        /// <summary>
        /// Propiedad de recpcion del titulo de ventana
        /// </summary>
        private string tituloCrudProductos;
        public string TituloCrudProductos { get { return tituloCrudProductos; } set { tituloCrudProductos = value; OnPropertyChanged("TituloCrudProductos"); } }

        /// <summary>
        /// Propiedad de deteccion de cambios en el producto a modificar
        /// </summary>
        private TypesLayer.TL_Products producto = new TL_Products();
        public TL_Products Producto { get => producto; set => SetProperty(ref producto, value); }

        /// <summary>
        /// Propiedades de visibilidad para los botones del CRUD
        /// </summary>
        private Visibility cRUDAcptCancEnable;
        public Visibility CRUDAcptCancEnable { get => cRUDAcptCancEnable; set => SetProperty(ref cRUDAcptCancEnable, value); }

        private Visibility cRUDCloseEnable;
        public Visibility CRUDCloseEnable { get => cRUDCloseEnable; set => SetProperty(ref cRUDCloseEnable, value); }

        private Visibility cRUDbtnActEnable;
        public Visibility CRUDbtnActEnable { get => cRUDbtnActEnable; set => SetProperty(ref cRUDbtnActEnable, value); }

        private Visibility cRUDbtnDelEnable;
        public Visibility CRUDbtnDelEnable { get => cRUDbtnDelEnable; set => SetProperty(ref cRUDbtnDelEnable, value); }

        /// <summary>
        /// Lista de nombres de proveedor para el combobox proveedores
        /// </summary>
        private List<string> nombreProveedor;
        public List<string> NombreProveedor { get => nombreProveedor; set => SetProperty(ref nombreProveedor, value); }

        /// <summary>
        /// Lista de nombres de grupo para el combobox grupos
        /// </summary>
        private List<string> nombreGrupo;
        public List<string> NombreGrupo { get => nombreGrupo; set => SetProperty(ref nombreGrupo, value); }

        /// <summary>
        /// Declaracion de la propiedad para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get => viewCommand; set => SetProperty(ref viewCommand, value); }

        /// <summary>
        /// Propiedad de deteccion de nombre de producto en el crud y validacion de errores en los datos insertados
        /// </summary>
        /// 
        private string ttextNombreProducto;
        public string TtextNombreProducto {  get => ttextNombreProducto; set => SetProperty(ref ttextNombreProducto, value); }
        //private string ttextNombreProducto = "nombre";
        //public string TtextNombreProducto { get => ttextNombreProducto; set { ttextNombreProducto = value;  ValidateErrors(); OnErrorsChanged("ttextNombreProducto"); } }

        /// <summary>
        /// Propiedad de deteccion de codigo de producto en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string ttexCodigoProducto;
        public string TtexCodigoProducto { get => ttexCodigoProducto; set => SetProperty(ref ttexCodigoProducto, value); }

        /// <summary>
        /// Propiedad de deteccion de costo de producto en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string costoProducto;
        public string CostoProducto { get => costoProducto; set => SetProperty(ref costoProducto, value); }

        /// <summary>
        /// Propiedad de deteccion de precio de producto en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string precioProducto;
        public string PrecioProducto { get => precioProducto; set => SetProperty(ref precioProducto, value); }

        /// <summary>
        /// Propiedad de deteccion de cantidad de producto en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string cantidadProducto;
        public string CantidadProducto { get => cantidadProducto; set => SetProperty(ref cantidadProducto, value); }

        /// <summary>
        /// Propiedad de deteccion de proveedor en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string proveedorProducto;
        public string ProveedorProducto { get => proveedorProducto; set => SetProperty(ref proveedorProducto, value); }

        /// <summary>
        /// Propiedad de deteccion de¿ grupo en el crud y validacion de errores en los datos insertados
        /// </summary>
        private string grupoProducto;
        public string GrupoProducto { get => grupoProducto; set => SetProperty(ref grupoProducto, value); }

        #endregion

        #region Normas de validacion de errores

        /// <summary>
        /// En la siguiente seccion se crean las reglas de validacion para los errores de insercion de datos
        /// </summary>
        /// 
        private readonly List<String> ErrorList = new List<string>();
        private void ValidateErrors()
        {
            #region Reglas para nombre de producto
            if (String.IsNullOrEmpty(TtextNombreProducto))
            {
                ErrorList.Add("El nombre del producto no puede estar vacio");
            }
            #endregion

            #region Reglas para codigo de producto
            if (String.IsNullOrEmpty(TtexCodigoProducto))
            {
                ErrorList.Add("El codigo del producto no puede estar vacio");
            }

            #endregion

            #region Reglas para costo de producto
            try
            {
                Double.Parse(CostoProducto);
            }
            catch (ArgumentNullException ex)
            {
                ErrorList.Add("Ingrese un costo de producto valido");
            }
            catch (FormatException ex)
            {
                ErrorList.Add("Ingrese un costo de producto valido");
            }
            #endregion

            #region Reglas para precio de producto
            try
            {
                Double.Parse(PrecioProducto);
            }
            catch (ArgumentNullException ex)
            {
                ErrorList.Add("Ingrese un precio de producto valido");
            }
            catch (FormatException ex)
            {
                ErrorList.Add("Ingrese un precio de producto valido");
            }
            #endregion

            #region Reglas para cantidad de producto
            try
            {
                Int32.Parse(CantidadProducto);
            }
            catch (ArgumentNullException ex)
            {
                ErrorList.Add("Ingrese una cantidad de producto valida");
            }
            catch (FormatException ex)
            {
                ErrorList.Add("Ingrese una cantidad de producto valida");
            }
            #endregion

            #region Reglas para proveedor de producto
            if (String.IsNullOrEmpty(ProveedorProducto))
            {
                ErrorList.Add("Ingrese un proveedor de producto valido");
            }
            #endregion

            #region Reglas para grupo de producto
            if (String.IsNullOrEmpty(GrupoProducto))
            {
                ErrorList.Add("Ingrese un grupo de producto valido");
            }
            #endregion

        }
        #endregion

        #region Comprobacion de cambios en el CRUD
        /// <summary>
        /// En la siguiente seccion se incluyen el proceso de comprobacion de cambios en el CRUD de productos
        /// </summary>
        private readonly List<string> ModList = new List<string>();
        public void Comprobacion()
        {
            #region Reglas para nombre de producto
            if (!String.Equals(TtextNombreProducto,producto.Nombre))
            {
                ModList.Add("Nombre");
            }
            #endregion

            #region Reglas para codigo de producto
            if (!String.Equals(TtexCodigoProducto,producto.Codigo))
            {
                ModList.Add("Codigo");
            }

            #endregion

            #region Reglas para costo de producto
            if (!String.Equals(Convert.ToString(producto.Costo),Convert.ToString(CostoProducto)))
            {
                ModList.Add("Costo");
            }
            #endregion

            #region Reglas para precio de producto
            if (!String.Equals(Convert.ToString(producto.Precio), Convert.ToString(PrecioProducto)))
            {
                ModList.Add("Precio");
            }
            #endregion

            #region Reglas para cantidad de producto
            if (!String.Equals(Convert.ToString(producto.Cantidad), Convert.ToString(CantidadProducto)))
            {
                ModList.Add("Cantidad");
            }
            #endregion

            #region Reglas para descripcion de producto
            if (!String.Equals(Producto.Descripcion, CRUD.TBdescripcionProducto.Text))
            {
                ModList.Add("Descripcion");
            }
            #endregion

            #region Reglas para proveedor de producto
            if (!String.Equals((Producto.Proveedor - 1).ToString(),CRUD.CBProveedor.SelectedIndex.ToString()))
            {
                ModList.Add("Proveedor");
            }
            #endregion

            #region Reglas para grupo de producto
            if (!String.Equals((Producto.Grupo - 1).ToString(), CRUD.CBGrupo.SelectedIndex.ToString()))
            {
                ModList.Add("Grupo");
            }
            #endregion
        }
        #endregion

        #region Constructor de clase
        public CrudProductsViewModel()
        {
            CargarDetalleCRUD();
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion

        #region Cargar nombre de grupos y proveedores
        public async Task CargarDetalleCRUD()
        {
            SetUpLayer.SUL_Proveedores obj_sul_provedores = new SetUpLayer.SUL_Proveedores();
            NombreProveedor = await obj_sul_provedores.NombreProveedor();
            SetUpLayer.SUL_Grupos obj_sul_grupos = new SetUpLayer.SUL_Grupos();
            NombreGrupo = obj_sul_grupos.NombreGrupo();
        }
        #endregion

        #region Reguion ejecuion de comandos para botones
        private bool ViewCommandCanExecute(string action)
        {
            switch (action.ToLower())
            {
                case "cancelar":
                    return true;

                case "agregar":
                    return true;

                case "modificar":
                    return true;

                case "cerrar":
                    return true;

                default:
                    break;
            }

            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action.ToLower())
            {
                case "cancelar":
                    #region Boton cancelar
                    DialogResult dialogresult = System.Windows.Forms.MessageBox.Show("Desea cancelar la insercion de producto?", "CANCELAR", (MessageBoxButtons)MessageBoxButton.YesNo);
                    switch (dialogresult)
                    {
                        case DialogResult.Yes:
                            Window.Close();
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                #endregion

                case "agregar":
                    #region Boton agregar
                    ValidateErrors();
                    bool errors = GetErrors();
                    if (!errors)
                    {
                        byte[] test = { 0x50 };
                        Producto.Nombre = TtextNombreProducto;
                        Producto.Codigo = TtexCodigoProducto;
                        Producto.Costo = Convert.ToDouble(CostoProducto);
                        Producto.Precio = Convert.ToDouble(PrecioProducto);
                        Producto.Cantidad = Convert.ToInt32(CantidadProducto);
                        Producto.Grupo = NombreGrupo.IndexOf(GrupoProducto) + 1;
                        Producto.Proveedor = NombreProveedor.IndexOf(ProveedorProducto) + 1;
                        Producto.Imagen = test;
                        Producto.Activo1 = true;
                        Producto.Descripcion = CRUD.TBdescripcionProducto.Text;
                        SetUpLayer.SUL_Products obj_Sul_Products = new SetUpLayer.SUL_Products();
                        await obj_Sul_Products.Insertar(Producto);

                        TtextNombreProducto = null;
                        TtexCodigoProducto = null;
                        CostoProducto = null;
                        PrecioProducto = null;
                        CantidadProducto = null;
                        PrecioProducto = null;
                        ProveedorProducto = null;
                        GrupoProducto = null;
                        CRUD.TBdescripcionProducto.Text = null;
                    }

                    #endregion
                    break;

                case "modificar":
                    #region Boton Modificar
                    ValidateErrors();
                    if (!GetErrors())
                    {
                        Comprobacion();
                        if (GetModifications())
                        {
                            SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
                            obj_sul_products.Modificar(Producto);
                        }
                    }
                    break;
                #endregion

                case "cerrar":
                    #region Boton cerrar
                    Comprobacion();
                    if (ModList.Count > 0)
                    {
                        string ModMessage = "Se perderan los cambios, desea continuar? \n\n";
                        DialogResult closequestion = System.Windows.Forms.MessageBox.Show(ModMessage, "Comprobacion de Cambios",(MessageBoxButtons)MessageBoxButton.YesNo);
                        switch (closequestion)
                        {
                            case DialogResult.Yes: 
                                Window.Close();
                                break;
                            case DialogResult.No:
                                break;
                            default:
                                break;
                        }
                    }
                    else Window.Close();
                    #endregion
                    break;

                case "eliminar":
                    #region Boton eliminar
                    string ModMessage1 = "Desea eliminar el producto " + Producto.Nombre + " ?";
                    DialogResult dialogresult1 = System.Windows.Forms.MessageBox.Show(ModMessage1, "Eliminar Producto", (MessageBoxButtons)MessageBoxButton.YesNo);
                    switch (dialogresult1)
                    {
                        case DialogResult.Yes:
                            SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
                            if (await obj_sul_products.EliminarProducto(Producto.IdArticulo))
                            {
                                MessageBox.Show("Producto eliminado correctamente");
                                Window.Close();
                            }
                            else MessageBox.Show("Ocurrio un error intente de nuevo");
                                 Window.Close();
                            break;

                        case DialogResult.No:
                            break;

                        default:
                            break;
                    }
                        
                    #endregion
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Deteccion de modificaciones en el CRUD 
        private bool GetModifications()
        {
            string ModString = null;
            foreach (var modification in ModList)
            {
                ModString = ModString + (modification) + "\n";
            }
            if (ModList.Count > 0)
            {
                string ModMessage = "Desea aplicar las siguientes modificaciones: \n\n";
                DialogResult dialogresult = System.Windows.Forms.MessageBox.Show(ModMessage + ModString, "Actulizar Producto", (MessageBoxButtons)MessageBoxButton.YesNo);
                switch (dialogresult)
                {
                    case DialogResult.Yes:
                        byte[] test = { 0x50 };
                        Producto.Nombre = TtextNombreProducto;
                        Producto.Codigo = TtexCodigoProducto;
                        Producto.Costo = Convert.ToDouble(CostoProducto);
                        Producto.Precio = Convert.ToDouble(PrecioProducto);
                        Producto.Descripcion = CRUD.TBdescripcionProducto.Text;
                        Producto.Cantidad = Convert.ToInt32(CantidadProducto);
                        Producto.Proveedor = CRUD.CBProveedor.SelectedIndex + 1;
                        producto.Imagen = test;
                        Producto.Grupo = CRUD.CBGrupo.SelectedIndex + 1;
                        Window.Close();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        break;
                }
                ModList.Clear();
                return true;
            }
            else
            {
                MessageBox.Show("Por favor aplique una modificacion o cierre la ventana");
                return false;
            }
        }
        #endregion

        #region Deteccion de errores en el CRUD
        private bool GetErrors()
        {
            string ErrorsString = null;
            foreach (var error in ErrorList)
            {
                ErrorsString = ErrorsString + (error) + "\n";
            }
            if (ErrorList.Count > 0)
            {
                string ErrorMessage = "Se han encontrado los siguientes errores \n\n";
                MessageBox.Show(ErrorMessage + ErrorsString);
                ErrorList.Clear();
                return true;
            }
            else return false;

        }
        #endregion

        public IEnumerable GetErrors(string propertyName)
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
