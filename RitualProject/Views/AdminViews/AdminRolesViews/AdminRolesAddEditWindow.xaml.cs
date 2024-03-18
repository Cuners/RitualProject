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
    /// Логика взаимодействия для AdminRolesAddEditWindow.xaml
    /// </summary>
    public partial class AdminRolesAddEditWindow : Window
    {
        public AdminRolesViewModelList adminViewModelList { get; set; }
        public AdminRolesAddEditWindow(AdminRolesViewModelList adminRolesViewModelList)
        {
            InitializeComponent();
            adminViewModelList = adminRolesViewModelList;
            this.DataContext = adminViewModelList;
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
