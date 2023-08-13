using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Task
    {
        private const int MAX_TITLE_LENGTH = 50;
        private const int MAX_DESCRIPTION_LENGTH = 300;

        private int id;
        private int boardID; 
        private DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private string assigneeEmail;
        private TaskDTO taskDTO;

        private ILog log = LogClass.log;


        public Task(int id,int boardID, string title, string description, DateTime dueDate,TaskController controller)
        {
            DateTime tempCreationTime = DateTime.Now;
            taskDTO = new TaskDTO(id, boardID, tempCreationTime, title, description, dueDate,null,controller,false);
            Title = title;
            Description = description;
            Id = id;
            CreationTime = tempCreationTime;
            DueDate = dueDate;
            AssigneeEmail = assigneeEmail;
            this.boardID = boardID;
        }
        public Task(TaskDTO taskDTO)
        {
            this.taskDTO = taskDTO;
            this.id = taskDTO.GettaskID();
            this.creationTime = taskDTO.GetcreationTime();
            this.title = taskDTO.GetTitle();
            this.description = taskDTO.GetDescription();
            this.dueDate = taskDTO.GetDueTime();
            this.assigneeEmail = taskDTO.GetAssigneEmail();
            this.boardID = taskDTO.GetboardID();
        }
        /// <summary>
        /// This method updates the task in the database.
        /// </summary>
        public void AdvanceTaskDTO()
        {
            taskDTO.AdvanceTask();
        }
        /// <summary>
        /// This method persists the data.
        /// </summary>
        public void PersistDTO()
        {
            taskDTO.Persist();
        }
        /// <summary>
        /// This method unpersists the data.
        /// </summary>
        public void UnpersistDTO()
        {
            taskDTO.Unpersist();
        }
        public int Id {
            get => id; 
            private set { 
                id = value;
            }
        }
        public string Title {
            get => title; 
            private set
            {
                if (value == null)
                {
                    log.Error("Title can't be null!");
                    throw new KanbanException("Title can't be null!");
                }
                if (value == "" || value.Length > MAX_TITLE_LENGTH)
                {
                    log.Error("Title cant be empty and has to have a maximum of " + MAX_TITLE_LENGTH + " characters");
                    throw new KanbanException("Title cant be empty and has to have a maximum of " + MAX_TITLE_LENGTH + " characters");
                }
                title = value;
                taskDTO.UpdateTitle(value);
            }
        }
        public string Description { 
            get => description;
            private set
            {
                if (value == null)
                {
                    log.Error("Description can't be null!");
                    throw new KanbanException("Description can't be null!");
                }
                if (value.Length > MAX_DESCRIPTION_LENGTH)
                {
                    log.Error("Description has to have a maximum of " + MAX_DESCRIPTION_LENGTH + " characters");
                    throw new KanbanException("Description has to have a maximum of " + MAX_DESCRIPTION_LENGTH + " characters");
                }
                description = value;
                taskDTO.UpdateDescription(value);
            }
        }
        public DateTime CreationTime { 
            get => creationTime;
            private set {creationTime = value; }
        }
        public DateTime DueDate {
            get => dueDate;
            private set
            {
                dueDate = value;
                taskDTO.UpdateDueDate(value);
            }
        }
        public string AssigneeEmail
        {
            get => assigneeEmail;
            private set
            {
                assigneeEmail = value;
                taskDTO.UpdateAssignee(value);
            }
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(string email, DateTime dueDate)
        {
            if (email!= null && assigneeEmail != null && assigneeEmail != email)
            {
                log.Error("This user is not assigned to this task!");
                throw new KanbanException("This user is not assigned to this task!");
            }
            DueDate = dueDate;
        }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email,string title)
        {
            if (email != null && assigneeEmail != null && assigneeEmail != email)
            {
                log.Error("This user is not assigned to this task!");
                throw new KanbanException("This user is not assigned to this task!");
            }
            Title = title;
        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email,string description)
        {
            if(email != null && assigneeEmail !=null && assigneeEmail != email)
            {
                log.Error("This user is not assigned to this task!");
                throw new KanbanException("This user is not assigned to this task!");
            }
            Description = description;
        }
        /// <summary>
        /// This method changes the email assignee of this task
        /// </summary>
        /// <param name="changerEmail"> The email of the user that wants to change the assignee</param>
        /// <param name="email"> The email to change the assigne email</param>
        public void AssignTask(string changerEmail,string email)
        {
            if(email != null && assigneeEmail != null && changerEmail != assigneeEmail)
            {
                if(assigneeEmail != null)
                {
                    log.Error("Only the assignee can assign his tasks!");
                    throw new KanbanException("Only the assignee can assign his tasks!");
                }
            }
            AssigneeEmail = email;
        }
        /// <summary>
        /// This method sets the assignee email to null
        /// </summary>
        public void SetAssignToNull()
        {
            AssigneeEmail = null;
        }
    }

   
}
