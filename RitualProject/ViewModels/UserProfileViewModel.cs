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
    public class UserProfileViewModel : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private User _UserEd;

        public User UserEd
        {
            get { return _UserEd; }
            set => Set(ref _UserEd, value);
        }
        private byte[] _image;

        public byte[] Image
        {
            get { return _image; }
            set => Set(ref _image, value);
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
        public UserProfileViewModel()
        {
            GetDataAsync(UserInfoConstant.ID);
        }
        public async Task GetDataAsync(int id)
        {
            try
            {
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/api/User/{id}");
                response.EnsureSuccessStatusCode();
                var user = await response.Content.ReadFromJsonAsync<User>();
                UserEd = user;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователя: {ex.Message}");
            }
        }
        public int ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return 1;
            }
            if (!email.Contains("@"))
            {
                return 2;
            }
            return 0;
        }
        public bool ValidateTelephone(string telephone)
        {
            if (string.IsNullOrWhiteSpace(telephone))
            {
                return false;
            }
           
            return true;
        }
        private RelayCommands _EditAddUserCommand;
        public RelayCommands EditAddUserCommand
        {
            get
            {
                return _EditAddUserCommand ?? (_EditAddUserCommand = new RelayCommands(async obj =>
                {
                    try
                    {
                        int email = ValidateEmail(UserEd.Email);
                        if (email == 1)
                        {
                            MessageBox.Show("Вы не заполнили email");
                            return;
                        }
                        else if (email == 2)
                        {
                            MessageBox.Show("Отсутствует @");
                            return;
                        }
                        bool flagphone = ValidateTelephone(UserEd.Phone);
                        if (!flagphone)
                        {
                            MessageBox.Show("Вы не заполнили телефон");
                            return;
                        }
                        var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/User/{UserEd.UserId}", new StringContent(JsonConvert.SerializeObject(UserEd), Encoding.UTF8, "application/json"));
                        response.EnsureSuccessStatusCode();
                        CloseWindow();
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
