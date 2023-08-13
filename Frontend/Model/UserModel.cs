using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class UserModel
    {
        private string email ="";
        public string Email
        {
            get => email;
            set
            {
                email = value;
               // RaisePropertyChanged("Email");
            }
        }
        public UserModel(string email)
        {
            this.email = email;
        }
    }
}
