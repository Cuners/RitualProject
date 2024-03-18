using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RitualProject
{
    public class ManagerVM : ViewModel, ICloseWindow
    {
        private UserControl _ManagerProductsUserControl;
        private UserControl _ManagerOrdersUserControl;
        private UserControl currentControl;
        public UserControl CurrentControl
        {
            get { return currentControl; }
            set => Set(ref currentControl, value);
        }
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
        public ManagerVM()
        {
            _ManagerProductsUserControl = new ManagerProductsUserControl();
            _ManagerOrdersUserControl=new ManagerOrdersUserControl();
            if (UserInfoConstant.Image != null)
            {
                ImageUser = UserInfoConstant.Image;
            }
            UserName = UserInfoConstant.FirstName;
            //_RoleUserControl = new AdminRolesUserControl();
            currentControl = _ManagerProductsUserControl;
        }


        public RelayCommands _ShowFirstUserControlCommand;

        public RelayCommands ShowFirstUserControlCommand
        {
            get
            {
                return _ShowFirstUserControlCommand ?? (_ShowFirstUserControlCommand = new RelayCommands(async obj =>
                {
                    CurrentControl = _ManagerProductsUserControl;
                }));
            }
        }
        public RelayCommands _ShowSecondUserControlCommand;

        public RelayCommands ShowSecondUserControlCommand
        {
            get
            {
                return _ShowSecondUserControlCommand ?? (_ShowSecondUserControlCommand = new RelayCommands(async obj =>
                {
                    CurrentControl = _ManagerOrdersUserControl;
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

                    MainWindow main = new MainWindow();
                    main.Show();
                    CloseWindow();
                }));
            }
        }


    }
}
