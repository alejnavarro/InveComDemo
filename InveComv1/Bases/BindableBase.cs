using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InveComv1.Bases
{
    /// <summary>
    /// Clase base para una clase Bindable
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Evento que informa de la propiedad actualizada
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Invoca el evento PropertyChanged</summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Invoca el evento PropertyChanged si los valores no son iguales</summary>
        public bool SetProperty<T>(ref T propertyBackStore, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(propertyBackStore, newValue)) return false;

            propertyBackStore = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
