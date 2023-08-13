using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Frontend.Model
{
    internal class BackendBoardController
    {
        private BoardService boardService;
        public BackendBoardController(BoardService boardService)
        {
            this.boardService = boardService;
        }
        public void CreateBoard(string email, string name)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.CreateBoard(email, name));
            if(response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            
        }
        public void DeleteBoard(string email, string name)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.DeleteBoard(email, name));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.LimitColumn(email,boardName,columnOrdinal,limit));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.GetColumnLimit(email,boardName,columnOrdinal));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return ((JsonElement)response.ReturnValue).GetInt32();
        }   
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.GetColumnName(email,boardName,columnOrdinal));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return  response.ReturnValue.ToString();
        }
        public List<TaskModel> GetColumn(string email, string boardName, int columnOrdinal)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.GetColumn(email,boardName,columnOrdinal));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            TaskToSend[] tasksToSend= JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)response.ReturnValue);
            List<TaskModel> list = new List<TaskModel>();
            foreach(TaskToSend t in tasksToSend)
            {
                list.Add(new TaskModel(t.Id,t.CreationTime,t.Title,t.Description,t.DueDate));
            }
            return list;
        }
        public void LeaveBoard(string email, int boardID)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.LeaveBoard(email,boardID));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public void JoinBoard(string email, int boardID)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.JoinBoard(email,boardID));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        public string GetBoardName(int boardID)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.GetBoardName(boardID));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
            return (string)response.ReturnValue;
        }
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response response = JsonSerializer.Deserialize<Response>(boardService.TransferOwnership(currentOwnerEmail,newOwnerEmail,boardName));
            if (response.ErrorOccured)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}
