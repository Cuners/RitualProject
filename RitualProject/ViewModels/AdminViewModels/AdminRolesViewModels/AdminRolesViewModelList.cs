using Microsoft.Win32;
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

namespace RitualProject
{
    public class AdminRolesViewModelList : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private Role _roleEd;

        public Role RoleEd
        {
            get { return _roleEd; }
            set => Set(ref _roleEd, value);
        }
        private AdminRolesViewModel adminViewModel;
        public AdminRolesViewModel AdminViewModel
        {
            get { return adminViewModel; }
            set => Set(ref adminViewModel, value);
        }
        public Action Close { get; set; }
        void CloseWindow()
        {
            Close?.Invoke();
        }
        public bool CanClose()
        {
            return true;
        }
        public string WhatNow { get; set; }
      

        private User _user;
        public User User
        {
            get { return _user; }
            set => Set(ref _user, value);
        }

        public AdminRolesViewModelList(Role role)
        {
           
            if (role != null)
            {
                WhatNow = "Редактирование роли";
                RoleEd = role;
            }
            else
            {
                WhatNow = "Добавление роли";
                RoleEd = new Role();
            }

        }
       
        private RelayCommands _EditAddRoleCommand;
        public RelayCommands EditAddRoleCommand
        {
            get
            {
                return _EditAddRoleCommand ?? (_EditAddRoleCommand = new RelayCommands(async obj =>
                {
                    if (WhatNow == "Редактирование сотрудника")
                    {
                       
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Role/{RoleEd.RolesId}", new StringContent(JsonConvert.SerializeObject(RoleEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        AdminViewModel.LoadDataAsync();
                        CloseWindow();
                    }
                    else
                    {
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Role", new StringContent(JsonConvert.SerializeObject(RoleEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        AdminViewModel.LoadDataAsync();
                        CloseWindow(); 
                    }
                }));
            }
        }

    }
}
