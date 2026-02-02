using Prism.Mvvm;
using System;

namespace Quizlet.Model
{
    public class GameSession : BindableBase
    {
        // Eindeutige ID für ein Spiel (steht für Globally Unique Identifier also eine weltweit eindeutig gedachte Kennung)
        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        // Host-Infos
        private int hostUserId;
        public int HostUserId
        {
            get { return hostUserId; }
            set { SetProperty(ref hostUserId, value); }
        }

        private string hostName;
        public string HostName
        {
            get { return hostName; }
            set { SetProperty(ref hostName, value); }
        }

        // Spiel-Infos
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        // Gegnername (leer = noch keiner)
        private string opponentName;
        public string OpponentName
        {
            get { return opponentName; }
            set
            {
                SetProperty(ref opponentName, value);
                // Anzeige aktualisieren
                RaisePropertyChanged(nameof(Display));
            }
        }

        // Spielstatus
        private GameState state;
        public GameState State
        {
            get { return state; }
            set
            {
                SetProperty(ref state, value);
                // Anzeige aktualisieren
                RaisePropertyChanged(nameof(Display));
            }
        }

        // Erstellungszeit
        private DateTime createdAt;
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { SetProperty(ref createdAt, value); }
        }

        // Text für ListBox Anzeige
        public string Display
        {
            get
            {
                string opp = "";
                if (!string.IsNullOrWhiteSpace(OpponentName))
                {
                    opp = " | Gegner: " + OpponentName;
                }

                return Title + " | Host: " + HostName + opp + " | " + State;
            }
        }

        public GameSession(int hostUserId, string hostName, string title, GameState state)
        {
            // Grundwerte setzen
            Id = Guid.NewGuid();
            HostUserId = hostUserId;
            HostName = hostName;
            Title = title;
            State = state;

            OpponentName = "";
            CreatedAt = DateTime.Now;
        }
    }
}
