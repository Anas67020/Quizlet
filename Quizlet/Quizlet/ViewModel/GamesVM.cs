using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Quizlet.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quizlet.ViewModel
{
    public class GamesVM : BindableBase
    {
        private readonly MainVM main;
        private readonly ModelGameHub hub;
        private readonly QuizApi api;

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

        // Create-Game Overlay
        private bool isCreateGameOpen;
        public bool IsCreateGameOpen
        {
            get { return isCreateGameOpen; }
            set { SetProperty(ref isCreateGameOpen, value); }
        }

        public ObservableCollection<CategoryApi> Categories { get; private set; }

        private CategoryApi selectedCategory;
        public CategoryApi SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                SetProperty(ref selectedCategory, value);
                ((DelegateCommand)CreateGameCommand).RaiseCanExecuteChanged();

                if (string.IsNullOrWhiteSpace(NewGameTitle) && selectedCategory != null)
                {
                    NewGameTitle = selectedCategory.Name;
                }
            }
        }

        private string newGameTitle;
        public string NewGameTitle
        {
            get { return newGameTitle; }
            set { SetProperty(ref newGameTitle, value); }
        }

        public ICommand StartNewGameCommand { get; private set; }
        public ICommand CreateGameCommand { get; private set; }
        public ICommand CancelCreateGameCommand { get; private set; }

        public ICommand JoinCommand { get; private set; }
        public ICommand ContinueCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public GamesVM(MainVM main)
        {
            this.main = main;
            this.hub = main.GameHub;
            this.api = new QuizApi();

            MyRunningGames = new ObservableCollection<GameSession>();
            OpenGames = new ObservableCollection<GameSession>();

            Categories = new ObservableCollection<CategoryApi>();

            StartNewGameCommand = new DelegateCommand(OpenCreateGame);
            CreateGameCommand = new DelegateCommand(CreateGame, CanCreateGame);
            CancelCreateGameCommand = new DelegateCommand(CancelCreateGame);

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

            foreach (GameSession s in hub.Sessions.OrderByDescending(x => x.CreatedAt))
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

        private async void OpenCreateGame()
        {
            if (main.Session.CurrentUserId < 0)
            {
                StatusText = "Bitte erst einloggen.";
                return;
            }

            StatusText = "";
            IsCreateGameOpen = true;

            await EnsureCategoriesLoadedAsync();

            if (SelectedCategory == null && Categories.Count > 0)
            {
                SelectedCategory = Categories[0];
            }

            if (string.IsNullOrWhiteSpace(NewGameTitle))
            {
                if (SelectedCategory != null)
                    NewGameTitle = SelectedCategory.Name;
                else
                    NewGameTitle = "Neues Quiz";
            }
        }

        private async Task EnsureCategoriesLoadedAsync()
        {
            if (Categories.Count > 0) return;

            try
            {
                StatusText = "Kategorien werden geladen...";

                // WICHTIG: AuthToken + ApiKey
                var resp = await api.GetCategoriesAsync(main.Session.AuthToken, main.Session.ApiKey);
                string json = await resp.Content.ReadAsStringAsync();

                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    StatusText = "Fehler beim Laden der Kategorien: " + json;
                    return;
                }

                var cats = JsonConvert.DeserializeObject<CategoryApi[]>(json);

                Categories.Clear();
                if (cats != null)
                {
                    foreach (var c in cats.OrderBy(x => x.Name))
                        Categories.Add(c);
                }

                StatusText = "";
            }
            catch (Exception ex)
            {
                StatusText = "Kategorien konnten nicht geladen werden: " + ex.Message;
            }
        }

        private bool CanCreateGame()
        {
            return SelectedCategory != null;
        }

        private void CreateGame()
        {
            StatusText = "";

            if (SelectedCategory == null)
            {
                StatusText = "Bitte eine Kategorie auswählen.";
                return;
            }

            string title = NewGameTitle;
            if (string.IsNullOrWhiteSpace(title))
                title = SelectedCategory.Name;

            GameSession game = hub.StartSingleplayerGame(
                main.Session.CurrentUserId,
                main.Session.CurrentUsername,
                title,
                SelectedCategory.Id,
                SelectedCategory.Name
            );

            IsCreateGameOpen = false;
            Refresh();

            main.ShowGame(game);
        }

        private void CancelCreateGame()
        {
            StatusText = "";
            IsCreateGameOpen = false;
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
