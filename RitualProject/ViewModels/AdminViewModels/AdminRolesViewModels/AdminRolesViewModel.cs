using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RitualServer.Model;

namespace RitualProject
{
    public class AdminRolesViewModel : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public ObservableCollection<Role> ResultRoles { get; set; }
        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> Roles
        {
            get { return _roles; }
            set => Set(ref _roles, value);
        }
        private string _searchRoleByName;
        public string SearchRoleByName
        {
            get { return _searchRoleByName; }
            set
            {
                if (_searchRoleByName != value)
                {
                    _searchRoleByName = value;
                    OnPropertyChanged("SearchRoleByName");
                    SearchRolesAsync();
                }
            }
        }
        public AdminRolesViewModel()
        {
            Roles = new ObservableCollection<Role>();
            ResultRoles = new ObservableCollection<Role>();
            LoadDataAsync();
        }
        public async Task LoadDataAsync()
        {
            try
            {

                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getRoles");
                response.EnsureSuccessStatusCode();
                var roleArray = await response.Content.ReadFromJsonAsync<Role[]>();
                Roles.Clear();
                ResultRoles.Clear();
                foreach (var role in roleArray)
                {
                    Roles.Add(role);
                    ResultRoles.Add(role);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        
        public async void SearchRolesAsync()
        {
            if (string.IsNullOrEmpty(SearchRoleByName))
            {
                await Task.Run(() =>
                {
                    Roles = ResultRoles;
                });
            }
            else
            {
                await Task.Run(() =>
                {
                    Roles = new ObservableCollection<Role>(ResultRoles.Where(x => x.Role1.ToLower().Contains(SearchRoleByName.ToLower())));
                });

            }
        }
        private RelayCommands _EditRoleCommand;
        public RelayCommands EditRoleCommand
        {
            get
            {
                return _EditRoleCommand ?? (_EditRoleCommand = new RelayCommands(async obj =>
                {
                    var RoleToEdit = obj as Role;
                    if (RoleToEdit != null)
                    {
                        var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/Role/{RoleToEdit.RolesId}");
                        response.EnsureSuccessStatusCode();
                        var roleArray = await response.Content.ReadFromJsonAsync<Role>();

                        AdminRolesViewModelList adminViewModel = new AdminRolesViewModelList(roleArray) { AdminViewModel = this };
                        AdminRolesAddEditWindow adminWindow = new AdminRolesAddEditWindow(adminViewModel);
                        adminWindow.ShowDialog();
                    }
                    else
                    {
                        AdminRolesViewModelList adminViewModel = new AdminRolesViewModelList(null) { AdminViewModel = this };
                        AdminRolesAddEditWindow adminWindow = new AdminRolesAddEditWindow(adminViewModel);
                        adminWindow.ShowDialog();
                    }
                }));
            }
        }
        private RelayCommands _DeleteRoleCommand;

        public RelayCommands DeleteRoleCommand
        {
            get
            {
                return _DeleteRoleCommand ?? (_DeleteRoleCommand = new RelayCommands(async obj =>
                {
                    try
                    {
                        var RoleToRemove = obj as Role;
                        if (RoleToRemove != null)
                        {
                            Roles.Remove(RoleToRemove);
                            ResultRoles.Remove(RoleToRemove);
                            var response = await _apiClient.Client.DeleteAsync($"{_apiClient.BaseUrl}/api/Role/{RoleToRemove.RolesId}");
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
