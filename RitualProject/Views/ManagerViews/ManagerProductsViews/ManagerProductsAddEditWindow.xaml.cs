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
    /// Логика взаимодействия для ManagerProductsAddEditWindow.xaml
    /// </summary>
    public partial class ManagerProductsAddEditWindow : Window
    {
        public ManagerProductsVMList ManagerProductsVMList { get; set; }
        public ManagerProductsAddEditWindow(ManagerProductsVMList managerProductsVMList)
        {
            InitializeComponent();
            ManagerProductsVMList=managerProductsVMList;
            this.DataContext=ManagerProductsVMList;
        }
        private void ButtonMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.window.Close();
        }
    }
}
