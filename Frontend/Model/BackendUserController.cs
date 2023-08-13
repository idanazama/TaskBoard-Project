using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class BackendUserController
    {
        private UserService userService;
        public BackendUserController(UserService userService)
        {
            this.userService = userService;
        }
        public UserModel Register(string email,string password)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.Register(email,password));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new UserModel(email);
        }
        public UserModel Login(string email, string password)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.Login(email, password));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new UserModel(email);
        }
        public void Logout(string email)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.Logout(email));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public TaskModel[] InProgressTasks(string email)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.InProgressTasks(email));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return JsonSerializer.Deserialize<TaskModel[]>((JsonElement)response.ReturnValue);
        }
        public void ChangePassword(string email, string oldPassword, string newPassword)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.ChangePassword(email,oldPassword,newPassword));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public string[] GetBoardsNames(string email)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.GetBoardsNames(email));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return JsonSerializer.Deserialize<string[]>((JsonElement)response.ReturnValue);
        }
        public int[] GetUserBoards(string email)
        {
            Response response = JsonSerializer.Deserialize<Response>(userService.GetUserBoards(email));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return JsonSerializer.Deserialize<int[]>((JsonElement)response.ReturnValue);
        }        
    }
}
