using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class GameVM : BindableBase
    {
        private readonly MainVM main;
        private readonly GameSession game;

        private readonly DemoQuestionService questionService;

        private int index;
        private int score;

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string questionText;
        public string QuestionText
        {
            get { return questionText; }
            set { SetProperty(ref questionText, value); }
        }

        public ObservableCollection<string> Options { get; private set; }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                SetProperty(ref selectedIndex, value);
                ((DelegateCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }

        public ICommand SubmitCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        private ObservableCollection<Question> questions;

        public GameVM(MainVM main, GameSession game)
        {
            this.main = main;
            this.game = game;

            questionService = new DemoQuestionService();
            Options = new ObservableCollection<string>();
            questions = new ObservableCollection<Question>();

            SubmitCommand = new DelegateCommand(Submit, CanSubmit);
            BackCommand = new DelegateCommand(Back);

            Title = game.Title;

            LoadQuestions();
            ShowQuestion(0);
        }

        private void LoadQuestions()
        {
            questions.Clear();
            foreach (Question q in questionService.GetQuestions())
            {
                questions.Add(q);
            }
        }

        private void ShowQuestion(int i)
        {
            StatusText = "";
            SelectedIndex = -1;

            if (i < 0 || i >= questions.Count) return;

            index = i;

            Question q = questions[i];
            QuestionText = q.Text;

            Options.Clear();
            foreach (string opt in q.Options)
            {
                Options.Add(opt);
            }
        }

        private bool CanSubmit()
        {
            return SelectedIndex >= 0;
        }

        private void Submit()
        {
            Question q = questions[index];

            if (SelectedIndex == q.CorrectIndex)
            {
                score = score + 1;
                StatusText = "Richtig!";
            }
            else
            {
                StatusText = "Falsch!";
            }

            int next = index + 1;
            if (next >= questions.Count)
            {
                main.ShowGameResult(game, score, questions.Count);
                return;
            }

            ShowQuestion(next);
        }

        private void Back()
        {
            main.ShowGames();
        }
    }
}
