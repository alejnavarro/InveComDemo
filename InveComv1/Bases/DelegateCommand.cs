using System;
using System.Windows.Input;

namespace InveComv1.Bases
{
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        /// <summary>
        /// Constructor del comando
        /// </summary>
        public DelegateCommand(Action execute)
        {
            this.execute = execute;
        }

        /// <summary>
        /// Constructor del comando
        /// </summary>
        public DelegateCommand(Action execute, Func<bool> canExecute)
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

            return this.canExecute();
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
            this.execute();
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
