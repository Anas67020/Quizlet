using Prism.Mvvm;
using System;

namespace Quizlet.Model
{
    public class GameSession : BindableBase
    {
        private Guid id;
        public Guid Id { get { return id; } set { SetProperty(ref id, value); } }

        private int hostUserId;
        public int HostUserId { get { return hostUserId; } set { SetProperty(ref hostUserId, value); } }

        private string hostName;
        public string HostName { get { return hostName; } set { SetProperty(ref hostName, value); } }

        private int opponentUserId;
        public int OpponentUserId
        {
            get { return opponentUserId; }
            set { SetProperty(ref opponentUserId, value); RaisePropertyChanged(nameof(Display)); }
        }

        private string opponentName;
        public string OpponentName
        {
            get { return opponentName; }
            set { SetProperty(ref opponentName, value); RaisePropertyChanged(nameof(Display)); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); RaisePropertyChanged(nameof(Display)); }
        }

        private GameState state;
        public GameState State
        {
            get { return state; }
            set { SetProperty(ref state, value); RaisePropertyChanged(nameof(Display)); }
        }

        private DateTime createdAt;
        public DateTime CreatedAt { get { return createdAt; } set { SetProperty(ref createdAt, value); } }

        // =====================
        // NEU: Kategorie / Mode
        // =====================
        private int categoryId;
        public int CategoryId
        {
            get { return categoryId; }
            set { SetProperty(ref categoryId, value); RaisePropertyChanged(nameof(Display)); }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set { SetProperty(ref categoryName, value); RaisePropertyChanged(nameof(Display)); }
        }

        private int gameModeId;
        public int GameModeId
        {
            get { return gameModeId; }
            set { SetProperty(ref gameModeId, value); RaisePropertyChanged(nameof(Display)); }
        }

        public string Display
        {
            get
            {
                string opp = "";
                if (!string.IsNullOrWhiteSpace(OpponentName))
                {
                    opp = $" | Gegner: {OpponentName}";
                }

                string cat = "";
                if (!string.IsNullOrWhiteSpace(CategoryName))
                {
                    cat = $" | Kategorie: {CategoryName}";
                }

                return $"{Title} | Host: {HostName}{opp}{cat} | {State}";
            }
        }

        // ALT bleibt (damit alter Code weiter läuft)
        public GameSession(int hostUserId, string hostName, string title, GameState state)
            : this(hostUserId, hostName, title, state, -1, "", 1)
        {
        }

        // NEU (mit Kategorie/Mode)
        public GameSession(int hostUserId, string hostName, string title, GameState state, int categoryId, string categoryName, int gameModeId)
        {
            Id = Guid.NewGuid();
            HostUserId = hostUserId;
            HostName = hostName;
            Title = title;
            State = state;

            OpponentUserId = -1;
            OpponentName = "";

            CategoryId = categoryId;
            CategoryName = categoryName;
            GameModeId = gameModeId;

            CreatedAt = DateTime.Now;
        }
    }
}
