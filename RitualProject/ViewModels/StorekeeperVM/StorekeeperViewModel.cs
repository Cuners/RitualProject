using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RitualProject
{
    public class StorekeeperViewModel : ViewModel, ICloseWindow
    {
        private byte[] _imageUser;
        public byte[] ImageUser
        {
            get { return _imageUser; }
            set => Set(ref _imageUser, value);
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set => Set(ref _userName, value);
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
        private UserControl _StoreKeeperProductsUserControl;
        private UserControl _MessengerUserControl;
        private UserControl currentControl;
        public UserControl CurrentControl
        {
            get { return currentControl; }
            set => Set(ref currentControl, value);
        }
        public StorekeeperViewModel()
        {
            _StoreKeeperProductsUserControl = new StorekeeperWareHouseUC();
            if (UserInfoConstant.Image != null)
            {
                ImageUser = UserInfoConstant.Image;
            }
            UserName = UserInfoConstant.FirstName;
            currentControl = _StoreKeeperProductsUserControl;
        }
        public RelayCommands _ShowFirstUserControlCommand;

        public RelayCommands ShowFirstUserControlCommand
        {
            get
            {
                return _ShowFirstUserControlCommand ?? (_ShowFirstUserControlCommand = new RelayCommands(async obj =>
                {
                    CurrentControl = _StoreKeeperProductsUserControl;
                }));
            }
        }
        private RelayCommands _ShowMessengerUserControlCommand;

        public RelayCommands ShowMessengerUserControlCommand
        {
            get
            {
                return _ShowMessengerUserControlCommand ?? (_ShowMessengerUserControlCommand = new RelayCommands(async obj =>
                {
                    _MessengerUserControl = new MessengerMainUserControl();
                    CurrentControl = _MessengerUserControl;
                }));
            }
        }
        private RelayCommands _CloseWindow;

        public RelayCommands CloseWindowNow
        {
            get
            {
                return _CloseWindow ?? (_CloseWindow = new RelayCommands(async obj =>
                {

                    Properties.Settings.Default.Login = "";
                    Properties.Settings.Default.Password = "";
                    Properties.Settings.Default.Save();
                    MainWindow main = new MainWindow();
                    main.Show();
                    CloseWindow();
                }));
            }
        }
    }
}
