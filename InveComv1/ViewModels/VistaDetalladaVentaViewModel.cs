using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InveComv1.ViewModels
{
    class VistaDetalladaVentaViewModel : Bases.BindableBase
    {
        #region Propiedades de la vista
        /// <summary>
        /// Propiedad de almacenamiento de los detalles de la venta consultada
        /// </summary>
        private TypesLayer.TL_Detalle_Completo_Venta detalleVenta;
        public TypesLayer.TL_Detalle_Completo_Venta DetalleVenta { get => detalleVenta; set => SetProperty(ref detalleVenta, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la ventana acutal para manejo de propiedades
        /// </summary>
        private Views.EditWindow window;
        public Views.EditWindow Window { get => window; set => SetProperty(ref window, value); }

        /// <summary>
        /// Propiedad de almacenamiento de los productos vendidas en la venta consultada
        /// </summary>
        private DataTable detalleProductos;
        public DataTable DetalleProductos { get => detalleProductos; set => SetProperty(ref detalleProductos, value); }


        /// <summary>
        /// Propiedad de ejecucion de comandos
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }
        #endregion

        #region Constructor de clase
        public VistaDetalladaVentaViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "cerrar":
                    Window.Close();
                    break;
            }

        }

    }
}
