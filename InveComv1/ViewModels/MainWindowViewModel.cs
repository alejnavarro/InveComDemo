using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InveComv1.ViewModels
{
    class MainWindowViewModel : Bases.BindableBase
    {
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        /// <summary>
        /// Propiedad de almacenamiento de la ventana principal 
        /// </summary>
        private MainWindow window;
        public MainWindow Window { get => window; set => SetProperty(ref window, value); }

        public MainWindowViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }

        private void ViewCommandExecute(string action)
        {

            if (action?.ToLower() == "botoncerrar")
            {
                Application.Current.Shutdown();
            }
            if (action?.ToLower() == "cerrarsesion")
            {
                Views.UserLogin userLogIn = new Views.UserLogin();
                userLogIn.DataContext = new UserLoginViewModel()
                {
                    UserLogIn = userLogIn
                };
                Window.Close();
                userLogIn.Show();
            }
        }
    }
}
