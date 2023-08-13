using Frontend.Model;
using Frontend.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainVM viewModel;
        private WrapperBackendController wrapperBackendController;
        internal MainWindow(WrapperBackendController wpc)
        {
            InitializeComponent();
            this.wrapperBackendController = wpc;
            viewModel = new MainVM(wrapperBackendController);
            this.DataContext = viewModel;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if (user!=null)
            {
                AllBoardsWindow allBoardsWindow = new AllBoardsWindow(this,wrapperBackendController,user);
                this.Hide();
                allBoardsWindow.Show();
            }
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Register();
            if (user != null)
            {
                AllBoardsWindow allBoardsWindow = new AllBoardsWindow(this, wrapperBackendController, user);
                this.Hide();
                allBoardsWindow.Show();
            }
        }
    }
}
