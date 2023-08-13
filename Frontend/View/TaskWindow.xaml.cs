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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private BoardWindow boardWindow;
        private TaskVM viewModel;
        internal TaskWindow(BoardWindow boardWindow,TaskModel task)
        {
            InitializeComponent();
            viewModel = new TaskVM(task);
            DataContext = viewModel;
            this.boardWindow = boardWindow;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            boardWindow.Show();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
