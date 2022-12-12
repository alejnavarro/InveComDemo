using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace InveComv1.ViewModels
{
    public class ValidarUsuarioViewModel : Bases.BindableBase
    {
        #region Propieades de la vista
        /// <summary>
        /// Propiedad de declaracion de las propiedades que permiten ejecutar los comandos para manejo de botones
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Propiedad de deteccion de la clave del usuario actual
        /// </summary>
        private string clave;
        public string Clave { get => clave; set => SetProperty(ref clave, value); }

        /// <summary>
        /// Propiedad de deteccion de la clave del usuario actual
        /// </summary>
        private Views.ValidarUsuario window;
        public Views.ValidarUsuario Window { get => window; set => SetProperty(ref window, value); }

        /// <summary>
        /// Propiedad de deteccion de la clave del usuario actual
        /// </summary>
        private string usuario;
        public string Usuario { get => usuario; set => SetProperty(ref usuario, value); }
        #endregion

        #region Contructor de clase
        public ValidarUsuarioViewModel()
        {
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
        }
        #endregion

        #region Propiedad de ejecucion de comandos para botones

        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "validar":
                    try
                    {
                        SetUpLayer.SUL_Usuarios ValidarUsuario = new SetUpLayer.SUL_Usuarios();
                        TypesLayer.TL_Usuario userLogIn = new TypesLayer.TL_Usuario();
                        userLogIn = await Task.Run (() => ValidarUsuario.LogIn(Usuario, Clave));

                        if (userLogIn.IdUsuario > 0)
                        {
                            Properties.Settings.Default.IdUsuario = userLogIn.IdUsuario;
                            Properties.Settings.Default.Tipo = userLogIn.Tipo;
                            Window.Close();
                        }
                        else
                        {
                            MessageBox.Show("Clave incorrecta, contacte al supervisor");
                            Clave = null;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Clave incorrecta, contacte al supervisor");
                    }
                    break;

                case "cancelar":
                    Window.Close();
                    break;


            }
        }
        #endregion
    }
}
