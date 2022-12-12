using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InveComv1.RCS.Templates;

namespace InveComv1.ViewModels
{

    public class AdministracionViewModel : Bases.BindableBase
    {
        private int selected;
        public int Selected { get => selected; set => SetProperty(ref selected, value); }

        private ObservableCollection<TabContentTemplate> tabView;
        public ObservableCollection<TabContentTemplate> TabView { get => tabView; set => SetProperty(ref tabView, value);}

        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        private Visibility ventanaAdminEnable;
        public Visibility VentanaAdminEnable { get => ventanaAdminEnable; set => SetProperty(ref ventanaAdminEnable, value); }

        private Visibility isVisible;
        public Visibility IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }

        private FrameworkElement userControl;
        public FrameworkElement UserControl { get { return userControl; } set { userControl = value; OnPropertyChanged("UserControl"); } }

        #region Constructor de clase
        public AdministracionViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
            TabView = new ObservableCollection<TabContentTemplate>();
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
                case "regresar":
                    UserControl = new Views.homepage();
                    VentanaAdminEnable = Visibility.Collapsed;
                    UserControl.DataContext = new homepageViewmodel();
                    break;

                case "devoluciones":
                    TabView.Add(new TabContentTemplate { Header = "Devoluciones  ", TabContent = new Views.Devoluciones()});
                    Selected++;
                    break;

                case "consultarventas":
                    TabView.Add(new TabContentTemplate { Header = " Consulta de Ventas  ", TabContent = new Views.ConsultaVentas() });
                    Selected++;
                    break;

                case "administracion":
                    UserControl = new Views.ModuloAdministrativo();
                    VentanaAdminEnable = Visibility.Collapsed;
                    UserControl.DataContext = new ModuloAdministrativoViewModel();
                    break;

                case "reporteventas":
                    TabView.Add(new TabContentTemplate { Header = " Consulta de Ventas  ", TabContent = new Views.ReporteVentas() });
                    Selected++;
                    break;

                case "cerrar":
                    if (Selected <= 0)
                    {
                        TabView.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        TabView.RemoveAt(Selected - 1);
                        break;
                    }

                default:
                    break;
            }

        }


    }
}
