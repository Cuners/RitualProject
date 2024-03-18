﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RitualProject
{
    /// <summary>
    /// Логика взаимодействия для ManagerOrdersUserControl.xaml
    /// </summary>
    public partial class ManagerOrdersUserControl : UserControl
    {
        public ManagerOrdersUserControl()
        {
            InitializeComponent();
            DataContext = new ManagerOrdersVM();
            (DataContext as ManagerOrdersVM).MoveToAddress += MoveToAddress;
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }
        private async void WebView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.Navigate("C:\\Users\\marat\\Downloads\\map.html");
        }

        private async void MoveToAddress(object sender, string address)
        {
            await webView.ExecuteScriptAsync($"moveToAddress('{address}');");
        }
    }
}
