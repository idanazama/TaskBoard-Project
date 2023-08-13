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
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardVM viewModel;
        private AllBoardsWindow allBoardsWindow;
        private WrapperBackendController wrapperBackendController;
        private UserModel user;
        internal BoardWindow(AllBoardsWindow allBoardsWindow, WrapperBackendController wrapperBackendController,UserModel user, BoardModel boardModel)
        {
            InitializeComponent();
            this.allBoardsWindow = allBoardsWindow;
            this.wrapperBackendController = wrapperBackendController;
            this.user = user;
            viewModel = new BoardVM(wrapperBackendController, boardModel);
            this.DataContext = viewModel;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            allBoardsWindow.Show();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ShowTaskDetails(this);
        }
    }
}
