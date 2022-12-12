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
    class CierreCajaViewModel : Bases.BindableBase
    {
        #region Propoiedades de la clase
        /// <summary>
        /// Propiedad de ejecucion de comandos
        /// </summary>
        private Bases.DelegateCommand<string> viewCommand;
        public Bases.DelegateCommand<string> ViewCommand { get { return viewCommand; } set { SetProperty(ref viewCommand, value); } }

        /// <summary>
        /// Propiedad de deteccion de la fecha del dia
        /// </summary>
        private TypesLayer.TL_CierreCaja cierreCaja;
        public TypesLayer.TL_CierreCaja CierreCaja { get { return cierreCaja; } set { SetProperty(ref cierreCaja, value); } }

        /// <summary>
        /// Propiedad de deteccion de la fecha del dia
        /// </summary>
        private Views.EditWindow window;
        public Views.EditWindow Window { get { return window; } set { SetProperty(ref window, value); } }

        #endregion

        #region Contructor de clase
        public CierreCajaViewModel()
        {
            CierreCaja = new TypesLayer.TL_CierreCaja();
            ViewCommand = new Bases.DelegateCommand<string>(ViewCommandExecute, ViewCommandCanExecute);
            Task.Run (() => loadcomponent());
        }
        #endregion

        public async void loadcomponent ()
        {
            SetUpLayer.SUL_Totalizacion obj_sul_totalizacion = new SetUpLayer.SUL_Totalizacion();
            CierreCaja = await Task.Run (() => obj_sul_totalizacion.PrepararCierre());
            CierreCaja.Usuario = Properties.Settings.Default.NameUsuario;
        }

        #region Ejecucion de comandos de la clase
        private bool ViewCommandCanExecute(string action)
        {
            return true;
        }

        private async void ViewCommandExecute(string action)
        {
            switch (action)
            {
                case "cancelar":
                    
                    DialogResult result = MessageBox.Show("Desea Cancelar el cierre de caja?", "Cancelar Cierre de Caja", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Window.Close();
                        break;
                    }
                    else
                    {
                        break;
                    }

                case "cerrarcaja":
                    SetUpLayer.SUL_Totalizacion obj_sul_totalizacion = new SetUpLayer.SUL_Totalizacion();
                    CierreCaja.IdUsuario = Properties.Settings.Default.IdUsuario;
                    if (await obj_sul_totalizacion.CerrarCaja(CierreCaja))
                    {
                        MessageBox.Show("Cierra de caja exitoso, Inicie sesion para continuar");
                        Window.Close();
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
