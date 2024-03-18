using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using RitualServer.Model;
using System.Net.Http.Json;
using MaterialDesignThemes.Wpf;

namespace RitualProject
{
    public class AuthorizationViewModel : ViewModel, ICloseWindow
    {
        private readonly ApiClient _apiClient = new ApiClient();
        private string _errorLogin;
        public string ErrorLogin
        {
            get { return _errorLogin; }
            set => Set(ref _errorLogin, value);
        }
        private string _login;
        public string Login
        {
            get { return _login; }
            set => Set(ref _login, value);
        }
        private string _errorPassword;
        public string ErrorPassword
        {
            get { return _errorPassword; }
            set => Set(ref _errorPassword, value);
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set => Set(ref _password, value);

        }
        private string _passLogin;
        public string PassLogin
        {
            get { return _passLogin; }
            set => Set(ref _passLogin, value);
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set => Set(ref _email, value);

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
        public async Task FindUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Login) && string.IsNullOrWhiteSpace(Password))
                {
                    ErrorLogin = "Логин пустой";
                    ErrorPassword = "Пароль пустой";
                    return;
                }
                
                else if(string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password))
                {
                    ErrorLogin = "Логин пустой";
                    ErrorPassword = "";
                    return;
                }
                else if (string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Login))
                {
                    ErrorLogin = "";
                    ErrorPassword = "Пароль пустой";
                    return;
                }
                ErrorPassword = "";
                ErrorLogin = "";
                var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getArticle");
                response.EnsureSuccessStatusCode();
                var userArray = await response.Content.ReadFromJsonAsync<User[]>();
                
                foreach (var user in userArray)
                {
                    if (user.Login == Login && user.Password == Password)
                    {
                        UserInfoConstant.FirstName = user.FirstName;
                        if (user.Image != null)
                        {
                            UserInfoConstant.Image = user.Image;
                        }
                        if(user.RoleId == 1)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //AdminWindow admin = new AdminWindow();
                                //admin.Show();
                                ManagerWindow managerWindow = new ManagerWindow();
                                managerWindow.Show();
                                CloseWindow();  
                                return;
                            });

                        }
                        else if (user.RoleId == 2)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //AdminWindow admin = new AdminWindow();
                                //admin.Show();
                                ManagerWindow managerWindow = new ManagerWindow();
                                managerWindow.Show();
                                CloseWindow();
                                return;
                            });

                        }
                        else if (user.RoleId == 3)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //AdminWindow admin = new AdminWindow();
                                //admin.Show();
                                ManagerWindow managerWindow = new ManagerWindow();
                                managerWindow.Show();
                                CloseWindow();
                                return;
                            });

                        }
                        else if (user.RoleId == 4)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //AdminWindow admin = new AdminWindow();
                                //admin.Show();
                                StorekeeperWindow storekeeperWindow = new StorekeeperWindow();
                                storekeeperWindow.Show();
                                CloseWindow();
                                return;
                            });

                        }
                        else if (user.RoleId == 5)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                AdminWindow admin = new AdminWindow();
                                admin.Show();
                                CloseWindow();
                                return;
                            });

                        }

                    }
                }
                //AdminWindow admin = new AdminWindow();
                //admin.Show();
                //StorekeeperWindow storekeeperWindow=new StorekeeperWindow();
                //storekeeperWindow.Show();
                ErrorLogin = "Не верный логин или пароль";
            }
               
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}");
            }
        }
        private RelayCommands _AuthCommand;

        public RelayCommands AuthCommand
        {
            get
            {
                return _AuthCommand ?? (_AuthCommand = new RelayCommands(async obj =>
                {
                    FindUser();
                    
                }));
            }
        }
    }
}
