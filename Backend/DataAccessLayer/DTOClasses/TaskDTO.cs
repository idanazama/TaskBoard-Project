using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    /// <summary>
    /// Represents a data transfer object for a task.
    /// </summary>
    internal class TaskDTO : DTO
    {
        int boardID;
        int taskID;
        int columnOrdinal;
        DateTime creationTime;
        string title;
        string description;
        DateTime dueDate;
        string assingeeEmail;

        public int GetboardID()
        {
            return boardID;
        }
        public int GettaskID()
        {
            return taskID;
        }
        public int GetcolumnOrdinal()
        {
            return columnOrdinal;
        }
        public DateTime GetcreationTime()
        {
            return creationTime;
        }
        public DateTime GetDueTime()
        {
            return dueDate;
        }
        public string GetTitle()
        {
            return title;
        }
        public string GetDescription()
        {
            return description;
        }
        public string GetAssigneEmail()
        {
            return assingeeEmail;
        }


        public TaskDTO(int taskID, int boardID, DateTime creationTime, string title, string description, DateTime dueDate, string assingeeEmail,TaskController controller,bool fromDB) : base(controller, fromDB)
        {
            this.boardID = boardID;
            this.taskID = taskID;
            this.columnOrdinal = 0;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.assingeeEmail = assingeeEmail;
        }
        public TaskDTO(int taskID, int boardID, DateTime creationTime, string title, string description, DateTime dueDate, string assingeeEmail,int columnOrdinal, TaskController controller, bool fromDB) : base(controller, fromDB)
        {
            this.boardID = boardID;
            this.taskID = taskID;
            this.columnOrdinal = columnOrdinal;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.assingeeEmail = assingeeEmail;
        }

        /// <summary>
        /// Persists the task into the database.
        /// </summary>
        public override void Persist()
        {
            Insert(new object[] { taskID, boardID, creationTime, title, description, dueDate, assingeeEmail,columnOrdinal });
            isPersisted = true;
        }

        /// <summary>
        /// Advances the task to the next column.
        /// </summary>
        public void AdvanceTask()
        {
            if(isPersisted)
            {
                columnOrdinal++;
                Update(new object[] { taskID, boardID }, "columnOrdinal", columnOrdinal);
            }
        }

        /// <summary>
        /// Updates the due date of the task.
        /// </summary>
        /// <param name="dueDate">The new due date.</param>
        public void UpdateDueDate(DateTime dueDate)
        {
            if (isPersisted)
            {
                Update(new object[] { taskID, boardID }, "dueDate", dueDate );
            }
        }

        /// <summary>
        /// Updates the title of the task.
        /// </summary>
        /// <param name="title">The new title.</param>
        public void UpdateTitle(string title)
        {
            if(isPersisted)
            {
                Update(new object[] { taskID, boardID }, "title", title);
            }
        }

        /// <summary>
        /// Updates the description of the task.
        /// </summary>
        /// <param name="description">The new description.</param>
        public void UpdateDescription(string description)
        {
            if (isPersisted)
            {
                Update(new object[] { taskID, boardID }, "description", description);
            }
        }

        /// <summary>
        /// Updates the assignee of the task.
        /// </summary>
        /// <param name="assigneeEmail">The email of the new assignee.</param>
        public void UpdateAssignee(string assigneeEmail)
        {
            if(isPersisted)
            {
                Update(new object[] { taskID, boardID }, "assigneeEmail", assigneeEmail);
            }
        }

        /// <summary>
        /// Deletes the task from the database.
        /// </summary>
        public void DeleteTask()
        {
            if (isPersisted)
            {
                Delete(new object[] { taskID, boardID });
                isPersisted = false;
            }
        }

        /// <summary>
        /// Unpersists the task.
        /// </summary>
        public void Unpersist()
        {
            isPersisted = false;
        }
    }
}
