using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RitualServer.Model;
namespace RitualProject
{
    public class AdminViewModel : ViewModel
    {
        private readonly ApiClient _apiClient=new ApiClient();
        
        private ObservableCollection<UserWithRole> _users;
        public ObservableCollection<UserWithRole> Users
        {
            get { return _users; }
            set => Set(ref _users, value);
        }
        public ObservableCollection<UserWithRole> ResultUsers { get; set; }
       
        private string _searchUserByName;
        public string SearchUserByName
        {
            get { return _searchUserByName; }
            set
            {
                if(_searchUserByName != value)
                {
                    _searchUserByName=value;
                    OnPropertyChanged("SearchUserByName");
                    SearchUsersAsync();
                }
            }
        }

        public AdminViewModel()
        {
            Users = new ObservableCollection<UserWithRole>();
            ResultUsers = new ObservableCollection<UserWithRole>();
            LoadUsersAsync();
        }
        
        public async Task LoadUsersAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getUserWithRole");
                response.EnsureSuccessStatusCode();
                var userArray = await response.Content.ReadFromJsonAsync<UserWithRole[]>();
                Users.Clear();
                foreach (var user in userArray)
                {
                    Users.Add(user);
                    ResultUsers.Add(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }
        public async void SearchUsersAsync()
        {
            if (string.IsNullOrEmpty(SearchUserByName))
            {
                await Task.Run(() =>
                {
                    Users = ResultUsers;
                });
            }
            else
            {
                await Task.Run(() =>
                {
                    Users = new ObservableCollection<UserWithRole>(ResultUsers.Where(x => x.Name.ToLower().Contains(SearchUserByName.ToLower())));
                });
                
            }
        }
    }
}
