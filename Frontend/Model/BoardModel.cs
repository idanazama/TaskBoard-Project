using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class BoardModel
    {
        public string Name { get; set; }
        public ColumnModel[] columns { get; set; }

        private const int COLUMNS_COUNT = 3;
        public BoardModel(string email, string boardName, WrapperBackendController wrapperBackendController) 
        {
            this.Name = boardName;
            columns = new ColumnModel[COLUMNS_COUNT];
            for(int i = 0; i < columns.Length; i++)
            {
                columns[i] = new ColumnModel(wrapperBackendController.backendboardController.GetColumn(email, boardName, i), wrapperBackendController.backendboardController.GetColumnLimit(email,boardName, i));
            }
        }

        internal string[] GetTitles(int columnOrdinal)
        {
            string[] titles = new string[columns[columnOrdinal].TaskModels.Count];
            for(int i = 0; i < titles.Length; i++)
            {
                titles[i] = columns[columnOrdinal].TaskModels[i].Title;
            }



            return titles;

        }
    }
}
