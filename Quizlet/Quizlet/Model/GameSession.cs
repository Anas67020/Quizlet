using Prism.Mvvm;
using System;

namespace Quizlet.Model
{
    public class GameSession : BindableBase
    {
        private Guid id;
        public Guid Id { get => id; set => SetProperty(ref id, value); }

        private int hostUserId;
        public int HostUserId { get => hostUserId; set => SetProperty(ref hostUserId, value); }

        private string hostName;
        public string HostName { get => hostName; set => SetProperty(ref hostName, value); }

        private int opponentUserId;
        public int OpponentUserId
        {
            get => opponentUserId;
            set { SetProperty(ref opponentUserId, value); RaisePropertyChanged(nameof(Display)); }
        }

        private string opponentName;
        public string OpponentName
        {
            get => opponentName;
            set { SetProperty(ref opponentName, value); RaisePropertyChanged(nameof(Display)); }
        }

        private string title;
        public string Title
        {
            get => title;
            set { SetProperty(ref title, value); RaisePropertyChanged(nameof(Display)); }
        }

        private GameState state;
        public GameState State
        {
            get => state;
            set { SetProperty(ref state, value); RaisePropertyChanged(nameof(Display)); }
        }

        private DateTime createdAt;
        public DateTime CreatedAt { get => createdAt; set => SetProperty(ref createdAt, value); }

        public string Display
        {
            get
            {
                string opp = string.IsNullOrWhiteSpace(OpponentName) ? "" : $" | Gegner: {OpponentName}";
                return $"{Title} | Host: {HostName}{opp} | {State}";
            }
        }

        public GameSession(int hostUserId, string hostName, string title, GameState state)
        {
            Id = Guid.NewGuid();
            HostUserId = hostUserId;
            HostName = hostName;
            Title = title;
            State = state;

            OpponentUserId = -1;
            OpponentName = "";
            CreatedAt = DateTime.Now;
        }
    }
}
