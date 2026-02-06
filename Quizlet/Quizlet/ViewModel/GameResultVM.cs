using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class GameResultVM : BindableBase
    {
        private readonly MainVM main;
        private readonly GameSession game;

        private string resultText;
        public string ResultText
        {
            get { return resultText; }
            set { SetProperty(ref resultText, value); }
        }

        public Quizlet.Model.AppSession Session
        {
            get { return main.Session; }
        }


        public ICommand BackToGamesCommand { get; private set; }
        public ICommand BackToLobbyCommand { get; private set; }

        public GameResultVM(MainVM main, GameSession game, int score, int maxScore)
        {
            this.main = main;
            this.game = game;

            ResultText = "Ergebnis: " + score + " / " + maxScore;

            BackToGamesCommand = new DelegateCommand(BackToGames);
            BackToLobbyCommand = new DelegateCommand(BackToLobby);
        }

        private void BackToGames()
        {
            main.ShowGames();
        }

        private void BackToLobby()
        {
            main.ShowLobby();
        }
    }
}
