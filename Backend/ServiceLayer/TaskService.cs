using IntroSE.Kanban.Backend.BusinessLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private BoardFacade boardFacade;
        private ILog log;
        internal TaskService(BoardFacade boardFacade)
        {
            this.boardFacade = boardFacade;
            log = WrapperService.log;
        }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                boardFacade.AddTask(email, boardName, title, description, dueDate);
                log.Info(email + " added task to board:" + boardName);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to add task to board {boardName} and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to add task to board {boardName} and got exception-" + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                boardFacade.UpdateTaskDueDate(email,boardName,columnOrdinal,taskId,dueDate);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to update task {taskId} DueDate from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to update task {taskId} DueDate from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                boardFacade.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to update task {taskId} title from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to update task {taskId} title from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                boardFacade.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to update task {taskId} description from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to update task {taskId} description from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardFacade.AdvanceTask(email, boardName, columnOrdinal, taskId);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to advance task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to advance task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }
        /// <summary>
        /// This method deletes a task
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardFacade.DeleteTask(email, boardName, columnOrdinal, taskId);
                log.Info($" {email} deleted task {taskId} from board {boardName} in column {columnOrdinal}");

                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to delete task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to delete task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>		 
        /// This method assigns a task to a user		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>		 
        /// <param name="taskId">The task to be updated identified a task ID</param>        		 
        /// <param name="emailAssignee">Email of the asignee user</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                boardFacade.AssignTask(email, boardName, columnOrdinal, taskId, emailAssignee);
                log.Info($" {email} deleted task {taskId} from board {boardName} in column {columnOrdinal}");

                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to delete task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to delete task {taskId} from board {boardName} in column {columnOrdinal} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }
    }
}
