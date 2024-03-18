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
    /// Логика взаимодействия для AdminUserAddEditWindow.xaml
    /// </summary>
    public partial class AdminUserAddEditWindow : Window
    {
        public AdminViewModelList adminViewModelList {  get; set; }
        public AdminUserAddEditWindow(AdminViewModelList ad)
        {
            InitializeComponent();
            adminViewModelList = ad;
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
