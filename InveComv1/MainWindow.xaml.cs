using System;
using System.Windows;
using System.Windows.Controls;

namespace InveComv1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Mover Ventana

        private void Mover(Border header)
        {
            var restaurar = false;
            header.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    if ((ResizeMode == ResizeMode.CanResize) || (ResizeMode == ResizeMode.CanResizeWithGrip))
                    {
                        CambiarEstado();
                    }
                }
                else
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        restaurar = true;
                    }
                    DragMove();
                }
            };
            header.MouseLeftButtonUp += (s, e) =>
            {
                restaurar = false;
            };
            header.MouseMove += (s, e) =>
            {
                if (restaurar)
                {
                    try
                    {
                        restaurar = false;
                        var mouseX = e.GetPosition(this).X;
                        var width = RestoreBounds.Width;
                        var x = mouseX - width / 2;
                        if (x < 0)
                        {
                            x = 0;
                        }
                        else if (x + width > SystemParameters.PrimaryScreenWidth)
                        {
                            x = SystemParameters.PrimaryScreenWidth - width;
                        }
                        WindowState = WindowState.Normal;
                        Left = x;
                        Top = 0;
                        DragMove();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            };
        }

        private void CambiarEstado()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        break;
                    }

                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        break;
                    }
            }
        }
        private void RestaurarVentana(object sender, RoutedEventArgs e)
        {
            Mover(sender as Border);
        }
        #endregion
    }

}
