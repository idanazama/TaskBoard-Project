using Frontend.Model;
using Frontend.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Frontend.ViewModel
{
    internal class AllBoardsVM
    {
        private WrapperBackendController wrapperBackendController;
        private ObservableCollection<ListBoxItem> boardsList;
        private UserModel user;
        private ListBoxItem selectedBoard;
        public AllBoardsVM(WrapperBackendController wrapperBackendController,UserModel user)
        {
            this.wrapperBackendController = wrapperBackendController;
            this.user = user;
            LoadBoards();
        }
        public ObservableCollection<ListBoxItem> BoardsList
        {
            get
            {
                return boardsList;
            }
        }
        public ListBoxItem SelectedBoard
        {
            private get { return selectedBoard; }
            set
            {
                selectedBoard = value;
            }
        }
        public void LoadBoards()
        {
            boardsList = new ObservableCollection<ListBoxItem>();
            string[] names = wrapperBackendController.backendUserController.GetBoardsNames(user.Email);
            foreach (string s in names)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = s;
                boardsList.Add(listBoxItem);
            }
        }
        public void CreateBoard(string boardName)
        {
            try
            {
                wrapperBackendController.backendboardController.CreateBoard(user.Email, boardName);
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = boardName;
                boardsList.Add(listBoxItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeleteBoard()
        {
            if(selectedBoard == null)
            {
                MessageBox.Show("No selected board to delete!");
                return;
            }
            try
            {
                wrapperBackendController.backendboardController.DeleteBoard(user.Email, (string)SelectedBoard.Content);
                boardsList.Remove(SelectedBoard);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void EnterBoard(AllBoardsWindow allBoardsWindow)
        {
            if(selectedBoard == null)
            {
                MessageBox.Show("No selected board!");
                return;
            }
            try
            {
                string boardName = (string)SelectedBoard.Content;
                BoardWindow boardWindow = new BoardWindow(allBoardsWindow, wrapperBackendController, user, new BoardModel(user.Email, boardName, wrapperBackendController));
                allBoardsWindow.Hide(); 
                boardWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
