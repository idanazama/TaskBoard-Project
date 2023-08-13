using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Board
    {
        private int idCounter;
        private List<Column> columns;
        private readonly string[] DEFAULT_COLUMNS = new string[] { "backlog", "in progress", "done" };
        private const int LIMITLESS_VALUE = -1;
        private const int DEFAULT_LIMIT = LIMITLESS_VALUE;
        int columns_count;
        private string owner;
        private List<Member> members;
        private int id;
        private string name;
        private BoardDTO boardDTO;
        private TaskController taskController;
        private ColumnController columnController;
        private MemberController memberController;

        private ILog log = LogClass.log;


        public Board(string ownerEmail, int id, string name, BoardController boardController, TaskController taskController, ColumnController columnController,MemberController memberController)
        {
            this.taskController = taskController;
            this.memberController = memberController;
            idCounter = 0;
            boardDTO = new BoardDTO(id, ownerEmail, name, boardController,false);
            columns = new List<Column>();
            for (int i = 0; i < DEFAULT_COLUMNS.Length; i++)
            {
                columns.Add(new Column(DEFAULT_COLUMNS[i], DEFAULT_LIMIT, id, i, columnController));
            }
            columns_count = DEFAULT_COLUMNS.Length;
            Owner = ownerEmail;
            members = new List<Member>();            
            Id = id;
            Name = name;
            AddMember(owner);
            boardDTO.Persist();
            foreach(Column column in columns)
            {
                column.PersistDTO();
            }
        }
        public Board(BoardDTO boardDTO,List<Member> members, TaskController taskController, MemberController memberController, ColumnController columnController, List<Column> columns )
        {
            idCounter = 0;
            columns_count = DEFAULT_COLUMNS.Length;
            this.columns = columns;
            this.members = members;
            this.boardDTO = boardDTO;
            this.taskController = taskController;
            this.memberController = memberController;
            this.columnController = columnController;
            this.owner = boardDTO.GetOwner();
            this.id = boardDTO.GetID();
            this.name = boardDTO.GetName();
        }
        public int Id { get => id; private set { id = value; } }
        public string Name { get => name; private set { name = value; } }
        public string Owner { get => owner; set
            {
                owner = value;
                boardDTO.UpdateOwner(owner);
            } }
        public List<Member> Members { get => members; set { members = value; } }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public void AddTask(string title, string description, DateTime DueDate)
        {
            if (columns[0].Tasks.Count == columns[0].Limit)
            {
                log.Error("Too much tasks! can't add another");
                throw new KanbanException("Too much tasks! can't add another");
            }
            Task task = new Task(idCounter,id, title, description, DueDate,taskController);
            idCounter++;
            columns[0].Tasks[task.Id] = task;
            task.PersistDTO();
        }
        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void LimitColumn(int columnOrdinal, int limit)
        {
            if(columnOrdinal < 0 || columnOrdinal>= columns_count) 
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            int tasksInOrdinal = columns[columnOrdinal].Tasks.Count;
            if(tasksInOrdinal > limit && limit!=LIMITLESS_VALUE)
            {
                log.Error("There are already more tasks then your limit!");
                throw new KanbanException("There are already more tasks then your limit!");
            }
            columns[columnOrdinal].Limit = limit;
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> An int with the specific column's limit</returns>
        public int GetColumnLimit(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            return columns[columnOrdinal].Limit;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="columnOrdinal"> The number of the column</param>
        /// <returns>A string with the column's name</returns>
        public string GetColumnName(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" +(columns_count-1));
            }
            return columns[columnOrdinal].ColumnName;
        }
        /// <summary>
        /// This method gets all of the tasks in a specific column and board
        /// </summary>
        /// <param name="columnOrdinal">The column ID</param>
        /// <returns> A list of tasks from the specific column and board</returns>
        public List<Task> GetColumn(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            List<Task> tasksOnColumn = new List<Task>();
            foreach (KeyValuePair<int, Task> kvp in columns[columnOrdinal].Tasks)
            {
                tasksOnColumn.Add(kvp.Value);
            }
            return tasksOnColumn;
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (columnOrdinal == columns_count-1)
            {
                log.Error("This task is done and can't be changed");
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null)
            {
                log.Error("This task doesn't exist");
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskDueDate(email, dueDate);
        }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (columnOrdinal == columns_count-1)
            {
                log.Error("This task is done and can't be changed");
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if(task == null)
            {
                log.Error("This task doesn't exist");
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskTitle(email, title);

        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email,int columnOrdinal, int taskId, string description)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if(columnOrdinal == columns_count - 1)
            {
                log.Error("This task is done and can't be changed");
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null)
            {
                log.Error("This task doesn't exist");
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskDescription(email, description);
        }
        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="userEmail">The email of the person trying to advance the task</param>

        public void AdvanceTask(int columnOrdinal, int taskId, string userEmail)
        {
            if(userEmail == null)
            {
                log.Error("Email can't be null!");
                throw new KanbanException("Email can't be null!");
            }
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if(columnOrdinal == columns_count - 1)
            {
                log.Error("Can't advance more!");
                throw new KanbanException("Can't advance more!");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null) {
                log.Error("This task doesn't exist!");
                throw new KanbanException("This task doesn't exist!");
            }
            if (task.AssigneeEmail != null)
                if (task.AssigneeEmail != userEmail)
                {
                    log.Error("Only the task's assignee is allowed to advance the task");
                    throw new KanbanException("Only the task's assignee is allowed to advance the task");
                }
            columns[columnOrdinal].Tasks.Remove(taskId);
            columns[columnOrdinal+1].Tasks.Add(taskId, task);
            task.AdvanceTaskDTO();
        }
        /// <summary>
        /// This method deletes a task
        /// </summary>

        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void DeleteTask(int columnOrdinal, int taskId)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                log.Error("ColumnOrdinal can be only between 0-" + (columns_count - 1));
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (!columns[columnOrdinal].Tasks.Remove(taskId))
            {
                log.Error("This task doesnt exist!");
                throw new KanbanException("This task doesnt exist!");
            }
            //needs to add - delete task from db.
        }
        /// <summary>
        /// This method assigns a task to a different user
        /// </summary>
        /// <param name="changerEmail"> The email of the user that wants to change the email assignee</param>
        /// <param name="columnOrdinal"> The column ordinal of the task</param>
        /// <param name="taskID"> The id of the task</param>
        /// <param name="emailAssignee"> The email of the user to assign the task to</param>
        internal void AssignTask(string changerEmail,int columnOrdinal, int taskID, string emailAssignee)
        {
            Column col = columns[columnOrdinal];
            if(!col.Tasks.ContainsKey(taskID)) 
            {
                log.Error("There isnt a task with this id!");
                throw new KanbanException("There isnt a task with this id!");
            }
            bool found = false;
            foreach(Member m in members)
            {
                if(m.memberEmail.Equals(emailAssignee))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                log.Error("The new assignee is not a member of this board!");
                throw new KanbanException("The new assignee is not a member of this board!");
            }
            col.Tasks[taskID].AssignTask(changerEmail,emailAssignee);
        }
        /// <summary>
        /// This method changes all of the tasks of the user to unassigned
        /// </summary>
        /// <param name="email"> The email of the user that wants to unassign all of his tasks</param>

        internal void ChangeToUnassigned(string email)
        {
            
            for(int i = 0;i<columns.Count;i++)
            {
                List<Task> tasks = GetColumn(i);
                foreach(Task task in tasks)
                {
                    if(task.AssigneeEmail == email) 
                    {
                        task.SetAssignToNull();
                    }
                }
            }

        }
        /// <summary>
        /// This methods add a new member to the board
        /// </summary>
        /// <param name="email">Email of the user we want to add</param>
        public void AddMember(string email)
        {
            Member newMember = new Member(Id, email, memberController);
            members.Add(newMember);
            newMember.PersistDTO();
        }
        /// <summary>
        /// This method delets this board from the database
        /// </summary>
        public void deleteBoard()
        {
            boardDTO.DeleteBoard();
            for(int i =0;i<columns.Count;i++)
            {
                foreach (Task task in columns[i].Tasks.Values)
                {
                    task.UnpersistDTO();
                }
                columns[i].UnpersistDTO();
            }
        }
    }
}
