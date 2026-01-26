using System;
using System.Windows.Controls;
using Quizlet.ViewModel;

namespace Quizlet.Views
{
    public partial class LobbyPage : Page
    {
        public LobbyPage()
        {
            InitializeComponent();

            LobbyVM vm = DataContext as LobbyVM;
            if (vm != null)
            {
                vm.NewGameRequested += Vm_NewGameRequested;
                vm.StatsRequested += Vm_StatsRequested;
                vm.SettingsRequested += Vm_SettingsRequested;
                vm.LogoutRequested += Vm_LogoutRequested;
            }
        }

        private void Vm_NewGameRequested(object sender, EventArgs e)
        {
            var shell = System.Windows.Application.Current.MainWindow as Quizlet.MainWindow;
            if (shell != null) shell.Navigate(new GamePage());
        }

        private void Vm_StatsRequested(object sender, EventArgs e)
        {
            var shell = System.Windows.Application.Current.MainWindow as Quizlet.MainWindow;
            if (shell != null) shell.Navigate(new StatsPage());
        }

        private void Vm_SettingsRequested(object sender, EventArgs e)
        {
            var shell = System.Windows.Application.Current.MainWindow as Quizlet.MainWindow;
            if (shell != null) shell.Navigate(new SettingsPage());
        }

        private void Vm_LogoutRequested(object sender, EventArgs e)
        {
            var shell = System.Windows.Application.Current.MainWindow as Quizlet.MainWindow;
            if (shell != null)
            {
                shell.Navigate(new AuthPage());
                shell.ClearHistory();
            }
        }
    }
}
