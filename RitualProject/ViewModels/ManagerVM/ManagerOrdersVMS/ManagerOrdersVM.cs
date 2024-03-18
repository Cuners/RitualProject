using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RitualProject
{
    public class ManagerOrdersVM : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public event EventHandler<string> MoveToAddress;
        public string Adres {  get; set; }
        public ObservableCollection<Order> ResultOrders { get; set; }
        public Shipment Shipments { get; set; }
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set => Set(ref _orders, value);
        }
        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                if (SelectedOrder != null)
                {
                    OnPropertyChanged("SelectedOrder");
                    foreach(var selected in SelectedOrder.Shipments)
                    {
                        Shipments=selected;
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
        public ManagerOrdersVM()
        {
            Orders = new ObservableCollection<Order>();
            ResultOrders = new ObservableCollection<Order>();
            LoadDataAsync();
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
                    ResultOrders.Add(role);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }

        private RelayCommands _DeleteRoleCommand;

        public RelayCommands DeleteRoleCommand
        {
            get
            {
                return _DeleteRoleCommand ?? (_DeleteRoleCommand = new RelayCommands(async obj =>
                {
                    var RoleToRemove = obj as Role;
                    if (RoleToRemove != null)
                    {
                        //.Remove(RoleToRemove);
                        //ResultRoles.Remove(RoleToRemove);
                        //var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/Role/{RoleToRemove.RolesId}");
                        //response.EnsureSuccessStatusCode();
                    }
                }));
            }
        }
    }
}

