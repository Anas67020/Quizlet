using Prism.Mvvm;
using Quizlet.Model;

namespace Quizlet.ViewModel
{
    public class MainVM : BindableBase
    {
        private object currentViewModel;
        public object CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        private Quizlet.Model.AppSession session;
        public Quizlet.Model.AppSession Session
        {
            get { return session; }
            private set { SetProperty(ref session, value); }
        }

        public ModelGameHub GameHub { get; set; }

        public MainVM()
        {
            Session = new Quizlet.Model.AppSession();
            GameHub = new ModelGameHub();
            ShowAuth();
        }

        public void ShowAuth()
        {
            CurrentViewModel = new AuthVM(this);
        }

        public void ShowLobby()
        {
            CurrentViewModel = new LobbyVM(this);
        }

        public void ShowSettings()
        {
            CurrentViewModel = new SettingsVM(this);
        }

        public void ShowGames()
        {
            CurrentViewModel = new GamesVM(this);
        }

        public void ShowStats()
        {
            CurrentViewModel = new StatsVM(this);
        }

        public void ShowGame(GameSession game)
        {
            CurrentViewModel = new GameVM(this, game);
        }

        public void ShowGameResult(GameSession game, int score, int maxScore)
        {
            CurrentViewModel = new GameResultVM(this, game, score, maxScore);
        }

        public void DoLogout()
        {
            Session.Clear();
            ShowAuth();
        }
    }
}
