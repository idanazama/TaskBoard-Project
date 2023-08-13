using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {

        private UserFacade userFacade;
        private int idCounter;
        private Dictionary<int, User> boardDict; // <boardID , OwnerUser>

        private ILog log = LogClass.log;


        public BoardFacade(UserFacade userFacade)
        {
            this.userFacade = userFacade;
            idCounter = 0;
            boardDict = new Dictionary<int, User>();

        }

        public BoardFacade(UserFacade userFacade, int minAvailableID, Dictionary<int, User> boardDict)
        {
            this.userFacade = userFacade;
            idCounter = minAvailableID;
            this.boardDict = boardDict;
        }
        /// <summary>
        /// This method updates the current boards.
        /// </summary>
        /// <param name="boardDict">Dictionary of the boards and their owners</param>
        public void AddDict(Dictionary<int, User> boardDict)
        {
            this.boardDict = boardDict;
            this.idCounter = boardDict.Keys.Max() + 1;
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        public void CreateBoard(string email, string name)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            user.AddBoard(name, idCounter);
            boardDict.Add(idCounter, user);
            idCounter++;
        }
        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        public void DeleteBoard(string email, string name)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(name);
            if(!(board.Owner == email))
            {
                log.Error("This user is not the owner of the board thus he cant delete it");
                throw new KanbanException("This user is not the owner of the board thus he cant delete it");
            }
            List<Member> members = board.Members;
            foreach (Member member in members)
            {
                user = userFacade.GetUser(member.memberEmail);
                int id = user.RemoveBoard(name);
                boardDict.Remove(id);
            }
            
        }
        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.LimitColumn(columnOrdinal, limit);
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> An int with the specific column's limit</returns>

        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumnLimit(columnOrdinal);
        }
        /// <summary>
        ///  This method gets the name of the column of a specific board of a specific user
        /// </summary>
        /// <param name="email"> The email of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal"> The number of the column</param>
        /// <returns>A string with The name of the column</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumnName(columnOrdinal);

        }
        /// <summary>
        /// This method gets all of the tasks in a specific column and board
        /// </summary>
        /// <param name="email"> The email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID</param>
        /// <returns> A list of tasks from the specific column and board</returns>
        public List<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumn(columnOrdinal);
        }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.AddTask(title, description, dueDate);
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskDueDate(email,columnOrdinal,taskId,dueDate);
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskTitle(email, columnOrdinal, taskId, title);
        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskDescription(email, columnOrdinal, taskId, description);
        }
        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.AdvanceTask(columnOrdinal, taskId, email);
        }
        /// <summary>
        /// This method deletes a task
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void DeleteTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.DeleteTask(columnOrdinal, taskId);
        }
        /// <summary>
        /// This method removed the user from the board
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="boardID">Id of the board we want to remove the user from</param>
        public void LeaveBoard(string email, int boardID)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            User owner = boardDict.GetValueOrDefault(boardID, null);
            if (owner == null)
            {
                log.Error("Board with this ID doesn't exist");
                throw new KanbanException("Board with this ID doesn't exist");
            }
            if (owner.Email == email)
            {
                log.Error("The owner can't leave the board");
                throw new KanbanException("The owner can't leave the board");
            }
            Board board = owner.GetBoard(GetBoardName(boardID));
            board.ChangeToUnassigned(email);
            user.LeaveBoard(board);
        }
        /// <summary>
        /// This method add the user to the board
        /// </summary>
        /// <param name="email"> Email of the user</param>
        /// <param name="boardID">ID of the board we want to add to the user</param>
        public void JoinBoard(string email, int boardID)
        {
            User user = userFacade.GetUser(email);
            if(!user.IsLoggedIn()) 
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            if (!boardDict.ContainsKey(boardID))
            {
                log.Error("Board with this ID doesn't exist");
                throw new KanbanException("Board with this ID doesn't exist");
            }
            Board board = boardDict[boardID].GetBoard(GetBoardName(boardID));
            user.JoinBoard(board);
        }
        /// <summary>
        /// This method assigns a task to a different user
        /// </summary>
        /// <param name="email"> email of the user that the task is assigned to</param>
        /// <param name="boardName"> the name of the board</param>
        /// <param name="columnOrdinal"> the column ordinal of the task</param>
        /// <param name="taskID"> the id of the task</param>
        /// <param name="emailAssignee"> the new email of the user to assign the task to</param>
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            User user = userFacade.GetUser(email);
            User assignee = userFacade.GetUser(emailAssignee);
            if (!user.IsLoggedIn() || !assignee.IsLoggedIn())
            {
                log.Error("User isn't logged in");
                throw new KanbanException("User isn't logged in");
            }
            user.AssignTask(boardName,columnOrdinal,taskID,emailAssignee);
        }
        /// <summary>
        /// This method returns the name of the board by its ID
        /// </summary>
        /// <param name="boardId">The given boards ID</param>
        /// <returns>the board name</returns>
        public string GetBoardName(int boardId)
        {
            if (!boardDict.ContainsKey(boardId))
            {
                log.Error("Board doesn't exist");
                throw new KanbanException("Board doesn't exist");
            }
            User owner = boardDict[boardId];

            return owner.GetBoard(boardId).Name;
        }
        /// <summary>
        /// This method changes the owner of the given board
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner</param>
        /// <param name="newOwnerEmail">Email of the person that we want to give the ownership to</param>
        /// <param name="boardName">The given boards name</param>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            User owner = userFacade.GetUser(currentOwnerEmail);
            User newOwner = userFacade.GetUser(newOwnerEmail);
            if (!owner.IsLoggedIn())
            {
                log.Error("owner isn't logged in");
                throw new KanbanException("owner isn't logged in");
            }
            Board board = owner.GetBoard(boardName);
            if (board == null)
            {
                log.Error("The board doesn't exist");
                throw new KanbanException("The board doesn't exist");
            }
            if (!boardDict.ContainsKey(board.Id))
            {
                log.Error("board doesn't exist");
                throw new KanbanException("board doesn't exist");
            }
                if (boardDict[board.Id].Email != currentOwnerEmail)
            {
                log.Error("The user isn't the owner of the board");
                throw new KanbanException("The user isn't the owner of the board");
            }
        
            if (newOwner.GetBoard(boardName) == null)
            {
                log.Error("The user isn't part of the board");
                throw new KanbanException("The user isn't part of the board");
            }
            board.Owner = newOwnerEmail;
            boardDict[board.Id] = newOwner;
        }
    }

  
}
