using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RitualProject
{
    internal class WorkerVMMain : ViewModel, ICloseWindow
    {
        private UserControl _WokrerOrdersUserControl;
        private UserControl _MessengerUserControl;
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
        public WorkerVMMain()
        {
            _WokrerOrdersUserControl = new WorkerOrdersUserControl();
           
            _MessengerUserControl = new MessengerMainUserControl();
            if (UserInfoConstant.Image != null)
            {
                ImageUser = UserInfoConstant.Image;
            }
            UserName = UserInfoConstant.FirstName;
            currentControl = _WokrerOrdersUserControl;
        }


        public RelayCommands _ShowFirstUserControlCommand;

        public RelayCommands ShowFirstUserControlCommand
        {
            get
            {
                return _ShowFirstUserControlCommand ?? (_ShowFirstUserControlCommand = new RelayCommands(async obj =>
                {
                    CurrentControl = _WokrerOrdersUserControl;
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
        public RelayCommands _ShowProfileUserControlCommand;

        public RelayCommands ShowProfileUserControlCommand
        {
            get
            {
                return _ShowProfileUserControlCommand ?? (_ShowProfileUserControlCommand = new RelayCommands(async obj =>
                {
                    UserProfileWindow profileWindow = new UserProfileWindow();
                    profileWindow.ShowDialog();
                }));
            }
        }
    }
}
