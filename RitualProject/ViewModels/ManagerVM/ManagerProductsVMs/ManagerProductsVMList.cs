
using Azure;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using Newtonsoft.Json;
using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RitualProject
{
    public class ManagerProductsVMList : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private byte[] _imageClothe;

        public byte[] ImageClothe
        {
            get { return _imageClothe; }
            set => Set(ref _imageClothe, value);
        }
        private byte[] _imageCoffin;

        public byte[] ImageCoffin
        {
            get { return _imageCoffin; }
            set => Set(ref _imageCoffin, value);
        }
        private byte[] _imageCross;

        public byte[] ImageCross
        {
            get { return _imageCross; }
            set => Set(ref _imageCross, value);
        }
        private byte[] _imageMonument;

        public byte[] ImageMonument
        {
            get { return _imageMonument; }
            set => Set(ref _imageMonument, value);
        }
        private byte[] _imageTape;

        public byte[] ImageTape
        {
            get { return _imageTape; }
            set => Set(ref _imageTape, value);
        }
        private byte[] _imageUrn;

        public byte[] ImageUrn
        {
            get { return _imageUrn; }
            set => Set(ref _imageUrn, value);
        }
        private Product _productEd;
        public Product ProductEd
        {
            get { return _productEd; }
            set => Set(ref _productEd, value);
        }
        private UserControl _ClotheUserControl;
        private UserControl _CoffinUserControl;
        private UserControl _MonumentUserControl;
        private UserControl _TapeUserControl;
        private UserControl _UrnUserControl;
        private UserControl _CrossUserControl;
        private UserControl currentControl;
        public UserControl CurrentControl
        {
            get { return currentControl; }
            set => Set(ref currentControl, value);
        }
        private ObservableCollection<Color> _colors;
        public ObservableCollection<Color> Colors
        {
            get { return _colors; }
            set => Set(ref _colors, value);
        }
        private ObservableCollection<Material> _materials;
        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            set => Set(ref _materials, value);
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set => Set(ref _categories, value);
        }
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
                if (Category.CategoryId == 1)
                {
                    CurrentControl = _CoffinUserControl;
                }
                else if(Category.CategoryId == 2)
                {
                     CurrentControl = _CrossUserControl;
                }
                else if (Category.CategoryId == 3)
                {
                    CurrentControl = _MonumentUserControl;
                }
                else if (Category.CategoryId == 4)
                {
                    CurrentControl = _TapeUserControl;
                }
                else if (Category.CategoryId == 5)
                {
                    CurrentControl = _ClotheUserControl;
                }
                else if (Category.CategoryId == 6)
                {
                    CurrentControl = _UrnUserControl;
                }

            }
        }
        private Urn _urns;
        public Urn Urns
        {
            get { return _urns; }
            set => Set(ref _urns, value);
        }
        private Monument _monuments;
        public Monument Monuments
        {
            get { return _monuments; }
            set => Set(ref _monuments, value);
        }
        private Cross _crosses;
        public Cross Crosses
        {
            get { return _crosses; }
            set => Set(ref _crosses, value);
        }
        private Tape _tapes;
        public Tape Tapes
        {
            get { return _tapes; }
            set => Set(ref _tapes, value);
        }
        private Coffin _coffins;
        public Coffin Coffins
        {
            get { return _coffins; }
            set => Set(ref _coffins, value);
        }
        private Clothe _clothe;
        public Clothe Cloth
        {
            get { return _clothe; }
            set => Set(ref _clothe, value);
        }
        private ManagerProductsVM managerViewModel;
        public ManagerProductsVM ManagerViewModel
        {
            get { return managerViewModel; }
            set => Set(ref managerViewModel, value);
        }
        public Action Close { get; set; }
        async void CloseWindow()
        {
            Close?.Invoke();
        }
        public bool CanClose()
        {
            return true;
        }
        public string WhatNow { get; set; }

        public ManagerProductsVMList(Product product)
        {
            Colors = new ObservableCollection<Color>();
            Materials=new ObservableCollection<Material>();
            Categories = new ObservableCollection<Category>();
            Category = new Category();
            LoadColorAsync();
            LoadMaterialAsync();
            LoadCategoriesAsync();
            _MonumentUserControl = new MonumentUserControl();
            _ClotheUserControl = new ClotheUserControl();
            _CoffinUserControl = new CoffinUserControl();
            _TapeUserControl=new TapeUserControl();
            _UrnUserControl = new UrnUserControl();
            _CrossUserControl = new CrossUserControl();
            if (product != null)
            {
                WhatNow = "Редактирование продукта";
                ProductEd = product;
                if (ProductEd.CategoryId == 1)
                {
                    
                    foreach(var product1 in ProductEd.Coffins)
                    {
                        Coffins=product1;
                    }
                    if (Coffins.Image != null)
                    {
                        ImageCoffin = Coffins.Image;
                    }

                    currentControl = _CoffinUserControl;
                }
                else if(ProductEd.CategoryId == 2)
                {
                    foreach (var product1 in ProductEd.Crosses)
                    {
                        Crosses = product1;
                    }
                    if (Crosses.Image != null)
                    {
                        ImageCross = Crosses.Image;
                    }
                    currentControl = _CrossUserControl;
                }
                else if (ProductEd.CategoryId == 3)
                {
                    
                    foreach (var product1 in ProductEd.Monuments)
                    {
                        Monuments = product1;
                    }
                    if (Monuments.Image != null)
                    {
                        ImageMonument = Monuments.Image;
                    }
                    currentControl = _MonumentUserControl;
                }
                else if (ProductEd.CategoryId == 4)
                {
                    foreach (var product1 in ProductEd.Tapes)
                    {
                        Tapes = product1;
                    }
                    if (Tapes.Image != null)
                    {
                        ImageTape = Tapes.Image;
                    }
                    currentControl = _TapeUserControl;
                }
                else if (ProductEd.CategoryId == 5)
                {
                    foreach (var product1 in ProductEd.Clothes)
                    {
                        Cloth = product1;
                    }
                    if (Cloth.Image != null)
                    {
                        ImageClothe = Cloth.Image;
                    }
                    currentControl = _ClotheUserControl;
                }
                else if (ProductEd.CategoryId == 6)
                {
                    foreach (var product1 in ProductEd.Urns)
                    {
                        Urns = product1;
                    }
                    if (Urns.Image != null)
                    {
                        ImageUrn = Urns.Image;
                    }
                    currentControl = _UrnUserControl;
                }
            }
            else
            {
                WhatNow = "Добавление роли";
                ProductEd = new Product();
                CurrentControl = null;
                Crosses = new Cross();
                Cloth = new Clothe();
                Monuments = new Monument();
                Urns = new Urn();
                Tapes = new Tape();
                Coffins = new Coffin();
            }

        }
        #region API
        public async Task LoadColorAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getColors");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Color[]>();
                Colors.Clear();
                foreach (var product in productArray)
                {
                    Colors.Add(product);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        public async Task LoadMaterialAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getMaterials");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Material[]>();
                Materials.Clear();
                foreach (var product in productArray)
                {
                    Materials.Add(product);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        public async Task LoadCategoriesAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getCategories");
                response.EnsureSuccessStatusCode();
                var productArray = await response.Content.ReadFromJsonAsync<Category[]>();
                Categories.Clear();
                foreach (var product in productArray)
                {
                    Categories.Add(product);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}");
            }
        }
        public async Task PutCoffinsAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Coffin/{Coffins.CoffinId}", new StringContent(JsonConvert.SerializeObject(Coffins), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PutCrossesAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Cross/{Crosses.CrossId}", new StringContent(JsonConvert.SerializeObject(Crosses), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PutMonumentsAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Monument/{Monuments.MonumentId}", new StringContent(JsonConvert.SerializeObject(Monuments), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PutTapesAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Tape/{Tapes.TapeId}", new StringContent(JsonConvert.SerializeObject(Tapes), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PutClothesAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Clothe/{Cloth.ClothId}", new StringContent(JsonConvert.SerializeObject(Cloth), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PutUrnsAsync()
        {
            try
            {
                var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Urn/{Urns.UrnId}", new StringContent(JsonConvert.SerializeObject(Urns), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostCoffinsAsync(int productID)
        {
            try
            {
                Coffins.ProductId = productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Coffin", new StringContent(JsonConvert.SerializeObject(Coffins), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostCrossesAsync(int productID)
        {
            try
            {
                Crosses.ProductId = productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Cross", new StringContent(JsonConvert.SerializeObject(Crosses), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostMonumentsAsync(int productID)
        {
            try
            {
                Monuments.ProductId = productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Monument", new StringContent(JsonConvert.SerializeObject(Monuments), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostTapesAsync(int productID)
        {
            try
            {
                Tapes.ProductId = productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Tape", new StringContent(JsonConvert.SerializeObject(Tapes), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostClothesAsync(int productID)
        {
            try
            {
                Cloth.ProductId=productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Clothe", new StringContent(JsonConvert.SerializeObject(Cloth), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        public async Task PostUrnsAsync(int productID)
        {
            try
            {
                Urns.ProductId = productID;
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Urn", new StringContent(JsonConvert.SerializeObject(Urns), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления урны: {ex.Message}");
            }
        }
        #endregion
        private RelayCommands _OpenImageHeroIcon;

        public RelayCommands OpenImageHeroIcon
        {
            get
            {
                return _OpenImageHeroIcon ?? (_OpenImageHeroIcon = new RelayCommands(async obj =>
                {
                    OpenFileDialog dlg = new OpenFileDialog()
                    {
                        Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                    };
                    if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                    {
                        if (Category.CategoryId == 1)
                        {
                            
                            Coffins.Image=File.ReadAllBytes(dlg.FileName);
                            ImageCoffin = Coffins.Image;
                        }
                        else if (Category.CategoryId == 2)
                        {
                            Crosses.Image = File.ReadAllBytes(dlg.FileName);
                            ImageCross = Crosses.Image;
                        }
                        else if (Category.CategoryId == 3)
                        {
                            Monuments.Image = File.ReadAllBytes(dlg.FileName);
                            ImageMonument= Monuments.Image;
                        }
                        else if (Category.CategoryId == 4)
                        {
                            Tapes.Image = File.ReadAllBytes(dlg.FileName);
                            ImageTape= Tapes.Image;
                        }
                        else if (Category.CategoryId == 5)
                        {
                            Cloth.Image = File.ReadAllBytes(dlg.FileName);
                            ImageClothe=Cloth.Image;
                        }
                        else if (Category.CategoryId == 6)
                        {
                            Urns.Image = File.ReadAllBytes(dlg.FileName);
                            ImageUrn=Urns.Image;
                        }
                    }
                }));
            }
        }

        private RelayCommands _EditAddProductCommand;
        public RelayCommands EditAddProductCommand
        {
            get
            {
                return _EditAddProductCommand ?? (_EditAddProductCommand = new RelayCommands(async obj =>
                {
                    if (WhatNow == "Редактирование продукта")
                    {
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Product/{ProductEd.ProductId}", new StringContent(JsonConvert.SerializeObject(ProductEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        
                        if (Category.CategoryId == 1)
                        {
                            await PutCoffinsAsync();
                        }
                        else if (Category.CategoryId == 2)
                        {
                            await PutCrossesAsync();
                        }
                        else if (Category.CategoryId == 3)
                        {
                            await PutMonumentsAsync();
                        }
                        else if (Category.CategoryId == 4)
                        {
                            await PutTapesAsync();
                        }
                        else if (Category.CategoryId == 5)
                        {
                            await PutClothesAsync();
                        }
                        else if (Category.CategoryId == 6)
                        {
                            await PutUrnsAsync();
                        }
                        CloseWindow();
                        ManagerViewModel.LoadProductAsync();
                    }
                    else
                    {
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Product", new StringContent(JsonConvert.SerializeObject(ProductEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var createdProduct = JsonConvert.DeserializeObject<Product>(responseBody);
                        var productId = createdProduct.ProductId;
                        if (Category.CategoryId == 1)
                        {
                            await PostCoffinsAsync(productId);
                        }
                        else if (Category.CategoryId == 2)
                        {
                            await PostCrossesAsync(productId);
                        }
                        else if (Category.CategoryId == 3)
                        {
                            await PostMonumentsAsync(productId);
                        }
                        else if (Category.CategoryId == 4)
                        {
                            await PostTapesAsync(productId);
                        }
                        else if (Category.CategoryId == 5)
                        {
                            await PostClothesAsync(productId);
                        }
                        else if (Category.CategoryId == 6)
                        {
                            await PostUrnsAsync(productId);
                        }
                        CloseWindow();
                        ManagerViewModel.LoadProductAsync();
                    }
                }));
            }
        }
    }
}
