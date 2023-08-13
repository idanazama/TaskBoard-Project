using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class BackendTaskController
    {
        private TaskService TaskService;
        public BackendTaskController(TaskService taskService)
        {
            TaskService = taskService;
        }
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {

        }
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {

        }
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {

        }
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {

        }
        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {

        }
        public void DeleteTask(string email, string boardName, int columnOrdinal, int taskId)
        {

        }
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {

        }

    }
}
