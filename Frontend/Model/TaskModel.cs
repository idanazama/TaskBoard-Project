using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class TaskModel : NotifiableObject
    {
        private int id;
        private DateTime creationTime;
        private string title;
        private string description;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }
        public DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged("Title");
            }
        }
        public DateTime DueDate { get; set; }
        public TaskModel(int id, DateTime CreationTime, string title, string description, DateTime DueDate)
        {
            this.Id = id;
            this.CreationTime = CreationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = DueDate;
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
