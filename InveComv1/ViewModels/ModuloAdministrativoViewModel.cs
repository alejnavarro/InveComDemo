using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InveComv1.ViewModels
{
    public class ModuloAdministrativoViewModel : Bases.BindableBase
    {
        #region Propieades de la clase
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        private FrameworkElement userControl;
        public FrameworkElement UserControl { get { return userControl; } set { userControl = value; OnPropertyChanged("UserControl"); } }

        private Visibility ventanaAdminEnable;
        public Visibility VentanaAdminEnable { get => ventanaAdminEnable; set => SetProperty(ref ventanaAdminEnable, value); }
        #endregion

        #region Constructor de clase
        public ModuloAdministrativoViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion

        #region Metodos para manejo de botones en la clase
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "regresar":
                    UserControl = new Views.Administracion();
                    VentanaAdminEnable = Visibility.Collapsed;
                    break;
            }
        }
        #endregion
    }
}
