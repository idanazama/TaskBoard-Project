using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for AllBoardsWindow.xaml
    /// </summary>
    public partial class AllBoardsWindow : Window
    {
        private AllBoardsVM viewModel;
        private MainWindow mainWindow;
        private WrapperBackendController wrapperBackendController;
        private UserModel user;
        internal AllBoardsWindow(MainWindow mainWindow, WrapperBackendController wrapperBackendController,UserModel user)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.wrapperBackendController = wrapperBackendController;
            this.user = user;
            viewModel = new AllBoardsVM(wrapperBackendController,user);
            this.DataContext = viewModel;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wrapperBackendController.backendUserController.Logout(user.Email);
            mainWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EnterBoard_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Enter Board Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                viewModel.EnterBoard(this);
            }
        }

        private void DeleteBoard_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                viewModel.DeleteBoard();
            }
        }
        private void CreateNewBoard_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow("Board name");
            if(inputWindow.ShowDialog() == true)
            {
                viewModel.CreateBoard(inputWindow.Answer());
            }
        }
    }
}
