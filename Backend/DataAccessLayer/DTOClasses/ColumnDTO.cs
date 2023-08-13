using IntroSE.Kanban.Backend.DataAccessLayer.ControllerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOClasses
{
    internal class ColumnDTO : DTO
    {
        int boardID;
        int columnOrdinal;
        int limit;

        public int GetColumnOrdinal()
        {
            return columnOrdinal;
        }
        public int GetboardID()
        {
            return boardID;
        }
        public int GetLimit()
        {
            return limit;
        }
        public ColumnDTO(int boardID, int columnOrdinal, int limit, ColumnController controller, bool fromDB) : base(controller, fromDB)
        {
            this.boardID = boardID;
            this.columnOrdinal = columnOrdinal;
            this.limit = limit;
        }

        /// <summary>
        /// Persists the column DTO into the database.
        /// </summary>
        public override void Persist()
        {
            Insert(new object[] { boardID, columnOrdinal, limit });
            isPersisted = true;
        }

        /// <summary>
        /// Updates the limit of the column in the database.
        /// </summary>
        /// <param name="limit">The new limit value.</param>
        public void UpdateLimit(int limit)
        {
            if(isPersisted)
            {
                Update(new object[] { boardID, columnOrdinal }, "columnLimit", limit);
            }
        }

        /// <summary>
        /// Removes the persistence state of the column DTO.
        /// </summary>
        public void Unpersist()
        {
            isPersisted = false;
        }
    }
}
