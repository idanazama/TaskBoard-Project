using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        private const int IN_PROGRESS_COLUMN_ORDINAL = 1;
        private Dictionary<string, User> users;
        private UserController userController;
        private BoardController boardController;
        private TaskController taskController;
        private ColumnController columnController;
        private MemberController memberController;

        private ILog log = LogClass.log;


        public UserFacade()
        {
            users = new Dictionary<string, User>();
            userController = new UserController();
            boardController = new BoardController();
            taskController = new TaskController();
            columnController = new ColumnController();
            memberController = new MemberController();
        }
        public UserFacade(Dictionary<string, User> users, UserController userController, BoardController boardController, TaskController taskController, ColumnController columnController, MemberController memberController)
        {
            this.users = users;
            this.boardController = boardController;
            this.taskController = taskController;
            this.columnController = columnController;
            this.userController = userController;
            this.memberController = memberController;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The new user that was created, unless an error occurs </returns>

        public User Register(string email, string password)
        {
            if (users.ContainsKey(email))
            {
                log.Error("User already exists in the system.");
                throw new KanbanException("User already exists in the system.");
            }
            User newUser = new User(email, password, userController, boardController, taskController, columnController, memberController);
            users.Add(email, newUser);
            newUser.PersistDTO();
            return newUser;
        }
        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>The new user that was created, unless an error occurs </returns>
        public User Login(string email, string password)
        {
            User user = GetUser(email);

            if (user.Login(password))
            {
                return user;
            }
            throw new KanbanException("Wrong Password.");

        }
        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout(string email)
        {
            User user = GetUser(email);

            user.Logout();
        }
        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A list of tasks of the user with this email</returns>

        public List<Task> InProgressTasks(string email)
        {
            User user = GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User must be logged in!");
                throw new KanbanException("User must be logged in!");
            }
            return user.GetTasks(IN_PROGRESS_COLUMN_ORDINAL);
        }
        /// <summary>
        /// This method checks if a user is logged in
        /// </summary>
        /// <returns>True if the user is logged in, False if not</returns>
        public bool IsLoggedIn(string email)
        {
            User user = GetUser(email);
            return user.IsLoggedIn();
        }
        /// <summary> 
        /// This method let a user change password.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldPassword">Current user password.</param>
        /// <param name="newPassword">New user password</param>
        public void ChangePassword(string email, string oldPassword, string newPassword)
        {
            User user = GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User is not logged in");
                throw new KanbanException("User is not logged in");
            }
            user.ChangePassword(oldPassword, newPassword);
        }
        /// <summary>
        /// This method gets a user by email
        /// </summary>
        /// <param name="email"> The email of the user to be returned</param>
        /// <returns> A user that registered with the same email</returns>
        public User GetUser(string email)
        {
            if (email == null)
            {
                log.Error("Email can't be null");
                throw new KanbanException("Email can't be null");
            }
            User user = users.GetValueOrDefault(email, null);
            if (user == null)
            {
                log.Error("This user doesn't exist in the system!");
                throw new KanbanException("This user doesn't exist in the system!");
            }
            return user;
        }

        /// <summary>
        /// This method returns the names of the user's boards
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns>A list of strings with the names of the boards</returns>
        public List<string> GetBoardsNames(string email)
        {
            User user = GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User is not logged in");
                throw new KanbanException("User is not logged in");
            }
            return user.GetBoardsNames();

        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns> a list of integers</returns>
        public List<int> GetUserBoards(string email)
        {
            User user = GetUser(email);
            if (!user.IsLoggedIn())
            {
                log.Error("User is not logged in");
                throw new KanbanException("User is not logged in");
            }
            return user.GetUserBoards();
        }

        /// <summary>
        /// This method retrieves all of the data regarding the boards and their owners.
        /// </summary>
        /// <returns>A dictionary of the board IDs and their owners</returns>
        public Dictionary<int,User> SelectAll()
        {

            List<UserDTO> usersDTO = userController.Select();
            List<User> allUsers = new List<User>();
            Dictionary<int, Board> allBoards = new Dictionary<int, Board>();

            foreach (UserDTO userDTO in usersDTO)
            {
                List<BoardDTO> boardDTOsOwner = boardController.SelectAllOwnerBoards(userDTO.GetEmail());
                Dictionary<int, Board> boards = new Dictionary<int, Board>();
                Dictionary<string, int> ids = new Dictionary<string, int>();

                foreach (BoardDTO boardDTO in boardDTOsOwner)
                {
                    List<Member> members = new List<Member>();
                    List<MemberDTO> membersDTO = memberController.SelectMembersFromID(boardDTO.GetID());
                    foreach (MemberDTO memberDTO in membersDTO)
                    {
                        members.Add(new Member(memberDTO));
                    }
                    List<ColumnDTO> columnDTOs = columnController.SelectColumnsFromID(boardDTO.GetID());
                    List<TaskDTO> taskDTOs = taskController.SelectAllTasksFromBoard(boardDTO.GetID());
                    Dictionary<int,Task>[] arrTasks = new Dictionary<int, Task>[columnDTOs.Count];
                    for(int i = 0; i<arrTasks.Length; i++)
                    {
                        arrTasks[i] = new Dictionary<int, Task>();
                    }
                    foreach(TaskDTO taskDTO in taskDTOs)
                    {
                        arrTasks[taskDTO.GetcolumnOrdinal()].Add(taskDTO.GettaskID(), new Task(taskDTO));
                    }
                    List<Column> columns = new List<Column>();
                    foreach (ColumnDTO columnDTO in columnDTOs)
                    {
                        columns.Add(new Column(columnDTO, arrTasks[columnDTO.GetColumnOrdinal()]));
                    }
                    ids.Add(boardDTO.GetName(), boardDTO.GetID());
                    Board board = new Board(boardDTO, members, taskController, memberController, columnController, columns);
                    boards.Add(boardDTO.GetID(), board);
                    if (!allBoards.ContainsKey(boardDTO.GetID())){
                        allBoards.Add(boardDTO.GetID(), board);
                    }
                }
                allUsers.Add(new User(userDTO, boards, ids,boardController, taskController, columnController, memberController));

            }
            foreach(User user in allUsers)
            {
                users.Add(user.Email, user);
            }
            Dictionary<int,User> boardDict = new Dictionary<int,User>();
            foreach(Board board in allBoards.Values)
            {
                boardDict.Add(board.Id, GetUser(board.Owner));
                List<Member> members = board.Members;
                foreach (Member member in members)
                {
                    User user = GetUser(member.memberEmail);
                    user.JoinBoardFromDTO(board);
                }
            }
            
            return boardDict;
        }

    }
}
