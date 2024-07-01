
using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Windows.Media;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace RitualProject
{
    public class ManagerOrdersVMList : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public string _whatNow;
        public string WhatNow
        {
            get { return _whatNow; }
            set => Set(ref _whatNow, value);
        }
        private ClientOrder _clientOrders;
        public ClientOrder ClientOrdersEd
        {
            get { return _clientOrders; }
            set => Set(ref _clientOrders, value);
        }
        private Client _client;
        public Client ClientEd
        {
            get { return _client; }
            set => Set(ref _client, value);
        }
        private OrderItem _orderItem;
        public OrderItem OrderItemEd
        {
            get { return _orderItem; }
            set => Set(ref _orderItem, value);
        }
        private OrderService _orderService;
        public OrderService OrderServiceEd
        {
            get { return _orderService; }
            set => Set(ref _orderService, value);
        }
        private ObservableCollection<UsersOrder> _UsersOrder;
        public ObservableCollection<UsersOrder> UsersOrder
        {
            get { return _UsersOrder; }
            set => Set(ref _UsersOrder, value);
        }
        public ObservableCollection<UsersOrder> UsersOrdersListBoxDeleted { get; set; }
        private ObservableCollection<UsersOrder> _UsersOrdersListBox;
        public ObservableCollection<UsersOrder> UsersOrdersListBox
        {
            get { return _UsersOrdersListBox; }
            set => Set(ref _UsersOrdersListBox, value);
        }
        
        public Action Close { get; set; }
        private Shipment _shipment;
        public Shipment ShipmentEd
        {
            get { return _shipment; }
            set => Set(ref _shipment, value);
        }
        private ObservableCollection<StatusOrder> _statusOrders;
        public ObservableCollection<StatusOrder> StatusOrder
        {
            get { return _statusOrders; }
            set => Set(ref _statusOrders, value);
        }
        private Order _orders;
        public Order OrderEd
        {
            get { return _orders; }
            set => Set(ref _orders, value);
        }
        private ObservableCollection<Product> _Products;
        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set => Set(ref _Products, value);
        }
        private ObservableCollection<Service> _services;
        public ObservableCollection<Service> Services
        {
            get { return _services; }
            set => Set(ref _services, value);
        }
        private UserControl _ProductUserControl;
        private UserControl _ServicesUserControl;
        private UserControl currentControl;
        public UserControl CurrentControl
        {
            get { return currentControl; }
            set => Set(ref currentControl, value);
        }
        private ManagerOrdersVM managerViewModel;
        public ManagerOrdersVM ManagerViewModel
        {
            get { return managerViewModel; }
            set => Set(ref managerViewModel, value);
        }
        private UsersOrder _usersOrdersListBoxSelectedItem;
        public UsersOrder UsersOrdersListBoxSelectedItem
        {
            get { return _usersOrdersListBoxSelectedItem; }
            set
            {
                _usersOrdersListBoxSelectedItem = value;
                if (UsersOrdersListBoxSelectedItem != null)
                {
                    OnPropertyChanged("UsersOrdersListBoxSelectedItem");
                }
            }
        }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set => Set(ref _users, value);
        }
        private User _usersSelectedItem;
        public User UsersSelectedItem
        {
            get { return _usersSelectedItem; }
            set
            {
                _usersSelectedItem = value;
                if (UsersSelectedItem != null)
                {
                    OnPropertyChanged("UsersOrderSelectedItem");
                }
            }
        }
        async void CloseWindow()
        {
            Close?.Invoke();
        }
        public bool CanClose()
        {
            return true;
        }
        private int _orderType;

        public int OrderType
        {
            get { return _orderType; }
            set
            {
                _orderType = value;
                OnPropertyChanged(nameof(OrderType));
                UpdateCurrentControl();
            }
        }
        private void UpdateCurrentControl()
        {
            if (OrderType == 1)
            {
                CurrentControl = _ProductUserControl;
                
            }
            else if (OrderType == 2) 
            { 
                CurrentControl=_ServicesUserControl;
            }
        }
        #region Api
        public async Task LoadUsersAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getArticle");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<User[]>();
                Users.Clear();
                foreach (var product in productArray)
                {
                    if (product.RoleId == 3)
                    {
                        Users.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }
        public async Task LoadServicesAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getServices");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Service[]>();
                Services.Clear();
                foreach (var product in productArray)
                {
                    Services.Add(product);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}");
            }
        }
        public async Task LoadStatusesAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getStatusOrders");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<StatusOrder[]>();
                StatusOrder.Clear();
                foreach (var product in productArray)
                {
                    StatusOrder.Add(product);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}");
            }
        }
        public async Task LoadProductsAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getProducts");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Product[]>();
                Products.Clear();
                foreach (var product in productArray)
                {
                    Products.Add(product);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}");
            }
        }
        public async Task LoadUsersOrdersAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getUserOrders");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<UsersOrder[]>();
                UsersOrder.Clear();
                foreach (var product in productArray)
                {
                    if (product.OrderID == ClientOrdersEd.OrderID)
                    {
                        UsersOrder.Add(product);
                        UsersOrdersListBox.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }
        public async Task LoadOrderItemAsync(ClientOrder clientOrder)
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/OrderItem?idorder={clientOrder.OrderID}");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var productArray = await response.Content.ReadFromJsonAsync<OrderItem>();
                    OrderItemEd = productArray;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task LoadOrderServiceAsync(ClientOrder clientOrder)
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/OrderService?idorder={clientOrder.OrderID}");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var productArray = await response.Content.ReadFromJsonAsync<OrderService>();
                    OrderServiceEd = productArray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task EditInitialize(ClientOrder client)
        {
                WhatNow = "Редактирование заказа";
                ClientOrdersEd = client;
                ClientEd = client.Clients;
                OrderEd = client.Orders;
                foreach (var selected in client.Orders.Shipments)
                {
                    ShipmentEd = selected;
                }
                await LoadOrderItemAsync(client);
                await LoadOrderServiceAsync(client);
                await LoadUsersOrdersAsync();
                if (OrderItemEd != null)
                {
                    OrderType = 1;
                }
                else if (OrderServiceEd != null)
                {
                    OrderType = 2;
                }
        }
        #endregion
        private async Task InitializeAsync(ClientOrder client)
        {
            UsersOrder = new ObservableCollection<UsersOrder>();
            UsersOrdersListBox = new ObservableCollection<UsersOrder>();
            UsersOrdersListBoxDeleted = new ObservableCollection<UsersOrder>();
            StatusOrder = new ObservableCollection<StatusOrder>();
            Products = new ObservableCollection<Product>();
            Services = new ObservableCollection<Service>();
            Users = new ObservableCollection<User>();
            await LoadServicesAsync();
            
            await LoadStatusesAsync();
            await LoadProductsAsync();
            await LoadUsersAsync();
            if (client != null)
            {
                await EditInitialize(client);
            }
            else
            {
                WhatNow = "Добавление заказа";
            }
        }
        public ManagerOrdersVMList(ClientOrder clientOrder)
        {
            _ProductUserControl = new ProductOrdersUserControl();
            _ServicesUserControl = new ServicesOrderUserControl();
            OrderType = 1;
            if(clientOrder != null) 
            {
                InitializeAsync(clientOrder);
            }
        }
        public RelayCommands _ShowProductsUserControlCommand;

        public RelayCommands ShowProductsUserControlCommand
        {
            get
            {
                return _ShowProductsUserControlCommand ?? (_ShowProductsUserControlCommand = new RelayCommands(async obj =>
                {
                    OrderType = 1;
                }));
            }
        }
        public RelayCommands _ShowServicesUserControlCommand;

        public RelayCommands ShowServicesUserControlCommand
        {
            get
            {
                return _ShowServicesUserControlCommand ?? (_ShowServicesUserControlCommand = new RelayCommands(async obj =>
                {
                    OrderType = 2;
                }));
            }
        }
        private RelayCommands _AddUser;

        public RelayCommands AddUser
        {
            get
            {
                return _AddUser ?? (_AddUser = new RelayCommands(async obj =>
                {
                    if (UsersSelectedItem != null)
                    {
                        UsersOrder dto = new UsersOrder()
                        {
                            UserID=UsersSelectedItem.UserId,
                            Users=UsersSelectedItem
                        };
                        var user=UsersOrdersListBox.Where(x => x.UserID == dto.UserID).FirstOrDefault();
                        if (user != null)
                        {
                            MessageBox.Show("Такой работник уже задействован");
                            return;
                        }
                        UsersOrdersListBox.Add(dto);
                        var userdeleted = UsersOrdersListBoxDeleted.Where(x => x.UserID == dto.UserID).FirstOrDefault();
                        if (userdeleted != null)
                        {
                            UsersOrdersListBoxDeleted.Remove(userdeleted);
                        }
                    }
                }));
            }
        }
        private RelayCommands _MinusUser;

        public RelayCommands MinusUser
        {
            get
            {
                return _MinusUser ?? (_MinusUser = new RelayCommands(async obj =>
                {
                    if (UsersOrdersListBoxSelectedItem != null)
                    {
                        UsersOrdersListBoxDeleted.Add(UsersOrdersListBoxSelectedItem);
                        UsersOrdersListBox.Remove(UsersOrdersListBoxSelectedItem);
                    }

                }));
            }
        }
        public async Task PostUsersOrders(int orderid)
        {
            try
            {
                foreach(var userorder in UsersOrdersListBox)
                {
                    UsersOrder order=UsersOrder.Where(x=>x.OrderID==userorder.OrderID).FirstOrDefault();
                    if (order == null)
                    {
                        userorder.OrderID = orderid;
                        userorder.Users = null;
                        userorder.Orders = null;
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/UserOrder", new StringContent(JsonConvert.SerializeObject(userorder), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task DeleteUsersOrders()
        {
            try
            {
                foreach (var userorder in UsersOrdersListBoxDeleted)
                {
                    UsersOrder order = UsersOrder.Where(x => x.OrderID == userorder.OrderID && x.UserID==userorder.UserID).FirstOrDefault();
                    if (order != null)
                    {
                        var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/UserOrder/{order.UsersOrderID}");
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task<Client> FindClient(string name, string email, string phone)
        {
            var queryString = $"search?name={WebUtility.UrlEncode(name)}&email={WebUtility.UrlEncode(email)}&phone={WebUtility.UrlEncode(phone)}";
            HttpResponseMessage response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/Client/{queryString}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Client>(responseBody);
            }

            return null;
        }
        private async Task<int> CreateClient(Client client)
        {
            HttpResponseMessage response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Client",
                new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<Client>(responseBody);
            return createdClient.ClientId;
        }
        private RelayCommands _EditAddOrderCommand;
        public RelayCommands EditAddOrderCommand
        {
            get
            {
                return _EditAddOrderCommand ?? (_EditAddOrderCommand = new RelayCommands(async obj =>
                {
                    if (WhatNow == "Редактирование заказа")
                    {
                        if (string.IsNullOrWhiteSpace(ClientEd.FIO) || string.IsNullOrWhiteSpace(ClientEd.Telephone) || string.IsNullOrWhiteSpace(ClientEd.Email))
                        {
                            MessageBox.Show("Введите информацию о клиенте");
                            return;
                        }
                        else if (OrderEd.StatusId == 0)
                        {
                            MessageBox.Show("Выберите статус");
                            return;
                        }
                        else if (OrderType == 1 && OrderItemEd.ProductId == 0)
                        {
                            MessageBox.Show("Выберите товар");
                            return;
                        }
                        else if (OrderType == 1 && OrderItemEd.Quantity == 0)
                        {
                            MessageBox.Show("Введите количество");
                            return;
                        }
                        else if (OrderType == 2 && OrderServiceEd.ServiceId == 0)
                        {
                            MessageBox.Show("Выберите услугу");
                            return;
                        }
                        var response6 = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Client/{ClientEd.ClientId}", new StringContent(JsonConvert.SerializeObject(ClientEd), Encoding.UTF8, "application/json"));
                        response6.EnsureSuccessStatusCode();
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Order/{OrderEd.OrderId}", new StringContent(JsonConvert.SerializeObject(OrderEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        var response2 = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/ClientOrder/{ClientOrdersEd.ClientOrdersID}", new StringContent(JsonConvert.SerializeObject(ClientOrdersEd), Encoding.UTF8, "application/json"));
                        response2.EnsureSuccessStatusCode();
                        var response5 = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Shipment/{ShipmentEd.ShipmentId}", new StringContent(JsonConvert.SerializeObject(ShipmentEd), Encoding.UTF8, "application/json"));
                        response5.EnsureSuccessStatusCode();
                        if (OrderType == 1)
                        {
                            var response3 = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/OrderItem/{OrderItemEd.OrderItemId}", new StringContent(JsonConvert.SerializeObject(OrderItemEd), Encoding.UTF8, "application/json"));
                            response3.EnsureSuccessStatusCode();
                        }
                        else if (OrderType == 2)
                        {
                            var response3 = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/OrderService/{OrderServiceEd.OrderServiceId}", new StringContent(JsonConvert.SerializeObject(OrderServiceEd), Encoding.UTF8, "application/json"));
                            response3.EnsureSuccessStatusCode();
                        }
                        await DeleteUsersOrders();
                        await PostUsersOrders(OrderEd.OrderId);
                        CloseWindow();
                        await ManagerViewModel.LoadClientOrderDataAsync();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(ClientEd.FIO) || string.IsNullOrWhiteSpace(ClientEd.Telephone) || string.IsNullOrWhiteSpace(ClientEd.Email))
                        {
                            MessageBox.Show("Введите информацию о клиенте");
                            return;
                        }
                        else if (OrderEd.StatusId == 0)
                        {
                            MessageBox.Show("Выберите статус");
                            return;
                        }
                        else if(OrderType==1 && OrderItemEd.ProductId == 0)
                        {
                            MessageBox.Show("Выберите товар");
                            return;
                        }
                        else if (OrderType == 1 && OrderItemEd.Quantity == 0)
                        {
                            MessageBox.Show("Введите количество");
                            return;
                        }
                        else if (OrderType == 2 && OrderServiceEd.ServiceId == 0)
                        {
                            MessageBox.Show("Выберите услугу");
                            return;
                        }
                        var client = await FindClient(ClientEd.FIO, ClientEd.Email, ClientEd.Telephone);
                        if (client == null)
                        {
                            // Создаем клиента, если его нет
                            client = new Client
                            {
                                FIO = ClientEd.FIO,
                                Email = ClientEd.Email,
                                Telephone = ClientEd.Telephone
                            };
                            client.ClientId = await CreateClient(client);
                        }
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Order", new StringContent(JsonConvert.SerializeObject(OrderEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var createdOrder = JsonConvert.DeserializeObject<Order>(responseBody);
                        var orderId = createdOrder.OrderId;
                        ClientOrdersEd.OrderID = orderId;
                        ClientOrdersEd.OrderID = client.ClientId;
                        var response2 = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/ClientOrder", new StringContent(JsonConvert.SerializeObject(ClientOrdersEd), Encoding.UTF8, "application/json"));
                        response2.EnsureSuccessStatusCode();
                        ShipmentEd.OrderId = orderId;
                        var response5 = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Shipment", new StringContent(JsonConvert.SerializeObject(ShipmentEd), Encoding.UTF8, "application/json"));
                        response5.EnsureSuccessStatusCode();
                        if (OrderType == 1)
                        {
                            OrderItemEd.OrderId=orderId;
                            var response3 = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/OrderItem", new StringContent(JsonConvert.SerializeObject(OrderItemEd), Encoding.UTF8, "application/json"));
                            response3.EnsureSuccessStatusCode();
                        }
                        else if (OrderType == 2)
                        {
                            OrderServiceEd.OrderId=orderId;
                            var response3 = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/OrderService", new StringContent(JsonConvert.SerializeObject(OrderServiceEd), Encoding.UTF8, "application/json"));
                            response3.EnsureSuccessStatusCode();
                        }
                        await PostUsersOrders(orderId);
                        CloseWindow();
                        await ManagerViewModel.LoadClientOrderDataAsync();
                    }
                }));
            }
        }
    }
}
