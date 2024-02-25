using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RitualProject
{
    public class AuthorizationViewModel : ViewModel
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
        private RelayCommands _AuthCommand;

        public RelayCommands AuthCommand
        {
            get
            {
                return _AuthCommand ?? (_AuthCommand = new RelayCommands(async obj =>
                {
                    ErrorLogin="123";
                    ErrorPassword = "123";
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    //using (MessengEntities messengEntities = new MessengEntities())
                    //{
                    //    var passbox = obj as PasswordBox;
                    //    var password = passbox.Password;
                    //    if (string.IsNullOrWhiteSpace(Login))
                    //    {
                    //        MessageBox.Show("Вы не ввели логин");
                    //        return;
                    //    }
                    //    await Task.Run(() =>
                    //    {
                    //        foreach (var polzovatel in messengEntities.Users)
                    //        {
                    //            if (polzovatel.Login == Login && polzovatel.Password == password)
                    //            {
                    //                LoginMod.IdUserNow = polzovatel.Id;
                    //                Application.Current.Dispatcher.Invoke(() =>
                    //                {
                    //                    MainWindow mainWindow = new MainWindow();
                    //                    mainWindow.Show();
                    //                });
                    //                return;
                    //            }
                    //        }
                    //    });
                    //    //await Task.Run(() =>  /* Если данных много*/
                    //    //{
                    //    //    Parallel.ForEach(messengEntities.Users, polzovatel =>
                    //    //    {
                    //    //        if (polzovatel.Login == Login && polzovatel.Password == password)
                    //    //        {
                    //    //            LoginMod.IdUserNow = polzovatel.Id;
                    //    //            Application.Current.Dispatcher.Invoke(() =>
                    //    //            {
                    //    //                MainWindow mainWindow = new MainWindow();
                    //    //                mainWindow.Show();
                    //    //            });
                    //    //        }
                    //    //    });
                    //    //});

                    //}
                }));
            }
        }
    }
}
