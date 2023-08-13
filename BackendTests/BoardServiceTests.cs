using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class BoardServiceTests
    {
        private string email;
        private string password;
        private UserService userService;
        private TaskService taskService;
        private BoardService boardService;
        private readonly string[] COLUMNS_NAMES = new string[] { "backlog", "in progress", "done" };
        public BoardServiceTests(WrapperService wrapperService)
        {
            email = "boardtest@gmail.com";
            password = "Aa1234567";
            userService = wrapperService.userService;
            boardService = wrapperService.boardService;
            taskService = wrapperService.taskService;
        }
        public void RunTests()
        {
            Console.WriteLine("Testing BoardService:");
            userService.Register(email, password);
            //tests for milestone1
            CreateBoard_BoardAlreadyExists_ErrorMesssage();
            CreateBoard_ProperUse_EmptyResponse();
            CreateBoard_LimitlessInit_MinusOne();
            CreateBoard_UserNotLoggedIn_ErrorMessage();
            CreateBoard_SameNameDifferentAccounts_EmptyResponse();
            DeleteBoard_BoardDoesNotExist_ErrorMesssage();
            DeleteBoard_ProperUse_EmptyResponse();
            DeleteBoard_UserNotLoggedIn_ErrorMessage();
            LimitColumn_InvalidColumnOrdinal_ErrorMessage();
            LimitColumn_InvalidNumber_ErrorMessage();
            LimitColumn_Limitless_EmptyResponse();
            LimitColumn_ProperUse_EmptyResponse();
            GetColumnLimit_InvalidBoard_ErrorMessage();
            GetColumnLimit_InvalidColumnOrdinal_ErrorMessage();
            GetColumnLimit_ProperUse_Limit();
            GetColumn_EmptyColumn_EmptyResponse();
            GetColumn_InvalidBoard_ErrorMessage();
            GetColumn_InvalidColumn_ErrorMessage();
            GetColumn_ProperUse_ResponseWithTasks();
            GetColumnName_AllColumns_ThreeColumnNames();
            //tests for milestone2
            JoinBoard_InvalidID_ErrorMessage();
            JoinBoard_ProperUse_EmptyResponse();
            JoinBoard_UserNotLoggedIn_ErrorMessage();
            JoinBoard_AlreadyAMember_ErrorMessage();
            LeaveBoard_NotInBoard_ErrorMessage();
            LeaveBoard_ProperUse_EmptyResponse();
            LeaveBoard_UserNotLoggedIn_ErrorMessage();
            LeaveBoard_OwnerOfTheBoard_ErrorMessage();
            GetBoardName_NonExistingID_ErrorMessage();
            GetBoardName_ProperUse_BoardName();
            TransferOwnership_InvalidParameters_ErrorMessage();
            TransferOwnership_NewOwnerNotAMember_ErrorMessage();
            TransferOwnership_OwnerNotLoggedIn_ErrorMessage();
            TransferOwnership_ProperUse_EmptyResponse();

            Console.WriteLine("");
        }

        //tests for milestone1

        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void CreateBoard_ProperUse_EmptyResponse()
        {
            string funcName = "CreateBoard_ProperUse_EmptyResponse";
            string response1 = boardService.CreateBoard(email, funcName + "1");
            string response2 = boardService.CreateBoard(email, funcName + "2");
            string response3 = boardService.CreateBoard(email, funcName + "3");
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
            Assert.IsEmptyResponse(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void CreateBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "CreateBoard_UserNotLoggedIn_ErrorMessage";
            userService.Logout(email);
            string response = boardService.CreateBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
            userService.Login(email, password);
        }
        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void CreateBoard_LimitlessInit_MinusOne()
        {
            string funcName = "CreateBoard_LimitlessInit_MinusOne";
            boardService.CreateBoard(email, funcName);
            string response = boardService.GetColumnLimit(email, funcName, 0);
            Assert.IsReturnEqualTo(response, "-1", funcName);
        }
        /// <summary>
        /// This function tests Requirement 6
        /// </summary>
        public void CreateBoard_BoardAlreadyExists_ErrorMesssage()
        {
            string funcName = "CreateBoard_BoardAlreadyExists_ErrorMesssage";
            boardService.CreateBoard(email, funcName);
            string response = boardService.CreateBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
        }  
        /// <summary>
        /// This function tests Requirement 6
        /// </summary>
        public void CreateBoard_SameNameDifferentAccounts_EmptyResponse()
        {
            string funcName = "CreateBoard_SameNameDifferentAccounts_EmptyResponse";
            string email2 = funcName + "@gmail.com";
            string response1 = boardService.CreateBoard(email, funcName);
            userService.Register(email2, "Aa1234567");
            string response2 = boardService.CreateBoard(email2, funcName);
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void DeleteBoard_ProperUse_EmptyResponse()
        {
            string funcName = "DeleteBoard_ProperUse_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsEmptyResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void DeleteBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "DeleteBoard_UserNotLoggedIn_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            userService.Logout(email);
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
            userService.Login(email, password);

        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void DeleteBoard_BoardDoesNotExist_ErrorMesssage()
        {
            string funcName = "DeleteBoard_BoardDoesNotExist_ErrorMesssage";
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);

        }
        /// <summary>
        /// This function tests Requirement 11
        /// </summary>
        public void LimitColumn_ProperUse_EmptyResponse()
        {
            string funcName = "LimitColumn_ProperUse_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, 0, 2);
            string response2 = taskService.AddTask(email, funcName , "title", "description", DateTime.Now);
            string response3 = taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
            Assert.IsEmptyResponse(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void LimitColumn_InvalidNumber_ErrorMessage()
        { 
            string funcName = "LimitColumn_InvalidNumber_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, 0,-2);
            Assert.IsErrorMessageResponse(response1, funcName);
        }
        /// <summary>
        /// This function tests Requirement 11
        /// </summary>
        public void LimitColumn_Limitless_EmptyResponse()
        {
            string funcName = "LimitColumn_Limitless_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName , 0, -1);
            Assert.IsEmptyResponse(response1, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void LimitColumn_InvalidColumnOrdinal_ErrorMessage()
        {
            string funcName = "LimitColumn_InvalidColumnOrdinal_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, -1, 5);
            string response2 = boardService.LimitColumn(email, funcName, 3, 5);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11 & 12
        /// </summary>
        public void GetColumnLimit_ProperUse_Limit()
        {
            string funcName = "GetColumnLimit_ProperUse_Limit";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnLimit(email, funcName, 0);
            string response2 = boardService.GetColumnLimit(email, funcName, 1);
            string response3 = boardService.GetColumnLimit(email, funcName, 2);
            boardService.LimitColumn(email, funcName, 0, 5);
            boardService.LimitColumn(email, funcName, 1, 5);
            boardService.LimitColumn(email, funcName, 2, 5);
            string response4 = boardService.GetColumnLimit(email, funcName, 0);
            string response5 = boardService.GetColumnLimit(email, funcName, 1);
            string response6 = boardService.GetColumnLimit(email, funcName, 2);
            Assert.IsReturnEqualTo(response1, "-1", funcName);
            Assert.IsReturnEqualTo(response2, "-1", funcName);
            Assert.IsReturnEqualTo(response3, "-1", funcName);
            Assert.IsReturnEqualTo(response4, "5", funcName);
            Assert.IsReturnEqualTo(response5, "5", funcName);
            Assert.IsReturnEqualTo(response6, "5", funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void GetColumnLimit_InvalidColumnOrdinal_ErrorMessage()
        {
            string funcName = "GetColumnLimit_InvalidColumnOrdinal_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnLimit(email, funcName, -1);
            string response2 = boardService.GetColumnLimit(email, funcName, 3);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void GetColumnLimit_InvalidBoard_ErrorMessage()
        {
            string funcName = "GetColumnLimit_InvalidBoard_ErrorMessage";
            string response = boardService.GetColumnLimit(email, funcName, 0);
            Assert.IsErrorMessageResponse(response , funcName);
        }
        /// <summary>
        /// This function tests Requirements 14
        /// </summary>
        public void GetColumn_ProperUse_ResponseWithTasks()
        {
            string funcName = "GetColumn_ProperUse_ResponseWithTasks";
            boardService.CreateBoard(email, funcName);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            try
            {
                string response1 = boardService.GetColumn(email, funcName, 0);
                Response? r1 = JsonSerializer.Deserialize<Response> (response1);
                TaskToSend[] tasks1 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r1.ReturnValue);
                if (tasks1.Length == 3)
                {
                    int task1 = tasks1[0].Id;
                    int task2 = tasks1[1].Id;
                    int task3 = tasks1[2].Id;
                    taskService.AdvanceTask(email, funcName, 0, task1);
                    taskService.AdvanceTask(email, funcName, 1, task1);
                    taskService.AdvanceTask(email, funcName, 0, task2);
                    string response2 = boardService.GetColumn(email, funcName, 0);
                    string response3 = boardService.GetColumn(email, funcName, 1);
                    string response4 = boardService.GetColumn(email, funcName, 2);
                    Response? r2 = JsonSerializer.Deserialize<Response>(response2);
                    Response? r3 = JsonSerializer.Deserialize<Response>(response3);
                    Response? r4 = JsonSerializer.Deserialize<Response>(response4);
                    TaskToSend[] tasks2 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r2.ReturnValue);
                    TaskToSend[] tasks3 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r3.ReturnValue);
                    TaskToSend[] tasks4 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r4.ReturnValue);
                    if (tasks2.Length == 1 && tasks3.Length == 1 && tasks4.Length == 1)
                    {
                        return;
                    }
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
            Console.WriteLine(funcName + ": failed");
        }
        /// <summary>
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_EmptyColumn_EmptyResponse()
        {
            string funcName = "GetColumn_EmptyColumn_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumn(email, funcName, 0);
            string response2 = boardService.GetColumn(email, funcName, 0);
            string response3 = boardService.GetColumn(email, funcName, 0);
            Assert.IsReturnValueEmptyArray(response1, funcName);
            Assert.IsReturnValueEmptyArray(response2, funcName);
            Assert.IsReturnValueEmptyArray(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_InvalidColumn_ErrorMessage()
        {
            string funcName = "GetColumn_InvalidColumn_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumn(email, funcName, -1);
            string response2 = boardService.GetColumn(email, funcName, 3);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);

        }
        /// <summary>
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_InvalidBoard_ErrorMessage()
        {
            string funcName = "GetColumn_InvalidBoard_ErrorMessage";
            string response = boardService.GetColumn(email, funcName, 0);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void GetColumnName_AllColumns_ThreeColumnNames()
        {
            string funcName = "GetColumnName_AllColumns_ThreeColumnNames";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnName(email, funcName, 0);
            string response2 = boardService.GetColumnName(email, funcName, 1);
            string response3 = boardService.GetColumnName(email, funcName, 2);
            Assert.IsReturnEqualTo(response1, COLUMNS_NAMES[0], funcName);
            Assert.IsReturnEqualTo(response2, COLUMNS_NAMES[1], funcName);
            Assert.IsReturnEqualTo(response3, COLUMNS_NAMES[2], funcName);
        }

        //tests for milestone2

        public void JoinBoard_ProperUse_EmptyResponse()
        {
            string funcName = "JoinBoard_ProperUse_EmptyResponse";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            boardService.CreateBoard(otherEmail, "board1");
            boardService.CreateBoard(otherEmail, "board2");
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                List<int> ids = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue);
                string response1 = boardService.JoinBoard(email, ids[0]);
                string response2 = boardService.JoinBoard(email, ids[1]);
                Assert.IsEmptyResponse(response1, funcName);
                Assert.IsEmptyResponse(response2, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void JoinBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "JoinBoard_UserNotLoggedIn_ErrorMessage";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            boardService.CreateBoard(otherEmail, "board1");
            userService.Logout(email);
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string response = boardService.JoinBoard(email, id);
                Assert.IsErrorMessageResponse(response, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
            finally
            {
                userService.Login(email, password);
            }
        }
        public void JoinBoard_AlreadyAMember_ErrorMessage()
        {
            string funcName = "JoinBoard_AlreadyAMember_ErrorMessage";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            boardService.CreateBoard(otherEmail, "board1");
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string response1 = boardService.JoinBoard(otherEmail, id);
                boardService.JoinBoard(email, id);
                string response2 = boardService.JoinBoard(email, id);
                Assert.IsErrorMessageResponse(response1, funcName);
                Assert.IsErrorMessageResponse(response2, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void JoinBoard_InvalidID_ErrorMessage()
        {
            //this test make an assumption that we wont add 242414 tasks
            //(with our current implementation)
            string funcName = "JoinBoard_InvalidID_ErrorMessage"; 
            string response1 = boardService.JoinBoard(email, -242414);
            string response2 = boardService.JoinBoard(email, 242414);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
        }
        public void LeaveBoard_ProperUse_EmptyResponse()
        {
            string funcName = "LeaveBoard_ProperUse_EmptyResponse";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            userService.Login(email, password);
            boardService.CreateBoard(otherEmail, "board1");
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                boardService.JoinBoard(email, id);
                string response1 = boardService.LeaveBoard(email, id);
                boardService.JoinBoard(email, id);
                boardService.TransferOwnership(otherEmail, email, "board1");
                string response2 = boardService.LeaveBoard(otherEmail,id);
                Assert.IsEmptyResponse(response1, funcName);
                Assert.IsEmptyResponse(response2, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void LeaveBoard_NotInBoard_ErrorMessage()
        {
            string funcName = "LeaveBoard_NotInBoard_ErrorMessage";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            boardService.CreateBoard(otherEmail, "boardL");
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string response1 = boardService.LeaveBoard(email, id);
                boardService.JoinBoard(email, id);
                boardService.TransferOwnership(otherEmail, email, "boardL");
                boardService.LeaveBoard(otherEmail, id);
                string response2 = boardService.LeaveBoard(otherEmail, id);
                Assert.IsErrorMessageResponse(response1, funcName);
                Assert.IsErrorMessageResponse(response2, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void LeaveBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "LeaveBoard_UserNotLoggedIn_ErrorMessage";
            string otherEmail = funcName + "@gmail.com";
            userService.Register(otherEmail, "Aa123456");
            boardService.CreateBoard(otherEmail, "board1");
            try
            {
                string temp = userService.GetUserBoards(otherEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                boardService.JoinBoard(email, id);
                userService.Logout(email);
                string response1 = boardService.LeaveBoard(email, id);
                userService.Logout(otherEmail);
                string response2 = boardService.LeaveBoard(otherEmail, id);
                Assert.IsErrorMessageResponse(response1, funcName);
                Assert.IsErrorMessageResponse(response2, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
            finally
            {
                userService.Login(email, password);
            }
        }
        public void LeaveBoard_OwnerOfTheBoard_ErrorMessage()
        {
            string funcName = "LeaveBoard_OwnerOfTheBoard_ErrorMessage";
            string email1 = funcName + "@gmail.com";
            userService.Register(email1, password);
            boardService.CreateBoard(email1, "board1");
            try
            {
                string temp = userService.GetUserBoards(email1);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                List<int> ids = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue);

                string response1 = boardService.LeaveBoard(email1, ids[0]);
                Assert.IsErrorMessageResponse(response1, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void GetBoardName_ProperUse_BoardName()
        {
            string funcName = "GetBoardName_ProperUse_BoardName";
            string email1 = funcName + "@gmail.com";
            userService.Register(email1, password);
            boardService.CreateBoard(email1, funcName);
            try
            {
                string temp = userService.GetUserBoards(email1);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string response1 = boardService.GetBoardName(id);
                Assert.IsReturnEqualTo(response1,funcName, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void GetBoardName_NonExistingID_ErrorMessage()
        {
            string funcName = "GetBoardName_NonExistingID_ErrorMessage";
            string email1 = funcName + "@gmail.com";
            userService.Register(email1, password);
            boardService.CreateBoard(email1, funcName);
            try
            {

                string response1 = boardService.GetBoardName(-1);
                Assert.IsErrorMessageResponse(response1, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void TransferOwnership_ProperUse_EmptyResponse()
        {
            string funcName = "TransferOwnership_ProperUse_EmptyResponse";
            string ownerEmail = funcName + "_owner@gmail.com";
            string transferEmail = funcName + "_transfer@gmail.com";
            userService.Register(ownerEmail, "Aa123456");
            userService.Register(transferEmail, "Bb123456");
            boardService.CreateBoard(ownerEmail, "board");
            try
            {
                string temp = userService.GetUserBoards(ownerEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                boardService.JoinBoard(transferEmail, id);
                string response = boardService.TransferOwnership(ownerEmail, transferEmail, "board");
                Assert.IsEmptyResponse(response, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void TransferOwnership_OwnerNotLoggedIn_ErrorMessage()
        {
            string funcName = "TransferOwnership_OwnerNotLoggedIn_ErrorMessage";
            string ownerEmail = funcName + "_owner@gmail.com";
            string transferEmail = funcName + "_transfer@gmail.com";
            userService.Register(ownerEmail, "Aa123456");
            userService.Register(transferEmail, "Bb123456");
            boardService.CreateBoard(ownerEmail, "board");
            try
            {
                string temp = userService.GetUserBoards(ownerEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                boardService.JoinBoard(transferEmail, id);
                userService.Logout(ownerEmail);
                string response = boardService.TransferOwnership(ownerEmail, transferEmail, "board");
                Assert.IsErrorMessageResponse(response, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void TransferOwnership_NewOwnerNotAMember_ErrorMessage()
        {
            string funcName = "TransferOwnership_NewOwnerNotAMember_ErrorMessage";
            string ownerEmail = funcName + "_owner@gmail.com";
            string transferEmail = funcName + "_transfer@gmail.com";
            userService.Register(ownerEmail, "Aa123456");
            userService.Register(transferEmail, "Bb123456");
            boardService.CreateBoard(ownerEmail, "board");
            try
            {
                string temp = userService.GetUserBoards(ownerEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string response = boardService.TransferOwnership(ownerEmail, transferEmail, "board");
                Assert.IsErrorMessageResponse(response, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }
        public void TransferOwnership_InvalidParameters_ErrorMessage()
        {
            string funcName = "TransferOwnership_InvalidParameters_ErrorMessage";
            string ownerEmail = funcName + "_owner@gmail.com";
            string transferEmail = funcName + "_transfer@gmail.com";
            userService.Register(ownerEmail, "Aa123456");
            userService.Register(transferEmail, "Bb123456");
            boardService.CreateBoard(ownerEmail, "board");
            try
            {
                string temp = userService.GetUserBoards(ownerEmail);
                Response temp2 = JsonSerializer.Deserialize<Response>(temp);
                int id = JsonSerializer.Deserialize<List<int>>((JsonElement)temp2.ReturnValue)[0];
                string invalidnewOwner_response = boardService.TransferOwnership(ownerEmail, transferEmail, "board");
                boardService.JoinBoard(transferEmail, id);
                string invalidboard_response = boardService.TransferOwnership(ownerEmail, transferEmail, "invalid_board");
                string invalidcurrentowner_response = boardService.TransferOwnership(transferEmail, ownerEmail, "board");
                Assert.IsErrorMessageResponse(invalidcurrentowner_response, funcName);
                Assert.IsErrorMessageResponse(invalidboard_response, funcName);
                Assert.IsErrorMessageResponse(invalidnewOwner_response, funcName);
            }
            catch
            {
                Console.WriteLine(funcName + ": failed.");
            }
        }

    }
}
