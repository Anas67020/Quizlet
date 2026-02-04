using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class LobbyVM : BindableBase
    {
        private readonly MainVM main;

        public ICommand GamesCommand { get; private set; }
        public ICommand StatsCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public LobbyVM(MainVM main)
        {
            this.main = main;

            GamesCommand = new DelegateCommand(OpenGames);
            StatsCommand = new DelegateCommand(OpenStats);
            SettingsCommand = new DelegateCommand(OpenSettings);
            LogoutCommand = new DelegateCommand(Logout);
        }

        private void OpenGames()
        {
            main.ShowGames();   
        }

        private void OpenStats()
        {
            main.ShowStats();  
        }

        private void OpenSettings()
        {
            main.ShowSettings();
        }

        private void Logout()
        {
            // NICHT static!
            main.Session.Clear();
            main.ShowAuth();
        }
    }
}
