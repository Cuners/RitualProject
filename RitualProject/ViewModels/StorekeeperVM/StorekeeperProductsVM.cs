using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RitualServer.Model;
namespace RitualProject
{
    public class StorekeeperProductsVM : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private int _idWareHouse;
        public int IdWareHouse
        {
            get { return _idWareHouse; }
            set=>Set(ref  _idWareHouse, value);
        }
        private string _product;
        public string Product
        {
            get { return _product; }
            set => Set(ref _product, value);
        }

        private int? _quantitySklad;
        public int? QuantitySklad
        {
            get { return _quantitySklad; }
            set
            {
                _quantitySklad=value;
                OnPropertyChanged("QuantitySklad");
                if(QuantitySklad != null || QuantitySklad!=0)
                {
                    int summa=Convert.ToInt32(Quantity)-Convert.ToInt32(QuantitySklad);
                    if (summa < 0)
                    {
                        DataIzlishkiIliNet = Convert.ToString(summa * Convert.ToInt32(Price) * (-1));
                        Status = "Излишки на:";
                    }
                    else if (summa > 0)
                    {
                        DataIzlishkiIliNet = Convert.ToString(summa * Convert.ToInt32(Price) * (-1));
                        Status = "Недостатки на:";
                    }
                    else if (summa == 0)
                    {
                        DataIzlishkiIliNet = "";
                        Status = "";
                    }
                }
            }
        }
        private int? _quantity;
        public int? Quantity
        {
            get { return _quantity; }
            set => Set(ref _quantity, value);
        }
        private string _price;
        public string Price
        {
            get { return _price; }
            set => Set(ref _price, value);
        }
        private string _dataIzlishkiIliNet;
        public string DataIzlishkiIliNet
        {
            get { return _dataIzlishkiIliNet; }
            set => Set(ref _dataIzlishkiIliNet, value);
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set => Set(ref _status, value);
        }

        public ObservableCollection<WareHouse> ResultWareHouse { get; set; }
        private ObservableCollection<WareHouse> _wareHouses;
        public ObservableCollection<WareHouse> WareHouses
        {
            get { return _wareHouses; }
            set => Set(ref _wareHouses, value);
        }
        private ObservableCollection<InventarizationDTO> _inventarizationDTO;
        public ObservableCollection<InventarizationDTO> InventarizationDTO
        {
            get { return _inventarizationDTO; }
            set=>Set(ref _inventarizationDTO, value);
        }
        private WareHouse _wareHouseSelectedItem;
        public WareHouse WareHouseSelectedItem
        {
            get { return _wareHouseSelectedItem; }
            set
            {
                _wareHouseSelectedItem = value;
                if(WareHouseSelectedItem != null)
                {
                    OnPropertyChanged("WareHouseSelectedItem");
                    IdWareHouse = WareHouseSelectedItem.CompositionId;
                    Product = WareHouseSelectedItem.Product.Name;
                    Quantity = WareHouseSelectedItem.Quantity;
                    Price= WareHouseSelectedItem.Product.Price;
                    
                }
            }
        }
        private InventarizationDTO _inventarizationDTOSelectedItem;
        public InventarizationDTO InventarizationDTSelectedItem
        {
            get { return _inventarizationDTOSelectedItem; }
            set
            {
                _inventarizationDTOSelectedItem = value;
                if (InventarizationDTSelectedItem != null)
                {
                    OnPropertyChanged("WareHouseSelectedItem");
                }
            }
        }
        public StorekeeperProductsVM()
        {
            ResultWareHouse = new ObservableCollection<WareHouse>();
            WareHouses=new ObservableCollection<WareHouse>();
            InventarizationDTO = new ObservableCollection<InventarizationDTO>();
            LoadDataAsync();
        }
        public async Task LoadDataAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getWareHouses");
                response.EnsureSuccessStatusCode();
                var wareArray = await response.Content.ReadFromJsonAsync<WareHouse[]>();
                ResultWareHouse.Clear();
                WareHouses.Clear();
                foreach (var ware in wareArray)
                {
                    WareHouses.Add(ware);
                    ResultWareHouse.Add(ware);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        private RelayCommands _AddInventarization;

        public RelayCommands AddInventarization
        {
            get
            {
                return _AddInventarization ?? (_AddInventarization = new RelayCommands(async obj =>
                {
                    InventarizationDTO dto = new InventarizationDTO()
                    {
                        CompositionID = IdWareHouse,
                        Product = Product,
                        Quantity = Convert.ToInt32(Quantity),
                        QuantityFact = Convert.ToInt32(QuantitySklad),
                        Price = Convert.ToInt32(Quantity) * Convert.ToInt32(Price),
                        Prices = Convert.ToInt32(Price),
                        PriceFact = Convert.ToInt32(DataIzlishkiIliNet)
                    };
                    InventarizationDTO.Add(dto);
                }));
            }
        }
        private RelayCommands _MinusInventarization;

        public RelayCommands MinusInventarization
        {
            get
            {
                return _MinusInventarization ?? (_MinusInventarization = new RelayCommands(async obj =>
                {
                   
                        if (InventarizationDTSelectedItem != null)
                        {
                            InventarizationDTO.Remove(InventarizationDTSelectedItem);
                        }
                    
                }));
            }
        }
        private RelayCommands _CreateDocument;

        public RelayCommands CreateDocument
        {
            get
            {
                return _CreateDocument ?? (_CreateDocument = new RelayCommands(async obj =>
                {

                    if (InventarizationDTO != null)
                    {
                        DocumentGeneratorInvent documentGeneratorInvent = new DocumentGeneratorInvent();
                        documentGeneratorInvent.GenerateDocument(InventarizationDTO,"Абобус");
                        InventarizationDTO.Clear();
                    }

                }));
            }
        }
    }
}
