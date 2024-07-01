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
    public class ManagerServicesVM : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public ObservableCollection<Service> ResultServices { get; set; }
        private ObservableCollection<Service> _Services;
        public ObservableCollection<Service> Services
        {
            get { return _Services; }
            set => Set(ref _Services, value);
        }
        private string _searchServiceByName;
        public string SearchServiceByName
        {
            get { return _searchServiceByName; }
            set
            {
                if (_searchServiceByName != value)
                {
                    _searchServiceByName = value;
                    OnPropertyChanged("SearchServiceByName");
                    SearchServicesAsync();
                }
            }
        }
        public ManagerServicesVM()
        {
            Services = new ObservableCollection<Service>();
            ResultServices = new ObservableCollection<Service>();
            LoadDataAsync();
        }
        public async Task LoadDataAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getServices");
                response.EnsureSuccessStatusCode();
                var ServiceArray = await response.Content.ReadFromJsonAsync<Service[]>();
                Services.Clear();
                ResultServices.Clear();
                foreach (var Service in ServiceArray)
                {
                    Services.Add(Service);
                    ResultServices.Add(Service);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}");
            }
        }

        public async void SearchServicesAsync()
        {
            if (string.IsNullOrEmpty(SearchServiceByName))
            {
                await Task.Run(() =>
                {
                    Services = ResultServices;
                });
            }
            else
            {
                await Task.Run(() =>
                {
                    Services = new ObservableCollection<Service>(ResultServices.Where(x => x.Name.ToLower().Contains(SearchServiceByName.ToLower())));
                });

            }
        }
        private RelayCommands _EditServiceCommand;
        public RelayCommands EditServiceCommand
        {
            get
            {
                return _EditServiceCommand ?? (_EditServiceCommand = new RelayCommands(async obj =>
                {
                    var ServiceToEdit = obj as Service;
                    if (ServiceToEdit != null)
                    {
                        var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/Service/{ServiceToEdit.ServicesId}");
                        response.EnsureSuccessStatusCode();
                        var ServiceArray = await response.Content.ReadFromJsonAsync<Service>();

                        ManagerServicesVMList managerViewModel = new ManagerServicesVMList(ServiceArray) { ManagerServicesViewModel = this };
                        ManagerServicesAddEditWindow managerWindow = new ManagerServicesAddEditWindow(managerViewModel);
                        managerWindow.ShowDialog();
                    }
                    else
                    {
                        ManagerServicesVMList managerViewModel = new ManagerServicesVMList(null) { ManagerServicesViewModel = this };
                        ManagerServicesAddEditWindow managerWindow = new ManagerServicesAddEditWindow(managerViewModel);
                        managerWindow.ShowDialog();
                    }
                }));
            }
        }
        private RelayCommands _DeleteServiceCommand;

        public RelayCommands DeleteServiceCommand
        {
            get
            {
                return _DeleteServiceCommand ?? (_DeleteServiceCommand = new RelayCommands(async obj =>
                {
                    var ServiceToRemove = obj as Service;
                    try
                    {
                        if (ServiceToRemove != null)
                        {
                            Services.Remove(ServiceToRemove);
                            ResultServices.Remove(ServiceToRemove);
                            var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/Service/{ServiceToRemove.ServicesId}");
                            response.EnsureSuccessStatusCode();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }
    }
}
