using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    /// <summary>
    /// Represents a data transfer object for a user.
    /// </summary>
    internal class UserDTO : DTO
    {
        string email;
        string password;

 
        public string GetEmail()
        {
            return email;
        }


        public string GetPassword()
        {
            return password;
        }


        public UserDTO(string email, string password,Controller controller,bool fromDB) : base(controller, fromDB)
        {
            this.email = email;
            this.password = password;
        }

        /// <summary>
        /// Persists the user DTO in the database.
        /// </summary>
        public override void Persist()
        {
            Insert(new object[] { email,password });
            isPersisted = true;
        }

        /// <summary>
        /// Updates the password of the user in the database.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        public void UpdatePassword(string newPassword)
        {
            if(isPersisted)
            {
                Update(new object[] { email }, "userPassword", newPassword);
            }
        }

    }
}
