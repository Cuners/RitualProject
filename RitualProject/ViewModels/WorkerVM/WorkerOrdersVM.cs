using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RitualProject
{

        public class WorkerOrdersVM : ViewModel
        {
            private readonly ApiClient _apiClient = new ApiClient();
            public event EventHandler<string> MoveToAddress;
            public string Adres { get; set; }
            public ObservableCollection<UsersOrder> ResultOrders { get; set; }
            public ObservableCollection<UsersOrder> UsersOrders { get; set; }
            public Shipment Shipments { get; set; }
            private ObservableCollection<Order> _orders;
            public ObservableCollection<Order> Orders
            {
                get { return _orders; }
                set => Set(ref _orders, value);
            }
            private UsersOrder _selectedOrder;
            public UsersOrder SelectedOrder
            {
                get { return _selectedOrder; }
                set
                {
                    _selectedOrder = value;
                    if (SelectedOrder != null)
                    {
                        OnPropertyChanged("SelectedOrder");
                        foreach (var selected in SelectedOrder.Orders.Shipments)
                        {
                            Shipments = selected;
                        }
                        Adres = Shipments.Address;
                        Search();
                    }
                }
            }

            private async void Search()
            {
                string address = Adres.Replace("'", "\\'");
                MoveToAddress?.Invoke(this, address);
            }
            private async Task InitializeAsync()
            {
                Orders = new ObservableCollection<Order>();
                ResultOrders = new ObservableCollection<UsersOrder>();
                UsersOrders = new ObservableCollection<UsersOrder>();
                await LoadDataAsync();
                await LoadUsersOrderDataAsync();
            }
            public WorkerOrdersVM()
            {
                InitializeAsync();
            }
            public async Task LoadDataAsync()
            {
                try
                {

                    var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getOrders");
                    response.EnsureSuccessStatusCode();
                    var roleArray = await response.Content.ReadFromJsonAsync<Order[]>();
                    Orders.Clear();
                    ResultOrders.Clear();
                    foreach (var role in roleArray)
                    {
                        Orders.Add(role);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
                }
            }
            public async Task LoadUsersOrderDataAsync()
            {
                try
                {

                    var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getUserOrders");
                    response.EnsureSuccessStatusCode();
                    var roleArray = await response.Content.ReadFromJsonAsync<UsersOrder[]>();
                    UsersOrders.Clear();
                    foreach (var role in roleArray)
                    {
                        if (role.UserID == UserInfoConstant.ID)
                        {
                            UsersOrders.Add(role);
                            ResultOrders.Add(role);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
                }
            }
           
        }
}
