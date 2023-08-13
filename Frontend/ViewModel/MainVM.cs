using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Frontend.ViewModel
{
    internal class MainVM : NotifiableObject
    {
        private string email = "";
        private string password = "";
        private WrapperBackendController wrapperBackendController;
        public MainVM(WrapperBackendController wrapperBackendController)
        {
            this.wrapperBackendController = wrapperBackendController;
        }
        public string Email
        {
            set
            {
                email = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
            get
            {
                return email;
            }
        }
        public string Password
        {
            set
            {
                password = value;
                FieldsAreNotEmpty = string.IsNullOrWhiteSpace(value);
            }
            get
            {
                return password;
            }
        }
        public bool FieldsAreNotEmpty
        {
            set
            {
                RaisePropertyChanged("FieldsAreNotEmpty");
            }
            get =>
                !string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password);
        }
        public UserModel? Register()
        {
            try
            {
                UserModel u = wrapperBackendController.backendUserController.Register(email, password);
                return u;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        public UserModel? Login()
        {
            try
            {
                UserModel u = wrapperBackendController.backendUserController.Login(email, password);
                return u;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
