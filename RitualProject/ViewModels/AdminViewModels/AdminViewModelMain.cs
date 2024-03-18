using RitualServer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace RitualProject
{
    public class AdminViewModelMain : ViewModel, ICloseWindow
    {


        private UserControl _AdminUsersUserControl;
        private UserControl _RoleUserControl;
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
            set=>Set(ref _userName, value); 
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
        public AdminViewModelMain()
        {
            _AdminUsersUserControl = new AdminUsersUserControl();
            _RoleUserControl = new AdminRolesUserControl();
            if (UserInfoConstant.Image != null)
            {
                ImageUser = UserInfoConstant.Image;
            }
            UserName = UserInfoConstant.FirstName;
            currentControl = _AdminUsersUserControl;
        }

       
        public RelayCommands _ShowFirstUserControlCommand;

        public RelayCommands ShowFirstUserControlCommand
        {
            get
            {
                return _ShowFirstUserControlCommand ?? (_ShowFirstUserControlCommand = new RelayCommands(async obj =>
                {
                    CurrentControl = _AdminUsersUserControl;
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
                    CurrentControl=_RoleUserControl;
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
