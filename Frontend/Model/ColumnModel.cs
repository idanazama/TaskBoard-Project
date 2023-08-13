using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    class ColumnModel
    {
        public List<TaskModel> TaskModels { get; set; }
        public int ColumnLimit { get; set; }
        public ColumnModel(List<TaskModel> taskModels,int columnLimit)
        {
            this.TaskModels = taskModels;
            this.ColumnLimit = columnLimit;
        }
    }
}
