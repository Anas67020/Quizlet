using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class MainVM : BindableBase
    {
        // Aktuelle ViewModel-Instanz, die angezeigt wird
        private BindableBase currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        // Gemeinsame Commands für Navigation
        private ICommand showAuthCommand;
        public ICommand ShowAuthCommand
        {
            get { return showAuthCommand; }
            set { SetProperty(ref showAuthCommand, value); }
        }

        private ICommand showLobbyCommand;
        public ICommand ShowLobbyCommand
        {
            get { return showLobbyCommand; }
            set { SetProperty(ref showLobbyCommand, value); }
        }

        private ICommand showGamesCommand;
        public ICommand ShowGamesCommand
        {
            get { return showGamesCommand; }
            set { SetProperty(ref showGamesCommand, value); }
        }

        private ICommand showSettingsCommand;
        public ICommand ShowSettingsCommand
        {
            get { return showSettingsCommand; }
            set { SetProperty(ref showSettingsCommand, value); }
        }

        private ICommand showStatsCommand;
        public ICommand ShowStatsCommand
        {
            get { return showStatsCommand; }
            set { SetProperty(ref showStatsCommand, value); }
        }

        public MainVM()
        {
            // Commands verbinden mit Methoden
            ShowAuthCommand = new DelegateCommand(ShowAuth);
            ShowLobbyCommand = new DelegateCommand(ShowLobby);
            ShowGamesCommand = new DelegateCommand(ShowGames);
            ShowSettingsCommand = new DelegateCommand(ShowSettings);
            ShowStatsCommand = new DelegateCommand(ShowStats);

            // Startansicht
            ShowAuth();
        }

        public void ShowAuth()
        {
            // AuthVM bekommt MainVM, damit es navigieren kann
            CurrentViewModel = new AuthVM(this);
        }

        public void ShowLobby()
        {
            CurrentViewModel = new LobbyVM(this);
        }

        public void ShowGames()
        {
            CurrentViewModel = new GamesVM(this);
        }

        public void ShowSettings()
        {
            CurrentViewModel = new SettingsVM(this);
        }

        public void ShowStats()
        {
            CurrentViewModel = new StatsVM(this);
        }
    }
}
