using Newtonsoft.Json;
using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
namespace RitualProject
{
    public class ManagerServicesVMList : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private Service _ServiceEd;

        public Service ServiceEd
        {
            get { return _ServiceEd; }
            set => Set(ref _ServiceEd, value);
        }
        private byte[] _image;

        public byte[] Image
        {
            get { return _image; }
            set => Set(ref _image, value);
        }
        private ManagerServicesVM _ManagerServicesViewModel;
        public ManagerServicesVM ManagerServicesViewModel
        {
            get { return _ManagerServicesViewModel; }
            set => Set(ref _ManagerServicesViewModel, value);
        }
        public ObservableCollection<CategoiresService> CategoriesServices { get; }
        public string WhatNow { get; set; }
        public Action Close { get; set; }
        void CloseWindow()
        {
            Close?.Invoke();
        }
        public bool CanClose()
        {
            return true;
        }
        public ManagerServicesVMList(Service Service)
        {
            CategoriesServices = new ObservableCollection<CategoiresService>();
            LoadDataAsync();
            if (Service != null)
            {
                WhatNow = "Редактирование услуги";
                ServiceEd = Service;

                if (ServiceEd.Image != null)
                {
                    Image = ServiceEd.Image;
                }
            }
            else
            {
                WhatNow = "Добавление услуги";
                ServiceEd = new Service();
            }

        }
        public async Task LoadDataAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getCategoriesServices");
                response.EnsureSuccessStatusCode();
                var categoryArray = await response.Content.ReadFromJsonAsync<CategoiresService[]>();
                foreach (var categoires in categoryArray)
                {
                    CategoriesServices.Add(categoires);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        private RelayCommands _EditAddServiceCommand;
        public RelayCommands EditAddServiceCommand
        {
            get
            {
                return _EditAddServiceCommand ?? (_EditAddServiceCommand = new RelayCommands(async obj =>
                {
                    if (string.IsNullOrWhiteSpace(ServiceEd.Name))
                    {
                        MessageBox.Show("Вы не ввели наименование");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(ServiceEd.Price))
                    {
                        MessageBox.Show("Вы не ввели цену");
                        return;
                    }
                    if (ServiceEd.CategoryId == 0)
                    {
                        MessageBox.Show("Вы не выбрали категорию");
                        return;
                    }
                    if (WhatNow == "Редактирование сотрудника")
                    {
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Service/{ServiceEd.ServicesId}", new StringContent(JsonConvert.SerializeObject(ServiceEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        ManagerServicesViewModel.LoadDataAsync();
                        CloseWindow();
                    }
                    else
                    {
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Service", new StringContent(JsonConvert.SerializeObject(ServiceEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        ManagerServicesViewModel.LoadDataAsync();
                        CloseWindow();
                    }
                }));
            }
        }
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
                        ServiceEd.Image = File.ReadAllBytes(dlg.FileName);
                        Image = ServiceEd.Image;
                    }
                }));
            }
        }
    }
}
