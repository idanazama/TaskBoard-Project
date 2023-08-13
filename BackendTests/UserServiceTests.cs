using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class UserServiceTests
    {
        private UserService userService;
        private BoardService boardService;
        private TaskService taskService;
        public UserServiceTests(WrapperService wrapperService)
        {
            userService = wrapperService.userService;
            boardService = wrapperService.boardService;
            taskService = wrapperService.taskService;
        }
        public void RunTests()
        {
            Console.WriteLine("Testing UserService:");
            //tests for milestone1
            GetBoardsNames_NewUser_EmptyArray();
            GetBoardsNames_UserWithBoards_BoardsNames();
            Register_ExistingEmail_ErrorMessage();
            Register_ImpoperPasswords_ErrorMessage();
            Register_ProperRegister_EmptyResponse();
            Register_AutomaticLogin_LoggedInUser();
            Login_NotExistingAccount_ErrorMessage();
            Login_ProperLogin_Email();
            Login_WrongPassword_ErrorMessage();
            Logout_NonExistingAccount_ErrorMessage();
            Logout_NotLoggedinAccount_ErrorMessage();
            Logout_ProperLogout_EmptyResponse();
            InProgressTasks_NotLoggedInAccount_ErrorMessage();
            InProgressTasks_UsersWithTasks_Tasks(); //updated to milestone2
            InProgressTasks_UserWithNoTasks_EmptyResponse();
            ChangePassword_NewPasswordNotValid_ErrorMessage();
            ChangePassword_ProperUse_EmptyResponse();
            ChangePassword_WrongOldPassword_ErrorMessage();
            //tests for milestone2
            GetUserBoards_ProperUse_ListOfIDs();
            GetUserBoards_UserDoesNotExist_ErrorMessage();
            GetUserBoards_UserNotLoggedIn_ErrorMessage();
            GetUserBoards_UserWithNoBoards_EmptyResponse();
            Console.WriteLine();
        }

        //tests for milestone1

        /// <summary>
        /// This function tests Requirement 10
        /// </summary>
        public void GetBoardsNames_NewUser_EmptyArray()
        {
            string funcName = "GetBoardsNames_NewUser_EmptyArray";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa1234567");
            string response = userService.GetBoardsNames(email);
            Assert.IsReturnValueEmptyArray(response, funcName);
        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void GetBoardsNames_UserWithBoards_BoardsNames()
        {
            string funcName = "GetBoardsNames_UserWithBoards_BoardsNames";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa1234567");
            string[] boardsNames = new string[] { "board1", "board2", "board3" };
            boardService.CreateBoard(email, boardsNames[0]);
            boardService.CreateBoard(email, boardsNames[1]);
            boardService.CreateBoard(email, boardsNames[2]);
            string response = userService.GetBoardsNames(email);
            bool[] inResponse = new bool[] { false, false, false };
            Response? r = JsonSerializer.Deserialize<Response?>(response);
            try
            {
                string[]? s = JsonSerializer.Deserialize<string[]>((JsonElement)r.ReturnValue);
                for(int i =0;i<3;i++)
                {
                    foreach(string s1 in s)
                    {
                        if (s1.Equals(boardsNames[i]))
                        {
                            inResponse[i] = true;
                        }
                    }
                }
                for(int i =0;i<3;i++)
                {
                    if (!inResponse[i])
                    {
                        Console.WriteLine(funcName + ": failed");
                        return;
                    }
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 7
        /// </summary>
        public void Register_ProperRegister_EmptyResponse()
        {
            string response = userService.Register("properregiser@gmail.com", "Aa1234567");
            Assert.IsEmptyResponse(response, "Register_ProperRegister_EmptyResponse");
        }
        /// <summary>
        /// This function tests Requirement 7
        /// </summary>
        public void Register_AutomaticLogin_LoggedInUser()
        {
            string funcName = "Register_AutomaticLogin_LoggedInUser";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa1234567");
            string response = userService.Login(email, "Aa1234567");
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 2
        /// </summary>
        public void Register_ImpoperPasswords_ErrorMessage()
        {
            string funcName = "Register_ImpoperPasswords_ErrorMessage";
            string response1 = userService.Register(funcName +" 1@gmail.com", "Aa123");
            string response2 = userService.Register(funcName + "2@gmail.com", "AA123456");
            string response3 = userService.Register(funcName + "3@gmail.com", "aa123456");
            string response4 = userService.Register(funcName + "4@gmail.com", "Aa1234567891234567891");
            Assert.IsErrorMessageResponse(response1, funcName + "1");
            Assert.IsErrorMessageResponse(response2, funcName + "2");
            Assert.IsErrorMessageResponse(response3, funcName + "3");
            Assert.IsErrorMessageResponse(response4, funcName + "4");
        }
        /// <summary>
        /// This function tests Requirement 3
        /// </summary>
        public void Register_ExistingEmail_ErrorMessage()
        {
            userService.Register("Register_ExistingEmail@gmail.com", "Aa123456");
            string response = userService.Register("Register_ExistingEmail@gmail.com", "Aa123456");
            Assert.IsErrorMessageResponse(response, "Register_ExistingEmail_ErrorMessage");
        }
        /// <summary>
        /// This function tests Requirement 1
        /// </summary>
        public void Login_ProperLogin_Email()
        {
            string funcName = "Login_ProperLogin_Email";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            userService.Logout(email);
            string response = userService.Login(email, "Aa123456");
            Assert.IsReturnEqualTo(response, email, funcName);

        }
        /// <summary>
        /// This function tests Requirement 1
        /// </summary>
        public void Login_NotExistingAccount_ErrorMessage()
        {
            string funcName = "Login_NotExistingAccount_ErrorMessage";
            string response = userService.Login(funcName + "@gmail.com", "Aa123456");
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 1
        /// </summary>
        public void Login_WrongPassword_ErrorMessage()
        {
            string funcName = "Login_WrongPassword_ErrorMessage";
            userService.Register(funcName + "@gmail.com", "Aa123456");
            string response = userService.Login(funcName + "@gmail.com", "Aa123456");
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 8
        /// </summary>
        public void Logout_ProperLogout_EmptyResponse()
        {
            string funcName = "Logout_ProperLogout_EmptyResponse";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            string response = userService.Logout(email);
            Assert.IsEmptyResponse(response, funcName);

        }
        /// <summary>
        /// This function tests Requirements 8
        /// </summary>
        public void Logout_NonExistingAccount_ErrorMessage()
        {
            string funcName = "Logout_NonExistingAccount_ErrorMessage";
            string response = userService.Logout(funcName + "@gmail.com");
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirements 8
        /// </summary>
        public void Logout_NotLoggedinAccount_ErrorMessage()
        {
            string funcName = "Logout_NotLoggedinAccount_ErrorMessage";
            userService.Register(funcName + "@gmail.com", "Aa123456");
            userService.Logout(funcName + "@gmail.com");
            string response = userService.Logout(funcName + "@gmail.com");
            Assert.IsErrorMessageResponse(response, funcName);
        }

        //new implementation because of milestone2 (found below).
        /// <summary>
        /// This function tests Requirement 17
        /// </summary>
        //public void InProgressTasks_UsersWithTasks_Tasks() 
        //{
        //    string funcName = "InProgressTasks_UsersWithTasks_Tasks";
        //    string board1 = funcName + "1";
        //    string board2 = funcName + "";
        //    string email = funcName + "@gmail.com";
        //    userService.Register(email, "Aa123456");
        //    userService.Login(email, "Aa123456");
        //    boardService.CreateBoard(email, board1);
        //    boardService.CreateBoard(email, board2);
        //    taskService.AddTask(email, board1, "title", "description", DateTime.Now);
        //    taskService.AddTask(email, board1, "title", "description", DateTime.Now);
        //    taskService.AddTask(email, board2, "title", "description", DateTime.Now);
        //    taskService.AddTask(email, board2, "title", "description", DateTime.Now);
        //    string toAdvance1 = boardService.GetColumn(email, board1, 0);
        //    string toAdvance2 = boardService.GetColumn(email, board2, 0);
        //    try
        //    {
        //        Response? r1 = JsonSerializer.Deserialize<Response>(toAdvance1);
        //        Response? r2 = JsonSerializer.Deserialize<Response>(toAdvance2);
        //        TaskToSend[] t1 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r1.ReturnValue);
        //        TaskToSend[] t2 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r2.ReturnValue);
        //        int[] ids = new int[] { t1[0].Id, t1[1].Id, t2[0].Id, t2[1].Id };
        //        taskService.AdvanceTask(email, board1, 0, ids[0]);
        //        taskService.AdvanceTask(email, board1, 0, ids[1]);
        //        taskService.AdvanceTask(email, board2, 0, ids[2]);
        //        taskService.AdvanceTask(email, board2, 0, ids[3]);
        //        string response = userService.InProgressTasks(email);
        //        Response? r = JsonSerializer.Deserialize<Response>(response);
        //        TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r.ReturnValue);
        //        if (tasks.Length == 4)
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        Console.WriteLine(funcName + ": failed");
        //    }
        //    Console.WriteLine(funcName + ": failed");
        //}

        /// <summary>
        /// This function tests Requirements 17
        /// </summary>
        public void InProgressTasks_NotLoggedInAccount_ErrorMessage()
        {
            string funcName = "InProgressTasks_NotLoggedInAccount_ErrorMessage";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            userService.Logout(email);
            string response = userService.InProgressTasks(email);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 17
        /// </summary>
        public void InProgressTasks_UserWithNoTasks_EmptyResponse()
        {
            string funcName = "InProgressTasks_UserWithNoTasks_EmptyResponse";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            string response1 = userService.InProgressTasks(email);
            boardService.CreateBoard(email, "abcabc");
            string response2 = userService.InProgressTasks(email);
            Assert.IsReturnValueEmptyArray(response1, funcName);
            Assert.IsReturnValueEmptyArray(response2, funcName);
        }
        public void ChangePassword_ProperUse_EmptyResponse()
        {
            string funcName = "ChangePassword_ProperUse_EmptyResponse";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            string response = userService.ChangePassword(email, "Aa123456","Aa12345");
            Assert.IsEmptyResponse(response, funcName);
        }
        public void ChangePassword_WrongOldPassword_ErrorMessage()
        {
            string funcName = "ChangePassword_WrongOldPassword_ErrorMessage";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            string response = userService.ChangePassword(email, "Aa123455", "Aa12345");
            Assert.IsErrorMessageResponse(response, funcName);
        }
        public void ChangePassword_NewPasswordNotValid_ErrorMessage()
        {
            string funcName = "ChangePassword_NewPasswordNotValid_ErrorMessage";
            userService.Register(funcName + "1.gmail.com", "Aa123456");
            userService.Register(funcName + "2.gmail.com", "Aa123456");
            userService.Register(funcName + "3.gmail.com", "Aa123456");
            string response1 = userService.ChangePassword(funcName + "1.gmail.com", "Aa123456", "Aa123");
            string response2 = userService.ChangePassword(funcName + "2.gmail.com", "Aa123456", "aa12345");
            string response3 = userService.ChangePassword(funcName + "3.gmail.com", "Aa123456", "AA12345");
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
            Assert.IsErrorMessageResponse(response3, funcName);
        }
        //tests for milestone2

        public void GetUserBoards_ProperUse_ListOfIDs()
        {
            string funcName = "GetUserBoards_ProperUse_ListOfIDs";
            string email = funcName + "gmail.com";
            userService.Register(email, "Aa123456");
            boardService.CreateBoard(email, "board1");
            boardService.CreateBoard(email, "board3");
            boardService.CreateBoard(email, "board2");
            string response = userService.GetUserBoards(email);
            try
            {
                Response r = JsonSerializer.Deserialize<Response>(response);
                if(r.ErrorMessage!=null)
                {
                    int[] ids = JsonSerializer.Deserialize<int[]>((JsonElement)r.ReturnValue);
                    if(ids.Length!=3)
                    {
                        Console.WriteLine(funcName + ": wrong amount of ids.");
                    }
                    else if (ids[0] == ids[1] || ids[0] == ids[2] || ids[1] == ids[2])
                    {
                        Console.WriteLine(funcName + ": found boards with non-unique id.");
                    }
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }

        }
        public void GetUserBoards_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "GetUserBoards_UserNotLoggedIn_ErrorMessage";
            string email = funcName + "gmail.com";
            userService.Register(email, "Aa123456");
            boardService.CreateBoard(email, "board1");
            boardService.CreateBoard(email, "board3");
            boardService.CreateBoard(email, "board2");
            userService.Logout(email);
            string response = userService.GetUserBoards(email);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        public void GetUserBoards_UserWithNoBoards_EmptyResponse()
        {
            string funcName = "GetUserBoards_UserWithNoBoards_EmptyResponse";
            string email = funcName + "gmail.com";
            userService.Register(email, "Aa123456");
            string response = userService.GetUserBoards(email);
            Assert.IsReturnValueEmptyArray(response, funcName);
        }
        public void GetUserBoards_UserDoesNotExist_ErrorMessage()
        {
            string funcName = "GetUserBoards_UserDoesNotExist_ErrorMessage";
            string email = funcName + "gmail.com";
            string response = userService.GetUserBoards(email);
            Assert.IsErrorMessageResponse(response, funcName);
        }

        /// <summary>
        /// This function tests Requirement 17
        /// </summary>
        public void InProgressTasks_UsersWithTasks_Tasks()
        {
            string funcName = "InProgressTasks_UsersWithTasks_Tasks";
            string board1 = funcName + "1";
            string email = funcName + "@gmail.com";
            userService.Register(email, "Aa123456");
            boardService.CreateBoard(email, board1);
            taskService.AddTask(email, board1, "title", "description", DateTime.Now);
            taskService.AddTask(email, board1, "title", "description", DateTime.Now);
            string toAdvance1 = boardService.GetColumn(email, board1, 0);
            try
            {
                Response? r1 = JsonSerializer.Deserialize<Response>(toAdvance1);
                TaskToSend[] t1 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r1.ReturnValue);
                int[] ids = new int[] { t1[0].Id, t1[1].Id};
                string response2 = userService.GetUserBoards(email);
                Response r2 = JsonSerializer.Deserialize<Response>(response2);
                int boardId = JsonSerializer.Deserialize<int[]>((JsonElement)r2.ReturnValue)[0];
                taskService.AssignTask(email, board1, 0, ids[0], email);
                taskService.AssignTask(email, board1, 0, ids[1], email);
                taskService.AdvanceTask(email, board1, 0, ids[0]);
                taskService.AdvanceTask(email, board1, 0, ids[1]);
                string response = userService.InProgressTasks(email);
                Response r = JsonSerializer.Deserialize<Response>(response);
                TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r.ReturnValue);
                if (tasks.Length == 2)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(funcName + ": failed");
            }
            Console.WriteLine(funcName + ": failed");
        }
    }
}
