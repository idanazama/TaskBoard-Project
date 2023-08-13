using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Task;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private ILog log;

        internal readonly UserFacade userFacade;
        internal UserService()
        {
            userFacade = new UserFacade();
            log = WrapperService.log;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            try
            {
                userFacade.Register(email, password);
                userFacade.Login(email, password);
                log.Info($"{email} registered");

                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to register and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to register and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            try
            {
                User u = userFacade.Login(email, password);
                log.Info($"{email} logged in");
                return JsonSerializer.Serialize(new Response(null, u.Email));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to login and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to login and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            try
            {
                userFacade.Logout(email);
                log.Info($"{email} logged out");
                return JsonSerializer.Serialize(new Response(null,null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to log out and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to log out and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            { 
                List<Task> tasks = userFacade.InProgressTasks(email);
                List<TaskToSend> tasksToSends = new List<TaskToSend>();
                foreach (Task task in tasks)
                {
                    tasksToSends.Add(new TaskToSend(task));
                }
                return JsonSerializer.Serialize(new Response(null, tasksToSends));

            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get his in progress tasks and got exception-" + ex.Message);

                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get his in progress tasks and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary> 
        /// This method let a user change password.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldPassword">Current user password.</param>
        /// <param name="newPassword">New user password</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string ChangePassword(string email, string oldPassword, string newPassword)
        {
            try
            {
                userFacade.ChangePassword(email, oldPassword, newPassword);
                log.Info($"{email} changed password");
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to change his password and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to change his password and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }
        /// <summary>
        /// This method returns the names of the user's boards
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns>A list with all the user boards names.</returns>
        public string GetBoardsNames(string email)
        {
            try
            {
                List<string> names = userFacade.GetBoardsNames(email);
                return JsonSerializer.Serialize(new Response(null, names));

            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get the names of his boards and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get the names of his boards and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }

        }
        /// <summary>		 
        /// This method returns a list of IDs of all user's boards.		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string GetUserBoards(string email)
        {
            try
            {
                List<int> ids = userFacade.GetUserBoards(email);
                return JsonSerializer.Serialize(new Response(null, ids));

            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get the ids of his boards and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get the ids of his boards and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }

        }
    }
}
