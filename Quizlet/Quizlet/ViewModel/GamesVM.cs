using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class GamesVM : BindableBase
    {
        private MainVM main;
        private ModelGameHub hub;

        private ObservableCollection<GameSession> myRunningGames;
        public ObservableCollection<GameSession> MyRunningGames
        {
            get { return myRunningGames; }
            set { SetProperty(ref myRunningGames, value); }
        }

        private ObservableCollection<GameSession> openGames;
        public ObservableCollection<GameSession> OpenGames
        {
            get { return openGames; }
            set { SetProperty(ref openGames, value); }
        }

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

        private ICommand startNewGameCommand;
        public ICommand StartNewGameCommand
        {
            get { return startNewGameCommand; }
            set { SetProperty(ref startNewGameCommand, value); }
        }

        private ICommand joinCommand;
        public ICommand JoinCommand
        {
            get { return joinCommand; }
            set { SetProperty(ref joinCommand, value); }
        }

        private ICommand continueCommand;
        public ICommand ContinueCommand
        {
            get { return continueCommand; }
            set { SetProperty(ref continueCommand, value); }
        }

        private ICommand backCommand;
        public ICommand BackCommand
        {
            get { return backCommand; }
            set { SetProperty(ref backCommand, value); }
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get { return refreshCommand; }
            set { SetProperty(ref refreshCommand, value); }
        }

        public GamesVM(MainVM main)
        {
            this.main = main;

            hub = new ModelGameHub();
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
            MyRunningGames.Clear();
            OpenGames.Clear();

            int uid = AppSession.CurrentUserId;

            foreach (GameSession s in hub.Sessions)
            {
                if (s.HostUserId == uid && s.State == GameState.Running)
                    MyRunningGames.Add(s);

                if (s.State == GameState.WaitingForPlayer && s.HostUserId != uid)
                    OpenGames.Add(s);
            }

            ((DelegateCommand)JoinCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ContinueCommand).RaiseCanExecuteChanged();
        }

        public void StartNewGame()
        {
            if (AppSession.CurrentUserId < 0) return;

            hub.StartNewGame(AppSession.CurrentUserId, AppSession.CurrentUsername, "Neues Quiz");
            Refresh();
        }

        private bool CanJoin()
        {
            return SelectedOpenGame != null;
        }

        public void Join()
        {
            if (SelectedOpenGame == null) return;

            hub.JoinGame(SelectedOpenGame, AppSession.CurrentUsername);
            Refresh();

            // Später: main.CurrentViewModel = new GameVM(main, SelectedOpenGame);
        }

        private bool CanContinue()
        {
            return SelectedMyGame != null;
        }

        public void Continue()
        {
            if (SelectedMyGame == null) return;

            // Später: main.CurrentViewModel = new GameVM(main, SelectedMyGame);
        }

        public void Back()
        {
            main.ShowLobby();
        }
    }
}
