using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using RitualServer.Model;
namespace RitualProject
{
    public class AdminViewModel : ViewModel
    {
        private readonly ApiClient _apiClient=new ApiClient();
        //private ObservableCollection<UserStatisticsRoles> _roleStatistics;
        //public ObservableCollection<UserStatisticsRoles> RoleStatistics
        //{
        //    get { return _roleStatistics; }
        //    set
        //    {
        //        _roleStatistics = value;
        //        OnPropertyChanged(nameof(RoleStatistics));
        //    }
        //}

        // Свойство для хранения коллекции статистики по ролям пользователей
        public ObservableCollection<UserRoleStatistics> RoleStatistics { get; set; }
        private ObservableCollection<UserWithRole> _users;
        public ObservableCollection<UserWithRole> Users
        {
            get { return _users; }
            set => Set(ref _users, value);
        }
        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> Roles
        {
            get { return _roles; }
            set => Set(ref _roles, value);
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
            RoleStatistics=new ObservableCollection<UserRoleStatistics>();
            Roles = new ObservableCollection<Role>();
            // Инициализируем RoleStatistics
            
            LoadUsersAsync();
            LoadRolesAsync();
        }
        public async Task LoadRolesAsync()
        {
            try
            {
                var response2 = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getRoles");
                response2.EnsureSuccessStatusCode();
                var roleArray = await response2.Content.ReadFromJsonAsync<Role[]>();
                Roles.Clear();
                foreach (var role in roleArray)
                {
                    Roles.Add(role);

                }
                GetRoleStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        public async Task LoadUsersAsync()
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getUserWithRole");
                response.EnsureSuccessStatusCode();
                var userArray = await response.Content.ReadFromJsonAsync<UserWithRole[]>();
                Users.Clear();
                ResultUsers.Clear();
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
        public void GetRoleStatistics()
        {
            //var roleCounts = Users
            //.GroupBy(user => user.Role)
            //.Select(group => new { RoleName = group.Key, UserCount = group.Count() });
            //foreach(var role in roleCounts)
            //{
            //    RoleStatistics.Add(new UserRoleStatistics { RoleName = role.RoleName, UserCount = role.UserCount });
            //}
            var roleCounts = Roles
                .Select(roleName => new
                    {
            RoleName = roleName.Role1,
            UserCount = Users.Count(user => user.Role == roleName.Role1)
                    });

            foreach (var role in roleCounts)
            {
                RoleStatistics.Add(new UserRoleStatistics { RoleName = role.RoleName, UserCount = role.UserCount });
            }
        }
        private RelayCommands _EditUserCommand;
        public RelayCommands EditUserCommand
        {
            get
            {
                return _EditUserCommand ?? (_EditUserCommand = new RelayCommands(async obj =>
                {
                    var UserToEdit = obj as UserWithRole;
                    if (UserToEdit != null)
                    {
                        var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/User/{UserToEdit.UserId}");
                        response.EnsureSuccessStatusCode();
                        var userArray = await response.Content.ReadFromJsonAsync<User>();
                        
                        AdminViewModelList adminViewModel = new AdminViewModelList(userArray) { AdminViewModel=this};
                        AdminUserAddEditWindow adminWindow = new AdminUserAddEditWindow(adminViewModel);
                        adminWindow.ShowDialog();
                    }
                    else
                    {
                        AdminViewModelList adminViewModel = new AdminViewModelList(null) { AdminViewModel = this };
                        AdminUserAddEditWindow adminWindow = new AdminUserAddEditWindow(adminViewModel);
                        adminWindow.ShowDialog();
                    }
                }));
            }
        }
        private RelayCommands _DeleteUserCommand;

        public RelayCommands DeleteUserCommand
        {
            get
            {
                return _DeleteUserCommand ?? (_DeleteUserCommand = new RelayCommands(async obj =>
                {
                   var UserToRemove = obj as UserWithRole;
                    if (UserToRemove != null)
                    {
                        Users.Remove(UserToRemove);
                        ResultUsers.Remove(UserToRemove);
                        GetRoleStatistics();
                    }
                }));
            }
        }
       

    }
}
