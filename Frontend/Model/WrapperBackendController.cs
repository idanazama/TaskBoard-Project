using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class WrapperBackendController
    {
        public readonly BackendUserController backendUserController;
        public readonly BackendBoardController backendboardController;
        public readonly BackendTaskController backendTaskController;
        public WrapperBackendController()
        {
            WrapperService service = new WrapperService();
            backendboardController = new BackendBoardController(service.boardService);
            backendTaskController = new BackendTaskController(service.taskService);
            backendUserController = new BackendUserController(service.userService);
        }
    }
}
