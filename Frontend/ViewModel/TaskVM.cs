using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class TaskVM
    {
        private TaskModel task;
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string CreationTime { get; set; }
        public TaskVM(TaskModel taskModel)
        {
            this.task = taskModel;
            this.Title = taskModel.Title;
            this.Description = taskModel.Description;
            this.CreationTime = taskModel.CreationTime.ToString();
            this.DueDate = taskModel.DueDate.ToString();
        }


    }
}
