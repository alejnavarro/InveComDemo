using InveComv1.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    class BuscarProductosViewModel : Bases.BindableBase
    {

        #region Constructor de clase
        public BuscarProductosViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion

        #region Propiedades de inicializacion de la clase

        /// <summary>
        /// Declaracion de la propiedad para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get => viewCommand; set => SetProperty(ref viewCommand, value); }

        /// <summary>
        /// Ventana de contenido de la pagina
        /// </summary>
        private Views.EditWindow windowBuscar;
        public Views.EditWindow WindowBuscar { get => windowBuscar; set => SetProperty(ref windowBuscar, value); }
       
        /// <summary>
        /// Propiedad que contiene la pagina con el contenido actual de la ventana
        /// </summary>
        private Views.BuscarProducto pagBuscarProducto;
        public BuscarProducto PagBuscarProducto { get => pagBuscarProducto; set => SetProperty(ref pagBuscarProducto, value); }

        /// <summary>
        /// Propiedad para deteccion del codigo de producto a buscar
        /// </summary>
        private string codigoBusqueda;
        public string CodigoBusqueda { get => codigoBusqueda; set => SetProperty(ref codigoBusqueda, value); }

        /// <summary>
        /// Propiedad de muestreo del producto buscado
        /// </summary>
        private DataTable products;
        public DataTable Products { get => products; set => SetProperty(ref products, value); }
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
            switch (action)
            {
                case "cerrar":
                    WindowBuscar.Close();
                    break;

                case "buscar":
                    SetUpLayer.SUL_Products obj_sul_products = new SetUpLayer.SUL_Products();
                    Products = await obj_sul_products.ConsultarProducto(CodigoBusqueda);
                    break;

                default:
                    break;
            }
        }




        #endregion
    }
}
