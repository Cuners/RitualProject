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
    public class ManagerProductsVM : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public ObservableCollection<Product> ResultProducts { get; set; }
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set => Set(ref _products, value);
        }

        #region Количество категории товаров
        private int _countUrns = 0;
        public int CountUrns
        {
            get { return _countUrns; }
            set => Set(ref _countUrns,value);
        }
        private int _countClothe = 0;
        public int CountClothe
        {
            get { return _countClothe; }
            set => Set(ref _countClothe, value);
        }
        private int _countMonuments = 0;
        public int CountMonuments
        {
            get { return _countMonuments; }
            set => Set(ref _countMonuments, value);
        }
        private int _countCrosses = 0;
        public int CountCrosses
        {
            get { return _countCrosses; }
            set => Set(ref _countCrosses, value);
        }
        private int _countTapes = 0;
        public int CountTapes
        {
            get { return _countTapes; }
            set => Set(ref _countTapes, value);
        }
        private int _countCoffins = 0;
        public int CountCoffins
        {
            get { return _countCoffins; }
            set => Set(ref _countCoffins, value);
        }
        #endregion
        private string _searchProductByName;
        public string SearchProductByName
        {
            get { return _searchProductByName; }
            set
            {
                if (_searchProductByName != value)
                {
                    _searchProductByName = value;
                    OnPropertyChanged("SearchProductByName");
                    SearchProductsAsync();
                }
            }
        }
        public ManagerProductsVM()
        {
            Products = new ObservableCollection<Product>();
            ResultProducts = new ObservableCollection<Product>();
            //LoadDataAsync();
            LoadProductAsync();
        }
        public async void SearchProductsAsync()
        {
            if (string.IsNullOrEmpty(SearchProductByName))
            {
                await Task.Run(() =>
                {
                    Products = ResultProducts;
                });
            }
            else
            {
                await Task.Run(() =>
                {
                    Products = new ObservableCollection<Product>(ResultProducts.Where(x => x.Name.ToLower().Contains(SearchProductByName.ToLower())));
                });

            }
        }
        public void CountProductsCategory(Product product)
        {
            if (product.CategoryId == 1)
            {
                CountCoffins++;
            }
            else if(product.CategoryId == 2)
            {
                CountCrosses++;
            }
            else if (product.CategoryId == 3)
            {
                CountMonuments++;
            }
            else if (product.CategoryId == 4)
            {
                CountTapes++;
            }
            else if (product.CategoryId == 5)
            {
                CountClothe++;
            }
            else if (product.CategoryId == 6)
            {
                CountUrns++;
            }
        }
        public async Task LoadProductAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getProducts");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Product[]>();
                Products.Clear();
                ResultProducts.Clear();
                foreach (var product in productArray)
                {
                    Products.Add(product);
                    ResultProducts.Add(product);
                    CountProductsCategory(product);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        private RelayCommands _EditProductCommand;
        public RelayCommands EditProductCommand
        {
            get
            {
                return _EditProductCommand ?? (_EditProductCommand = new RelayCommands(async obj =>
                {
                    var ProductToEdit = obj as Product;
                    if (ProductToEdit != null)
                    {
                        var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/Product/{ProductToEdit.ProductId}");
                        response.EnsureSuccessStatusCode();
                        var productArray = await response.Content.ReadFromJsonAsync<Product>();

                        ManagerProductsVMList managerViewModel = new ManagerProductsVMList(productArray) { ManagerViewModel = this };
                        ManagerProductsAddEditWindow managerWindow = new ManagerProductsAddEditWindow(managerViewModel);
                        managerWindow.ShowDialog();
                    }
                    else
                    {
                        ManagerProductsVMList managerViewModel = new ManagerProductsVMList(null) { ManagerViewModel = this };
                        ManagerProductsAddEditWindow managerWindow = new ManagerProductsAddEditWindow(managerViewModel);
                        managerWindow.ShowDialog();
                    }
                }));
            }
        }
        private RelayCommands _DeleteProductCommand;

        public RelayCommands DeleteProductCommand
        {
            get
            {
                return _DeleteProductCommand ?? (_DeleteProductCommand = new RelayCommands(async obj =>
                {
                    var ProductToRemove = obj as Product;
                    if (ProductToRemove != null)
                    {
                        Products.Remove(ProductToRemove);
                        ResultProducts.Remove(ProductToRemove);
                        var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/Product/{ProductToRemove.ProductId}");
                        response.EnsureSuccessStatusCode();
                    }
                }));
            }
        }
    }
}
