using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Column
    {
        private readonly string[] DEFAULT_COLUMNS = new string[] { "backlog", "in progress", "done" };
        private Dictionary<int, Task> tasks;
        private string columnName;
        private int limit;
        private ColumnDTO columnDTO;

        public Column(string name, int limit,int boardID,int columnOrdinal,ColumnController columnController)
        {
            columnDTO = new ColumnDTO(boardID, columnOrdinal, limit, columnController,false);
            Limit = limit;
            ColumnName = name;
            Tasks = new Dictionary<int, Task>();
        }
        public Column(ColumnDTO columnDTO, Dictionary<int,Task> tasks) 
        {
            this.columnDTO = columnDTO;
            this.tasks = tasks;
            this.columnName = DEFAULT_COLUMNS[columnDTO.GetColumnOrdinal()];
            this.limit = columnDTO.GetLimit();

        }
        /// <summary>
        /// This method persists the data.
        /// </summary>
        public void PersistDTO()
        {
            columnDTO.Persist();
        }
        /// <summary>
        /// This method unpersists the data.
        /// </summary>
        public void UnpersistDTO()
        {
            columnDTO.Unpersist();
        }
        public Dictionary<int, Task> Tasks
        {
            get => tasks; set { tasks = value; }
        }
        public string ColumnName { get=>columnName; set { columnName = value; } }
        public int Limit { get=>limit; set { 
                limit = value;
                columnDTO.UpdateLimit(limit);
            } }

    }
}
