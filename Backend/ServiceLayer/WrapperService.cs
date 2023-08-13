using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class WrapperService
    {
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;
        internal static ILog log;

        public WrapperService() 
        {
            log = LogClass.log;
            userService = new UserService();
            boardService = new BoardService(userService.userFacade);
            taskService = new TaskService(boardService.boardFacade);
            LoadAllData();
            //dataForMielstone3();
        }
        private void dataForMielstone3()
        {
            DeleteData();
            string mail = "mail@mail.com";
            string board = "board1";
            userService.Register(mail, "Password1");
            boardService.CreateBoard(mail, board);
            DateTime firstDT = new DateTime(2023, 6, 30);
            DateTime secondDT = new DateTime(2023, 6, 28);
            DateTime thirdDT = new DateTime(2023, 6, 27);
            taskService.AddTask(mail, board, "task1", "This is task1", firstDT);
            taskService.AddTask(mail, board, "task2", "This is task2", secondDT);
            taskService.AddTask(mail, board, "task3", "This is task3", thirdDT);
            boardService.CreateBoard(mail, "board2");
            taskService.AdvanceTask(mail, board, 0, 1);
            taskService.AdvanceTask(mail, board, 0, 2);
            taskService.AdvanceTask(mail, board, 1, 2);


        }

        ///<summary>This method deletes all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b>		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 z
        /// </summary>		 
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string DeleteData()
        {
            try
            {
                BoardController BC = new BoardController();
                BC.DeleteAll();
                userService = new UserService();
                boardService = new BoardService(userService.userFacade);
                taskService = new TaskService(boardService.boardFacade);

                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"Tried to delete all persistent data and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: Tried to delete all persistent data and got exception-  " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        ///<summary>This method loads all persisted data.		 
        ///<para>		 
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.		 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.		 
        ///</para>		 
        /// </summary>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        public string LoadAllData()
        {
            try
            {
                Dictionary<int, User> boardDict = userService.userFacade.SelectAll();
                this.boardService.boardFacade.AddDict(boardDict);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"Tried to load all data and got exception-" + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: Tried to load all data and got exception-  " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

    }
}
