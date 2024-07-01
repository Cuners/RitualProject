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
    /// Логика взаимодействия для AgentDocumentOrderWindow.xaml
    /// </summary>
    public partial class AgentDocumentOrderWindow : Window
    {
        public AgentOrdersVMList AgentOrderVMList { get; set; }
        public AgentDocumentOrderWindow(AgentOrdersVMList agentOrdersVMList)
        {
            InitializeComponent();
            AgentOrderVMList = agentOrdersVMList;
            this.DataContext = AgentOrderVMList;
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
