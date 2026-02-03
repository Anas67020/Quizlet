using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class LobbyVM : BindableBase
    {
        private MainVM main;

        private ICommand gamesCommand;
        public ICommand GamesCommand { get { return gamesCommand; } set { SetProperty(ref gamesCommand, value); } }

        private ICommand statsCommand;
        public ICommand StatsCommand { get { return statsCommand; } set { SetProperty(ref statsCommand, value); } }

        private ICommand settingsCommand;
        public ICommand SettingsCommand { get { return settingsCommand; } set { SetProperty(ref settingsCommand, value); } }

        private ICommand logoutCommand;
        public ICommand LogoutCommand { get { return logoutCommand; } set { SetProperty(ref logoutCommand, value); } }

        public LobbyVM(MainVM main)
        {
            this.main = main;

            GamesCommand = new DelegateCommand(OpenGames);
            StatsCommand = new DelegateCommand(OpenStats);
            SettingsCommand = new DelegateCommand(OpenSettings);
            LogoutCommand = new DelegateCommand(Logout);
        }

        public void OpenGames()
        {
            //main.ShowGames();
        }

        public void OpenStats()
        {
            //main.ShowStats();
        }

        public void OpenSettings()
        {
            main.ShowSettings();
        }

        public void Logout()
        {
            AppSession.CurrentUserId = -1;
            AppSession.CurrentUsername = "";
            main.ShowAuth();
        }
    }
}
