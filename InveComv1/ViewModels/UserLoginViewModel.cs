using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InveComv1.ViewModels
{
    public class UserLoginViewModel : Bases.BindableBase
    {
        #region Propiedades de la vista

        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Propiedad de deteccion del nombre de usuario que quiere iniciar sesion
        /// </summary>
        private string usuario;
        public string Usuario { get => usuario; set => SetProperty(ref usuario, value); }

        /// <summary>
        /// Propiedad de deteccion del nombre de usuario que quiere iniciar sesion
        /// </summary>
        private Views.UserLogin userLogIn;
        public Views.UserLogin UserLogIn { get => userLogIn; set => SetProperty(ref userLogIn, value); }

        /// <summary>
        /// Propiedad de deteccion de la clave del usuario actual
        /// </summary>
        private string clave;
        public string Clave { get => clave; set => SetProperty(ref clave, value); }

        #endregion

        #region Constructor de clase
        public UserLoginViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion


        #region Ejecucion de comandos para control de botones en la vista

        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "cancelar":
                    App.Current.Shutdown();
                    if (UserLogIn!= null)
                    {
                        UserLogIn.Close(); ;
                    }
                    break;

                case "ingresar":
                    try
                    {
                        SetUpLayer.SUL_Usuarios ValidarUsuario = new SetUpLayer.SUL_Usuarios();
                        TypesLayer.TL_Usuario userLogIn = new TypesLayer.TL_Usuario();
                        userLogIn = ValidarUsuario.LogIn(Usuario, Clave);

                        if (userLogIn.IdUsuario > 0)
                        {
                            Properties.Settings.Default.IdUsuario = userLogIn.IdUsuario;
                            Properties.Settings.Default.NameUsuario = userLogIn.Usuario;
                            Properties.Settings.Default.Tipo = userLogIn.Tipo;

                            MainWindow main = new MainWindow();
                            main.DataContext = new MainWindowViewModel()
                            {
                                Window = main
                            };
                            main.Show();
                            if (UserLogIn != null)
                            {
                                UserLogIn.Close(); ;
                            }
                            else
                            {
                                Application.Current.MainWindow.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Usuario o Clave Invalida, Intente de nuevo");
                            Usuario = null;
                            Clave = null;
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        //MessageBox.Show("Usuario o Clave Invalida, Intente de nuevo");
                    }

                    break;


                default:
                    break;
            }
        }

                #endregion
            }
}
