using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        private const int a_CHAR_VALUE = 'a';
        private const int A_CHAR_VALUE = 'A';
        private const int ZERO_CHAR_VALUE = '0';
        private const int NUMBERS = 10;
        private const int LETTERS = 26;

        private Dictionary<int, Board> boards;
        private Dictionary<string, int> ids;
        private string email;
        private string password;
        private bool loggedIn;
        private UserDTO userDTO;
        private BoardController boardController;
        private TaskController taskController;
        private ColumnController columnController;
        private MemberController memberController;

        private ILog log = LogClass.log;


        public User(string email, string password,UserController controller,BoardController boardController,TaskController taskController, ColumnController columnController, MemberController memberController)
        {
            this.boardController = boardController;
            this.taskController = taskController;
            this.columnController = columnController;
            this.memberController = memberController;
            userDTO = new UserDTO(email, password, controller,false);
            Email = email;
            Password = password;
            boards = new Dictionary<int, Board>();
            ids = new Dictionary<string, int>();
            loggedIn = false;
        }
        public void PersistDTO()
        {
            userDTO.Persist();
        }
        public User(UserDTO userDTO, Dictionary<int, Board> boards, Dictionary<string, int> ids, BoardController boardController, TaskController taskController, ColumnController columnController, MemberController memberController)
        {
            this.userDTO = userDTO;
            this.boardController= boardController;
            this.taskController = taskController;
            this.columnController = columnController;
            this.memberController = memberController;
            this.email = userDTO.GetEmail();
            this.password = userDTO.GetPassword();
            this.ids = ids;
            this.boards = boards;
        }
        public string Email
        {
            get => email;
            private set
            {
                if (value == null)
                {
                    log.Error("Email can't be null");
                    throw new KanbanException("Email can't be null");
                }
                if (value.Length == 0)
                {
                    log.Error("Email can't be empty!");
                    throw new KanbanException("Email can't be empty!");
                }
                email = value;
            }
        }
        private string Password
        {
            set
            {
                if (value == null)
                {
                    log.Error("Password can't be null");
                    throw new KanbanException("Password can't be null");
                }
                if (value.Length < 6 || value.Length > 20)
                {
                    log.Error("password too short\too long");
                    throw new KanbanException("password too short\too long");
                }
                int upper = 0;
                int lower = 0;
                int number = 0;
                foreach (char c in value)
                {
                    int testNumber = c - ZERO_CHAR_VALUE;
                    int testLowerCase = c - a_CHAR_VALUE;
                    int testUpperCase = c - A_CHAR_VALUE;
                    if (testNumber >= 0 && testNumber < NUMBERS)
                    {
                        number++;
                    }
                    if (testLowerCase >= 0 && testLowerCase < LETTERS)
                    {
                        lower++;
                    }
                    if (testUpperCase >= 0 && testUpperCase < LETTERS)
                    {
                        upper++;
                    }
                }
                if (upper == 0)
                {
                    log.Error("Password does not inclued a upper case letter");
                    throw new KanbanException("Password does not inclued a upper case letter");
                }
                if (lower == 0)
                {
                    log.Error("Password does not inclued a lower case letter");
                    throw new KanbanException("Password does not inclued a lower case letter");
                }
                if (number == 0)
                {
                    log.Error("Password does not inclued a number");
                    throw new KanbanException("Password does not inclued a number");
                }
                password = value;
                userDTO.UpdatePassword(value);
            }
        }
        /// <summary>
        /// This method changes the password of the user.
        /// </summary>
        /// <param name="oldPassword">Current pasword of the user</param>
        /// <param name="newPassword">The password the users wants it to be</param>
        /// <exception cref="KanbanException"></exception>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (!Authenticate(oldPassword))
            {
                log.Error("wrong old password");
                throw new KanbanException("wrong old password");
            }
            Password = newPassword;
        }
        /// <summary>
        /// This method adds a board to this user
        /// </summary>
        /// <param name="boardName"> The name of the new board</param>
        public void AddBoard(string boardName,int id) 
        {
            if (boardName == null)
            {
                log.Error("Board name can't be null");
                throw new KanbanException("Board name can't be null");
            }
            if (ids.ContainsKey(boardName))
            {
                log.Error("A board with this name already exists!");
                throw new KanbanException("A board with this name already exists!");
            }
            ids.Add(boardName,id);
            boards.Add(id, new Board(email,id, boardName,boardController,taskController,columnController,memberController));

        }
        /// <summary>
        /// This method removes a certain board from this user's boards
        /// </summary>
        /// <param name="boardName">The name of the board to be removed</param>

        public int RemoveBoard(string boardName)
        {
            if (boardName == null)
            {
                log.Error("Board name can't be null");
                throw new KanbanException("Board name can't be null");
            }
            if (!ids.ContainsKey(boardName))
            {
                log.Error("The user doesnt have a board with this name!");
                throw new KanbanException("The user doesnt have a board with this name!");
            }
            int id = ids[boardName];
            ids.Remove(boardName);
            Board board;
            boards.TryGetValue(id, out board);
            board.deleteBoard();
            boards.Remove(id);
            return id;
        }
        /// <summary>
        /// This method gets a board that has the same name from this user's boards
        /// </summary>
        /// <param name="boardName"> The name of the board</param>
        /// <returns> A board instance with the name</returns>

        public Board GetBoard(string boardName)
        {
            if (boardName == null)
            {
                log.Error("Board name can't be null");
                throw new KanbanException("Board name can't be null");
            }
            int id = ids.GetValueOrDefault(boardName, -1);
            if(id == -1)
            {
                log.Error("The user doesnt have a board with this name!");
                throw new KanbanException("The user doesnt have a board with this name!");
            }
            return boards[id];
        }
        /// <summary>
        /// This method gets all of the tasks from a desired column
        /// </summary>
        /// <param name="columnOrdinal"> The no. of the column </param>
        /// <returns> All the tasks from the column with the same column ordinal</returns>
        public List<Task> GetTasks(int columnOrdinal)
        {
            List<Task> tasks = new List<Task>();
            foreach(Board board in boards.Values)
            {
                List<Task> toAdd = board.GetColumn(columnOrdinal);
                foreach(Task task in toAdd)
                {
                    if(task.AssigneeEmail == email)
                        tasks.Add(task);
                }
            }
            return tasks;
        }

        /// <summary>
        /// This method checks if a user is logged in
        /// </summary>
        /// <returns>True if the user is logged in, False if not</returns>
        public bool IsLoggedIn() { return loggedIn; }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>The new user that was created, unless an error occurs </returns>
        public bool Login(string password)
        {
            bool success = Authenticate(password);
            if (success)
            {
                if (loggedIn)
                {
                    log.Error("User already logged in");
                    throw new KanbanException("User already logged in");
                }
                loggedIn = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout()
        {
            if (loggedIn)
            {
                loggedIn = false;
            }
            else
            {
                log.Error("User is not logged in.");
                throw new KanbanException("User is not logged in.");
            }
        }
        /// <summary>
        /// This method checks if the passwords match
        /// </summary>
        /// <param name="password"> The password of the user we want to autheticate</param>
        /// <returns> True if the passwords match, else false</returns>
        public bool Authenticate(string password)
        {
            return this.password.Equals(password);
        }
        /// <summary>
        /// This method returns the names of the user's boards
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns>A list of strings with the names of the boards</returns>
        public List<string> GetBoardsNames()
        {
            return new List<string>(this.ids.Keys);
        }
        /// <summary>
        /// This method joins the user to the given board
        /// </summary>
        /// <param name="board"> the board that we want to add the user to</param>
        internal void JoinBoard(Board board)
        {
            if (ids.ContainsKey(board.Name))
            {
                log.Error("User alredy has a board with this name");
                throw new KanbanException("User alredy has a board with this name");
            }
            int id = board.Id;
            ids.Add(board.Name,id);
            board.AddMember(this.email);
            boards.Add(id, board);
        }
        /// <summary>
        /// This method adds a board to this users fields without updating the DTO
        /// </summary>
        /// <param name="board"> The board to add to the users fields</param>
        internal void JoinBoardFromDTO(Board board)
        {
            if (!ids.ContainsKey(board.Name))
            {
                int id = board.Id;
                ids.Add(board.Name, id);
                boards.Add(id, board);
            } 
        }
        /// <summary>
        /// This method return the ids of the users boards.
        /// </summary>
        /// <returns>a list of ints</returns>
        public List<int> GetUserBoards()
        {
            return new List<int>(this.boards.Keys);
        }
        /// <summary>
        /// This method removed the user from the given board
        /// </summary>
        /// <param name="board"> the board that we want to remove the user from</param>
        public void LeaveBoard(Board board)
        {
            if (!ids.ContainsKey(board.Name))
            {
                log.Error("User is alredy unassigned to the board");
                throw new KanbanException("User is alredy unassigned to the board");
            }
            int id = board.Id;
            ids.Remove(board.Name);
            boards.Remove(id);
        }
        /// <summary>
        /// This method assigns the task to a different user
        /// </summary>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal"> The column ordinal of the task</param>
        /// <param name="taskID"> The id of the task</param>
        /// <param name="emailAssignee"> The new email to be changed to</param>
        public void AssignTask(string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            if (!ids.ContainsKey(boardName))
            {
                log.Error("User doesn't have a board with this name");
                throw new KanbanException("User doesn't have a board with this name");
            }
            int id = ids[boardName];
            Board board = boards[id];
            board.AssignTask(email, columnOrdinal,taskID, emailAssignee);
        }

        public Board GetBoard(int id)
        {
            return boards[id];
        }
    }


}
