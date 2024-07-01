
using Microsoft.VisualBasic.ApplicationServices;
using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;

namespace RitualProject
{
    public class AgentOrdersVMList : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private ObservableCollection<ActDTO> _ActDTO;
        public ObservableCollection<ActDTO> ActDTO
        {
            get { return _ActDTO; }
            set => Set(ref _ActDTO, value);
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
            catch (Exception ex)
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
        private async Task InitializeAsync(ClientOrder client)
        {
            await LoadOrderItemAsync(client);
            await LoadOrderServiceAsync(client);
            if (OrderItemEd != null)
            {
                var Act = new ActDTO()
                {
                    Zakaz = OrderItemEd.Product.Name,
                    Price = OrderItemEd.Product.Price,
                    Quantity = OrderItemEd.Quantity
                };
                ActDTO.Add(Act);
            }
            else if (OrderServiceEd != null)
            {
                try
                {
                    double price = Convert.ToDouble(OrderServiceEd.Service.Price);
                    var Act = new ActDTO()
                    {
                        Zakaz = OrderServiceEd.Service.Name,
                        Price = price,
                        Quantity = 1
                    };
                    ActDTO.Add(Act);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public AgentOrdersVMList(ClientOrder clientOrder)
        {
            ActDTO = new ObservableCollection<ActDTO>();
            if (clientOrder != null)
            {
                InitializeAsync(clientOrder);
            }
            
        }
        public RelayCommands _CreateActDocument;

        public RelayCommands CreateActDocument
        {
            get
            {
                return _CreateActDocument ?? (_CreateActDocument = new RelayCommands(async obj =>
                {
                    DocumentGeneratorAct documentGeneratorInvent = new DocumentGeneratorAct();
                    documentGeneratorInvent.GenerateDocument(ActDTO, UserInfoConstant.FullName);
                }));
            }
        }

        private RelayCommands _CreateCheckDocument;

        public RelayCommands CreateCheckDocument
        {
            get
            {
                return _CreateCheckDocument ?? (_CreateCheckDocument = new RelayCommands(async obj =>
                {

                    
                }));
            }
        }
    }
}
