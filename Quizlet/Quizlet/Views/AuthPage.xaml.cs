using System;
using System.Windows.Controls;
using Quizlet.ViewModel;

namespace Quizlet.Views
{
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

            AuthVM vm = DataContext as AuthVM;
            if (vm != null)
                vm.AuthSucceeded += Vm_AuthSucceeded;
        }

        private void Vm_AuthSucceeded(object sender, EventArgs e)
        {
            Quizlet.MainWindow shell = System.Windows.Application.Current.MainWindow as Quizlet.MainWindow;
            if (shell != null)
            {
                shell.Navigate(new LobbyPage());
                shell.ClearHistory(); // kein "Zurück" zum Login
            }
        }
    }
}
