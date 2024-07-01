using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RitualServer.Model;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace RitualProject
{
    public class AdminViewModelList:ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private User _userEd;
       
        public User UserEd
        {
            get { return _userEd; }
            set => Set(ref _userEd, value);
        }
        private byte[] _image;

        public byte[] Image
        {
            get { return _image; }
            set => Set(ref _image, value);
        }
        private AdminViewModel adminViewModel;
        public AdminViewModel AdminViewModel
        {
            get { return adminViewModel;}
            set=>Set(ref adminViewModel, value);
        }
        public ObservableCollection<Role> Roles { get;}
        public string WhatNow { get; set; }
        public string Name { get; set; }
        public Action Close {  get; set; }
        void CloseWindow()
        {
            Close?.Invoke();
        }
        public bool CanClose()
        {
            return true;
        }
        public AdminViewModelList(User user)
        {
            Roles = new ObservableCollection<Role>();
            LoadDataAsync();
            if (user != null)
            {
                WhatNow = "Редактирование сотрудника";
                UserEd = user;

                Name = user.FirstName + " " + user.LastName;
                if (UserEd.Image != null)
                {
                    Image = UserEd.Image;
                }
            }
            else
            {
                WhatNow = "Добавление сотрудника";
                UserEd = new User();
            }
            
        }
        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getRoles");
                response.EnsureSuccessStatusCode();
                var roleArray = await response.Content.ReadFromJsonAsync<Role[]>();
                foreach(var role in roleArray)
                {
                    Roles.Add(role);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
            }
        }
        public bool ValidateEmail(string email)
        {
            foreach(char c in email)
            {
                if (c == '@')
                {
                    return true;
                }
            }
            return false;
        }
        private RelayCommands _EditAddUserCommand;
        public RelayCommands EditAddUserCommand
        {
            get
            {
                return _EditAddUserCommand ?? (_EditAddUserCommand = new RelayCommands(async obj =>
                {
                    if (string.IsNullOrWhiteSpace(UserEd.Login))
                    {
                        MessageBox.Show("Вы не ввели логин");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.Password))
                    {
                        MessageBox.Show("Вы не ввели пароль");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.FirstName))
                    {
                        MessageBox.Show("Вы не ввели имя");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.LastName))
                    {
                        MessageBox.Show("Вы не ввели фамилию");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.Email))
                    {
                        MessageBox.Show("Вы не ввели email");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.Phone))
                    {
                        MessageBox.Show("Вы не ввели телефон");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(UserEd.Adress))
                    {
                        MessageBox.Show("Вы не ввели адрес проживания");
                        return;
                    }
                    if (UserEd.RoleId ==0)
                    {
                        MessageBox.Show("Вы не выбрали роль");
                        return;
                    }
                    if (UserEd.Phone.Length != 11) 
                    {
                        MessageBox.Show("Телефон не полный");
                        return;
                    }
                    if (!ValidateEmail(UserEd.Email))
                    {
                        MessageBox.Show("Введите полный email");
                        return;
                    }
                    if (WhatNow == "Редактирование сотрудника")
                    {
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/User/{UserEd.UserId}", new StringContent(JsonConvert.SerializeObject(UserEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        AdminViewModel.LoadUsersAsync();
                        CloseWindow();
                    }
                    else
                    {
                        var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/User", new StringContent(JsonConvert.SerializeObject(UserEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        AdminViewModel.LoadUsersAsync();
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
                        UserEd.Image = File.ReadAllBytes(dlg.FileName);
                        Image = UserEd.Image;
                    }
                }));
            }
        }
    }
}
