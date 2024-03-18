using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RitualProject
{
    /// <summary>
    /// Логика взаимодействия для StorekeeperWindow.xaml
    /// </summary>
    public partial class StorekeeperWindow : Window
    {
        public StorekeeperWindow()
        {
            InitializeComponent();
        }
        private void ButtonMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_ClickMaxMin(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaxMinIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.DockWindow;
            }
            else
            {
                WindowState = WindowState.Normal;
                MaxMinIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Maximize;
            }
        }

        private void Button_ClickClose(object sender, RoutedEventArgs e)
        {
            this.window.Close();
        }
    }
}
