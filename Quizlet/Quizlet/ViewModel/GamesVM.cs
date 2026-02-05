using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class GamesVM : BindableBase
    {
        private readonly MainVM main;
        private readonly ModelGameHub hub;

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public ObservableCollection<GameSession> MyRunningGames { get; private set; }
        public ObservableCollection<GameSession> OpenGames { get; private set; }

        private GameSession selectedMyGame;
        public GameSession SelectedMyGame
        {
            get { return selectedMyGame; }
            set
            {
                SetProperty(ref selectedMyGame, value);
                ((DelegateCommand)ContinueCommand).RaiseCanExecuteChanged();
            }
        }

        private GameSession selectedOpenGame;
        public GameSession SelectedOpenGame
        {
            get { return selectedOpenGame; }
            set
            {
                SetProperty(ref selectedOpenGame, value);
                ((DelegateCommand)JoinCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand StartNewGameCommand { get; private set; }
        public ICommand JoinCommand { get; private set; }
        public ICommand ContinueCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public GamesVM(MainVM main)
        {
            this.main = main;
            this.hub = main.GameHub; // WICHTIG: gleiche Instanz wie im MainVM!

            MyRunningGames = new ObservableCollection<GameSession>();
            OpenGames = new ObservableCollection<GameSession>();

            StartNewGameCommand = new DelegateCommand(StartNewGame);
            JoinCommand = new DelegateCommand(Join, CanJoin);
            ContinueCommand = new DelegateCommand(Continue, CanContinue);
            BackCommand = new DelegateCommand(Back);
            RefreshCommand = new DelegateCommand(Refresh);

            Refresh();
        }

        public void Refresh()
        {
            StatusText = "";
            MyRunningGames.Clear();
            OpenGames.Clear();

            int uid = main.Session.CurrentUserId;
            string uname = main.Session.CurrentUsername;

            foreach (GameSession s in hub.Sessions)
            {
                bool iAmHost = (s.HostUserId == uid);
                bool iAmOpponent = (string.IsNullOrWhiteSpace(uname) == false && s.OpponentName == uname);

                if (s.State == GameState.Running && (iAmHost || iAmOpponent))
                    MyRunningGames.Add(s);

                if (s.State == GameState.WaitingForPlayer && s.HostUserId != uid)
                    OpenGames.Add(s);
            }

            ((DelegateCommand)JoinCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ContinueCommand).RaiseCanExecuteChanged();
        }

        private void StartNewGame()
        {
            if (main.Session.CurrentUserId < 0)
            {
                StatusText = "Bitte erst einloggen.";
                return;
            }

            GameSession game = hub.StartNewGame(main.Session.CurrentUserId, main.Session.CurrentUsername, "Neues Quiz");
            Refresh();

            // Optional direkt rein:
            main.ShowGame(game);
        }

        private bool CanJoin()
        {
            return SelectedOpenGame != null;
        }

        private void Join()
        {
            StatusText = "";

            if (SelectedOpenGame == null) return;

            try
            {
                hub.JoinGame(SelectedOpenGame, main.Session.CurrentUsername);
                Refresh();
                main.ShowGame(SelectedOpenGame);
            }
            catch (Exception ex)
            {
                StatusText = ex.Message;
            }
        }

        private bool CanContinue()
        {
            return SelectedMyGame != null;
        }

        private void Continue()
        {
            if (SelectedMyGame == null) return;
            main.ShowGame(SelectedMyGame);
        }

        private void Back()
        {
            main.ShowLobby();
        }
    }
}
