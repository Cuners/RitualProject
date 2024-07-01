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
    public class AgentOrdersVM : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        
        public string Adres { get; set; }
        public ObservableCollection<ClientOrder> ResultOrders { get; set; }
        public ObservableCollection<ClientOrder> ClientOrders { get; set; }
        public Shipment Shipments { get; set; }
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set => Set(ref _orders, value);
        }

      
        private async Task InitializeAsync()
        {
            Orders = new ObservableCollection<Order>();
            ResultOrders = new ObservableCollection<ClientOrder>();
            ClientOrders = new ObservableCollection<ClientOrder>();
            await LoadDataAsync();
            await LoadClientOrderDataAsync();
        }
        public AgentOrdersVM()
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
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        public async Task LoadClientOrderDataAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getClientOrders");
                response.EnsureSuccessStatusCode();
                var roleArray = await response.Content.ReadFromJsonAsync<ClientOrder[]>();
                ClientOrders.Clear();
                foreach (var role in roleArray)
                {
                    ClientOrders.Add(role);
                    ResultOrders.Add(role);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        private RelayCommands _DeleteOrderCommand;

        public RelayCommands DeleteOrderCommand
        {
            get
            {
                return _DeleteOrderCommand ?? (_DeleteOrderCommand = new RelayCommands(async obj =>
                {
                    try
                    {
                        var OrderToRemove = obj as ClientOrder;
                        if (OrderToRemove != null)
                        {
                            var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/Order/{OrderToRemove.OrderID}");
                            response.EnsureSuccessStatusCode();
                            ClientOrders.Remove(OrderToRemove);
                            ResultOrders.Remove(OrderToRemove);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }
        private RelayCommands _DocumentOrderCommand;
        public RelayCommands DocumentOrderCommand
        {
            get
            {
                return _DocumentOrderCommand ?? (_DocumentOrderCommand = new RelayCommands(async obj =>
                {
                    var ClientOrders = obj as ClientOrder;
                    if (ClientOrders != null)
                    {
                        var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/ClientOrder/{ClientOrders.ClientOrdersID}");
                        response.EnsureSuccessStatusCode();
                        var orderArray = await response.Content.ReadFromJsonAsync<ClientOrder>();

                        AgentOrdersVMList orderViewModel = new AgentOrdersVMList(orderArray);
                        AgentDocumentOrderWindow orderWindow = new AgentDocumentOrderWindow(orderViewModel);
                        orderWindow.ShowDialog();
                    }
                }));
            }
        }
    }
}
