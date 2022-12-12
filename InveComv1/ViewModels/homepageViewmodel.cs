using System.Windows;

namespace InveComv1.ViewModels
{
    class homepageViewmodel : Bases.BindableBase
    {
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        private Views.Ventas viewVentas;
        public Views.Ventas ViewVentas { get => viewVentas; set => SetProperty(ref viewVentas, value); }

        private FrameworkElement inventario;
        public FrameworkElement Inventario { get => inventario; set => SetProperty(ref inventario, value); }

        private Visibility btnHomePageEnable;
        public Visibility BtnHomePageEnable { get => btnHomePageEnable; set => SetProperty(ref btnHomePageEnable, value); }

        /// <summary>
        /// Propiedad de almacenamiento de la ventana principal 
        /// </summary>
        private MainWindow window;
        public MainWindow Window { get => window; set => SetProperty(ref window, value); }



        public homepageViewmodel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);

        }

        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private void ViewCommandExecute(string action)
        {
            if (action?.ToLower() == "botoninventario")
            {
                //UserControl = new Views.Inventario();
                //UserControl.DataContext = new ViewModels.InventoryViewModel() { Texto = "Inventario Actual de productos"};
                Inventario = new Views.Inventario();
                Inventario.DataContext = new ViewModels.InventoryViewModel() { Texto = "Inventario Actual de productos", InventoryEnabled  = Visibility.Visible};
                BtnHomePageEnable = Visibility.Collapsed;
            }
            if (action?.ToLower() == "botonventas")
            {
                ViewVentas = new Views.Ventas();
                ViewVentas.DataContext = new VentasViewModel()
                {
                    VistaVentas = ViewVentas
                };
                BtnHomePageEnable = Visibility.Collapsed;
            }
            if (action?.ToLower() == "administracion")
            {
                Inventario = new Views.Administracion();
                BtnHomePageEnable = Visibility.Collapsed;
            }
        }
    }
}
