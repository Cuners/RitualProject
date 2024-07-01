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
    /// Логика взаимодействия для ManagerServicesAddEditWindow.xaml
    /// </summary>
    public partial class ManagerServicesAddEditWindow : Window
    {
        public ManagerServicesVMList ManagerServicesVMList { get; set; }
        public ManagerServicesAddEditWindow(ManagerServicesVMList managerServicesVMList)
        {
            InitializeComponent();
            ManagerServicesVMList = managerServicesVMList;
            this.DataContext = ManagerServicesVMList;
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
