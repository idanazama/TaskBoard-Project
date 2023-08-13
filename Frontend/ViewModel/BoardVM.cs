using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Frontend.ViewModel
{
    internal class BoardVM : NotifiableObject
    {
        private WrapperBackendController wrapperBackendController;
        private ObservableCollection<TaskModel> backLogTasks;
        private ObservableCollection<TaskModel> inProgressTasks;
        private ObservableCollection<TaskModel> doneTasks;
        private int[] column_limits;
        private const int COLUMNS_COUNT = 3;
        private BoardModel board;
        private const int backLog_Ordinal = 0;
        private const int inProgress_Ordinal = 1;
        private const int done_Ordinal = 2;
        private TaskModel selectedTask;


        public BoardVM(WrapperBackendController wrapperBackendController, BoardModel boardModel)
        {
            this.wrapperBackendController = wrapperBackendController;
            this.column_limits = new int[COLUMNS_COUNT];
            for(int i= 0; i < column_limits.Length; i++)
            {
                column_limits[i] = boardModel.columns[i].ColumnLimit;
            }
            this.board = boardModel;
            LoadBackLogTasks();
            LoadInProgressTasks();
            LoadDoneTasks();
        }
        public ObservableCollection<TaskModel> BackLogTasks
        {
            get
            {
                return backLogTasks;
            }
        }
        public ObservableCollection<TaskModel> InProgressTasks
        {
            get
            {
                return inProgressTasks;
            }
        }
        public ObservableCollection<TaskModel> DoneTasks
        {
            get
            {
                return doneTasks;
            }
        }
        public TaskModel SelectedTask
        {
            private get { return selectedTask; }
            set
            {
                selectedTask = value;
            }
        }

        private void LoadBackLogTasks()
        {
            backLogTasks = new ObservableCollection<TaskModel>();
            foreach(TaskModel task in board.columns[backLog_Ordinal].TaskModels)
            {
                backLogTasks.Add(task);
            }
        }
        private void LoadInProgressTasks()
        {
            inProgressTasks = new ObservableCollection<TaskModel>();
            foreach (TaskModel task in board.columns[inProgress_Ordinal].TaskModels)
            {
                inProgressTasks.Add(task);
            }
        }
        private void LoadDoneTasks()
        {
            doneTasks = new ObservableCollection<TaskModel>();
            foreach (TaskModel task in board.columns[done_Ordinal].TaskModels)
            {
                doneTasks.Add(task);
            }

        }

        internal void ShowTaskDetails(BoardWindow boardWindow)
        {
            if (SelectedTask == null)
            {
                MessageBox.Show("No selected task!");
                return;
            }
            try
            {
                TaskWindow taskWindow = new TaskWindow(boardWindow, selectedTask);
                boardWindow.Hide();
                taskWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
