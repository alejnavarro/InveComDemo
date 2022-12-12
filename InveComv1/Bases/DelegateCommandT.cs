using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InveComv1.Bases
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        /// <summary>
        /// Constructor del comando
        /// </summary>
        public DelegateCommand(Action<T> execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Constructor del comando
        /// </summary>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Indica si se puede ejecutar el comando
        /// </summary>
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
                return true;

            if (parameter != null)
            {
                return this.canExecute((T)parameter);
            }

            return this.canExecute(default(T));
        }

        /// <summary>
        /// Evento para informar el cambio en CanExecute
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Ejectua el comando
        /// </summary>
        public void Execute(object parameter)
        {
            if (parameter != null)
                this.execute((T)parameter);
            else
                this.execute(default(T));
        }

        /// <summary>
        /// Invoca el evento CanExecuteChanged
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
